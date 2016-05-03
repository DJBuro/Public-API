
/// <reference path="MyAndromeda.DeliveryZonesByRadius.App.ts" />
/// <reference path="../../scripts/typings/rx/rx.d.ts" />

module MyAndromeda.DeliveryZonesByRadius.Services {
    Angular.ServicesInitilizations.push((app) => {
        app.factory(DeliveryZonesByRadiusWebApiService.Name, [
            Services.ContextService.Name,
            (contextService) => {
                var instnance = new DeliveryZonesByRadiusWebApiService(contextService);
                return instnance;
            }
        ]);
    });

    export class DeliveryZonesByRadiusWebApiService {
        public static Name: string = "DeliveryZonesByRadiusWebApiService";

        public IsDeliveryZonesBusy: Rx.BehaviorSubject<boolean>;
        public IsSavingBusy: Rx.BehaviorSubject<boolean>;

        public Search: Rx.Subject<string>;


        public DataSource: kendo.data.DataSource;

        public Context: Services.ContextService;


        public constructor(context: Services.ContextService) {
            if (!Settings.AndromedaSiteId) { throw "Settings.AndromedaSiteId is undefined"; }

            this.Context = context;
            this.IsDeliveryZonesBusy = new Rx.BehaviorSubject<boolean>(false);
            this.IsSavingBusy = new Rx.BehaviorSubject<boolean>(false);

            this.Search = new Rx.Subject<string>();
            this.DataSource = new kendo.data.DataSource({});

            this.IsDeliveryZonesBusy.onNext(true);

            var read = kendo.format(Settings.ReadRoute, Settings.AndromedaSiteId);
            var promise = $.getJSON(read);

            promise.done((result: Models.IDeliveryZoneNameViewModelSettings) => {
                var oldPostcodes: any = result.PostCodeSectors;
                this.Context.Model = result;

                result.PostCodeSectors = new kendo.data.ObservableArray(oldPostcodes);
                this.Context.ModelSubject.onNext(result);

                this.IsDeliveryZonesBusy.onNext(false);
            });

            promise.fail(() => {
                this.IsDeliveryZonesBusy.onNext(false);
            });

        }

        public Update(): void {
            var update = kendo.format(Settings.UpdateRoute, Settings.AndromedaSiteId);
            console.log("sync");

            var raw = JSON.stringify(this.Context.Model);

            var currentPostCodes = this.Context.Model.PostCodeSectors;
            var accepted = currentPostCodes.filter((item: Models.IPostCodeSectorsViewModel) => {
                return item.IsSelected;
            });

            console.log(currentPostCodes.length);


            this.IsSavingBusy.onNext(true);
            this.IsDeliveryZonesBusy.onNext(true);

            var promise = $.ajax({
                url: update,
                type: "POST",
                contentType: 'application/json',
                dataType: "json",
                data: raw,
                //async: false
            });

            promise.done((result: Models.IDeliveryZoneNameViewModelSettings) => {
                var postcodeArea = result;
                var oldPostcodes: any = postcodeArea.PostCodeSectors;

                var returnedRows = postcodeArea.PostCodeSectors;
                var acceptedReturnedRows = returnedRows.filter((item: Models.IPostCodeSectorsViewModel) => {
                    return item.IsSelected;
                });
                if (currentPostCodes.length !== returnedRows.length) {
                    var alertMsg = kendo.format("this is not the expected amount of postcodes: {0} records sent.! Received: {1}", currentPostCodes.length, returnedRows.length);
                    alert(alertMsg);
                }
                if (accepted.length !== acceptedReturnedRows.length) {
                    var alertMsg = kendo.format("this is not the expected amount of 'accepted' postcodes: {0}. records sent.! Received: {1}", accepted.length, acceptedReturnedRows.length);
                    alert(alertMsg);
                }

                this.Context.Model = postcodeArea;
                this.Context.Model.PostCodeSectors = new kendo.data.ObservableArray(oldPostcodes);
                this.Context.ModelSubject.onNext(postcodeArea);
                
            });
            promise.always(() => {
                this.IsDeliveryZonesBusy.onNext(false)
                this.IsSavingBusy.onNext(false);
            });
        }

        public GeneratePostCodes(): void {
            var generatePostCodes = kendo.format(Settings.ReadPostCodesRoute, Settings.AndromedaSiteId);
            var raw = JSON.stringify(this.Context.Model);
            var request =
                $.ajax({
                    url: generatePostCodes,
                    type: "POST",
                    contentType: 'application/json',
                    dataType: "json",
                    data: raw
                });

            this.IsDeliveryZonesBusy.onNext(true);

            request.done((result: string[]) => {
                if (result.length === 0) {
                    alert("There are no postcodes near hear apparently...");
                }

                var postcodes: Models.IPostCodeSectorsViewModel[] = result.map(postcode => <Models.IPostCodeSectorsViewModel> {
                    PostCodeSector: postcode,
                    IsSelected: true
                });

                this.Context.PostcodeModels.onNext(postcodes);
                this.IsDeliveryZonesBusy.onNext(false);

                $("#loader").addClass('hidden');
                this.Context.Model.HasPostCodes = true;
            });

            request.fail(() => {
                this.IsDeliveryZonesBusy.onNext(false);
            });
        }

    }
} 