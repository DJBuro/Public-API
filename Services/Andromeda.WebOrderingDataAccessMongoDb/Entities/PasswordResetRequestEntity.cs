using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Andromeda.WebOrderingDataAccess.Domain;
using MongoDB.Bson;

namespace Andromeda.WebOrderingDataAccessMongoDb.Entities
{
    public class PasswordResetRequestEntity
    {
        public PasswordResetRequestEntity(PasswordResetRequest passwordResetRequest)
        {
            ObjectId objectId;
            if (ObjectId.TryParse(passwordResetRequest.Id, out objectId))
            {
                this.Id = objectId;
            }

            this.Username = passwordResetRequest.Username;
            this.Token = passwordResetRequest.Token;
            this.RequestedDateTime = passwordResetRequest.RequestedDateTime;
        }

        public ObjectId Id { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
        public DateTime RequestedDateTime { get; set; }

        public PasswordResetRequest GetPasswordResetRequest()
        {
            return new PasswordResetRequest()
            {
                Id = this.Id.ToString(),
                RequestedDateTime = this.RequestedDateTime,
                Token = this.Token,
                Username = this.Username
            };
        }
    }
}
