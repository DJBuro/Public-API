using System;

namespace MyAndromeda.Menus.Events
{
    public class DatabaseUpdatedEventContext 
    {
        public DatabaseUpdatedEventContext(int andromedaId)
        {
            this.AndromedaId = andromedaId;
            this.TimeStamp = DateTime.UtcNow;
        }

        public DatabaseUpdatedEventContext(string group) 
        {
            this.Group = group;
            this.TimeStamp = DateTime.UtcNow;
        }

        public int AndromedaId { get; private set; }

        public string Group { get; private set; }

        public string Version { get; set; }

        public DateTime TimeStamp { get; private set; }
    }
}