using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace AndroAdminTests
{
    public class DatabaseHelper
    {
        public const string CreateAndroAdminDatabaseScriptFilename =
            @"C:\Andromeda Development Source\Rameses Web Components\DataAccess\AndroAdminDataAccess\EntityFramework\CreateAndroAdminDatabase.sql";

        public const string CreateAndroUsersDatabaseScriptFilename =
            @"C:\Andromeda Development Source\Rameses Web Components\DataAccess\AndroUsersDataAccess\EntityFramework\CreateAndroUsersDatabase.sql";
        public const string CreateAndroUsersDataScriptFilename =
            @"C:\Andromeda Development Source\Rameses Web Components\DataAccess\AndroUsersDataAccess\EntityFramework\CreateAndroUsersData.sql";

        private const string MasterConnectionString = "Server=ROBDEVVM\\SQL2012;Database=master;Trusted_Connection=True;";

        public static void DeleteTestDatabases()
        {
            // Create the database tables
            using (SqlConnection sqlConnection = new SqlConnection(DatabaseHelper.MasterConnectionString))
            {
                sqlConnection.Open();

                List<string> deleteDatabases = new List<string>();

                using (SqlCommand sqlCommand = new SqlCommand("SELECT name FROM sys.databases;", sqlConnection))
                {
                    // Run the SQL
                    SqlDataReader reader = sqlCommand.ExecuteReader();

                    while (reader.Read())
                    {
                        string databaseName = reader.GetString(0);

                        if (databaseName.StartsWith("AT_AA_"))
                        {
                            deleteDatabases.Add(databaseName);
                        }
                    }

                    reader.Close();
                }

                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    foreach (string deleteDatabase in deleteDatabases)
                    {
                        sqlCommand.CommandText = "drop database [" + deleteDatabase + "]";
                        sqlCommand.ExecuteNonQuery();
                    }
                }
            }
        }

        public static string CreateTestAndroUsersDatabase(string databaseScriptFilename, string dataScriptFilename)
        {
            // We create the database schema and data seperately because it's easier to script the SQL seperately in SQL Server management studio

            // HOW TO GENERATE TEST DATABASE SCHEMA
            // To generate the database schema run SQL Server management studio 2012 in the tree view on the left open databases and right click on the "AndroUsers" database.
            // On the context menu select "Tasks" and then "Generate Scripts".  
            // Select "Select specific database objects" and then tick all the tables.
            // Click next then select the "Save to new query window" radio button and click next through to the end.
            // Cut and paste the generated SQL into the "CreateAndroUsersDatabase.sql" file in the "AndroUsersDataAccess" project.
            // The first two lines should be "USE [AndroUsers]" and "GO".  Delete them. Job done!

            // HOW TO GENERATE TEST DATABASE INITIAL DATA
            // To generate the initial data run SQL Server management studio 2012 in the tree view on the left open databases and right click on the "AndroUsers" database.
            // On the context menu select "Tasks" and then "Generate Scripts".  
            // Select "Select specific database objects" and then tick only the tables that contain the required data.  
            // At the time of writing this would be the "dbo.Permission" table.
            // Click next then click the "Advanced" button on the top right
            // Scroll to the bottom and look for "Types of data to script".  Change this to "Data only" and then click OK.
            // select the "Save to new query window" radio button and click next through to the end.
            // Cut and paste the generated SQL into the "CreateAndroUsersData.sql" file in the "AndroUsersDataAccess" project.
            // The first two lines should be "USE [AndroUsers]" and "GO".  Delete them. Job done!

            // Create a unique database name.  We can automatically delete test databases as they all start AT_ (Automated Test)
            string databaseName = "AT_AA_" + Guid.NewGuid().ToString().Replace("{", "").Replace("}", "").Replace("-", "");
            string connectionString = "Server=ROBDEVVM\\SQL2012;Database=" + databaseName + ";Trusted_Connection=True;";
            string efConnectionString = "metadata=res://*/EntityFramework.EntityModel.csdl|res://*/EntityFramework.EntityModel.ssdl|res://*/EntityFramework.EntityModel.msl;provider=System.Data.SqlClient;provider connection string='data source=ROBDEVVM\\SQL2012;initial catalog=" + databaseName + ";persist security info=True;Trusted_Connection=True;multipleactiveresultsets=True;application name=EntityFramework'";

            // Get the SQL that we need to run
            string createDatabaseSql = "";
            using (StreamReader streamReader = new StreamReader(databaseScriptFilename))
            {
                createDatabaseSql = streamReader.ReadToEnd();
            }

            string createDataSql = "";
            using (StreamReader streamReader = new StreamReader(dataScriptFilename))
            {
                createDataSql = streamReader.ReadToEnd();
            }

            // Create a blank database
            using (SqlConnection sqlConnection = new SqlConnection(DatabaseHelper.MasterConnectionString))
            {
                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand(createDatabaseSql, sqlConnection))
                {
                    // Create a blank database
                    sqlCommand.CommandText = "create database [" + databaseName + "]";

                    // Run the SQL
                    sqlCommand.ExecuteNonQuery();
                }
            }

            // Create the database tables
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                // Create the database schema

                // SQL management studio generates databse schema scripts with GOs but they are not valid SQL statements
                // Replace all GOs with semi colons
                createDatabaseSql = createDatabaseSql.Replace("GO", ";");

                // Make sure the script doesn't have any usings
                createDatabaseSql = createDatabaseSql.Replace("USE [AndroUsers]\r\n;", "");

                using (SqlCommand sqlCommand = new SqlCommand(createDatabaseSql, sqlConnection))
                {
                    // Run the SQL
                    sqlCommand.ExecuteNonQuery();
                }
                
                // Insert the initial data

                // Replace all GOs with semi colons
                createDataSql = createDataSql.Replace("GO", ";");

                using (SqlCommand sqlCommand = new SqlCommand(createDataSql, sqlConnection))
                {
                    // Run the SQL
                    sqlCommand.ExecuteNonQuery();
                }
            }

            return connectionString;
        }
    }
}
