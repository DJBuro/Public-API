namespace AndroCloudDataAccessEntityFramework.Exceptions
{
    using System;

    internal class NotExistingEntityException : Exception
    {
        public NotExistingEntityException()
        {
        }

        public NotExistingEntityException(string message)
            : base(message)
        {
        }
    }
}
