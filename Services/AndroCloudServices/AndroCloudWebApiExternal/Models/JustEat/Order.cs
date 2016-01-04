using System;
namespace AndroCloudWebApiExternal.Models.JustEat
{
    public class Order
    {
        public string NoteToRestaurant { get; set; }

        public string ServiceType { get; set; }

        public DateTime PlacedDate { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime DueDateWithUtcOffset { get; set; }

        public DateTime InitialDueDate { get; set; }

        public DateTime InitialDueDateWithUtcOffset { get; set; }

        public bool PromptAsap { get; set; }
    }
}