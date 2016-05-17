using System;
using System.Collections.Generic;
using System.Linq;
using MyAndromeda.Data.MenuDatabase.Models.Database.MenuTableAdapters;
using MyAndromeda.Data.MenuDatabase.Models.Database;

namespace MyAndromeda.Data.MenuDatabase.Services
{
    public interface IAccessGroupDataService : IAccessMenuDatabaseQuery<Menu.n_GroupsRow>
    {

    }

    public class AccessGroupDataService : IAccessGroupDataService
    {
        private readonly n_GroupsTableAdapter adaptor;
        private readonly Menu.n_GroupsDataTable dataTable;

        public AccessGroupDataService(n_GroupsTableAdapter adaptor)
        {
            this.adaptor = adaptor;
            this.dataTable = new Menu.n_GroupsDataTable();
        }

        public IEnumerable<Menu.n_GroupsRow> List(Func<Menu.n_GroupsRow, bool> query = null)
        {
            if (dataTable.Count == 0) { this.adaptor.Fill(dataTable); }

            return this.dataTable.Where(query);
        }

        public void SaveChanges()
        {
            this.adaptor.Update(this.dataTable);
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
                if (this.dataTable != null)
                {
                    this.dataTable.Dispose();
                }
                if (this.adaptor != null)
                {
                    this.adaptor.Dispose();
                }
            }
        }
    }
}
