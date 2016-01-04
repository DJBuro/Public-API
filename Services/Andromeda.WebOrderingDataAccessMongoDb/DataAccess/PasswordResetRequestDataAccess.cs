using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Andromeda.WebOrderingDataAccess.DataAccess;
using Andromeda.WebOrderingDataAccess.Domain;
using Andromeda.WebOrderingDataAccessMongoDb.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Andromeda.WebOrderingDataAccessMongoDb.DataAccess
{
    public class PasswordResetRequestDataAccess : IPasswordResetRequest
    {
        public string ConnectionStringOverride { get; set; }

        public string Add(PasswordResetRequest passwordResetRequest)
        {
            PasswordResetRequestEntity passwordResetRequestEntity = new PasswordResetRequestEntity(passwordResetRequest);

            // Add the request
            MongoDatabase mongoDatabase = Helper.GetDatabase();
            MongoCollection collection = mongoDatabase.GetCollection<PasswordResetRequest>("PasswordResetRequest");
            WriteConcernResult writeConcernResult = collection.Insert(passwordResetRequestEntity);

            if (writeConcernResult.Ok != true || writeConcernResult.HasLastErrorMessage)
            {
                // Problem!
            }

            return "";
        }

        public string Get(string token, out PasswordResetRequest passwordResetRequest)
        {
            // Add the request
            MongoDatabase mongoDatabase = Helper.GetDatabase();
            MongoCollection collection = mongoDatabase.GetCollection<PasswordResetRequest>("PasswordResetRequest");

            var query = Query<PasswordResetRequestEntity>.EQ(e => e.Token, token);
            var entity = collection.FindOneAs<PasswordResetRequestEntity>(query);

            passwordResetRequest = entity.GetPasswordResetRequest();

            return "";
        }
    }
}
