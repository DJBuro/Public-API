using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Andromeda.WebOrderingDataAccessMongoDb
{
    public class Helper
    {
        public static MongoDatabase GetDatabase()
        {
            string mongoServer = ConfigurationManager.AppSettings["mongoServer"];
            string mongoDatabase = ConfigurationManager.AppSettings["mongoDatabase"];

            MongoClient mongoClient = new MongoClient(mongoServer);

            MongoServer server = mongoClient.GetServer();
            MongoDatabase database = server.GetDatabase(mongoDatabase);

            return database;
        }
    }
}
