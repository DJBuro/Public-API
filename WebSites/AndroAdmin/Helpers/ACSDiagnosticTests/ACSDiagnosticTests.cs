using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR.Hubs;
using System.Xml.Linq;
using Microsoft.AspNet.SignalR;

namespace AndroAdmin.Helpers.ACSDiagnosticTests
{
    public class ACSDiagnosticTests
    {
        internal IHubContext HubConnectionContext { get; set; }
        internal bool Completed = false;
        internal string messages = "";
        private static Dictionary<string, ACSDiagnosticTestData> ACSDiagnosticTestData = new Dictionary<string, ACSDiagnosticTestData>();

        internal static ACSDiagnosticTestData GetACSDiagnosticTestData(string prefix, string signpostServer, string host)
        {
            string hostSignpostServer = ACSDiagnosticTests.GetDomainName(signpostServer);
            string hostDomain = ACSDiagnosticTests.GetDomainName(host);

            string key = prefix + "_" + hostSignpostServer + "_" + hostDomain;

            ACSDiagnosticTestData acsDiagnosticTestData = null;
            if (!ACSDiagnosticTests.ACSDiagnosticTestData.TryGetValue(key, out acsDiagnosticTestData))
            {
                acsDiagnosticTestData = new ACSDiagnosticTestData();
                ACSDiagnosticTests.ACSDiagnosticTestData.Add(key, acsDiagnosticTestData);
            }
            return acsDiagnosticTestData;
        }
        private static string GetDomainName(string host)
        {
            int index = host.IndexOf("//");
            if (index != -1)
            {
                host = host.Substring(index + 2);
            }

            index = host.IndexOf("/");
            if (index != -1)
            {
                host = host.Substring(0, index);
            }

            return host;
        }

        internal void Start()
        {
            try
            {
                lock (this)
                {
                    ACSDiagnosticTests.ACSDiagnosticTestData = new Dictionary<string, ACSDiagnosticTestData>();

                    ACSDiagnosticTestsV1 acsDiagnosticTestsV1 = new ACSDiagnosticTestsV1();
                    ACSDiagnosticTestsV2 acsDiagnosticTestsV2 = new ACSDiagnosticTestsV2();


                    this.HubConnectionContext = GlobalHost.ConnectionManager.GetHubContext<MenuSync>();

                    this.SendBlank();
                    this.SendMessage(false, MessageTypeEnum.Info, "ACS Diagnostic tests started");

                    acsDiagnosticTestsV1.TestSignPostServers(this);
                    acsDiagnosticTestsV2.TestSignPostServers(this);

                    this.SendBlank();
                    this.SendMessage(false, MessageTypeEnum.Info, "ACS Diagnostic tests completed");

                    this.Completed = true;
                }
            }
            catch (Exception exception)
            {
                this.SendBlank();
                this.SendBlank();
                this.SendMessage(false, MessageTypeEnum.Error, "ACS Diagnostic tests failed with an unhandled exception: " + exception.Message);
                this.Completed = true;
            }
        }

        internal void SendBlank()
        {
            this.SendMessage(false, MessageTypeEnum.Info, "");
        }

        internal void SendFailedTestMessage(bool isVerbose, string message)
        {
            this.SendMessage(isVerbose, MessageTypeEnum.Error, " ... failed");
            this.SendMessage(isVerbose, ACSDiagnosticTests.MessageTypeEnum.Error, message);
        }

        internal void SendSuccessfulTestMessage(bool isVerbose)
        {
            this.SendMessage(isVerbose, MessageTypeEnum.Success, " ... success");
        }

        public enum MessageTypeEnum { Error, Success, Info, StartTest };
        internal void SendMessage(bool isVerbose, MessageTypeEnum messageType, string message)
        {
            bool newLine = false;
            if (messageType == MessageTypeEnum.Error)
            {
                message = "<span class=\"acsTestError\">" + message + "</span>";
                newLine = true;
            }
            else if (messageType == MessageTypeEnum.Success)
            {
                message = "<span class=\"acsTestSuccess\">" + message + "</span>";
                newLine = true;
            }
            else if (messageType == MessageTypeEnum.Info)
            {
                newLine = true;
            }

            message = "<span" + (isVerbose ? " class=\"acsTestVerbose\"" : "") + ">" + message + "</span>";

            if (newLine) message += "<br />";

            this.messages += message;

            lock (this)
            {
                this.HubConnectionContext.Clients.All.message(message);
            }
        }
    }
}