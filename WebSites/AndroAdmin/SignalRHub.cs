using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Web;
using AndroAdmin.Helpers;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using AndroAdmin.Helpers.ACSDiagnosticTests;

namespace AndroAdmin
{
    public class MenuSync : Hub
    {
        //public string cloud1ConnectionString = "Data Source=54.247.90.227;Initial Catalog=ACS;Persist Security Info=True;User ID=ACSUser;Password=Pass123;MultipleActiveResultSets=True;Application Name=EntityFramework";
        //public string cloud2ConnectionString = "Data Source=dmel6muo0u.database.windows.net;Initial Catalog=ACS;Persist Security Info=True;User ID=andromeda;Password=h6ruh7kS2#4;MultipleActiveResultSets=True;Application Name=EntityFramework";
        //public string cloud2ConnectionString = "data source=ROBDEVVM\\SQL2012;initial catalog=ACS;persist security info=True;user id=acsuser;password=Pass123;multipleactiveresultsets=True;App=EntityFramework";

        private static ACSDiagnosticTests acsDiagnosticTests = null;

        public void StartMenuSync()
        {
            try
            {
                int menuCount = 0;

                Clients.All.message("Getting menu list from cloud1");
                Dictionary<int, Dictionary<string, MenuDetails>> cloud1menus = this.GetMenuList(ConfigurationManager.ConnectionStrings["MenuSyncSource"].ConnectionString, out menuCount);

                Clients.All.message(menuCount.ToString() + " menus retrieved from cloud1");

                Clients.All.message("Getting menu list from cloud2");
                Dictionary<int, Dictionary<string, MenuDetails>> cloud2menus = this.GetMenuList(ConfigurationManager.ConnectionStrings["MenuSyncTarget"].ConnectionString, out menuCount);

                Clients.All.message(menuCount.ToString() + " menus retrieved from cloud2");

                Clients.All.message("Analysing differences between cloud1 and cloud2");

                int processedMenus = 0;
                int missingMenus = 0;
                int outOfDateMenus = 0;
                int uptoDateMenus = 0;
                int noMenuOnCloud1 = 0;

                List<MenuDetails> uploadMenus = new List<MenuDetails>();

                // Check the menus for each store
                foreach (KeyValuePair<int, Dictionary<string, MenuDetails>> cloud1Store in cloud1menus)
                {
                    // Does the store exist in cloud2?
                    Dictionary<string, MenuDetails> cloud2Store = null;
                    if (cloud2menus.TryGetValue(cloud1Store.Key, out cloud2Store))
                    {
                        // Check that all the menus in cloud1 are in cloud2 for this store
                        foreach (KeyValuePair<string, MenuDetails> cloud1Menu in cloud1Store.Value)
                        {
                            processedMenus++;

                            // Does the menu exist in cloud2 (key is XML or JSON)?
                            MenuDetails cloud2MenuDetails = null;
                            if (cloud2Store.TryGetValue(cloud1Menu.Key, out cloud2MenuDetails))
                            {
                                // Is there a menu on cloud1?
                                if (cloud1Menu.Value.LastUpdated.HasValue)
                                {
                                    // Is there a menu on cloud2?
                                    if (cloud2MenuDetails.LastUpdated.HasValue)
                                    {
                                        // Do the menus have the same last uploaded datetime?
                                        if (cloud1Menu.Value.LastUpdated.Value.Year == cloud2MenuDetails.LastUpdated.Value.Year &&
                                            cloud1Menu.Value.LastUpdated.Value.Month == cloud2MenuDetails.LastUpdated.Value.Month &&
                                            cloud1Menu.Value.LastUpdated.Value.Day == cloud2MenuDetails.LastUpdated.Value.Day &&
                                            cloud1Menu.Value.LastUpdated.Value.Hour == cloud2MenuDetails.LastUpdated.Value.Hour &&
                                            cloud1Menu.Value.LastUpdated.Value.Minute == cloud2MenuDetails.LastUpdated.Value.Minute)
                                        {
                                            uptoDateMenus++;
                                        }
                                        else
                                        {
                                            outOfDateMenus++;
                                            uploadMenus.Add(cloud1Menu.Value);
                                        }
                                    }
                                    else
                                    {
                                        missingMenus++;
                                        uploadMenus.Add(cloud1Menu.Value);
                                    }
                                }
                                else
                                {
                                    noMenuOnCloud1++;
                                }
                            }
                            else
                            {
                                missingMenus++;
                                uploadMenus.Add(cloud1Menu.Value);
                            }
                        }
                    }
                    else
                    {
                        foreach (MenuDetails menuDetails in cloud1Store.Value.Values)
                        {
                            processedMenus++;
                            missingMenus++;

                            //missingMenus += cloud1Store.Value.Count;

                            uploadMenus.Add(menuDetails);
                        }
                    }
                }

                Clients.All.message(processedMenus + " cloud1 menus analysed: " + uptoDateMenus + " cloud2 menus are upto date, " + noMenuOnCloud1 + " menus are not on cloud1, " + missingMenus + " menus are missing from cloud2, " + outOfDateMenus.ToString() + " cloud2 menus are out of date");

                Clients.All.message(uploadMenus.Count + " menus need updating, " + uptoDateMenus + " cloud2 menus are upto date, " + noMenuOnCloud1  + " menus are not on cloud1");

                // Do the menu sync
                this.SyncMenus(uploadMenus);
                
                // Fin
                Clients.All.message("Processing complete");
            }
            catch (Exception exception)
            {
                Clients.All.message("Failed with unhandled exception: " + exception.Message);
            }
        }

        private Dictionary<int, Dictionary<string, MenuDetails>> GetMenuList(string connectionString, out int menuCount)
        {
            menuCount = 0;
            string sql =
                "select s.android, sm.lastupdated, sm.MenuType, sm.version, s.ExternalSiteName " +
                "from sitemenus sm " +
                "inner join sites s " +
                "on sm.siteid = s.id " +
                "order by s.android";

            // Each store can (and should) have multiple menus (JSON & XML)
            Dictionary<int, Dictionary<string, MenuDetails>> menuList = new Dictionary<int, Dictionary<string, MenuDetails>>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        while (sqlDataReader.Read())
                        {
                            int storeId = sqlDataReader.GetInt32(0);

                            // Is there already a list of menus for this store?
                            Dictionary<string, MenuDetails> storeMenus = null;
                            if (!menuList.TryGetValue(storeId, out storeMenus))
                            {
                                storeMenus = new Dictionary<string, MenuDetails>();
                                menuList.Add(storeId, storeMenus);
                            }

                            // Add the menu to the list for this store
                            string menuType = sqlDataReader.GetString(2);
                            storeMenus.Add
                            (
                                menuType,
                                new MenuDetails()
                                {
                                    AndroId = sqlDataReader.GetInt32(0),
                                    LastUpdated = sqlDataReader.IsDBNull(1) ? null : (DateTime?)sqlDataReader.GetDateTime(1),
                                    MenuType = menuType,
                                    Version = sqlDataReader.GetInt32(3),
                                    StoreName = sqlDataReader.GetString(4)
                                }
                            );

                            menuCount++;
                        }
                    }
                }
            }

            return menuList;
        }

        private string GetMenuForStore(string connectionString, string menuType, int storeId)
        {
            string menu = "";

            string sql =
                "select sm.menuData " +
                "from sitemenus sm " +
                "inner join sites s " +
                "on sm.siteid = s.id " +
                "where sm.MenuType = '" + menuType + "' " +
                "and s.AndroId = " + storeId.ToString();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        if (sqlDataReader.Read())
                        {
                            menu = sqlDataReader.GetString(0);
                        }
                    }
                }
            }

            return menu;
        }

        private string UpsertMenuForStore(string connectionString, MenuDetails menuDetails, string menu)
        {
            string sql =
                "MERGE SiteMenus AS T " +
                "USING  " +
                "( " +
                "    select  " +
                "        ( " +
                "            select sitemenus.[id] as [id] " +
                "            from sites " +
                "            left join sitemenus " +
                "            on sites.[id] = sitemenus.[siteid] " +
                "            where sites.android = " + menuDetails.AndroId + "  " +
                "            and sitemenus.menutype = '" + menuDetails.MenuType + "' " +
                "        ) as sitemenusid, " +
                "        (select id from sites where android=" + menuDetails.AndroId + ") as [siteid],  " +
                "        " + menuDetails.Version + " as [Version],  " +
                "        '" + menuDetails.MenuType + "' as [MenuType], " +
                "        '" + menu + "' as [menudata], " +
                "        '" + (menuDetails.LastUpdated.HasValue ? menuDetails.LastUpdated.Value.ToString("yyyy-MM-dd HH:mm:ss") : DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")) + "' as [lastupdated] " +
                ") AS S " +
                "ON (T.[Id] = S.sitemenusid)  " +
                "WHEN NOT MATCHED BY TARGET " +
                "    THEN INSERT([SiteId], [Version], [MenuType], [menuData], [LastUpdated]) " +
                "    VALUES(S.[SiteId], S.[Version], S.[MenuType], S.[MenuData], S.[LastUpdated]) " +
                "WHEN MATCHED  " +
                "    THEN UPDATE SET T.[Version] = S.version, T.[menuData] = S.menudata, [LastUpdated] = S.lastupdated; ";

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    int rowAffected = sqlCommand.ExecuteNonQuery();
                }
            }

            return menu;
        }

        private void SyncMenus(List<MenuDetails> uploadMenus)
        {
            foreach (MenuDetails menuDetails in uploadMenus)
            {
                Clients.All.message("Getting " + menuDetails.MenuType + " menu for store " + menuDetails.AndroId + " from cloud1");

                // Get the menu from cloud1
                string menu = this.GetMenuForStore(ConfigurationManager.ConnectionStrings["MenuSyncSource"].ConnectionString, menuDetails.MenuType, menuDetails.AndroId);

                Clients.All.message("Updating " + menuDetails.MenuType + " menu for store " + menuDetails.AndroId + " / " + menuDetails.StoreName + " in cloud2");

                // Update the menu in cloud2
                this.UpsertMenuForStore(ConfigurationManager.ConnectionStrings["MenuSyncTarget"].ConnectionString, menuDetails, menu);
            }
        }

        public void StartACSDiagnosticTests()
        {            
            if (MenuSync.acsDiagnosticTests == null || acsDiagnosticTests.Completed)
            {
                MenuSync.acsDiagnosticTests = new ACSDiagnosticTests();

                lock (acsDiagnosticTests)
                {
                    //GlobalHost.ConnectionManager.GetHubContext<SignalRHub>
                    acsDiagnosticTests.Start();
                }
            }
            else
            {
                acsDiagnosticTests.HubConnectionContext.Clients.All.message(acsDiagnosticTests.messages);
            }
        }
    }

    public class MenuDetails
    {
        public string StoreName { get; set; }
        public int AndroId { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string MenuType { get; set; }
        public int Version { get; set; }
    }
}