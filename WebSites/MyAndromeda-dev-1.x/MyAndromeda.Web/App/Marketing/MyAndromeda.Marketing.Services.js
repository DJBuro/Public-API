/// <reference path="myandromeda.marketing.ts" />
/// <reference path="myandromeda.marketing.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var Marketing;
    (function (Marketing) {
        var TokenDataService = (function () {
            function TokenDataService() {
                var dataSource = new kendo.data.DataSource({
                    transport: {
                        read: {
                            url: "/api/email/tokens",
                            "type": "GET"
                        }
                    }
                });
                this.dataSource = dataSource;
            }
            return TokenDataService;
        })();
        Marketing.TokenDataService = TokenDataService;
        var MarketingEventService = (function () {
            function MarketingEventService($http) {
                this.$http = $http;
            }
            MarketingEventService.prototype.LoadContactDetails = function (andromedaSiteId) {
                var route = kendo.format(Marketing.Routes.ContactRoute, andromedaSiteId);
                var promise = this.$http.get(route);
                return promise;
            };
            MarketingEventService.prototype.SaveContact = function (andromedaSiteId, model) {
                var route = kendo.format(Marketing.Routes.ContactRoute, andromedaSiteId);
                var promise = this.$http.post(route, model);
                return promise;
            };
            MarketingEventService.prototype.SaveEvent = function (andromedaSiteId, model) {
                var route = kendo.format(Marketing.Routes.Save, andromedaSiteId);
                var promise = this.$http.post(route, model);
                return promise;
            };
            MarketingEventService.prototype.LoadUnRegistered = function (andromedaSiteId) {
                var route = kendo.format(Marketing.Routes.RegisteredAndInactiveRoute, andromedaSiteId);
                var promise = this.$http.get(route);
                return promise;
            };
            MarketingEventService.prototype.LoadSevenDays = function (andromedaSiteId) {
                var route = kendo.format(Marketing.Routes.InactiveForSevenDaysRoute, andromedaSiteId);
                var promise = this.$http.get(route);
                return promise;
            };
            MarketingEventService.prototype.LoadOneMonthSettings = function (andromedaSiteId) {
                var route = kendo.format(Marketing.Routes.InactiveForOneMonthRoute, andromedaSiteId);
                var promise = this.$http.get(route);
                return promise;
            };
            MarketingEventService.prototype.LoadThreeMonthSettings = function (andromedaSiteId) {
                var route = kendo.format(Marketing.Routes.InactiveForThreeMonthsRoute, andromedaSiteId);
                var promise = this.$http.get(route);
                return promise;
            };
            MarketingEventService.prototype.LoadTestSettings = function (andromedaSiteId) {
                var route = kendo.format(Marketing.Routes.TestType, andromedaSiteId);
                var promise = this.$http.get(route);
                return promise;
            };
            MarketingEventService.prototype.PreviewEmail = function (andromedaSiteId, model) {
                var route = kendo.format(Marketing.Routes.Preview, andromedaSiteId);
                var promise = this.$http.post(route, model);
                return promise;
            };
            MarketingEventService.prototype.SendNow = function (andromedaSiteId, model) {
                var route = kendo.format(Marketing.Routes.SendNow, andromedaSiteId);
                var promise = this.$http.post(route, model);
                return promise;
            };
            return MarketingEventService;
        })();
        Marketing.MarketingEventService = MarketingEventService;
        var RecipientService = (function () {
            function RecipientService($http) {
                this.$http = $http;
            }
            RecipientService.prototype.LoadRecipients = function (andromedaSiteId, model) {
                var route = kendo.format(Marketing.Routes.PreviewRecipients, andromedaSiteId);
                var promise = this.$http.post(route, model);
                return promise;
            };
            return RecipientService;
        })();
        Marketing.RecipientService = RecipientService;
        Marketing.m.service("recipientService", RecipientService);
        Marketing.m.service("marketingEventService", MarketingEventService);
        Marketing.m.service("tokenDataService", TokenDataService);
    })(Marketing = MyAndromeda.Marketing || (MyAndromeda.Marketing = {}));
})(MyAndromeda || (MyAndromeda = {}));
