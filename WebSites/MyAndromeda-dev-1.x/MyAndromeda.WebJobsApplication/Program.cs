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
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using MyAndromeda.WebJobs.EventMarketing;
using MyAndromeda.WebJobs.EventMarketing.Startup;
using Newtonsoft.Json;
using System;
using System.Configuration;
using MyAndromeda.Logging;

namespace MyAndromeda.WebJobs.EventMarketing
{
    //******************************************************************************************************
    // This will show you how to perform common scenarios using the Microsoft Azure Queue storage service using 
    // the Microsoft Azure WebJobs SDK. The scenarios covered include triggering a function when a new message comes
    // on a queue, sending a message on a queue.   
    // 
    // In this sample, the Program class starts the JobHost and creates the demo data. The Functions class
    // contains methods that will be invoked when messages are placed on the queues, based on the attributes in 
    // the method headers.
    //
    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976
    //
    // TODO: Open app.config and paste your Storage connection string into the AzureWebJobsDashboard and
    //      AzureWebJobsStorage connection string settings.
    //*****************************************************************************************************
    public class Program
    {
        static void Main()
        {
            System.Diagnostics.Trace.WriteLine("MyAndromeda. Starting.");

            var config = new JobHostConfiguration()
            {
                JobActivator = new Activators.MyAndromedaActivator(),
                NameResolver = new Activators.MyAndromedaNameResolver(),
            };

            var logger = config.JobActivator.CreateInstance<IMyAndromedaLogger>();
            logger.Debug("Check storage configuration");
            
            if (!Startup.Configuration.VerifyStorageConfiguration(config.JobActivator))
            {
                Console.ReadLine();
                return;
            }

            JobHost host = new JobHost(config);

            host.CallAsync(typeof(MarketingCheckingEventTasks).GetMethod("AlwaysRunning"));

            try
            {
                logger.Debug("Start host.RunAndBlock");
                host.RunAndBlock();
            }
            catch (Exception ex)
            {
                logger.Debug("Blew up.");
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
                throw;
            }

            
        }


    }
}
