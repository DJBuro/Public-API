using System;
using Microsoft.Azure.WebJobs;
using MyAndromeda.Services.Marketing.WebJobs;

namespace MyAndromeda.WebJobs.EventMarketing.Activators
{
    public class MyAndromedaNameResolver : INameResolver 
    {
        private readonly IQueueNames queueNames = new QueueNames();
        
        public string Resolve(string name)
        {
            //if (!name.StartsWith("%")) { return name; }

            var guess = queueNames.GetName(name);

            if (!string.IsNullOrWhiteSpace(guess)) 
            {
                return guess;
            }

            return name;
        }
    }
}