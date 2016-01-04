/// <reference path="../../scripts/typings/jquery/jquery.d.ts" />
/// <reference path="../../scripts/typings/androwebordering/androwebordering.d.ts" />
/// <reference path="../../scripts/typings/google.analytics/ga.d.ts" />
var AndroWeb;
(function (AndroWeb) {
    var telemetrySessionMessage = (function () {
        function telemetrySessionMessage() {
            this.serverUrl = viewModel.serverUrl;
            this.siteId = viewModel.selectedSite() === undefined ? undefined : viewModel.selectedSite().siteId;
            this.session = new telemetrySession();
        }
        return telemetrySessionMessage;
    })();
    AndroWeb.telemetrySessionMessage = telemetrySessionMessage;
    var telemetrySession = (function () {
        function telemetrySession() {
            this.createdDateTime = helper.formatUTCDate(new Date());
            this.referrer = document.referrer;
            this.browserDetails = navigator.userAgent;
        }
        return telemetrySession;
    })();
    AndroWeb.telemetrySession = telemetrySession;
    var telemetryMessage = (function () {
        function telemetryMessage(sessionId, action, extraInfo) {
            this.serverUrl = viewModel.serverUrl;
            this.siteId = viewModel.selectedSite() === undefined ? undefined : viewModel.selectedSite().siteId;
            this.telemetryData = new telemetry();
            this.telemetryData.sessionId = sessionId;
            this.telemetryData.action = action;
            this.telemetryData.extraInfo = extraInfo;
        }
        return telemetryMessage;
    })();
    AndroWeb.telemetryMessage = telemetryMessage;
    var telemetry = (function () {
        function telemetry() {
            this.dateTime = helper.formatUTCDate(new Date());
            this.action = "";
            this.extraInfo = "";
        }
        return telemetry;
    })();
    AndroWeb.telemetry = telemetry;
    var telemetryHelper = (function () {
        function telemetryHelper() {
            if (Worker === undefined)
                return;
            // Initialise telemetry session background worker
            var telemetrySessionBlob = new Blob([document.querySelector('#telemetrySession').textContent]);
            this.telemetrySessionWorker = new Worker(URL.createObjectURL(telemetrySessionBlob));
            var self = this; // Stupid typescript "this" points to the wrong object in event handlers
            this.telemetrySessionWorker.onmessage = function (e) {
                var reponse = JSON.parse(e.data);
                self.sessionId = reponse.sessionId;
            };
            // Initialise telemetry background worker
            var telemetryBlob = new Blob([document.querySelector('#telemetry').textContent]);
            this.telemetryWorker = new Worker(URL.createObjectURL(telemetryBlob));
            // Start a session
            this.beginAndroSession();
        }
        telemetryHelper.prototype.beginAndroSession = function () {
            var message = new telemetrySessionMessage();
            this.telemetrySessionWorker.postMessage(message);
        };
        telemetryHelper.prototype.sendAndroTelemetry = function (action, extraInfo) {
            if (this.sessionId === undefined)
                return; // No telemetry session = no telemetry data
            var message = new telemetryMessage(this.sessionId, action, extraInfo);
            this.telemetryWorker.postMessage(message);
        };
        telemetryHelper.prototype.sendTelemetryData = function (category, action, label, extraInfo) {
            // Send to Google
            ga("send", "event", {
                eventCategory: category,
                eventAction: action,
                eventLabel: "Completed",
                eventValue: 1,
                metric1: 1
            });
            // Send to Andro
            var action = category + "/" + action + "/" + label;
            while (action.substr(action.length - 1, 1) === "/") {
                action = action.substr(0, action.length - 1);
            }
            this.sendAndroTelemetry(action, extraInfo);
        };
        return telemetryHelper;
    })();
    AndroWeb.telemetryHelper = telemetryHelper;
})(AndroWeb || (AndroWeb = {}));
//# sourceMappingURL=telemetryHelper.js.map