using System;
using System.Collections.Generic;
using System.Linq;
using MyAndromeda.Data.MenuDatabase.Context;
using MyAndromeda.Data.MenuDatabase.Models;
using MyAndromeda.Data.MenuDatabase.Models.Database;
using MyAndromeda.Data.MenuDatabase.Models.Database.MenuTableAdapters;
using MyAndromeda.Logging;
using MyAndromeda.Framework.Contexts;

namespace MyAndromeda.Data.MenuDatabase.Services
{
    public class AccessMenuDataSetService : IAccessMenuDataSetService 
    {
        private readonly IMyAndromedaLogger logger;

        private n_MenuTableAdapter menuItemTableAdaptor;
        private t_MenuTableAdapter menuStructureTableAdaptor;
        private n_PricesTableAdapter pricesTableAdaptor;
        
        private Menu dataSet;

        private MenuConnectionStringContext connectionStringContext;

        public AccessMenuDataSetService(WorkContextWrapper workContextWrapper, IMyAndromedaLogger logger) 
        {
            this.logger = logger;
            if (!workContextWrapper.Available)
            {
                return;
            }
            if(!workContextWrapper.Current.CurrentSite.Available)
            {
                return;
            }

            var andromedaSiteId = workContextWrapper.Current.CurrentSite.AndromediaSiteId;

            this.SetupWithAndromedaSiteId(andromedaSiteId);
        }

        public int? AndromedaSiteId
        {
            get;
            private set;
        }

        public void SetupWithAndromedaSiteId(int andromedaSiteId)
        {
            this.AndromedaSiteId = andromedaSiteId;
            this.connectionStringContext = new MenuConnectionStringContext(andromedaSiteId);
            this.Setup();
        }

        private void Setup() 
        {
            //cleanup existing items? 
            this.Dispose();
            //create 
            this.dataSet = new Menu();

            //reusing a connection seems to break things .. ergo new connections.
            this.menuItemTableAdaptor = new n_MenuTableAdapter() { Connection = new System.Data.OleDb.OleDbConnection(connectionStringContext.ConnectionString) };
            this.menuStructureTableAdaptor = new t_MenuTableAdapter() { Connection = new System.Data.OleDb.OleDbConnection(connectionStringContext.ConnectionString) };
            this.pricesTableAdaptor = new n_PricesTableAdapter() { Connection = new System.Data.OleDb.OleDbConnection(connectionStringContext.ConnectionString) };
            //this.menuItemPricesAdaptor = new Models.


            this.SetupPricesUpdateStatement();
            this.SetMenuStructureUpdateStatement();        
        }

        private void SetMenuStructureUpdateStatement() 
        {
            const string MenuUpdateStatement = @"
                  UPDATE  t_Menu
                  SET     [n_product price] = @webPrice, n_TakeAwayPrice = @takeawayPrice, n_EatinPrice = @eatInPrice
                  WHERE   (t_Menu.n_ProductID = @itemId)";

            this.menuStructureTableAdaptor.Adapter.UpdateCommand = new System.Data.OleDb.OleDbCommand(MenuUpdateStatement, this.menuItemTableAdaptor.Connection);

            var mp = this.menuStructureTableAdaptor.Adapter.UpdateCommand;
            mp.Parameters.Add("@webPrice", System.Data.OleDb.OleDbType.Integer, 12, "n_product price");
            mp.Parameters.Add("@takeawayPrice", System.Data.OleDb.OleDbType.Integer, 12, "n_TakeAwayPrice");
            mp.Parameters.Add("@eatInPrice", System.Data.OleDb.OleDbType.Integer, 12, "n_EatinPrice");
            mp.Parameters.Add("@itemId", System.Data.OleDb.OleDbType.Integer, 12, "n_ProductID");
        }

        private void SetupPricesUpdateStatement()
        {
            const string PriceUpdate = @"
                UPDATE  n_Prices
                SET     nPrice = @nPrice
                WHERE   (n_Prices.nPriceID = @nPriceID) AND (n_Prices.nMenuID = @nMenuID)";

            var priceUpdateCommand = this.pricesTableAdaptor.Adapter.UpdateCommand = new System.Data.OleDb.OleDbCommand(PriceUpdate, this.pricesTableAdaptor.Connection);

            priceUpdateCommand.Parameters.Add("@nPrice", System.Data.OleDb.OleDbType.Integer, 12, "nPrice");
            priceUpdateCommand.Parameters.Add("@nPriceID", System.Data.OleDb.OleDbType.Integer, 12, "nPriceID");
            priceUpdateCommand.Parameters.Add("@nMenuID", System.Data.OleDb.OleDbType.Integer, 12, "nMenuID");
        }

        private readonly string priceSelectOld = @"SELECT [nMenuID], [nPriceID], [nTaxCode], [nTaxType], [nAvailable], [nStoreID], [nPrice] FROM [n_Prices]";
        private readonly string priceSelectNew = @"SELECT [nMenuID], [nPriceID], [nTaxCode], [nTaxType], [nAvailable], [nStoreID], [nPrice], [nTaxBand] FROM [n_Prices]";
        
        private void InitDataSet() 
        {


            if (this.dataSet.n_Menu.Count == 0)
            {
                this.menuItemTableAdaptor.Fill(dataSet.n_Menu);
            }
            if (this.dataSet.t_Menu.Count == 0)
            {
                this.menuStructureTableAdaptor.Fill(dataSet.t_Menu);
            }

            if (this.dataSet.n_Prices.Count == 0)
            {
                bool exhaused = false;
                while (!exhaused)
                { 
                    try 
                    {
                        this.pricesTableAdaptor.Adapter.SelectCommand = new System.Data.OleDb.OleDbCommand(priceSelectNew, this.pricesTableAdaptor.Connection);
                        this.pricesTableAdaptor.Fill(dataSet.n_Prices);
                        exhaused = true;
                    }
                    catch (Exception e) { this.logger.Error("try 1 of 2:"); this.logger.Error(e.Message); this.logger.Error(e.StackTrace); }

                    if (!exhaused) 
                    { 
                        try
                        {
                            this.pricesTableAdaptor.Adapter.SelectCommand = new System.Data.OleDb.OleDbCommand(priceSelectOld, this.pricesTableAdaptor.Connection);
                            this.pricesTableAdaptor.Fill(dataSet.n_Prices);
                            exhaused = true;
                        }
                        catch (Exception e) { this.logger.Error("try 2 of 2:"); this.logger.Error(e.Message); this.logger.Error(e.StackTrace); }
                    }

                    exhaused = true;
                }
            }
        }

        public void UpdatePrice(int menuItemId, int? inStore, int? delivery, int? collection)
        {
            var priceRows = this.ListPrices(e => e.nMenuID == menuItemId);
            var structureRows = this.ListStructure(e => e.n_ProductID == menuItemId);

            foreach (var row in priceRows)
            {
                switch (row.nPriceID)
                {
                    
                    case 1:
                        {
                            row.nPrice = collection.HasValue ? collection.Value : row.nPrice;
                            break;
                        }
                    case 2:
                        {
                            row.nPrice = delivery.HasValue ? delivery.Value : row.nPrice;
                            break;
                        }
                    default: //0
                        {
                            row.nPrice = delivery.HasValue ? delivery.GetValueOrDefault() : row.nPrice;
                            break;
                        }
                    
                }
            }

            foreach (var row in structureRows) 
            {
                row.n_product_price = delivery.HasValue ? delivery.Value : row.n_product_price;
                row.n_TakeAwayPrice = collection.HasValue ? collection.Value : row.n_TakeAwayPrice;
                row.n_EatinPrice = inStore.HasValue ? inStore.Value : row.n_EatinPrice;
            }

            //this.SaveChanges();
        }

        public PriceCollection GetPrices(int menuItemId)
        {
            var result = new PriceCollection() 
            {
                
            };

            var menuItem = this.List(e => e.nUID == menuItemId).Single();

            //menuItem.


            //var prices = menuItem.Getn_PricesRows();
            //var structure = menuItem.Gett_MenuRows();

            //foreach (var row in prices)
            //{
            //    if (row.nPriceID == 2)
            //    {
            //        result.InStore = row.Value(e=> e.nPrice, 0);
            //    }
            //    if (row.nPriceID == 1)
            //    {
            //        result.Collection = row.Value(e=> e.nPrice, 0);
            //    }
            //    if (row.nPriceID == 0)
            //    {
            //        result.Delivery = row.Value(e=> e.nPrice, 0);
            //    }
            //}

            //foreach (var row in structure) 
            //{
            //    if (row.n_EatinPrice > 0)
            //    {
            //        result.InStore = row.Value(e=> e.n_EatinPrice, 0);
            //    }
            //    if (row.n_TakeAwayPrice > 0)
            //    {
            //        result.Collection = row.Value(e=> e.n_TakeAwayPrice, 0);
            //    }
            //    if (row.n_product_price > 0)
            //    {
            //        result.Delivery = row.Value("n_product price", 0);
            //    }
            //}

            return result;
        }

        

        public void SaveChanges()
        {
            //using (var transaction = connection.BeginTransaction()) 
            {
                try
                {
                    this.menuItemTableAdaptor.Update(this.dataSet.n_Menu);
                    this.menuStructureTableAdaptor.Update(this.dataSet.t_Menu);
                    this.pricesTableAdaptor.Update(this.dataSet.n_Prices);
                      
                    this.dataSet.AcceptChanges();
                }
                catch (Exception e) 
                { 
                    var ex = e;
                    while (ex != null)
                    { 
                        this.logger.Error(e);
                        ex = ex.InnerException;
                    }
                    throw;
                }
            }
        }

        
        public IEnumerable<Menu.n_MenuRow> List(Func<Menu.n_MenuRow, bool> query = null)
        {
            this.InitDataSet();

            return query == null ? 
                   this.dataSet.n_Menu.Where(e => true) :
                   this.dataSet.n_Menu.Where(query);
        }

        public IEnumerable<Menu.n_PricesRow> List(Func<Menu.n_PricesRow, bool> query = null)
        {
            return this.ListPrices(query);
        }

        public IEnumerable<Menu.t_MenuRow> List(Func<Menu.t_MenuRow, bool> query = null)
        {
            return this.ListStructure(query);
        }

        public IEnumerable<Menu.n_PricesRow> ListPrices(Func<Menu.n_PricesRow, bool> query = null)
        {
            this.InitDataSet();

            return query == null ? 
                this.dataSet.n_Prices.Where(e => true) : 
                this.dataSet.n_Prices.Where(query);
        }

        public IEnumerable<Menu.t_MenuRow> ListStructure(Func<Menu.t_MenuRow, bool> query = null)
        {
            this.InitDataSet();

            return query == null ? 
                this.dataSet.t_Menu.Where(e => true) : 
                this.dataSet.t_Menu.Where(query);
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
                if (menuItemTableAdaptor != null)
                {
                    this.menuItemTableAdaptor.Dispose();
                }
                if (this.menuStructureTableAdaptor != null)
                {
                    this.menuStructureTableAdaptor.Dispose();
                }
                if (this.pricesTableAdaptor != null)
                {
                    this.pricesTableAdaptor.Dispose();
                }
                if (this.dataSet != null)
                {
                    this.dataSet.Dispose();
                }
            }
        }
    }
}