var MyAndromeda;
(function (MyAndromeda) {
    (function (DeliveryZonesByRadius) {
        /// <reference path="../../Scripts/typings/rx.js/rx.binding-lite.d.ts" />
        /// <reference path="MyAndromeda.DeliveryZonesByRadius.App.ts" />
        (function (Services) {
            DeliveryZonesByRadius.Angular.ServicesInitilizations.push(function (app) {
                app.factory(DeliveryZonesByRadiusWebApiService.Name, [
                    Services.ContextService.Name,
                    function (contextService) {
                        var instnance = new DeliveryZonesByRadiusWebApiService(contextService);
                        return instnance;
                    }
                ]);
            });

            var DeliveryZonesByRadiusWebApiService = (function () {
                function DeliveryZonesByRadiusWebApiService(context) {
                    var _this = this;
                    this.Context = context;
                    this.IsDeliveryZonesBusy = new Rx.BehaviorSubject(false);
                    this.Search = new Rx.Subject();
                    this.DataSource = new kendo.data.DataSource({});

                    if (!DeliveryZonesByRadius.Settings.AndromedaSiteId) {
                        throw "Settings.AndromedaSiteId is undefined";
                    }

                    var read = kendo.format(DeliveryZonesByRadius.Settings.ReadRoute, DeliveryZonesByRadius.Settings.AndromedaSiteId);

                    this.IsDeliveryZonesBusy.onNext(true);

                    var promise = $.getJSON(read);

                    promise.done(function (result) {
                        var oldPostcodes = result.PostCodeSectors;
                        _this.Context.Model = result;

                        result.PostCodeSectors = new kendo.data.ObservableArray(oldPostcodes);
                        _this.Context.ModelSubject.onNext(result);

                        _this.IsDeliveryZonesBusy.onNext(false);
                    });

                    promise.fail(function () {
                        _this.IsDeliveryZonesBusy.onNext(false);
                    });
                }
                DeliveryZonesByRadiusWebApiService.prototype.Update = function () {
                    var _this = this;
                    var update = kendo.format(DeliveryZonesByRadius.Settings.UpdateRoute, DeliveryZonesByRadius.Settings.AndromedaSiteId);
                    console.log("sync");

                    var raw = JSON.stringify(this.Context.Model);

                    var currentPostCodes = this.Context.Model.PostCodeSectors;
                    var accepted = currentPostCodes.filter(function (item) {
                        return item.IsSelected;
                    });

                    console.log(currentPostCodes.length);

                    var promise = $.ajax({
                        url: update,
                        type: "POST",
                        contentType: 'application/json',
                        dataType: "json",
                        data: raw,
                        async: false
                    });

                    this.IsDeliveryZonesBusy.onNext(true);

                    promise.done(function (result) {
                        var postcodeArea = result;
                        var oldPostcodes = postcodeArea.PostCodeSectors;

                        var returnedRows = postcodeArea.PostCodeSectors;
                        var acceptedReturnedRows = returnedRows.filter(function (item) {
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

                        _this.Context.Model = postcodeArea;
                        _this.Context.Model.PostCodeSectors = new kendo.data.ObservableArray(oldPostcodes);
                        _this.Context.ModelSubject.onNext(postcodeArea);
                        _this.IsDeliveryZonesBusy.onNext(false);
                    });
                    promise.fail(function () {
                        _this.IsDeliveryZonesBusy.onNext(false);
                    });
                };

                DeliveryZonesByRadiusWebApiService.prototype.GeneratePostCodes = function () {
                    var _this = this;
                    var generatePostCodes = kendo.format(DeliveryZonesByRadius.Settings.ReadPostCodesRoute, DeliveryZonesByRadius.Settings.AndromedaSiteId);
                    var raw = JSON.stringify(this.Context.Model);
                    var request = $.ajax({
                        url: generatePostCodes,
                        type: "POST",
                        contentType: 'application/json',
                        dataType: "json",
                        data: raw
                    });

                    this.IsDeliveryZonesBusy.onNext(true);

                    request.done(function (result) {
                        if (result.length === 0) {
                            alert("There are no postcodes near hear apparently...");
                        }

                        var postcodes = result.map(function (postcode) {
                            return {
                                PostCodeSector: postcode,
                                IsSelected: true
                            };
                        });

                        _this.Context.PostcodeModels.onNext(postcodes);
                        _this.IsDeliveryZonesBusy.onNext(false);

                        $("#loader").addClass('hidden');
                        _this.Context.Model.HasPostCodes = true;
                    });

                    request.fail(function () {
                        _this.IsDeliveryZonesBusy.onNext(false);
                    });
                };
                DeliveryZonesByRadiusWebApiService.Name = "DeliveryZonesByRadiusWebApiService";
                return DeliveryZonesByRadiusWebApiService;
            })();
            Services.DeliveryZonesByRadiusWebApiService = DeliveryZonesByRadiusWebApiService;
        })(DeliveryZonesByRadius.Services || (DeliveryZonesByRadius.Services = {}));
        var Services = DeliveryZonesByRadius.Services;
    })(MyAndromeda.DeliveryZonesByRadius || (MyAndromeda.DeliveryZonesByRadius = {}));
    var DeliveryZonesByRadius = MyAndromeda.DeliveryZonesByRadius;
})(MyAndromeda || (MyAndromeda = {}));
