namespace MyAndromeda.SendGridService.Models
{
    public static class SendGridEventTypes 
    {
        public const string Processed = "Processed";
        public const string Dropped = "Dropped";
        public const string Delivered = "Delivered";
        public const string Deferred = "Deferred";
        public const string Bounce = "Bounce";
        public const string Open = "Open";
        public const string Click = "Click";
        public const string SpamReport = "Spam Report";
        public const string Unsubscribe = "Unsubscribe";
        public const string GroupUnsubscribe = "Group Unsubscribe";
        public const string GroupResubscribe = "Group Resubscribe";
    }
}