using System;
using System.Collections.Generic;
using System.Linq;
using MyAndromeda.Data.MenuDatabase.Models.Database;
using MyAndromeda.Data.MenuDatabase.Models.Database.MenuTableAdapters;
using MyAndromeda.Data.MenuDatabase.Context;
using MyAndromeda.Logging;
using System.Data.OleDb;

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

}
