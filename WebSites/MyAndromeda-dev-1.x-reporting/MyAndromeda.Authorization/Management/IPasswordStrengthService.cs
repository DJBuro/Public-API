using System;
using System.Linq;
using MyAndromeda.Core;
using System.Collections.Generic;
using MyAndromeda.Framework.Authorization;

namespace MyAndromeda.Authorization.Management
{
    public interface IPasswordStrengthService : IDependency
    {
        IEnumerable<string> InvalidPasswords { get; set; }


        IEnumerable<string> ProblemsWithPassword(string password);
    }

    public class PasswordStrengthService : IPasswordStrengthService
    {
        private readonly IMyAndromedaRegistrationSettings settings;

        private readonly IEnumerable<IPasswordStrengthEvent> passwordEvents;

        public PasswordStrengthService(IMyAndromedaRegistrationSettings settings, IEnumerable<IPasswordStrengthEvent> passwordEvents)
        {
            this.settings = settings;
            this.passwordEvents = passwordEvents;

        }

        public IEnumerable<string> InvalidPasswords
        {
            get;
            set;
        }

        public IEnumerable<string> ProblemsWithPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                yield return string.Format("Password is required");
                yield break;
            }

            if (password.Length < settings.MinRequiredPasswordLength)
            {
                yield return string.Format("The password must be {0} characters long", settings.MinRequiredPasswordLength);
            }

            var banned = Banned();
            if (banned.Any(word => word.Equals(password, StringComparison.InvariantCultureIgnoreCase)))
            {
                yield return string.Format("The password entered cannot be used. It is too common");
            }

            if (settings.MinRequiredNonAlphanumericCharacters > 0)
            {
                var test = password.Length - password.Count(char.IsLetterOrDigit);
                if (test < settings.MinRequiredNonAlphanumericCharacters)
                {
                    yield return string.Format("The password requires {0} non alphanumeric characters");
                }
            }
        }


        public string[] Banned()
        {
            return new[] {
                "1234",
                "12345",
                "123456",
                "123456789",
                "password",
                "pass",
                "pass1",
                "pass12",
                "pass123",
                "pass1234",
                "iloveyou",
                "princess",
                "rockyou",
                "1234567",
                "12345678",
                "abc123",
                "babygirl",
                "monkey",
                "lovely",
                "654321",
                "qwerty",
                "qwerty1234",
                "password1",
                "welcome",
                "welcome1",
                "password2",
                "password01",
                "password3",
                "p@ssw0rd",
                "passw0rd",
                "password4",
                "password123",
                "summer09",
                "password6",
                "password7",
                "password9",
                "password8",
                "welcome2",
                "welcome01",
                "winter12",
                "spring2012",
                "summer12",
                "summer2012",
                "password123456789",
                "password12345678910",
                "password1234567890"
            };
        }
    }

    public interface IPasswordStrengthEvent : ITransientDependency
    {
        void CheckingPassword(string password);
        void ValidPassword(string password);
        void InvalidPassword(string password);
    }
}