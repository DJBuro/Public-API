using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Management.Smo;

namespace AndroCloudServicesTests
{
    public class DatabaseHelper
    {
        private Server myServer = null;

        public void BackupDatabase()
        {
            this.myServer = new Server(@"10.0.0.39");
            //Using windows authentication
            myServer.ConnectionContext.LoginSecure = false;
            myServer.ConnectionContext.Login = "IntegrationTestUser";
            myServer.ConnectionContext.Password = "IntegrationTestUser";
            myServer.ConnectionContext.Connect();            

            Backup bkpDBFull = new Backup();
            /* Specify whether you want to back up database or files or log */
            bkpDBFull.Action = BackupActionType.Database;
            /* Specify the name of the database to back up */
            bkpDBFull.Database = "ACS";
            /* You can take backup on several media type (disk or tape), here I am
             * using File type and storing backup on the file system */
            bkpDBFull.Devices.AddDevice(@"C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\Backup\acstest.bak", DeviceType.File);
            bkpDBFull.BackupSetName = "ACS test backup";
            bkpDBFull.BackupSetDescription = "ACS test backup";
            /* You can specify the expiration date for your backup data
             * after that date backup data would not be relevant */
            bkpDBFull.ExpirationDate = DateTime.Today.AddDays(10);

            /* You can specify Initialize = false (default) to create a new 
             * backup set which will be appended as last backup set on the media. You
             * can specify Initialize = true to make the backup as first set on the
             * medium and to overwrite any other existing backup sets if the all the
             * backup sets have expired and specified backup set name matches with
             * the name on the medium */
            bkpDBFull.Initialize = true;

            /* Wiring up events for progress monitoring */
            bkpDBFull.Complete += new Microsoft.SqlServer.Management.Common.ServerMessageEventHandler(bkpDBFull_Complete);

            /* SqlBackup method starts to take back up
             * You can also use SqlBackupAsync method to perform the backup 
             * operation asynchronously */
            bkpDBFull.SqlBackup(this.myServer);

            
        }

        void bkpDBFull_Complete(object sender, Microsoft.SqlServer.Management.Common.ServerMessageEventArgs e)
        {
            try
            {
                myServer.KillAllProcesses("ACSTest");
            }
            catch { }

            Restore restoreDB = new Restore();
            restoreDB.Database = "ACSTest";
            /* Specify whether you want to restore database or files or log etc */
            restoreDB.Action = RestoreActionType.Database;
            restoreDB.Devices.AddDevice(@"C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\Backup\acstest.bak", DeviceType.File);

            /* You can specify ReplaceDatabase = false (default) to not create a new
             * database, the specified database must exist on SQL Server instance.
             * You can specify ReplaceDatabase = true to create new database 
             * regardless of the existence of specified database */
            restoreDB.ReplaceDatabase = true;

            /* If you have a differential or log restore to be followed, you would 
             * specify NoRecovery = true, this will ensure no recovery is done
             * after the restore and subsequent restores are completed. The database
             * would be in a recovered state. */
            restoreDB.NoRecovery = false;

            /* RelocateFiles collection allows you to specify the logical file names
             * and physical file names (new locations) if you want to restore to a
             * different location.*/
            restoreDB.RelocateFiles.Add(new RelocateFile("ACS", @"C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\ACSTest_Data.mdf"));
            restoreDB.RelocateFiles.Add(new RelocateFile("ACS_log", @"C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\ACSTest_Log.ldf"));

            /* Wiring up events for progress monitoring */
            restoreDB.Complete += new Microsoft.SqlServer.Management.Common.ServerMessageEventHandler(restoreDB_Complete);

            /* SqlRestore method starts to restore database
             * You can also use SqlRestoreAsync method to perform restore 
             * operation asynchronously */
            restoreDB.SqlRestore(myServer);
        }

        void restoreDB_Complete(object sender, Microsoft.SqlServer.Management.Common.ServerMessageEventArgs e)
        {
            if (this.myServer.ConnectionContext.IsOpen)
            {
                this.myServer.ConnectionContext.Disconnect();
            }
        }
    }
}
