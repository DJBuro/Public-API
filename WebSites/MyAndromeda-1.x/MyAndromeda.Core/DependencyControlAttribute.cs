using System;

namespace MyAndromeda.Core
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DependencyControlAttribute : System.Attribute 
    {
        private bool enabled;

        public bool Enabled
        {
            get
            {
                return enabled;
            }
            set
            {
                enabled = value;
            }
        }

        public float Order { get; set; }
    }
}