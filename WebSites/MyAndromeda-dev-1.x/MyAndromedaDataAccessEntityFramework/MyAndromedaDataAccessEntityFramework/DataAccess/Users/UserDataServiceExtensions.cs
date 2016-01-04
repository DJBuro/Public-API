using MyAndromeda.Core.User;
using MyAndromeda.Data.Model.MyAndromeda;

namespace MyAndromedaDataAccessEntityFramework.DataAccess.Users
{
    public static class UserDataServiceExtensions 
    {
        public static MyAndromedaUser ToDomain(this UserRecord user) 
        {
            var domainModel = new MyAndromedaUser()
            {
                Id = user.Id,
                Firstname = user.FirstName,
                Surname = user.LastName,
                Username = user.Username,
                Enabled = user.IsEnabled
            };

            return domainModel;
        }
    }
}