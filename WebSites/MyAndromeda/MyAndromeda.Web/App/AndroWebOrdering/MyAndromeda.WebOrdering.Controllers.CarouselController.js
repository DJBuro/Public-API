/// <reference path="MyAndromeda.WebOrdering.App.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    (function (WebOrdering) {
        (function (Controllers) {
            WebOrdering.Angular.ControllersInitilizations.push(function (app) {
                app.controller(CarouselController.Name, [
                    '$scope', '$timeout',
                    WebOrdering.Services.ContextService.Name,
                    WebOrdering.Services.WebOrderingWebApiService.Name,
                    function ($scope, $timeout, contextService, webOrderingWebApiService) {
                        CarouselController.OnLoad($scope, $timeout, contextService, webOrderingWebApiService);

                        /* going to leave kendo to manage the observable object */
                        CarouselController.SetupCarousels($scope, $timeout, contextService);
                    }
                ]);
            });

            var CarouselController = (function () {
                function CarouselController() {
                }
                CarouselController.OnLoad = function ($scope, $timout, contextService, webOrderingWebApiService) {
                    /* Add image auto upload */
                    $scope.ShowCarousel = true;
                    $scope.ShowHtmlEditor = false;
                    $scope.ShowImageEditor = false;

                    $scope.SaveChanges = function () {
                        var data = $scope.CarouselBlocks;
                        data.forEach(function (block) {
                            console.log("sync block " + block.Carousel.Name);
                            /* this important otherwise the local data source does not update the main object */
                            //block.DataSource.sync();
                            //block.Carousel.h
                        });

                        webOrderingWebApiService.Update();
                    };

                    /* These are setup depending on the task ... create / edit */
                    $scope.CancelItem = function () {
                    };
                    $scope.SaveItem = function () {
                    };
                };

                CarouselController.CreateGuid = function () {
                    var S4 = function () {
                        return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
                    };

                    // then to call it, plus stitch in '4' in the third group
                    var guid = (S4() + S4() + "-" + S4() + "-4" + S4().substr(0, 3) + "-" + S4() + "-" + S4() + S4() + S4()).toLowerCase();
                    return guid;
                };

                CarouselController.CreateItem = function (type) {
                    return new kendo.data.Model({
                        Id: CarouselController.CreateGuid(),
                        Type: type,
                        ImageUrl: "",
                        Description: "",
                        HTML: ""
                    });
                };

                CarouselController.LoadImageEditor = function ($scope, $timout, carousel, item) {
                    $scope.ShowImageEditor = true;
                    $scope.ShowCarousel = false;
                    $scope.ShowHtmlEditor = false;

                    $scope.HasImageSlideUrl = item.ImageUrl;
                    kendo.unbind("#ImageTemplate");
                    kendo.bind("#ImageTemplate", item);

                    //part of the scope again.
                    var upload = $scope.CarouselImageUpload;

                    var carouselImageUploadUrl = kendo.format("/api/{0}/AndroWebOrdering/{1}/UploadCarouselImage/{2}/{3}", WebOrdering.Settings.AndromedaSiteId, WebOrdering.Settings.WebSiteId, carousel.Name, item.Id);

                    upload.setOptions({
                        async: {
                            saveUrl: carouselImageUploadUrl,
                            autoUpload: true
                        },
                        showFileList: false,
                        multiple: false
                    });

                    upload.unbind("success");
                    upload.bind("success", function (result) {
                        console.log(result.response);
                        console.log(result.response.Url);
                        var r = Math.floor(Math.random() * 99999) + 1;
                        item.set("ImageUrl", result.response.Url + "?k=" + r);

                        $timout(function () {
                            $scope.HasImageSlideUrl = true;
                            $scope.BindCarouselsList();
                        });
                    });
                };

                CarouselController.LoadHtmlEditor = function ($scope, item) {
                    $scope.ShowCarousel = false;
                    $scope.ShowHtmlEditor = true;
                    $scope.ShowImageEditor = false;

                    kendo.unbind("#HtmlTemplate");
                    kendo.bind("#HtmlTemplate", item);
                };

                CarouselController.ResetForm = function ($scope, $timeout, contextService) {
                    $timeout(function () {
                        $scope.ShowCarousel = true;
                        $scope.ShowHtmlEditor = false;
                        $scope.ShowImageEditor = false;
                    });
                    $scope.BindCarouselsList();
                };

                CarouselController.SetupCarousels = function ($scope, $timeout, contextService) {
                    var _this = this;
                    var actionsTemplate = $("#EditButtonTemplate").html();

                    $scope.EditCarouselItem = function (carouselName, carouselItemId) {
                        console.log("clicked on a item");
                        var s = kendo.format("index: {0}; item: {1}", carouselName, carouselItemId);

                        var carousels = $scope.CarouselBlocks.filter(function (e) {
                            return e.Carousel.Name == carouselName;
                        });
                        var c = carousels[0];

                        //find the carousel item
                        var m = c.DataSource.view().find(function (item) {
                            return item.Id == carouselItemId;
                        });
                        $scope.HtmlBeforeEdit = m.HTML;

                        switch (m.Type.toLowerCase()) {
                            case "image":
                                _this.LoadImageEditor($scope, $timeout, c.Carousel, m);
                                break;
                            case "html":
                                _this.LoadHtmlEditor($scope, m);
                                break;
                            default:
                                throw kendo.format("{0} type is not supported", m.Type);
                        }

                        $scope.SaveItem = function () {
                            //(c.DataSource.view().find((item: Models.ICarouselItem) => { return item.Id == carouselItemId; })).ImageUrl += "?k=" + r;
                            CarouselController.ResetForm($scope, $timeout, contextService);
                            $scope.SaveChanges();
                        };
                        $scope.CancelItem = function (Id) {
                            (c.DataSource.view().find(function (item) {
                                return item.Id == carouselItemId;
                            })).HTML = $scope.HtmlBeforeEdit;
                            CarouselController.ResetForm($scope, $timeout, contextService);
                            $scope.HtmlBeforeEdit = "";
                        };
                    };

                    $scope.CreateImageCarouselItem = function (carouselName) {
                        var carousels = $scope.CarouselBlocks.filter(function (e) {
                            return e.Carousel.Name == carouselName;
                        });
                        var c = carousels[0];

                        var newCarouselItem = CarouselController.CreateItem("image");

                        //c.DataSource.add(newCarouselItem);
                        $scope.SaveItem = function () {
                            c.DataSource.add(newCarouselItem);
                            CarouselController.ResetForm($scope, $timeout, contextService);
                            $scope.SaveChanges();
                        };

                        $scope.CancelItem = function () {
                            CarouselController.ResetForm($scope, $timeout, contextService);
                        };

                        CarouselController.LoadImageEditor($scope, $timeout, c.Carousel, newCarouselItem);
                    };

                    $scope.CreateHtmlCarouselItem = function (carouselName) {
                        var carousels = $scope.CarouselBlocks.filter(function (e) {
                            return e.Carousel.Name == carouselName;
                        });
                        var c = carousels[0];
                        var newCarouselItem = CarouselController.CreateItem("Html");

                        $scope.SaveItem = function () {
                            c.DataSource.add(newCarouselItem);
                            CarouselController.ResetForm($scope, $timeout, contextService);
                            $scope.SaveChanges();
                        };

                        $scope.CancelItem = function () {
                            CarouselController.ResetForm($scope, $timeout, contextService);
                        };

                        CarouselController.LoadHtmlEditor($scope, newCarouselItem);
                    };

                    $scope.RemoveCarouselItem = function (carouselName, carouselItemId) {
                        var carousels = $scope.CarouselBlocks.filter(function (e) {
                            return e.Carousel.Name == carouselName;
                        });
                        var c = carousels[0];
                        var m = c.DataSource.view().find(function (item) {
                            return item.Id == carouselItemId;
                        });

                        //remove from data source
                        c.DataSource.remove(m);
                        $scope.SaveChanges();
                    };

                    $scope.BindCarouselsList = function () {
                        var toolbar = $("#CarouselHeaderTemplate").html();
                        var editTemplate = $("#CarouselItemEditTemplate").html();
                        var descriptionTemplate = $("#DescriptionTemplate").html();

                        var settingsSubscription = contextService.ModelSubject.where(function (e) {
                            return e !== null;
                        }).subscribe(function (websiteSettings) {
                            $timeout(function () {
                                var carousels = websiteSettings.Home.Carousels;
                                var carouselEditors = carousels.map(function (carousel, index) {
                                    var dataSource = new kendo.data.DataSource({
                                        data: carousel.Items,
                                        schema: {
                                            id: "Id"
                                        }
                                    });

                                    var model = {
                                        Carousel: carousel,
                                        DataSource: dataSource,
                                        GridOptions: {
                                            name: carousel.Name,
                                            toolbar: [
                                                {
                                                    name: "add-button",
                                                    template: toolbar
                                                }
                                            ],
                                            columns: [
                                                {
                                                    field: "Type",
                                                    title: "Type",
                                                    width: "120px"
                                                },
                                                {
                                                    field: "Description",
                                                    title: "Description",
                                                    template: descriptionTemplate
                                                },
                                                {
                                                    title: "Actions",
                                                    template: editTemplate,
                                                    width: "120px"
                                                }
                                            ],
                                            dataSource: dataSource
                                        }
                                    };

                                    //dataSource.bind("change", () => {
                                    //    websiteSettings.Home.Carousels
                                    //});
                                    return model;
                                });

                                console.log("setup carousels:" + carouselEditors.length);

                                $scope.CarouselBlocks = carouselEditors;
                            });
                        });

                        $scope.$on('$destroy', function () {
                            settingsSubscription.dispose();
                        });
                    };
                    $scope.BindCarouselsList();
                };
                CarouselController.Name = "CarouselController";
                return CarouselController;
            })();
            Controllers.CarouselController = CarouselController;
        })(WebOrdering.Controllers || (WebOrdering.Controllers = {}));
        var Controllers = WebOrdering.Controllers;
    })(MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
    var WebOrdering = MyAndromeda.WebOrdering;
})(MyAndromeda || (MyAndromeda = {}));
