using System;
using System.Collections.Generic;
using System.Linq;
using MyAndromeda.Data.MenuDatabase.Models.Database;
using MyAndromeda.Data.MenuDatabase.Models.Database.MenuTableAdapters;

namespace MyAndromeda.Data.MenuDatabase.Services
{
    public interface IAccessSecondaryCategoryDataService : IAccessMenuDatabaseQuery<Menu.n_SecondaryCatRow>
    {
    }

    public class SecondaryCategoryDataService : IAccessSecondaryCategoryDataService
    {
        private readonly n_SecondaryCatTableAdapter adaptor;
        private readonly Menu.n_SecondaryCatDataTable dataTable;

        public SecondaryCategoryDataService(n_SecondaryCatTableAdapter adaptor)
        {
            this.adaptor = adaptor;
            this.dataTable = new Menu.n_SecondaryCatDataTable();
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
                this.dataTable.Dispose();
                this.adaptor.Dispose();
            }
        }

        public IEnumerable<Menu.n_SecondaryCatRow> List(Func<Menu.n_SecondaryCatRow, bool> query)
        {
            adaptor.Fill(this.dataTable);

            return this.dataTable.Where(query);
        }

        public void SaveChanges()
        {
            this.dataTable.AcceptChanges();
        }
    }
}