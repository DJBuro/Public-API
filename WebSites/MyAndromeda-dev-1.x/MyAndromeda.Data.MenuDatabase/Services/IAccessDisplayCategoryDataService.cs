using System;
using System.Collections.Generic;
using System.Linq;
using MyAndromeda.Data.MenuDatabase.Models.Database.MenuTableAdapters;
using MyAndromeda.Data.MenuDatabase.Models.Database;

namespace MyAndromeda.Data.MenuDatabase.Services
{
    public interface IAccessDisplayCategoryDataService : IAccessMenuDatabaseQuery<Menu.w_MenuSectionsRow>
    {
    }

    public class MenuSectionDataService : IAccessDisplayCategoryDataService
    {
        private readonly w_MenuSectionsTableAdapter adaptor;
        private readonly Menu.w_MenuSectionsDataTable dataTable;

        public MenuSectionDataService(w_MenuSectionsTableAdapter adaptor)
        {
            this.adaptor = adaptor;
            this.dataTable = new Menu.w_MenuSectionsDataTable();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(true);
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

        public IEnumerable<Menu.w_MenuSectionsRow> List(Func<Menu.w_MenuSectionsRow, bool> query)
        {
            if (dataTable.Count == 0) { this.adaptor.Fill(dataTable); }

            return query == null ?
                this.dataTable.Where(e => true) :
                this.dataTable.Where(query);
        }

        public void SaveChanges()
        {
            this.adaptor.Update(this.dataTable);
        }
    }
}