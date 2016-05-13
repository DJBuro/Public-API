/// <reference path="myandromeda.marketing.ts" />
/// <reference path="myandromeda.marketing.ts" />
module MyAndromeda.Marketing {
    
    export class TokenDataService {
        public dataSource: kendo.data.DataSource;

        constructor() {
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
    }

    export class MarketingEventService {
        public $http: ng.IHttpService;

        constructor($http: ng.IHttpService) {
            this.$http = $http;
        }

        public LoadContactDetails(andromedaSiteId: number): ng.IHttpPromise<Models.IMarketingContact> {
            var route = kendo.format(Routes.ContactRoute, andromedaSiteId);
            var promise = this.$http.get(route);

            return promise;
        }

        public SaveContact(andromedaSiteId: number, model: Models.IMarketingContact): ng.IHttpPromise<Models.IMarketingContact> {
            var route = kendo.format(Routes.ContactRoute, andromedaSiteId);
            var promise = this.$http.post(route, model);

            return promise;
        }

        public SaveEvent(andromedaSiteId: number, model: Models.IMarketingContact): ng.IHttpPromise<Models.IMarketingType> {
            var route = kendo.format(Routes.Save, andromedaSiteId);
            var promise = this.$http.post(route, model);

            return promise;
        }

        public LoadUnRegistered(andromedaSiteId: number): ng.IHttpPromise<Models.IMarketingType> {
            var route = kendo.format(Routes.RegisteredAndInactiveRoute, andromedaSiteId);
            var promise = this.$http.get(route);

            return promise;
        }

        public LoadSevenDays(andromedaSiteId: number): ng.IHttpPromise<Models.IMarketingType> {
            var route = kendo.format(Routes.InactiveForSevenDaysRoute, andromedaSiteId);
            var promise = this.$http.get(route);

            return promise;
        }

        public LoadOneMonthSettings(andromedaSiteId: number): ng.IHttpPromise<Models.IMarketingType> {
            var route = kendo.format(Routes.InactiveForOneMonthRoute, andromedaSiteId);
            var promise = this.$http.get(route);

            return promise;
        }

        public LoadThreeMonthSettings(andromedaSiteId: number): ng.IHttpPromise<Models.IMarketingType> {
            var route = kendo.format(Routes.InactiveForThreeMonthsRoute, andromedaSiteId);
            var promise = this.$http.get(route);

            return promise;
        }

        public LoadTestSettings(andromedaSiteId: number): ng.IHttpPromise<Models.IMarketingType> {
            var route = kendo.format(Routes.TestType, andromedaSiteId);
            var promise = this.$http.get(route);

            return promise;
        }

        public PreviewEmail(andromedaSiteId: number, model: Models.IPreviewMarketing): ng.IHttpPromise<Models.IMarketingType> {
            var route = kendo.format(Routes.Preview, andromedaSiteId);
            var promise = this.$http.post(route, model);

            return promise;
        }

        public SendNow(andromedaSiteId: number, model: Models.IMarketingType): ng.IHttpPromise<Models.IMarketingType> {
            var route = kendo.format(Routes.SendNow, andromedaSiteId);
            var promise = this.$http.post(route, model);

            return promise;
        }

    }

    export class RecipientService
    {
        private $http: ng.IHttpService;

        constructor($http: ng.IHttpService)
        {
            this.$http = $http;
        }

        public LoadRecipients(andromedaSiteId: number, model: Models.IMarketingType)
        {
            var route = kendo.format(Routes.PreviewRecipients, andromedaSiteId);
            var promise = this.$http.post(route, model);

            return promise;
        }
    }

    m.service("recipientService", RecipientService);

    m.service("marketingEventService", MarketingEventService);

    m.service("tokenDataService", TokenDataService); 

} 