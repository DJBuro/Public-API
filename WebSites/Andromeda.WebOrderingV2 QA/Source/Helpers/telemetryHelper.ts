/// <reference path="../../scripts/typings/jquery/jquery.d.ts" />
/// <reference path="../../scripts/typings/androwebordering/androwebordering.d.ts" />
/// <reference path="../../scripts/typings/google.analytics/ga.d.ts" />

module AndroWeb
{
    export class telemetrySessionMessage
    {
        public serverUrl: string = viewModel.serverUrl;
        public siteId: string = viewModel.selectedSite() === undefined ? undefined : viewModel.selectedSite().siteId;
        public session: telemetrySession = new telemetrySession();
    }

    export class telemetrySession
    {
        public createdDateTime: string = helper.formatUTCDate(new Date());
        public referrer: string = document.referrer;
        public browserDetails: string = navigator.userAgent;
    }

    export class telemetryMessage
    {
        public serverUrl: string = viewModel.serverUrl;
        public siteId: string = viewModel.selectedSite() === undefined ? undefined : viewModel.selectedSite().siteId;
        public telemetryData: telemetry = new telemetry();

        constructor(sessionId: string, action: string, extraInfo: string)
        {
            this.telemetryData.sessionId = sessionId;
            this.telemetryData.action = action;
            this.telemetryData.extraInfo = extraInfo;
        }
    }

    export class telemetry
    {
        public sessionId: string;
        public dateTime: string = helper.formatUTCDate(new Date());
        public action: string = "";
        public extraInfo: string = "";
    }

    export class telemetryHelper
    {
        private telemetrySessionWorker: Worker;
        private telemetryWorker: Worker;

        public sessionId: string;

        constructor()
        {
            if (Worker === undefined) return;

            // Initialise telemetry session background worker
            var telemetrySessionBlob = new Blob([document.querySelector('#telemetrySession').textContent]);
            this.telemetrySessionWorker = new Worker(URL.createObjectURL(telemetrySessionBlob));

            var self = this;  // Stupid typescript "this" points to the wrong object in event handlers
            this.telemetrySessionWorker.onmessage = function (e)
            {
                var reponse = JSON.parse(e.data);
                self.sessionId = reponse.sessionId;
            };

            // Initialise telemetry background worker
            var telemetryBlob = new Blob([document.querySelector('#telemetry').textContent]);
            this.telemetryWorker = new Worker(URL.createObjectURL(telemetryBlob));

            // Start a session
            this.beginAndroSession();
        }

        public beginAndroSession()
        {
            var message = new telemetrySessionMessage();

            this.telemetrySessionWorker.postMessage(message);
        }

        private sendAndroTelemetry(action: string, extraInfo: string)
        {
            if (this.sessionId === undefined) return; // No telemetry session = no telemetry data

            var message = new telemetryMessage(this.sessionId, action, extraInfo);

            this.telemetryWorker.postMessage(message);
        }

        public sendTelemetryData(category: string, action: string, label: string, extraInfo: string)
        {
            // Send to Google
            ga
            (
                "send",
                "event",
                {
                    eventCategory: category,
                    eventAction: action,
                    eventLabel: "Completed",
                    eventValue: 1,
                    metric1: 1
                }
            );

            // Send to Andro
            var action = category + "/" + action + "/" + label;

            while (action.substr(action.length - 1, 1) === "/")
            {
                action = action.substr(0, action.length - 1);
            }

            this.sendAndroTelemetry(action, extraInfo);
        }
    }
}