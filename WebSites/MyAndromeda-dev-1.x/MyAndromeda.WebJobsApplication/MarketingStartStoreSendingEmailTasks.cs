//----------------------------------------------------------------------------------
// Microsoft Developer & Platform Evangelism
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
//----------------------------------------------------------------------------------
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//----------------------------------------------------------------------------------
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using MyAndromeda.Logging;
using MyAndromeda.Services.Marketing.Models;
using MyAndromeda.WebJobs.EventMarketing.Services;
using Newtonsoft.Json;

namespace MyAndromeda.WebJobs.EventMarketing
{
    public class MarketingStartStoreSendingEmailTasks 
    {
        private readonly IMyAndromedaLogger logger;
        private readonly ISendMarketingService sendMarketingService;

        public MarketingStartStoreSendingEmailTasks(IMyAndromedaLogger logger, ISendMarketingService sendMarketingService)
        {
            this.sendMarketingService = sendMarketingService;
            this.logger = logger;
        }

        /// <summary>
        /// Take a message that it needs to be sent. 
        /// </summary>
        /// <param name="queueMessage"></param>
        /// <returns></returns>
        public async Task ProccessMarketingEvent(
            [QueueTrigger("%marketing-event-store-queue%")] string queueMessage,
            [Queue("%marketing-event-audit-history-queue%")] ICollector<string> historyOutputQueue)
        {
            this.logger.Debug("Processing marketing event for: " + queueMessage);

            //defines which store to send to
            var message = JsonConvert.DeserializeObject<MarketingStoreEventQueueMessage>(queueMessage);

            //switch the sent the mail to false to past this stage. 
            var outputMessageObject = await this.sendMarketingService.SendAsyc(message, 
                addMatt: true, 
                sendTheEmail: true);

            //the task has been canceled. 
            if (outputMessageObject == null) 
            {
                return;
            }

            var outputMessage = JsonConvert.SerializeObject(outputMessageObject);

            //add the message to the queue to process tracking for the recipients
            historyOutputQueue.Add(outputMessage);
        }
    }
}
