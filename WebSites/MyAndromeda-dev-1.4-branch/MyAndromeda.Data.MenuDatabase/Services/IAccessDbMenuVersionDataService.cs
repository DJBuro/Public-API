using System;
using System.Collections.Generic;
using System.Linq;
using MyAndromeda.Data.MenuDatabase.Models.Database;
using MyAndromeda.Data.MenuDatabase.Models.Database.MenuTableAdapters;
using MyAndromeda.Data.MenuDatabase.Context;
using System.Data.SqlClient;
using MyAndromeda.Logging;

namespace MyAndromeda.Data.MenuDatabase.Services
{
    public interface IAccessDbMenuVersionDataService : IAccessMenuDatabaseQuery<Menu.n_UserRow>
    {
        bool IsAvailable(int andromedaId);

        Menu.n_UserRow GetMenuVersionRow(int andromedaId);
        Menu.n_UserRow GetTempMenuVersionRow(int andromedaId);

        string GetLastError();
        string GetConnectionString(int andromedaId);

        /// <summary>
        /// Increments the version.
        /// </summary>
        /// <returns>The final version</returns>
        int IncrementVersion(int andromedaId);
    }

    public class AccessDbMenuVersionDataService : IAccessDbMenuVersionDataService, ICompareAccessDbVersionDataService
    {
        private readonly IMyAndromedaLogger logger;

        private int andromedaId; 
        private n_UserTableAdapter tempDbAdaptor;
        private n_UserTableAdapter menuDbAdaptor;

        private readonly Menu.n_UserDataTable actualDataTable;
        private readonly Menu.n_UserDataTable tempDataTable;

        private Exception lastError;


        public AccessDbMenuVersionDataService(IMyAndromedaLogger logger) 
        {
            this.logger = logger;
            this.actualDataTable = new Menu.n_UserDataTable();
            this.tempDataTable = new Menu.n_UserDataTable();
        }

        public void Init(int andromedaId) 
        {
            if (this.andromedaId == andromedaId) { return; }

            this.andromedaId = andromedaId;
            MenuConnectionStringContext connectionContext = new MenuConnectionStringContext(andromedaId);

            this.menuDbAdaptor = new n_UserTableAdapter()
            {
                Connection = new System.Data.OleDb.OleDbConnection(connectionContext.ConnectionString)
            };

            this.tempDbAdaptor = new n_UserTableAdapter()
            {
                Connection = new System.Data.OleDb.OleDbConnection(connectionContext.TempConnectionString)
            };   
        }

        public LatestMenuVersion GetLatest(int andromedaId)
        {
            this.Init(andromedaId);

            //both missing ? 
            if (!this.IsAvailable(andromedaId) && !this.IsTempAvailable(andromedaId)) 
            {
                this.logger.Debug("Both access menus were not found: {0}", andromedaId);
                return LatestMenuVersion.Unknown; 

            }
            //temp is here... yay 
            if (this.IsTempAvailable(andromedaId) && !this.IsAvailable(andromedaId)) 
            {
                this.logger.Debug("Only the temp db for {0} is around, use that", andromedaId);
                return LatestMenuVersion.Temp; 
            }

            var currentRow = this.GetMenuVersionRow(andromedaId);
            var current = new { 
                LastUpdated = currentRow.tLastUpdated,
                RowVersion = currentRow.nVersion
            };

            var tempRow = this.GetTempMenuVersionRow(andromedaId);
            var temp = new {
                 LastUpdate = tempRow.tLastUpdated,
                 RowVersion = tempRow.nVersion
            };

            this.logger.Debug("Check two databases:");
            this.logger.Debug("Current: {0} ({1}); Temp: {2} ({3})", current.RowVersion, current.LastUpdated, temp.LastUpdate, temp.RowVersion);

            //compare both 1. 
            if (current.LastUpdated < temp.LastUpdate) 
            {
                this.logger.Debug("'Last updated' field is newer on the 'temp' file, use that for {0}", andromedaId);
                return LatestMenuVersion.Temp;
            }
            //compare both 2.
            if (current.RowVersion < temp.RowVersion)
            {
                this.logger.Debug("'Version' field on the 'temp' database is greater than the stored, use that for {0}", andromedaId);
                return LatestMenuVersion.Temp;
            }

            this.logger.Debug("The current version is the correct one for: {0}", andromedaId);
            return LatestMenuVersion.Menu;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) 
        {
            if (disposing) 
            {
                if (this.actualDataTable != null)
                {
                    this.actualDataTable.Dispose();
                }
                if (this.menuDbAdaptor != null)
                {
                    this.menuDbAdaptor.Dispose();
                }
            }
        }

        public IEnumerable<Menu.n_UserRow> List(Func<Menu.n_UserRow, bool> query = null)
        {
            if (actualDataTable.Count == 0) { this.menuDbAdaptor.Fill(actualDataTable); }

            return query == null ? 
                this.actualDataTable.Where(e=> true) : 
                this.actualDataTable.Where(query);
        }

        public IEnumerable<Menu.n_UserRow> TempList(Func<Menu.n_UserRow, bool> query = null)
        {
            if (tempDataTable.Count == 0) { this.tempDbAdaptor.Fill(tempDataTable); }

            return query == null ?
                this.tempDataTable.Where(e => true) :
                this.tempDataTable.Where(query);
        }

        public bool IsAvailable(int andromedaId)
        {
            this.Init(andromedaId);

            bool canOpenAndStatus = false;

            try
            {
                this.menuDbAdaptor.Connection.Open();
                this.menuDbAdaptor.Connection.Close();

                canOpenAndStatus = true;
            }
            catch (Exception e) {
                this.lastError = e;
            }

            return canOpenAndStatus;
        }

        public bool IsTempAvailable(int andromedaId) 
        {
            this.Init(andromedaId);

            bool status = false;

            try
            {
                this.tempDbAdaptor.Connection.Open();
                this.tempDbAdaptor.Connection.Close();

                status = true;
            }
            catch (Exception e)
            {
                this.lastError = e;
            }

            return status;
        }

        public Menu.n_UserRow GetMenuVersionRow(int andromedaId)
        {
            this.Init(andromedaId);

            return this.List().SingleOrDefault();
        }

        public Menu.n_UserRow GetTempMenuVersionRow(int andromedaId) 
        {
            this.Init(andromedaId);

            return this.TempList().SingleOrDefault();
        }

        public string GetLastError()
        {
            return this.lastError == null ? string.Empty : this.lastError.Message;
        }

        public string GetConnectionString(int andromedaId)
        {
            this.Init(andromedaId);

            return this.menuDbAdaptor.Connection.ConnectionString;
        }

        public int IncrementVersion(int andromedaId) 
        {
            this.Init(andromedaId);

            var version = this.List().First();
            version.nVersion = version.nVersion + 1;
            version.tLastUpdated = DateTime.Now;

            this.SaveChanges();
            return version.nVersion;
        }

        public void SaveChanges()
        {
            const string Update = @"UPDATE n_User SET [nVersion] = @nVersion, [tLastUpdated] =@tLastUpdated WHERE nSiteID=@nSiteID";

            try
            {
                var cmd = this.menuDbAdaptor.Adapter.UpdateCommand = new System.Data.OleDb.OleDbCommand(Update, this.menuDbAdaptor.Connection);
                var versionColumn = cmd.Parameters.Add("@nVersion", System.Data.OleDb.OleDbType.Integer);
                var lastUpdatedColumn = cmd.Parameters.Add("@tLastUpdated", System.Data.OleDb.OleDbType.Date);
                var siteIdColumn = cmd.Parameters.Add("@nSiteID", System.Data.OleDb.OleDbType.Integer);

                var row = this.List().Single();
                versionColumn.Value = row.nVersion;
                lastUpdatedColumn.Value = row.tLastUpdated;
                siteIdColumn.Value = row.nSiteID;

                this.menuDbAdaptor.Update(actualDataTable);

            }
            catch (Exception e) 
            {
                throw;
            }
            //this.dataTable.AcceptChanges();
        }
    }
}
