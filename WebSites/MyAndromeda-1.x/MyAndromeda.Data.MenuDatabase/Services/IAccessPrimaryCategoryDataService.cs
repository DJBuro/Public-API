using System;
using System.Collections.Generic;
using System.Linq;
using MyAndromeda.Data.MenuDatabase.Models.Database.MenuTableAdapters;
using MyAndromeda.Data.MenuDatabase.Models.Database;

namespace MyAndromeda.Data.MenuDatabase.Services
{
    //public interface IDisplayCategoryDataService : IMenuDatabaseQuery<Menu.displ>
    //{
    //}
    public interface IAccessPrimaryCategoryDataService: IAccessMenuDatabaseQuery<Menu.n_PrimaryCatRow>
    {
    }

    public class PrimaryCategoriesDataService : IAccessPrimaryCategoryDataService
    {
        private readonly n_PrimaryCatTableAdapter adaptor;
        private readonly Menu.n_PrimaryCatDataTable dataTable;

        public PrimaryCategoriesDataService(n_PrimaryCatTableAdapter adaptor) 
        {
            this.adaptor = adaptor;
            this.dataTable = new Menu.n_PrimaryCatDataTable();
        }

        public IEnumerable<Menu.n_PrimaryCatRow> List(Func<Menu.n_PrimaryCatRow, bool> query) 
        {
            if (dataTable.Count == 0) { this.adaptor.Fill(dataTable); }

            return this.dataTable.Where(query);
        }

        public void SaveChanges() {
            //this.dataTable.AcceptChanges();
            this.adaptor.Update(this.dataTable);
        }

        public void Dispose()
        {
            this.dataTable.Dispose();
            this.adaptor.Dispose();
        }
    }

}
