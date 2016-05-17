using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;
using MyAndromeda.Data.Model.MyAndromeda;
using MyAndromeda.Logging;
using MyAndromeda.Framework.Translation;
using MyAndromeda.Core.User.Events;
using MyAndromeda.Core.User;
using MyAndromeda.Data.DataAccess.Users;
using System.Web;


namespace MyAndromeda.Framework.Services
{
    public class MembershipService : IMembershipService
    {
        private const string HashAlgorithmType = "SHA1";

        private readonly IEnumerable<IUserEventHandler> userEventHandlers;
        private readonly IEnumerable<IUserUpdatedEvent> userUpdatedEvents;
        private readonly IUserDataService userDataService;

        private readonly ITranslator translator;

        public MembershipService(IEnumerable<IUserEventHandler> userEventHandlers,
            IEnumerable<IUserUpdatedEvent> userUpdatedEvents,
            IMyAndromedaLogger logger,
            IUserDataService userDataService,
            ITranslator translator)
        {
            this.userEventHandlers = userEventHandlers;
            this.Logger = logger;
            this.userDataService = userDataService;
            this.userUpdatedEvents = userUpdatedEvents;
            this.translator = translator;
        }

        public IMyAndromedaLogger Logger { get; private set; }
        
        public MyAndromedaUser CreateUser(MyAndromedaUser user, string password)
        {
            this.Logger.Debug("CreateUser {0}", user.Username);

            var entity = this.userDataService.New();
            
            entity.Username = user.Username;
            entity.FirstName = user.Firstname;
            entity.LastName = user.Surname;
            entity.HashAlgorithm = HashAlgorithmType;
            entity.PasswordFormat = MembershipPasswordFormat.Hashed.ToString();
            entity.IsEnabled = true;

            this.SetPassword(entity, password, false);

            var userContext = new UserEventContext { User = user, Cancel = false };
            foreach (var userEventHandler in this.userEventHandlers)
            {
                userEventHandler.Creating(userContext);
            }

            if (userContext.Cancel)
            {
                return null;
            }

            this.userDataService.Add(entity);
            user.Id = entity.Id;

            foreach (var userEventHandler in this.userEventHandlers)
            {
                userEventHandler.Created(userContext);
            }

            return user;
        }

        public MyAndromedaUser UpdateUser(MyAndromedaUser user)
        {
            this.Logger.Debug("UpdateUser {0}", user.Username);

            var entity = this.userDataService.GetByUserId(user.Id);
            entity.Username = user.Username;
            entity.FirstName = user.Firstname;
            entity.LastName = user.Surname;
            //entity.IsEnabled = true;

            var userContext = new UserEditContext(user, false);
            Exception ex = new Exception();
            this.userUpdatedEvents.Each(i => i.UserModifying(userContext), i=>i.GetBaseException());
            
            //foreach (var userUpdatedEvent in this.userUpdatedEvents)
            //{
            //    userUpdatedEvent.UserModifying(userContext);
            //}
            if (userContext.Cancel)
            {
                return null;
            }

            this.userDataService.Update(entity);

            this.userUpdatedEvents.Each(i => i.UserModified(userContext), i => i.GetBaseException());
            //foreach (var userUpdatedEvent in this.userUpdatedEvents)
            //{
            //    userUpdatedEvent.UserModified(userContext);
            //}
            return user;
        }

        public MyAndromedaUser GetUser(string username)
        {
            var user = this.userDataService.Query(entity => entity.Username == username).SingleOrDefault();

            return user.ToDomainModel();
        }

        public UserRecord GetUserRecord(string username)
        {
            var user = this.userDataService.Query(entity => entity.Username == username).SingleOrDefault();

            return user;
        }

        public MyAndromedaUser ValidateUserLogin(string userNameOrEmail, string password)
        {
            var user = this.userDataService
                .Query(entity => entity.Username == userNameOrEmail)
                .Where(e=> e.IsEnabled)
                .SingleOrDefault(); 
            
            if (user == null)
                return null;

            if (!this.ValidatePassword(user, password)) 
                return null;

            var context = new UserEventContext()
            {
                User = user.ToDomainModel()
            };

            foreach (var userEvent in this.userEventHandlers) 
            {
                userEvent.Login(context); 
            }

            if (context.Cancel) 
            {
                return null;
            }

            return user.ToDomainModel();
        }

        public bool SetPassword(UserRecord entity, string password, bool saveChanges = true)
        {
            var context = new UserEventContext()
            {
                User = entity.ToDomainModel()
            };

            foreach (var userEvent in this.userEventHandlers)
            {
                userEvent.ChangingPassword(context);
            }

            if (context.Cancel)
                return false;

            if (string.IsNullOrWhiteSpace(entity.PasswordFormat)) 
            {
                entity.PasswordFormat = MembershipPasswordFormat.Hashed.ToString();
                entity.HashAlgorithm = HashAlgorithmType;
            }

            if (entity.PasswordFormat.Equals(MembershipPasswordFormat.Clear.ToString(), StringComparison.InvariantCultureIgnoreCase))
            { 
                SetPasswordClear(entity, password);

                if (saveChanges) 
                { 
                    this.userDataService.Update(entity);
                }

                return true;
            }

            if (entity.PasswordFormat.Equals(MembershipPasswordFormat.Hashed.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                SetPasswordHashed(entity, password);

                if (saveChanges)
                {
                    this.userDataService.Update(entity);
                }

                return true;
            }

            throw new ApplicationException("Unexpected password format value");
        }

        public void ValidateUserModel(MyAndromedaUser model, ModelStateDictionary modelState, bool isUpdate)
        {
            if (isUpdate)
                ValidateUserModelOnUpdate(model, modelState);
            else
            {
                if (this.userDataService.GetByUserName(model.Username) != null)
                {
                    modelState.AddModelError("UserName", translator.T("This username already exists."));
                    return;
                }
            }
        }

        public void ValidateUserModelOnUpdate(MyAndromedaUser model, ModelStateDictionary modelState)
        {
            MyAndromedaUser userInDB = this.userDataService.GetByUserName(model.Username);
            if (this.userDataService.GetByUserName(model.Username) != null && model.Id != userInDB.Id)
            {
                modelState.AddModelError("UserName", translator.T("This username already exists."));
                return;
            }
        }

        public void ValidatePassword(string password, ModelStateDictionary modelState) 
        {
            if(string.IsNullOrWhiteSpace(password))
            {
                modelState.AddModelError("Password", translator.T("The password is missing!"));
            }

            if (!string.IsNullOrWhiteSpace(password))
            {
                var min = Membership.MinRequiredPasswordLength;
                if (password.Length < min)
                {
                    modelState.AddModelError("Password", translator.T("The password does not meet the minimum required length: {0}", min));
                }
            }
        }

        private static void SetPasswordClear(UserRecord partRecord, string password)
        {
            partRecord.PasswordFormat = MembershipPasswordFormat.Clear.ToString();
            partRecord.Password = password;
            partRecord.PasswordSalt = null;
        }

        private static bool ValidatePasswordClear(UserRecord partRecord, string password)
        {
            return partRecord.Password == password;
        }

        private static void SetPasswordHashed(UserRecord entity, string password)
        {
            var saltBytes = new byte[0x10];
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetBytes(saltBytes);
            }

            var passwordBytes = Encoding.Unicode.GetBytes(password);

            var combinedBytes = saltBytes.Concat(passwordBytes).ToArray();

            byte[] hashBytes;
            using (var hashAlgorithm = HashAlgorithm.Create(entity.HashAlgorithm))
            {
                hashBytes = hashAlgorithm.ComputeHash(combinedBytes);
            }

            entity.PasswordFormat = MembershipPasswordFormat.Hashed.ToString();
            entity.Password = Convert.ToBase64String(hashBytes);
            entity.PasswordSalt = Convert.ToBase64String(saltBytes);
        }

        private static bool ValidatePasswordHashed(UserRecord partRecord, string password)
        {
            var saltBytes = Convert.FromBase64String(partRecord.PasswordSalt);

            var passwordBytes = Encoding.Unicode.GetBytes(password);

            var combinedBytes = saltBytes.Concat(passwordBytes).ToArray();

            byte[] hashBytes;
            using (var hashAlgorithm = HashAlgorithm.Create(partRecord.HashAlgorithm))
            {
                hashBytes = hashAlgorithm.ComputeHash(combinedBytes);
            }

            return partRecord.Password == Convert.ToBase64String(hashBytes);
        }

        private bool ValidatePassword(UserRecord entity, string password)
        {
            // Note - the password format stored with the record is used
            // otherwise changing the password format on the site would invalidate
            // all logins
            if (string.IsNullOrWhiteSpace(entity.HashAlgorithm)) 
            {
                return ValidatePasswordClear(entity, password);
            }

            if (entity.PasswordFormat.Equals(MembershipPasswordFormat.Clear.ToString(), StringComparison.InvariantCultureIgnoreCase))
                return ValidatePasswordClear(entity, password);
            if (entity.PasswordFormat.Equals(MembershipPasswordFormat.Hashed.ToString(), StringComparison.InvariantCultureIgnoreCase))
                return ValidatePasswordHashed(entity, password);

            throw new ApplicationException("Unexpected password format value");
        }
    }
}