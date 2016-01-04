using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAndromeda.Data.MenuDatabase.Models.Database.MenuTableAdapters;
using MyAndromeda.Data.MenuDatabase.Models.Database;

namespace MyAndromeda.Data.MenuDatabase.Services
{
    public interface IQuickMenuDataService : IAccessMenuDatabaseQuery<Menu.QuickMenuRow>
    {
    
    }

    public class QuickMenuDataService : IQuickMenuDataService
    {
        private readonly QuickMenuTableAdapter adaptor;
        private readonly Menu.QuickMenuDataTable dataTable;

        public QuickMenuDataService(QuickMenuTableAdapter adaptor) 
        {
            this.adaptor = adaptor;
            this.dataTable = new Menu.QuickMenuDataTable();
        }


        public IEnumerable<Menu.QuickMenuRow> List(Func<Menu.QuickMenuRow, bool> query = null)
        {
            if (this.dataTable.Count == 0) 
            {
                this.adaptor.Fill(dataTable);
            }

            return this.dataTable.Where(query);
        }

        public void SaveChanges()
        {
            // TODO: Implement this method
            throw new NotImplementedException();
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
    }
}
