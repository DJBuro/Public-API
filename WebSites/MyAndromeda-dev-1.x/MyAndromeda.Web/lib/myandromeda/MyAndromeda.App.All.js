var MyAndromeda;
(function (MyAndromeda) {
    var Menu;
    (function (Menu) {
        "use strict";
        var Logger = (function () {
            function Logger() {
                this.UseNotify = true;
                this.UseDebug = true;
                this.UseError = true;
            }
            Logger.Notify = function (o) {
                if (logger.UseNotify) {
                    console.log(o);
                }
            };
            Logger.Debug = function (o) {
                if (logger.UseDebug) {
                    console.log(o);
                }
            };
            Logger.Error = function (o) {
                if (logger.UseError) {
                    console.log(o);
                }
            };
            Logger.SettingUpController = function (name, state) {
                if (logger.UseNotify) {
                    console.log("setting up controller - " + name + " : " + state);
                }
            };
            Logger.SettingUpService = function (name, state) {
                if (logger.UseNotify) {
                    console.log("setting up service - " + name + " : " + state);
                }
            };
            Logger.AllowDebug = function (value) {
                logger.UseDebug = value;
            };
            Logger.AllowError = function (value) {
                logger.UseError = value;
            };
            return Logger;
        })();
        Menu.Logger = Logger;
        var logger = new Logger();
    })(Menu = MyAndromeda.Menu || (MyAndromeda.Menu = {}));
})(MyAndromeda || (MyAndromeda = {}));
/// <reference path="../Menu/MyAndromeda.Menu.Logger.ts" />
/// <reference path="../../Scripts/typings/angularjs/angular-route.d.ts" />
/// <reference path="../../Scripts/typings/angularjs/angular.d.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var WebOrdering;
    (function (WebOrdering) {
        "use strict";
        var WebOrderingApp = (function () {
            function WebOrderingApp() {
            }
            WebOrderingApp.ApplicationName = "WebOrderingApplication";
            return WebOrderingApp;
        })();
        WebOrdering.WebOrderingApp = WebOrderingApp;
        var Angular = (function () {
            function Angular() {
            }
            Angular.Init = function () {
                WebOrdering.Logger.Notify("bootstrap-WebOrdering");
                var element = document.getElementById("WebOrdering");
                var app = angular.module(WebOrderingApp.ApplicationName, [
                    "ContextServiceModule",
                    "androweb-upsell-module",
                    "kendo.directives",
                    //"ngRoute",
                    "ngAnimate",
                    "ngSanitize"
                ]);
                WebOrdering.Logger.Notify("Assign " + Angular.ServicesInitilizations.length + " services");
                Angular.ServicesInitilizations.forEach(function (value) {
                    value(app);
                });
                WebOrdering.Logger.Notify("Assign " + Angular.ControllersInitilizations.length + " controllers");
                Angular.ControllersInitilizations.forEach(function (value) {
                    value(app);
                });
                //app.config(['$routeProvider', function ($routeProvider: ng.route.IRouteProvider) {
                //    $routeProvider.when(WebOrdering.Controllers.PickThemeController.Route, {
                //        controller: Controllers.PickThemeController.Name
                //    });
                //    $routeProvider.otherwise({ redirectTo: "/" });
                //}]);
                angular.bootstrap(element, [WebOrderingApp.ApplicationName]);
                WebOrdering.Logger.Notify("bootstrap-WebOrdering-complete");
            };
            Angular.ServicesInitilizations = [];
            Angular.ControllersInitilizations = [];
            return Angular;
        })();
        WebOrdering.Angular = Angular;
    })(WebOrdering = MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
})(MyAndromeda || (MyAndromeda = {}));
/// <reference path="MyAndromeda.WebOrdering.App.ts" />
/// <reference path="MyAndromeda.WebOrdering.App.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var WebOrdering;
    (function (WebOrdering) {
        var Controllers;
        (function (Controllers) {
            WebOrdering.Angular.ControllersInitilizations.push(function (app) {
                app.controller(AnalyticsController.Name, [
                    '$scope', '$timeout',
                    WebOrdering.Services.ContextService.Name,
                    WebOrdering.Services.WebOrderingWebApiService.Name,
                    function ($scope, $timeout, contextService, webOrderingWebApiService) {
                        AnalyticsController.OnLoad($scope, $timeout, contextService, webOrderingWebApiService);
                        /* going to leave kendo to manage the observable object */
                        AnalyticsController.SetupKendoMvvm($scope, $timeout, contextService);
                    }
                ]);
            });
            var AnalyticsController = (function () {
                function AnalyticsController() {
                }
                AnalyticsController.OnLoad = function ($scope, $timout, contextService, webOrderingWebApiService) {
                    $scope.SaveChanges = function () {
                        //console.log("save");                
                        webOrderingWebApiService.Update();
                    };
                    //going to move to just the analytics id 
                    //$scope.EncodeScript = (analyticsScript : string) => {                               
                    //    webOrderingWebApiService.Context.Model.AnalyticsSettings.set("AnalyticsScript", encodeURI(analyticsScript));
                    //};
                };
                AnalyticsController.SetupKendoMvvm = function ($scope, $timout, contextService) {
                    var settingsSubscription = contextService.ModelSubject
                        .where(function (e) { return e !== null; })
                        .subscribe(function (websiteSettings) {
                        var viewElement = $("#AnalyticsController");
                        kendo.bind(viewElement, websiteSettings.AnalyticsSettings);
                        $timout(function () {
                            //want to decode here
                            //websiteSettings.AnalyticsSettings.AnalyticsScript = decodeURI(websiteSettings.AnalyticsSettings.AnalyticsScript);
                            //$scope.AnalyticsScript = websiteSettings.AnalyticsSettings.get("AnalyticsScript")
                        });
                    });
                    $scope.$on('$destroy', function () {
                        settingsSubscription.dispose();
                    });
                };
                AnalyticsController.Name = "AnalyticsController";
                return AnalyticsController;
            })();
            Controllers.AnalyticsController = AnalyticsController;
        })(Controllers = WebOrdering.Controllers || (WebOrdering.Controllers = {}));
    })(WebOrdering = MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
})(MyAndromeda || (MyAndromeda = {}));
/// <reference path="MyAndromeda.WebOrdering.App.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var WebOrdering;
    (function (WebOrdering) {
        var Controllers;
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
                    $scope.CancelItem = function () { };
                    $scope.SaveItem = function () { };
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
                    WebOrdering.Logger.Notify("carouselImageUploadUrl" + carouselImageUploadUrl);
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
                        var carousels = $scope.CarouselBlocks.filter(function (e) { return e.Carousel.Name == carouselName; });
                        var c = carousels[0];
                        //find the carousel item 
                        var m = c.DataSource.view().find(function (item) { return item.Id == carouselItemId; });
                        $scope.HtmlBeforeEdit = m.HTML;
                        switch (m.Type.toLowerCase()) {
                            case "image":
                                _this.LoadImageEditor($scope, $timeout, c.Carousel, m);
                                break;
                            case "html":
                                _this.LoadHtmlEditor($scope, m);
                                break;
                            default: throw kendo.format("{0} type is not supported", m.Type);
                        }
                        $scope.SaveItem = function () {
                            //(c.DataSource.view().find((item: Models.ICarouselItem) => { return item.Id == carouselItemId; })).ImageUrl += "?k=" + r;
                            CarouselController.ResetForm($scope, $timeout, contextService);
                            $scope.SaveChanges();
                        };
                        $scope.CancelItem = function (Id) {
                            (c.DataSource.view().find(function (item) { return item.Id == carouselItemId; })).HTML = $scope.HtmlBeforeEdit;
                            CarouselController.ResetForm($scope, $timeout, contextService);
                            $scope.HtmlBeforeEdit = "";
                        };
                    };
                    $scope.CreateImageCarouselItem = function (carouselName) {
                        var carousels = $scope.CarouselBlocks.filter(function (e) { return e.Carousel.Name == carouselName; });
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
                        var carousels = $scope.CarouselBlocks.filter(function (e) { return e.Carousel.Name == carouselName; });
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
                        var carousels = $scope.CarouselBlocks.filter(function (e) { return e.Carousel.Name == carouselName; });
                        var c = carousels[0];
                        var m = c.DataSource.view().find(function (item) { return item.Id == carouselItemId; });
                        //remove from data source
                        c.DataSource.remove(m);
                        $scope.SaveChanges();
                    };
                    $scope.BindCarouselsList = function () {
                        var toolbar = $("#CarouselHeaderTemplate").html();
                        var editTemplate = $("#CarouselItemEditTemplate").html();
                        var descriptionTemplate = $("#DescriptionTemplate").html();
                        var settingsSubscription = contextService.ModelSubject
                            .where(function (e) { return e !== null; })
                            .subscribe(function (websiteSettings) {
                            $timeout(function () {
                                var carousels = websiteSettings.Home.Carousels;
                                var carouselEditors = carousels.map(function (carousel, index) {
                                    var dataSource = new kendo.data.DataSource({
                                        data: carousel.Items,
                                        schema: {
                                            model: {
                                                id: "Id"
                                            }
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
        })(Controllers = WebOrdering.Controllers || (WebOrdering.Controllers = {}));
    })(WebOrdering = MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var WebOrdering;
    (function (WebOrdering) {
        var Controllers;
        (function (Controllers) {
            WebOrdering.Angular.ControllersInitilizations.push(function (app) {
                app.controller("CmsPagesController", function ($scope, $timeout, contextService, webOrderingWebApiService) {
                    var siteSettings = {
                        Pages: []
                    };
                    contextService.ModelSubject.where(function (e) { return e !== null; }).subscribe(function (settings) {
                        $scope.pages = settings.Pages;
                        siteSettings = settings;
                    });
                    $scope.page = null;
                    $scope.add = function () {
                        var checkExisting = function (title) {
                            var existing = siteSettings.Pages.filter(function (item) { return item.Title === title; });
                            return existing.length > 0;
                        };
                        var newTitle = "Sample page " + (siteSettings.Pages.length + 1);
                        while (checkExisting(newTitle)) {
                            newTitle += "_";
                        }
                        var page = {
                            Title: newTitle,
                            Content: "Sample text",
                            Enabled: true
                        };
                        //$scope.page = page;
                        siteSettings.Pages.push(page);
                        var loading = $("#CmsEditors");
                        kendo.ui.progress(loading, true);
                        $timeout(function () {
                            var loadPage = siteSettings.Pages.filter(function (item) {
                                return item.Title === newTitle;
                            });
                            $scope.page = loadPage[0];
                            kendo.ui.progress(loading, false);
                        }, 500);
                    };
                    $scope.edit = function (page) {
                        $scope.page = page;
                    };
                    $scope.delete = function (page) {
                        if (!confirm("Sure you want to delete this item. There is no way of getting it back")) {
                            return;
                        }
                        siteSettings.Pages = siteSettings.Pages.filter(function (item) {
                            return item.Title !== page.Title;
                        });
                        $scope.pages = siteSettings.Pages;
                        if ($scope.page !== null && $scope.page.Title == page.Title) {
                            $scope.page = null;
                        }
                    };
                    $scope.save = function () {
                        webOrderingWebApiService.Update();
                    };
                });
            });
        })(Controllers = WebOrdering.Controllers || (WebOrdering.Controllers = {}));
    })(WebOrdering = MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
})(MyAndromeda || (MyAndromeda = {}));
/// <reference path="MyAndromeda.WebOrdering.App.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var WebOrdering;
    (function (WebOrdering) {
        var Controllers;
        (function (Controllers) {
            WebOrdering.Angular.ControllersInitilizations.push(function (app) {
                app.controller(CustomBackgroundImagesController.Name, [
                    '$scope', '$timeout',
                    WebOrdering.Services.ContextService.Name,
                    WebOrdering.Services.WebOrderingWebApiService.Name,
                    function ($scope, $timeout, contextService, webOrderingWebApiService) {
                        CustomBackgroundImagesController.OnLoad($scope, $timeout, contextService, webOrderingWebApiService);
                        /* going to leave kendo to manage the observable object */
                        CustomBackgroundImagesController.SetupKendoMvvm($scope, $timeout, contextService);
                    }
                ]);
            });
            var CustomBackgroundImagesController = (function () {
                function CustomBackgroundImagesController() {
                }
                CustomBackgroundImagesController.OnLoad = function ($scope, $timeout, contextService, webOrderingWebApiService) {
                    $scope.SaveChanges = function () {
                        console.log("save");
                        webOrderingWebApiService.Update();
                    };
                    $scope.DeleteCustomBackgroundImage = function (imageType) {
                        var webSiteImageUploadRoute = kendo.format("/api/{0}/AndroWebOrdering/{1}/DeleteBackgroundImage/{2}", WebOrdering.Settings.AndromedaSiteId, WebOrdering.Settings.WebSiteId, imageType);
                        var deleteImage = $.ajax({
                            url: webSiteImageUploadRoute,
                            type: "POST",
                            contentType: 'application/json',
                            dataType: "json"
                        });
                        $timeout(function () {
                            if (imageType == 'desktop') {
                                $scope.HasDesktopBackgroundImage = false;
                                $scope.TempDesktopBackgroundImagePath = contextService.Model.CustomThemeSettings.DesktopBackgroundImagePath = null;
                            }
                            else {
                                $scope.HasMobileBackgroundImage = false;
                                $scope.TempMobileBackgroundImagePath = contextService.Model.CustomThemeSettings.MobileBackgroundImagePath = null;
                            }
                            $scope.SaveChanges();
                        });
                    };
                    $scope.HasDesktopBackgroundImage = false;
                    $scope.HasMobileBackgroundImage = false;
                    $scope.TempDesktopBackgroundImagePath = "";
                    $scope.TempMobileBackgroundImagePath = "";
                };
                CustomBackgroundImagesController.SetupKendoMvvm = function ($scope, $timeout, contextService) {
                    var settingsSubscription = contextService.ModelSubject
                        .where(function (e) { return e !== null; })
                        .subscribe(function (websiteSettings) {
                        var customThemeSettings = websiteSettings.CustomThemeSettings;
                        var viewElement = $("#CustomBackgroundImagesController");
                        kendo.bind(viewElement, websiteSettings.CustomThemeSettings);
                        $timeout(function () {
                            var r = Math.floor(Math.random() * 99999) + 1;
                            $scope.TempDesktopBackgroundImagePath = customThemeSettings.DesktopBackgroundImagePath + "?k=" + r;
                            $scope.TempMobileBackgroundImagePath = customThemeSettings.MobileBackgroundImagePath + "?k=" + r;
                            $scope.HasDesktopBackgroundImage = customThemeSettings.DesktopBackgroundImagePath && customThemeSettings.DesktopBackgroundImagePath.length > 0;
                            $scope.HasMobileBackgroundImage = customThemeSettings.MobileBackgroundImagePath && customThemeSettings.MobileBackgroundImagePath.length > 0;
                        });
                    });
                    var webSiteImageUploadRoute = kendo.format("/api/{0}/AndroWebOrdering/{1}/UploadBackgroundImage/{2}", WebOrdering.Settings.AndromedaSiteId, WebOrdering.Settings.WebSiteId, "desktop");
                    var desktopUpload = $("#DesktopBackgroundUpload").kendoUpload({
                        async: {
                            saveUrl: webSiteImageUploadRoute,
                            autoUpload: true,
                            batch: false
                        },
                        showFileList: false
                    }).data("kendoUpload");
                    desktopUpload.bind("success", function (result) {
                        console.log(result);
                        var observableObject = contextService.Model.CustomThemeSettings;
                        observableObject.set("DesktopBackgroundImagePath", result.response.Url);
                        var r = Math.floor(Math.random() * 99999) + 1;
                        $timeout(function () {
                            $scope.TempDesktopBackgroundImagePath = result.response.Url + "?k=" + r;
                            $scope.HasDesktopBackgroundImage = true;
                        });
                    });
                    var mobileImageUploadRoute = kendo.format("/api/{0}/AndroWebOrdering/{1}/UploadBackgroundImage/{2}", WebOrdering.Settings.AndromedaSiteId, WebOrdering.Settings.WebSiteId, "mobile");
                    var mobileUpload = $("#MobileBackgroundUpload").kendoUpload({
                        async: {
                            saveUrl: mobileImageUploadRoute,
                            autoUpload: true,
                            batch: false
                        },
                        showFileList: false
                    }).data("kendoUpload");
                    mobileUpload.bind("success", function (result) {
                        console.log(result);
                        var observableObject = contextService.Model.CustomThemeSettings;
                        observableObject.set("MobileBackgroundImagePath", result.response.Url);
                        var r = Math.floor(Math.random() * 99999) + 1;
                        $timeout(function () {
                            $scope.TempMobileBackgroundImagePath = result.response.Url + "?k=" + r;
                            $scope.HasMobileBackgroundImage = true;
                        });
                    });
                    $scope.$on('$destroy', function () {
                        settingsSubscription.dispose();
                    });
                };
                CustomBackgroundImagesController.Name = "CustomBackgroundImagesController";
                return CustomBackgroundImagesController;
            })();
            Controllers.CustomBackgroundImagesController = CustomBackgroundImagesController;
        })(Controllers = WebOrdering.Controllers || (WebOrdering.Controllers = {}));
    })(WebOrdering = MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
})(MyAndromeda || (MyAndromeda = {}));
/// <reference path="MyAndromeda.WebOrdering.App.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var WebOrdering;
    (function (WebOrdering) {
        var Controllers;
        (function (Controllers) {
            WebOrdering.Angular.ControllersInitilizations.push(function (app) {
                app.controller("CustomEmailTemplateController", function ($scope, $timeout, contextService, webOrderingWebApiService) {
                    var setLiveColours = function () {
                        WebOrdering.Logger.Notify("Set live colours");
                        contextService.Model.CustomEmailTemplate.LiveHeaderColour =
                            contextService.Model.CustomEmailTemplate.HeaderColour;
                        contextService.Model.CustomEmailTemplate.LiveFooterColour =
                            contextService.Model.CustomEmailTemplate.FooterColour;
                    };
                    var resetColours = function () {
                        WebOrdering.Logger.Notify("Resetting email template colors");
                        var customEmailTemplate = contextService.Model.CustomEmailTemplate;
                        customEmailTemplate.HeaderColour = customEmailTemplate.LiveHeaderColour;
                        customEmailTemplate.FooterColour = customEmailTemplate.LiveFooterColour;
                    };
                    var saveChanges = function () {
                        WebOrdering.Logger.Notify("Save");
                        setLiveColours();
                        webOrderingWebApiService.Update();
                    };
                    $scope.SetLiveColors = setLiveColours;
                    $scope.ResetColors = resetColours;
                    $scope.SaveChanges = saveChanges;
                    contextService.ModelSubject
                        .where(function (e) { return e !== null; })
                        .subscribe(function (websiteSettings) {
                        //set the scope
                        var customEmailTemplate = websiteSettings.CustomEmailTemplate;
                        var correct = function (key, value) {
                            if (!customEmailTemplate.get(key)) {
                                customEmailTemplate.set(key, "#EEEEEE");
                            }
                        };
                        correct("HeaderColour", "HeaderColour");
                        correct("FooterColour", "FooterColour");
                        setLiveColours();
                        $scope.CustomEmailTemplate = websiteSettings.CustomEmailTemplate;
                    });
                });
            });
        })(Controllers = WebOrdering.Controllers || (WebOrdering.Controllers = {}));
    })(WebOrdering = MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
})(MyAndromeda || (MyAndromeda = {}));
/// <reference path="MyAndromeda.WebOrdering.App.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var WebOrdering;
    (function (WebOrdering) {
        var Controllers;
        (function (Controllers) {
            WebOrdering.Angular.ControllersInitilizations.push(function (app) {
                app.controller(CustomerAccountSettingsController.Name, [
                    '$scope', '$timeout',
                    WebOrdering.Services.ContextService.Name,
                    WebOrdering.Services.WebOrderingWebApiService.Name,
                    function ($scope, $timeout, contextService, webOrderingWebApiService) {
                        CustomerAccountSettingsController.OnLoad($scope, $timeout, webOrderingWebApiService);
                        /* going to leave kendo to manage the observable object */
                        CustomerAccountSettingsController.SetupKendoMvvm($scope, contextService);
                    }
                ]);
            });
            var CustomerAccountSettingsController = (function () {
                function CustomerAccountSettingsController() {
                }
                CustomerAccountSettingsController.OnLoad = function ($scope, $timout, webOrderingWebApiService) {
                    $scope.SaveChanges = function () {
                        webOrderingWebApiService.Update();
                    };
                };
                CustomerAccountSettingsController.SetupKendoMvvm = function ($scope, contextService) {
                    var settingsSubscription = contextService.ModelSubject
                        .where(function (e) { return e !== null; })
                        .subscribe(function (websiteSettings) {
                        var viewElement = $("#CustomerAccountSettingsController");
                        kendo.bind(viewElement, websiteSettings.CustomerAccountSettings);
                    });
                    $scope.$on('$destroy', function () {
                        settingsSubscription.dispose();
                    });
                };
                CustomerAccountSettingsController.Name = "CustomerAccountSettingsController";
                return CustomerAccountSettingsController;
            })();
            Controllers.CustomerAccountSettingsController = CustomerAccountSettingsController;
        })(Controllers = WebOrdering.Controllers || (WebOrdering.Controllers = {}));
    })(WebOrdering = MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
})(MyAndromeda || (MyAndromeda = {}));
/// <reference path="MyAndromeda.WebOrdering.App.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var WebOrdering;
    (function (WebOrdering) {
        var Controllers;
        (function (Controllers) {
            WebOrdering.Angular.ControllersInitilizations.push(function (app) {
                app.controller("ThemeSettingsController", function ($scope, $timeout, contextService, webOrderingWebApiService) {
                    contextService.ModelSubject.where(function (e) { return e !== null; }).subscribe(function (model) {
                        if (typeof (model.CustomThemeSettings.IsPageHeaderVisible) === 'undefined') {
                            model.CustomThemeSettings.IsPageHeaderVisible = true;
                        }
                        $timeout(function () {
                            $scope.CustomThemeSettings = model.CustomThemeSettings;
                        });
                    });
                    $scope.SaveChanges = function () {
                        webOrderingWebApiService.Update();
                    };
                });
                app.controller("CustomThemeSettingsController", function ($scope, $timeout, contextService, webOrderingWebApiService) {
                    /* going to leave kendo to manage the observable object */
                    //CustomThemeSettingsController.SetupKendoMvvm($scope, $timeout, ContextService);
                    var defaultColors = {
                        colour1: "#6ac142",
                        colour2: "#ffffff",
                        colour3: "#6ac142",
                        colour4: "#000000",
                        colour5: "#6ac142",
                        colour6: "#070707"
                    };
                    var reset = function () {
                        WebOrdering.Logger.Notify("Resetting colors..");
                        var customThemeSettings = contextService.Model.CustomThemeSettings;
                        customThemeSettings.ColourRange1 = defaultColors.colour1; //customThemeSettings.LiveColourRange1;
                        customThemeSettings.ColourRange2 = defaultColors.colour2; //customThemeSettings.LiveColourRange2;
                        customThemeSettings.ColourRange3 = defaultColors.colour3; //customThemeSettings.LiveColourRange3;
                        customThemeSettings.ColourRange4 = defaultColors.colour4; //customThemeSettings.LiveColourRange4;
                        customThemeSettings.ColourRange5 = defaultColors.colour5; //customThemeSettings.LiveColourRange5;
                        customThemeSettings.ColourRange6 = defaultColors.colour6; //customThemeSettings.LiveColourRange6;
                    };
                    var setLive = function () {
                        var customThemeSettings = contextService.Model.CustomThemeSettings;
                        customThemeSettings.LiveColourRange1 = customThemeSettings.ColourRange1;
                        customThemeSettings.LiveColourRange2 = customThemeSettings.ColourRange2;
                        customThemeSettings.LiveColourRange3 = customThemeSettings.ColourRange3;
                        customThemeSettings.LiveColourRange4 = customThemeSettings.ColourRange4;
                        customThemeSettings.LiveColourRange5 = customThemeSettings.ColourRange5;
                        customThemeSettings.LiveColourRange6 = customThemeSettings.ColourRange6;
                    };
                    var createStyle = function (colour) {
                        return {
                            'background-color': colour
                        };
                    };
                    $scope.CreateStyle = createStyle;
                    $scope.ResetColors = reset;
                    $scope.SetLiveColors = setLive;
                    $scope.SaveChanges = function () {
                        WebOrdering.Logger.Notify("save");
                        setLive();
                        webOrderingWebApiService.Update();
                    };
                    var settingsSubscription = contextService.ModelSubject
                        .where(function (e) { return e !== null; })
                        .subscribe(function (websiteSettings) {
                        var customThemeSettings = websiteSettings.CustomThemeSettings;
                        var correct = function (key, value) {
                            if (!customThemeSettings.get(key)) {
                                customThemeSettings.set(key, null);
                            }
                        };
                        correct("ColourRange1", "ColourRange1");
                        correct("ColourRange2", "ColourRange2");
                        correct("ColourRange3", "ColourRange3");
                        correct("ColourRange4", "ColourRange4");
                        correct("ColourRange5", "ColourRange5");
                        correct("ColourRange6", "ColourRange6");
                        setLive();
                        //var viewElement = $("#CustomThemeSettingsController");
                        //kendo.bind(viewElement, websiteSettings.CustomThemeSettings);
                        $timeout(function () {
                            $scope.CustomThemeSettings = websiteSettings.CustomThemeSettings;
                        });
                    });
                    $scope.$on('$destroy', function () {
                        settingsSubscription.dispose();
                    });
                });
            });
            var CustomThemeSettingsController = (function () {
                function CustomThemeSettingsController() {
                }
                CustomThemeSettingsController.OnLoad = function ($scope, $timout, contextService, webOrderingWebApiService) {
                    $scope.ResetColors = function () {
                        var viewElement = $("#CustomThemeSettingsController");
                        kendo.bind(viewElement, contextService.Model.CustomThemeSettings);
                    };
                    $scope.SaveChanges = function () {
                        WebOrdering.Logger.Notify("save");
                        $scope.SetLiveColors();
                        webOrderingWebApiService.Update();
                        var viewElement = $("#CustomThemeSettingsController");
                        kendo.bind(viewElement, contextService.Model.CustomThemeSettings);
                    };
                    $scope.SetLiveColors = function () {
                    };
                };
                CustomThemeSettingsController.Name = "CustomThemeSettingsController";
                return CustomThemeSettingsController;
            })();
            Controllers.CustomThemeSettingsController = CustomThemeSettingsController;
        })(Controllers = WebOrdering.Controllers || (WebOrdering.Controllers = {}));
    })(WebOrdering = MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var WebOrdering;
    (function (WebOrdering) {
        var Controllers;
        (function (Controllers) {
            WebOrdering.Angular.ControllersInitilizations.push(function (app) {
                /* Store editor section */
                app.directive("storeEmailSection", function () {
                    WebOrdering.Logger.Notify("directive loaded");
                    return {
                        restrict: "E",
                        transclude: true,
                        template: $("#StoreEmailSection").html(),
                        //templateUrl: "#StoreEmailSection",
                        require: '^ngModel',
                        scope: {
                            ngModel: "="
                        },
                        link: function (scope, element, attrs, controller, transclude) {
                            //transclude(scope, function (clone, scope) {
                            //    element.append(clone);
                            //});
                        },
                        controller: function ($scope, $timeout, contextService) {
                            var store = $scope.ngModel;
                            var lookup = function (store) {
                                var pages = contextService.Model.CustomEmailTemplate.CustomTemplates[store.AndromedaSiteId];
                                var context = {
                                    Store: store,
                                    Pages: pages,
                                    Page: pages[0]
                                };
                                return context;
                            };
                            var context = lookup(store);
                            $scope.context = context;
                            WebOrdering.Logger.Notify("s: ");
                            WebOrdering.Logger.Notify(context);
                            $scope.edit = function (page) {
                                context.Page = page;
                            };
                            $scope.delete = function (page) {
                                if (!confirm("Sure you want to delete this item. There is no way of getting it back")) {
                                    return;
                                }
                                context.Pages = context.Pages.filter(function (item) {
                                    return item.Title !== page.Title;
                                });
                                if (context.Page !== null && context.Page.Title == page.Title) {
                                    context.Page = null;
                                }
                            };
                        }
                    };
                });
                app.controller("EmailCmsPagesController", function ($scope, $timeout, contextService, webOrderingWebApiService) {
                    var settings = contextService.ModelSubject.where(function (e) { return e !== null; });
                    var stores = contextService.StoreSubject.where(function (e) { return e.length > 0; });
                    var emailSettings = {
                        Pages: [
                            //region on the email to inject content into, if enabled. 
                            { Title: "Custom Content", Content: "", Enabled: false }
                        ]
                    };
                    $scope.stores = [];
                    $scope.page = null;
                    var addSectionsForStore = function (store, sections) {
                        Rx.Observable.fromArray(emailSettings.Pages).subscribe(function (page) {
                            //if its got it ... don't care. 
                            if (sections.filter(function (item) { return item.Title === page.Title; }).length > 0) {
                                return;
                            }
                            var newPage = JSON.parse(JSON.stringify(page));
                            //add any missing email sections. 
                            sections.push(newPage);
                        });
                    };
                    var both = Rx.Observable
                        .zip(settings, stores, function (settings, stores) {
                        return { settings: settings, stores: stores };
                    })
                        .subscribe(function (storesAndSettings) {
                        var settings = storesAndSettings.settings.CustomEmailTemplate;
                        for (var i = 0; i < storesAndSettings.stores.length; i++) {
                            var store = storesAndSettings.stores[i], storeId = store.AndromedaSiteId;
                            if (!settings.CustomTemplates[storeId]) {
                                settings.CustomTemplates[storeId] = [];
                            }
                            var sections = settings.CustomTemplates[storeId];
                            addSectionsForStore(store, sections);
                        }
                        //Logger.Notify("Stores in email settings: " settings.CustomTemplates.);
                    });
                    stores.subscribe(function (storeList) {
                        $scope.stores = storeList;
                    });
                    $scope.SaveChanges = function () {
                        webOrderingWebApiService.Update();
                    };
                });
            });
        })(Controllers = WebOrdering.Controllers || (WebOrdering.Controllers = {}));
    })(WebOrdering = MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
})(MyAndromeda || (MyAndromeda = {}));
/// <reference path="MyAndromeda.WebOrdering.App.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var WebOrdering;
    (function (WebOrdering) {
        var Controllers;
        (function (Controllers) {
            WebOrdering.Angular.ControllersInitilizations.push(function (app) {
                app.controller(FacebookCrawlerSettingsController.Name, [
                    '$scope', '$timeout',
                    WebOrdering.Services.ContextService.Name,
                    WebOrdering.Services.WebOrderingWebApiService.Name,
                    function ($scope, $timeout, contextService, webOrderingWebApiService) {
                        FacebookCrawlerSettingsController.OnLoad($scope, $timeout, contextService, webOrderingWebApiService);
                        /* going to leave kendo to manage the observable object */
                        FacebookCrawlerSettingsController.SetupKendoMvvm($scope, $timeout, contextService);
                    }
                ]);
            });
            var FacebookCrawlerSettingsController = (function () {
                function FacebookCrawlerSettingsController() {
                }
                FacebookCrawlerSettingsController.OnLoad = function ($scope, $timeout, contextService, webOrderingWebApiService) {
                    $scope.SaveChanges = function () {
                        if (contextService.Model.FacebookCrawlerSettings.FacebookProfileLogoPath == null || contextService.Model.FacebookCrawlerSettings.FacebookProfileLogoPath == "") {
                            $("#FacebookCrawlerLogo").attr("required", "required");
                            $("#FacebookCrawlerLogo").attr("title", "Please select an image");
                        }
                        else {
                            if ($scope.FacebookCrawlerSettingsValidator.validate()) {
                                webOrderingWebApiService.Update();
                            }
                        }
                    };
                    $scope.HasFacebookProfileLogo = false;
                    $scope.TempFaceboolProfileLogoPath = "";
                };
                FacebookCrawlerSettingsController.SetupUploaders = function ($scope, $timeout, contextService) {
                    var facebookImageUploadRoute = kendo.format("/api/{0}/AndroWebOrdering/{1}/UploadFacebookLogo/{2}", WebOrdering.Settings.AndromedaSiteId, WebOrdering.Settings.WebSiteId, "FacebookCrawler");
                    $scope.FacebookProfileLogoUpload.setOptions({ async: { saveUrl: facebookImageUploadRoute, autoUpload: true }, showFileList: false, multiple: false });
                    $scope.FacebookProfileLogoUpload.bind("success", function (result) {
                        console.log(result);
                        var observableObject = contextService.Model.FacebookCrawlerSettings;
                        observableObject.set("FacebookProfileLogoPath", result.response.Url);
                        var r = Math.floor(Math.random() * 99999) + 1;
                        $timeout(function () {
                            $scope.TempFaceboolProfileLogoPath = result.response.Url + "?k=" + r;
                            contextService.Model.FacebookCrawlerSettings.FacebookProfileLogoPath = result.response.Url;
                            $("#FacebookCrawlerLogo").removeAttr("required");
                            $("#FacebookCrawlerLogo").removeAttr("title");
                            $scope.HasFacebookProfileLogo = true;
                        });
                    });
                };
                FacebookCrawlerSettingsController.SetupKendoMvvm = function ($scope, $timeout, contextService) {
                    var _this = this;
                    var settingsSubscription = contextService.ModelSubject
                        .where(function (e) { return e !== null; })
                        .subscribe(function (webSiteSettings) {
                        var viewElement = $("#FacebookCrawlerSettingsController");
                        kendo.bind(viewElement, webSiteSettings.FacebookCrawlerSettings);
                        //added 500ms timeout as there are random issues. 
                        $timeout(function () {
                            _this.SetupUploaders($scope, $timeout, contextService);
                            var r = Math.floor(Math.random() * 99999) + 1;
                            $scope.TempFaceboolProfileLogoPath = webSiteSettings.FacebookCrawlerSettings.FacebookProfileLogoPath + "?k=" + r;
                            $scope.HasFacebookProfileLogo = webSiteSettings.FacebookCrawlerSettings.FacebookProfileLogoPath && webSiteSettings.FacebookCrawlerSettings.FacebookProfileLogoPath.length > 0;
                        }, 500, true);
                    });
                    $scope.$on('$destroy', function () {
                        settingsSubscription.dispose();
                    });
                };
                FacebookCrawlerSettingsController.Name = "FacebookCrawlerSettingsController";
                return FacebookCrawlerSettingsController;
            })();
            Controllers.FacebookCrawlerSettingsController = FacebookCrawlerSettingsController;
        })(Controllers = WebOrdering.Controllers || (WebOrdering.Controllers = {}));
    })(WebOrdering = MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
})(MyAndromeda || (MyAndromeda = {}));
/// <reference path="MyAndromeda.WebOrdering.App.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var WebOrdering;
    (function (WebOrdering) {
        var Controllers;
        (function (Controllers) {
            WebOrdering.Angular.ControllersInitilizations.push(function (app) {
                app.controller("GeneralSettingsController", function ($scope, $timeout, contextService, webOrderingWebApiService) {
                    GeneralSettingsController.OnLoad($scope, $timeout, contextService, webOrderingWebApiService);
                    /* going to leave kendo to manage the observable object */
                    GeneralSettingsController.SetupKendoMvvm($scope, $timeout, contextService);
                });
            });
            var GeneralSettingsController = (function () {
                function GeneralSettingsController() {
                }
                GeneralSettingsController.OnLoad = function ($scope, $timeout, contextService, webOrderingWebApiService) {
                    $scope.SaveChanges = function () {
                        if ($scope.GeneralSettingsValidator.validate()) {
                            webOrderingWebApiService.Update();
                        }
                    };
                    $scope.ResetToDefault = function () {
                        if (confirm("Are you sure you want to update general settings")) {
                            contextService.Model.set("GeneralSettings", kendo.observable(GeneralSettingsController.GeneralSettingsDefault));
                            $scope.MinimumDeliveryAmount = 0;
                        }
                        if (confirm("Are you sure you want to update the customer account settings")) {
                            contextService.Model.set("CustomerAccountSettings", kendo.observable(GeneralSettingsController.CustomerAccountsDefault));
                            GeneralSettingsController.WatchForValidLoginSettings($scope, $timeout, contextService.Model.get("CustomerAccountSettings"));
                            GeneralSettingsController.ShowFacebookIdInput($scope, $timeout, false);
                        }
                    };
                };
                GeneralSettingsController.SetupKendoMvvm = function ($scope, $timeout, contextService) {
                    //$scope.$watch("$scope.DineInServiceChargeLimit",)
                    var getDineInServiceCharge = function () {
                        var element = $("#DineInServiceCharge");
                        return element.data("kendoNumericTextBox");
                    };
                    var getDineInServiceChargeLimit = function () {
                        var element = $("#LegalDineInServiceChargeLimit");
                        return element.data("kendoNumericTextBox");
                    };
                    var settingsSubscription = contextService.ModelSubject
                        .where(function (e) { return e !== null; })
                        .subscribe(function (websiteSettings) {
                        var viewElement = $("#GeneralSettingsController");
                        var generalSettings = websiteSettings.GeneralSettings;
                        var customerAccountSettings = websiteSettings.CustomerAccountSettings;
                        /* todo remove the rest of the kendo mvvm binding with the angular ones*/
                        kendo.bind(viewElement, websiteSettings);
                        var minDeliveryValue = websiteSettings.GeneralSettings.get("MinimumDeliveryAmount");
                        $scope.MinimumDeliveryAmount = minDeliveryValue ? minDeliveryValue / 100 : 0;
                        var visible = customerAccountSettings.get("EnableFacebookLogin");
                        GeneralSettingsController.ShowFacebookIdInput($scope, $timeout, visible);
                        GeneralSettingsController.WatchForValidLoginSettings($scope, $timeout, customerAccountSettings);
                        $scope.JivoChatSettings = websiteSettings.JivoChatSettings;
                        $scope.GeneralSettings = websiteSettings.GeneralSettings;
                    });
                    $scope.DineInServiceChargeOptions = {
                        min: 0
                    };
                    $scope.DineInServiceChargeLimitOptions = {
                        min: 0,
                        change: function () {
                            WebOrdering.Logger.Notify("limit change");
                            var charge = getDineInServiceCharge();
                            var limit = getDineInServiceChargeLimit();
                            if (limit.value() < charge.value()) {
                                //some reason is not updating the model...
                                charge.value(limit.value());
                                //force model update
                                $scope.GeneralSettings.set("DineInServiceCharge", limit.value());
                            }
                            charge.max(limit.value());
                        }
                    };
                    $scope.$on('$destroy', function () {
                        settingsSubscription.dispose();
                    });
                };
                GeneralSettingsController.ShowFacebookIdInput = function ($scope, $timeout, visible) {
                    $timeout(function () {
                        $scope.ShowFacebookAppId = visible;
                    });
                };
                GeneralSettingsController.WatchForValidLoginSettings = function ($scope, $timeout, customerAccountSettings) {
                    $timeout(function () {
                        var hasFacebookLogin = customerAccountSettings.get("EnableFacebookLogin");
                        var hasAndromedaLogin = customerAccountSettings.get("EnableAndromedaLogin");
                        $scope.HasLoginOptions = hasAndromedaLogin || hasFacebookLogin;
                        console.log("$scope.HasLoginOptions: " + $scope.HasLoginOptions);
                    });
                    customerAccountSettings.bind("change", function (e) {
                        if (e.field === "EnableFacebookLogin" || e.field === "EnableAndromedaLogin") {
                            var hasFacebookLogin = customerAccountSettings.get("EnableFacebookLogin");
                            var hasAndromedaLogin = customerAccountSettings.get("EnableAndromedaLogin");
                            $timeout(function () {
                                $scope.HasLoginOptions = hasAndromedaLogin || hasFacebookLogin;
                            });
                        }
                    });
                    $scope.$watch("HasLoginOptions", function (newValue, oldValue) {
                        customerAccountSettings.set("IsEnable", newValue);
                    });
                };
                GeneralSettingsController.Name = "GeneralSettingsController";
                GeneralSettingsController.GeneralSettingsDefault = {
                    EnableHomePage: true,
                    MinimumDeliveryAmount: 0
                };
                GeneralSettingsController.CustomerAccountsDefault = {
                    IsEnable: true,
                    EnableAndromedaLogin: true,
                    EnableFacebookLogin: true
                };
                return GeneralSettingsController;
            })();
            Controllers.GeneralSettingsController = GeneralSettingsController;
        })(Controllers = WebOrdering.Controllers || (WebOrdering.Controllers = {}));
    })(WebOrdering = MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
})(MyAndromeda || (MyAndromeda = {}));
/// <reference path="MyAndromeda.WebOrdering.App.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var WebOrdering;
    (function (WebOrdering) {
        var Controllers;
        (function (Controllers) {
            WebOrdering.Angular.ControllersInitilizations.push(function (app) {
                app.controller(HomePageController.Name, [
                    '$scope', '$timeout',
                    WebOrdering.Services.ContextService.Name,
                    WebOrdering.Services.WebOrderingWebApiService.Name,
                    function ($scope, $timeout, contextService, webOrderingWebApiService) {
                        HomePageController.OnLoad($scope, $timeout, webOrderingWebApiService);
                        /* going to leave kendo to manage the observable object */
                        HomePageController.SetupKendoMvvm($scope, contextService);
                    }
                ]);
            });
            var HomePageController = (function () {
                function HomePageController() {
                }
                HomePageController.OnLoad = function ($scope, $timout, webOrderingWebApiService) {
                    $scope.SaveChanges = function () {
                        webOrderingWebApiService.Update();
                    };
                };
                HomePageController.SetupKendoMvvm = function ($scope, contextService) {
                    var settingsSubscription = contextService.ModelSubject
                        .where(function (e) { return e !== null; })
                        .subscribe(function (websiteSettings) {
                        var viewElement = $("#HomePageController");
                        var model = websiteSettings.Home;
                        kendo.bind(viewElement, model);
                    });
                    $scope.$on('$destroy', function () {
                        settingsSubscription.dispose();
                    });
                };
                HomePageController.Name = "HomePageController";
                return HomePageController;
            })();
            Controllers.HomePageController = HomePageController;
        })(Controllers = WebOrdering.Controllers || (WebOrdering.Controllers = {}));
    })(WebOrdering = MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
})(MyAndromeda || (MyAndromeda = {}));
/// <reference path="MyAndromeda.WebOrdering.App.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var WebOrdering;
    (function (WebOrdering) {
        var Controllers;
        (function (Controllers) {
            WebOrdering.Angular.ControllersInitilizations.push(function (app) {
                app.controller(LegalNoticesController.Name, [
                    '$scope', '$timeout',
                    WebOrdering.Services.ContextService.Name,
                    WebOrdering.Services.WebOrderingWebApiService.Name,
                    function ($scope, $timeout, contextService, webOrderingWebApiService) {
                        LegalNoticesController.OnLoad($scope, $timeout, webOrderingWebApiService);
                        /* going to leave kendo to manage the observable object */
                        LegalNoticesController.SetupKendoMvvm($scope, contextService);
                    }
                ]);
            });
            var LegalNoticesController = (function () {
                function LegalNoticesController() {
                }
                LegalNoticesController.OnLoad = function ($scope, $timout, webOrderingWebApiService) {
                    $scope.SaveChanges = function () {
                        webOrderingWebApiService.Update();
                    };
                };
                LegalNoticesController.SetupKendoMvvm = function ($scope, contextService) {
                    var settingsSubscription = contextService.ModelSubject
                        .where(function (e) { return e !== null; })
                        .subscribe(function (websiteSettings) {
                        var viewElement = $("#LegalNoticesController");
                        kendo.bind(viewElement, websiteSettings.LegalNotices);
                    });
                    $scope.$on('$destroy', function () {
                        settingsSubscription.dispose();
                    });
                };
                LegalNoticesController.Name = "LegalNoticesController";
                return LegalNoticesController;
            })();
            Controllers.LegalNoticesController = LegalNoticesController;
        })(Controllers = WebOrdering.Controllers || (WebOrdering.Controllers = {}));
    })(WebOrdering = MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
})(MyAndromeda || (MyAndromeda = {}));
/// <reference path="MyAndromeda.WebOrdering.App.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var WebOrdering;
    (function (WebOrdering) {
        var Controllers;
        (function (Controllers) {
            WebOrdering.Angular.ControllersInitilizations.push(function (app) {
                app.controller(PickThemeController.Name, [
                    '$scope', '$timeout',
                    WebOrdering.Services.ContextService.Name,
                    WebOrdering.Services.WebOrderingWebApiService.Name,
                    WebOrdering.Services.WebOrderingThemeWebApiService.Name,
                    function ($scope, $timeout, contextService, webOrderingWebApiService, webOrderingThemeWebApiService) {
                        PickThemeController.OnLoad($scope, $timeout, webOrderingWebApiService, webOrderingThemeWebApiService);
                        PickThemeController.SetupScope($scope);
                        PickThemeController.SetupSelection($scope, webOrderingWebApiService);
                        PickThemeController.SetupCurrentSelection($scope, $timeout, contextService);
                    }
                ]);
            });
            var PickThemeController = (function () {
                function PickThemeController() {
                }
                PickThemeController.OnLoad = function ($scope, $timout, webOrderingWebApiService, webOrderingThemeWebApiService) {
                    var isThemesBusySubscription = webOrderingThemeWebApiService.IsBusy.subscribe(function (value) {
                        $timout(function () {
                            $scope.IsThemesBusy = value;
                        });
                    });
                    var isDataBusySubscription = webOrderingWebApiService.IsLoading.subscribe(function (value) {
                        $timout(function () {
                            $scope.IsDataBusy = value;
                        });
                    });
                    $scope.ListViewTemplate = $("#ListViewTemplate").html();
                    $scope.DataSource = webOrderingThemeWebApiService.GetThemeDataSource();
                    $scope.HasPreviewTheme = false;
                    $scope.HasCurrentTheme = false;
                    $scope.SearchTemplates = function () {
                        webOrderingThemeWebApiService.SearchText($scope.SearchText);
                    };
                    $scope.$on('$destroy', function () {
                        isThemesBusySubscription.dispose();
                    });
                };
                PickThemeController.SetupCurrentSelection = function ($scope, $timout, contextService) {
                    var modelSubscription = contextService.ModelSubject.where(function (value) {
                        return value !== null;
                    }).subscribe(function (settings) {
                        var s = settings;
                        s.bind("change", function () {
                            $timout(function () {
                                //console.log("set current theme settings");
                                console.log(settings.ThemeSettings);
                                $scope.CurrentTheme = settings.ThemeSettings;
                                $scope.HasCurrentTheme = true;
                            });
                        });
                        console.log(settings.ThemeSettings);
                        $scope.CurrentTheme = settings.ThemeSettings;
                        $scope.HasCurrentTheme = true;
                    });
                    $scope.$on("$destroy", function () {
                        modelSubscription.dispose();
                    });
                };
                PickThemeController.SetupSelection = function ($scope, webOrderingWebApiService) {
                    $scope.SelectTemplate = function (id) {
                        var dataSource = $scope.DataSource;
                        var previewItem = dataSource.data().find(function (item) {
                            return item.Id === id;
                        });
                        $scope.HasPreviewTheme = true;
                        $scope.SelectedTheme = previewItem;
                    };
                    $scope.SelectPreviewTheme = function (theme) {
                        console.log(theme);
                        webOrderingWebApiService.UpdateThemeSettings(theme);
                    };
                };
                PickThemeController.SetupScope = function ($scope) {
                    $scope.$on('$destroy', function () { });
                };
                PickThemeController.Name = "PickThemeController";
                PickThemeController.Route = "/";
                return PickThemeController;
            })();
            Controllers.PickThemeController = PickThemeController;
        })(Controllers = WebOrdering.Controllers || (WebOrdering.Controllers = {}));
    })(WebOrdering = MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
})(MyAndromeda || (MyAndromeda = {}));
/// <reference path="MyAndromeda.WebOrdering.App.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var WebOrdering;
    (function (WebOrdering) {
        var Controllers;
        (function (Controllers) {
            WebOrdering.Angular.ControllersInitilizations.push(function (app) {
                app.controller(SEOSettingsController.Name, [
                    '$scope', '$timeout',
                    WebOrdering.Services.ContextService.Name,
                    WebOrdering.Services.WebOrderingWebApiService.Name,
                    function ($scope, $timeout, contextService, webOrderingWebApiService) {
                        SEOSettingsController.OnLoad($scope, $timeout, contextService, webOrderingWebApiService);
                        /* going to leave kendo to manage the observable object */
                        SEOSettingsController.SetupKendoMvvm($scope, $timeout, contextService);
                    }
                ]);
            });
            var SEOSettingsController = (function () {
                function SEOSettingsController() {
                }
                SEOSettingsController.OnLoad = function ($scope, $timeout, contextService, webOrderingWebApiService) {
                    $scope.SaveChanges = function () {
                        if ($scope.SEOSettingsValidator.validate()) {
                            webOrderingWebApiService.Update();
                        }
                    };
                };
                SEOSettingsController.SetupKendoMvvm = function ($scope, $timeout, contextService) {
                    var settingsSubscription = contextService.ModelSubject
                        .where(function (e) { return e !== null; })
                        .subscribe(function (webSiteSettings) {
                        var viewElement = $("#SEOSettingsController");
                        kendo.bind(viewElement, webSiteSettings.SEOSettings);
                        $scope.ShowSEODescription = webSiteSettings.SEOSettings.get("IsEnableDescription");
                        //added 500ms timeout as there are random issues. 
                        $timeout(function () {
                        }, 500, true);
                    });
                    $scope.$on('$destroy', function () {
                        settingsSubscription.dispose();
                    });
                };
                SEOSettingsController.Name = "SEOSettingsController";
                return SEOSettingsController;
            })();
            Controllers.SEOSettingsController = SEOSettingsController;
        })(Controllers = WebOrdering.Controllers || (WebOrdering.Controllers = {}));
    })(WebOrdering = MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
})(MyAndromeda || (MyAndromeda = {}));
/// <reference path="MyAndromeda.WebOrdering.App.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var WebOrdering;
    (function (WebOrdering) {
        var Controllers;
        (function (Controllers) {
            WebOrdering.Angular.ControllersInitilizations.push(function (app) {
                app.controller(SiteDetailsController.Name, [
                    '$scope', '$timeout',
                    WebOrdering.Services.ContextService.Name,
                    WebOrdering.Services.WebOrderingWebApiService.Name,
                    function ($scope, $timeout, contextService, webOrderingWebApiService) {
                        SiteDetailsController.OnLoad($scope, $timeout, contextService, webOrderingWebApiService);
                        /* going to leave kendo to manage the observable object */
                        SiteDetailsController.SetupKendoMvvm($scope, $timeout, contextService);
                    }
                ]);
            });
            var SiteDetailsController = (function () {
                function SiteDetailsController() {
                }
                SiteDetailsController.OnLoad = function ($scope, $timeout, contextService, webOrderingWebApiService) {
                    $scope.SaveChanges = function () {
                        if ($scope.SiteDetailsValidator.validate()) {
                            webOrderingWebApiService.Update();
                        }
                    };
                    $scope.HasWebsiteLogo = false;
                    $scope.HasMobileLogo = false;
                    $scope.TempWebsiteLogoPath = "";
                    $scope.TempMobileLogoPath = "";
                };
                SiteDetailsController.SetupUploaders = function ($scope, $timeout, contextService) {
                    if (!$scope.MainImageUpload) {
                        alert("MainImageUpload hasn't been created. Pester Matt");
                    }
                    if (!$scope.MobileImageUpload) {
                        alert("MobileImageUpload hasn't been created. Pester Matt");
                    }
                    var webSiteImageUploadRoute = kendo.format("/api/{0}/AndroWebOrdering/{1}/UploadLogo/{2}", WebOrdering.Settings.AndromedaSiteId, WebOrdering.Settings.WebSiteId, "website");
                    $scope.MainImageUpload.setOptions({
                        async: {
                            saveUrl: webSiteImageUploadRoute,
                            autoUpload: true
                        },
                        showFileList: false,
                        multiple: false
                    });
                    var mobileImageUploadRoute = kendo.format("/api/{0}/AndroWebOrdering/{1}/UploadLogo/{2}", WebOrdering.Settings.AndromedaSiteId, WebOrdering.Settings.WebSiteId, "mobile");
                    $scope.MobileImageUpload.setOptions({
                        async: {
                            saveUrl: mobileImageUploadRoute,
                            autoUpload: true
                        },
                        showFileList: false,
                        multiple: false
                    });
                    var FaviconUploadRoute = kendo.format("/api/{0}/AndroWebOrdering/{1}/UploadLogo/{2}", WebOrdering.Settings.AndromedaSiteId, WebOrdering.Settings.WebSiteId, "favicon");
                    $scope.FaviconImageUpload.setOptions({
                        async: {
                            saveUrl: FaviconUploadRoute,
                            autoUpload: true
                        },
                        showFileList: false,
                        multiple: false
                    });
                    $scope.MainImageUpload.bind("success", function (result) {
                        WebOrdering.Logger.Notify(result);
                        var observableObject = contextService.Model.SiteDetails;
                        observableObject.set("WebsiteLogoPath", result.response.Url);
                        var r = Math.floor(Math.random() * 99999) + 1;
                        $timeout(function () {
                            $scope.TempWebsiteLogoPath = result.response.Url + "?k=" + r;
                            $scope.HasWebsiteLogo = true;
                        });
                    });
                    $scope.MobileImageUpload.bind("success", function (result) {
                        WebOrdering.Logger.Notify(result);
                        var observableObject = contextService.Model.SiteDetails;
                        observableObject.set("MobileLogoPath", result.response.Url);
                        var r = Math.floor(Math.random() * 99999) + 1;
                        $timeout(function () {
                            $scope.TempMobileLogoPath = result.response.Url + "?k=" + r;
                            $scope.HasMobileLogo = true;
                        });
                    });
                    $scope.FaviconImageUpload.bind("success", function (result) {
                        console.log(result);
                        var observableObject = contextService.Model.SiteDetails;
                        observableObject.set("FaviconPath", result.response.Url);
                        var r = Math.floor(Math.random() * 99999) + 1;
                        $timeout(function () {
                            $scope.TempFaviconLogoPath = result.response.Url + "?k=" + r;
                            $scope.HasFaviconLogo = true;
                        });
                    });
                };
                SiteDetailsController.SetupKendoMvvm = function ($scope, $timeout, contextService) {
                    var _this = this;
                    var settingsSubscription = contextService.ModelSubject
                        .where(function (e) { return e !== null; })
                        .subscribe(function (webSiteSettings) {
                        var viewElement = $("#SiteDetailsController");
                        kendo.bind(viewElement, webSiteSettings.SiteDetails);
                        $scope.WebSiteSettings = webSiteSettings;
                        $scope.SiteDetails = webSiteSettings.SiteDetails;
                        //added 500ms timeout as there are random issues. 
                        $timeout(function () {
                            _this.SetupUploaders($scope, $timeout, contextService);
                            var r = Math.floor(Math.random() * 99999) + 1;
                            if (webSiteSettings.SiteDetails.WebsiteLogoPath && webSiteSettings.SiteDetails.WebsiteLogoPath !== null) {
                                $scope.TempWebsiteLogoPath = webSiteSettings.SiteDetails.WebsiteLogoPath + "?k=" + r;
                            }
                            if (webSiteSettings.SiteDetails.MobileLogoPath && webSiteSettings.SiteDetails.MobileLogoPath !== null) {
                                $scope.TempMobileLogoPath = webSiteSettings.SiteDetails.MobileLogoPath + "?k=" + r;
                            }
                            if (webSiteSettings.SiteDetails.FaviconPath && webSiteSettings.SiteDetails.FaviconPath !== null) {
                                $scope.TempFaviconLogoPath = webSiteSettings.SiteDetails.FaviconPath + "?k=" + r;
                            }
                            $scope.HasWebsiteLogo = webSiteSettings.SiteDetails.WebsiteLogoPath && webSiteSettings.SiteDetails.WebsiteLogoPath.length > 0;
                            $scope.HasMobileLogo = webSiteSettings.SiteDetails.MobileLogoPath && webSiteSettings.SiteDetails.MobileLogoPath.length > 0;
                            $scope.HasFaviconLogo = webSiteSettings.SiteDetails.FaviconPath && webSiteSettings.SiteDetails.FaviconPath.length > 0;
                        }, 500, true);
                    });
                    $scope.$on('$destroy', function () {
                        settingsSubscription.dispose();
                    });
                };
                SiteDetailsController.Name = "SiteDetailsController";
                return SiteDetailsController;
            })();
            Controllers.SiteDetailsController = SiteDetailsController;
        })(Controllers = WebOrdering.Controllers || (WebOrdering.Controllers = {}));
    })(WebOrdering = MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
})(MyAndromeda || (MyAndromeda = {}));
/// <reference path="MyAndromeda.WebOrdering.App.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var WebOrdering;
    (function (WebOrdering) {
        var Controllers;
        (function (Controllers) {
            WebOrdering.Angular.ControllersInitilizations.push(function (app) {
                app.controller(SocialNetworkSettingsController.Name, [
                    '$scope', '$timeout',
                    WebOrdering.Services.ContextService.Name,
                    WebOrdering.Services.WebOrderingWebApiService.Name,
                    function ($scope, $timeout, contextService, webOrderingWebApiService) {
                        SocialNetworkSettingsController.SetupValidatorOptions($scope, $timeout, webOrderingWebApiService);
                        SocialNetworkSettingsController.OnLoad($scope, $timeout, webOrderingWebApiService);
                        /* going to leave kendo to manage the observable object */
                        SocialNetworkSettingsController.SetupKendoMvvm($scope, $timeout, contextService);
                    }
                ]);
            });
            var SocialNetworkSettingsController = (function () {
                function SocialNetworkSettingsController() {
                }
                SocialNetworkSettingsController.OnLoad = function ($scope, $timout, webOrderingWebApiService) {
                    //$scope.SocialNetworkSettingsValidator.ru
                    $scope.SaveChanges = function () {
                        if ($scope.SocialNetworkSettingsValidator.validate()) {
                            webOrderingWebApiService.Update();
                        }
                    };
                    var s = $scope;
                    s.FacebookSettings = {};
                };
                SocialNetworkSettingsController.SetupValidatorOptions = function ($scope, $timout, contextService) {
                    var validatorOptions = {
                        name: "",
                        rules: {
                            FacebookUrlRequired: function (input) {
                                if (!input.is("[data-required-if-facebook]")) {
                                    return true;
                                }
                                var isEnabled = contextService.Model.SocialNetworkSettings.FacebookSettings.get("IsEnable");
                                var text = input.val();
                                return text.length > 0;
                            },
                            TwitterRequired: function (intput) { }
                        }
                    };
                };
                SocialNetworkSettingsController.SetupKendoMvvm = function ($scope, $timout, contextService) {
                    var settingsSubscription = contextService.ModelSubject
                        .where(function (e) { return e !== null; })
                        .subscribe(function (websiteSettings) {
                        var viewElement = $("#SocialNetworkSettingsController");
                        kendo.bind(viewElement, websiteSettings.SocialNetworkSettings);
                        $scope.SocialNetworkSettings = websiteSettings.SocialNetworkSettings;
                        $scope.GeneralSettings = websiteSettings.GeneralSettings;
                        $scope.CustomerAccountSettings = websiteSettings.CustomerAccountSettings;
                        $scope.ShowFacebookSettings = websiteSettings.SocialNetworkSettings.FacebookSettings.get("IsEnable");
                        $scope.ShowTwitterSettings = websiteSettings.SocialNetworkSettings.TwitterSettings.get("IsEnable");
                        $scope.ShowInstagramSettings = websiteSettings.SocialNetworkSettings.InstagramSettings.get("IsEnable");
                        // $scope.ShowTripAdvisorSettings = websiteSettings.TripAdvisorSettings.get("IsEnable");
                    });
                    $scope.$on('$destroy', function () {
                        settingsSubscription.dispose();
                    });
                };
                SocialNetworkSettingsController.Name = "SocialNetworkSettingsController";
                return SocialNetworkSettingsController;
            })();
            Controllers.SocialNetworkSettingsController = SocialNetworkSettingsController;
        })(Controllers = WebOrdering.Controllers || (WebOrdering.Controllers = {}));
    })(WebOrdering = MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
})(MyAndromeda || (MyAndromeda = {}));
/// <reference path="MyAndromeda.WebOrdering.App.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var WebOrdering;
    (function (WebOrdering) {
        var Controllers;
        (function (Controllers) {
            WebOrdering.Angular.ControllersInitilizations.push(function (app) {
                app.controller(StatusController.Name, [
                    '$scope', '$timeout',
                    WebOrdering.Services.ContextService.Name,
                    WebOrdering.Services.WebOrderingWebApiService.Name,
                    function ($scope, $timeout, contextService, webOrderingWebApiService) {
                        StatusController.OnLoad($scope, $timeout, webOrderingWebApiService);
                    }
                ]);
            });
            var StatusController = (function () {
                function StatusController() {
                }
                StatusController.OnLoad = function ($scope, $timout, webOrderingWebApiService) {
                    $scope.SaveChanges = function () {
                        webOrderingWebApiService.Update();
                    };
                    $scope.PublishChanges = function () {
                        webOrderingWebApiService.Publish();
                    };
                    $scope.PreviewChanges = function () {
                        webOrderingWebApiService.Preview();
                    };
                    webOrderingWebApiService.IsSaving.subscribe(function (e) {
                        $timout(function () {
                            $scope.Saving = e;
                        });
                    });
                    webOrderingWebApiService.IsPublishPreviewBusy.subscribe(function (e) {
                        $timout(function () {
                            $scope.PublishPreviewBusy = e;
                        });
                    });
                    webOrderingWebApiService.IsPublishLiveBusy.subscribe(function (e) {
                        $timout(function () {
                            $scope.PublishLiveBusy = e;
                        });
                    });
                    //webOrderingWebApiService.IsPreviewReady.subscribe((busy) => {
                    //    $scope.PreviewWindow.refresh({
                    //        //content: {
                    //        //    url: webOrderingWebApiService.Context.Model.SiteDetails.DomainName + "?q" + Math.random() 
                    //        //}
                    //    });
                    //    $scope.PreviewWindow.open();
                    //    $scope.PreviewWindow.center();
                    //});
                    //webOrderingWebApiService.IsWebOrderingBusy.subscribe((busy) => {
                    //    $timout(() => {
                    //        if (busy) {
                    //            if (!$scope.Modal) { return; }
                    //            var m = <any>$scope.Modal;
                    //            m.open();
                    //        }
                    //        else {
                    //            if (!$scope.Modal) { return; }
                    //            var m = <any>$scope.Modal;
                    //            m.close();
                    //        }
                    //        $scope.IsBusy = busy;
                    //    });
                    //});
                };
                StatusController.Name = "StatusController";
                return StatusController;
            })();
            Controllers.StatusController = StatusController;
        })(Controllers = WebOrdering.Controllers || (WebOrdering.Controllers = {}));
    })(WebOrdering = MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var WebOrdering;
    (function (WebOrdering) {
        var Controllers;
        (function (Controllers) {
            WebOrdering.Angular.ControllersInitilizations.push(function (app) {
                app.controller("StoresController", function ($scope, contextService) {
                    var dataSource = new kendo.data.DataSource();
                    $scope.storeGridOptions = {
                        dataSource: dataSource,
                        sortable: true,
                        columns: [{
                                field: "Name",
                                title: "Store Name",
                            }]
                    };
                    contextService.StoreSubject.subscribe(function (stores) {
                        WebOrdering.Logger.Notify("I have stores" + stores.length);
                        dataSource.data(stores);
                    });
                });
            });
        })(Controllers = WebOrdering.Controllers || (WebOrdering.Controllers = {}));
    })(WebOrdering = MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
})(MyAndromeda || (MyAndromeda = {}));
/// <reference path="MyAndromeda.WebOrdering.App.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var WebOrdering;
    (function (WebOrdering) {
        var Controllers;
        (function (Controllers) {
            WebOrdering.Angular.ControllersInitilizations.push(function (app) {
                app.controller(TripAdvisorSettingsController.Name, [
                    '$scope', '$timeout',
                    WebOrdering.Services.ContextService.Name,
                    WebOrdering.Services.WebOrderingWebApiService.Name,
                    function ($scope, $timeout, contextService, webOrderingWebApiService) {
                        TripAdvisorSettingsController.SetupValidatorOptions($scope, $timeout, webOrderingWebApiService);
                        TripAdvisorSettingsController.OnLoad($scope, $timeout, webOrderingWebApiService);
                        /* going to leave kendo to manage the observable object */
                        TripAdvisorSettingsController.SetupKendoMvvm($scope, $timeout, contextService);
                    }
                ]);
            });
            var TripAdvisorSettingsController = (function () {
                function TripAdvisorSettingsController() {
                }
                TripAdvisorSettingsController.OnLoad = function ($scope, $timout, webOrderingWebApiService) {
                    //$scope.SaveChanges = () => {
                    //    if ($scope.TripAdvisorSettingsValidator.validate()) {
                    //        webOrderingWebApiService.Update();
                    //    }
                    //};
                    //var s = <any>$scope;
                    //s.FacebookSettings = {};
                };
                TripAdvisorSettingsController.SetupValidatorOptions = function ($scope, $timout, contextService) {
                    var validatorOptions = {
                        name: "",
                        rules: {
                            TripadvisorScirptRequired: function (input) {
                                if (!input.is("[data-required-if-tripadvisor]")) {
                                    return true;
                                }
                                var isEnabled = contextService.Model.TripAdvisorSettings.get("IsEnable");
                                var text = input.val();
                                return text.length > 0;
                            }
                        }
                    };
                };
                TripAdvisorSettingsController.SetupKendoMvvm = function ($scope, $timout, contextService) {
                    var settingsSubscription = contextService.ModelSubject
                        .where(function (e) { return e !== null; })
                        .subscribe(function (websiteSettings) {
                        var viewElement = $("#TripAdvisorSettingsController");
                        kendo.bind(viewElement, websiteSettings.TripAdvisorSettings);
                        $scope.ShowTripAdvisorSettings = websiteSettings.TripAdvisorSettings.get("IsEnable");
                    });
                    $scope.$on('$destroy', function () {
                        settingsSubscription.dispose();
                    });
                };
                TripAdvisorSettingsController.Name = "TripAdvisorSettingsController";
                return TripAdvisorSettingsController;
            })();
            Controllers.TripAdvisorSettingsController = TripAdvisorSettingsController;
        })(Controllers = WebOrdering.Controllers || (WebOrdering.Controllers = {}));
    })(WebOrdering = MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var WebOrdering;
    (function (WebOrdering) {
        "use strict";
        var Logger = (function () {
            function Logger() {
                this.UseNotify = true;
                this.UseDebug = true;
                this.UseError = true;
            }
            Logger.Notify = function (o) {
                if (logger.UseNotify) {
                    console.log(o);
                }
            };
            Logger.Debug = function (o) {
                if (logger.UseDebug) {
                    console.log(o);
                }
            };
            Logger.Error = function (o) {
                if (logger.UseError) {
                    console.log(o);
                }
            };
            Logger.SettingUpController = function (name, state) {
                if (logger.UseNotify) {
                    console.log("setting up controller - " + name + " : " + state);
                }
            };
            Logger.SettingUpService = function (name, state) {
                if (logger.UseNotify) {
                    console.log("setting up service - " + name + " : " + state);
                }
            };
            Logger.AllowDebug = function (value) {
                logger.UseDebug = value;
            };
            Logger.AllowError = function (value) {
                logger.UseError = value;
            };
            return Logger;
        })();
        WebOrdering.Logger = Logger;
        var logger = new Logger();
    })(WebOrdering = MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
})(MyAndromeda || (MyAndromeda = {}));
/// <reference path="MyAndromeda.WebOrdering.App.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var WebOrdering;
    (function (WebOrdering) {
        var Services;
        (function (Services) {
            WebOrdering.Angular.ServicesInitilizations.push(function (app) {
                app.factory(ContextService.Name, [
                    function () {
                        var instnance = new ContextService();
                        return instnance;
                    }
                ]);
            });
            var contextServiceModule = angular.module("ContextServiceModule", []);
            var ContextService = (function () {
                function ContextService() {
                    this.Model = null;
                    this.ModelSubject = new Rx.BehaviorSubject(null);
                    this.StoreSubject = new Rx.BehaviorSubject([]);
                }
                ContextService.Name = "contextService";
                return ContextService;
            })();
            Services.ContextService = ContextService;
            contextServiceModule.service("contextService", ContextService);
        })(Services = WebOrdering.Services || (WebOrdering.Services = {}));
    })(WebOrdering = MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
})(MyAndromeda || (MyAndromeda = {}));
/// <reference path="../../scripts/typings/rx/rx.d.ts" />
/// <reference path="MyAndromeda.WebOrdering.App.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var WebOrdering;
    (function (WebOrdering) {
        var Services;
        (function (Services) {
            WebOrdering.Angular.ServicesInitilizations.push(function (app) {
                app.factory(WebOrderingThemeWebApiService.Name, [
                    function () {
                        WebOrdering.Logger.Notify("new WebOrderingThemeWebApiService");
                        var instnance = new WebOrderingThemeWebApiService();
                        return instnance;
                    }
                ]);
            });
            var WebOrderingThemeWebApiService = (function () {
                function WebOrderingThemeWebApiService() {
                    var _this = this;
                    this.IsBusy = new Rx.BehaviorSubject(false);
                    this.Search = new Rx.Subject();
                    if (!WebOrdering.Settings.AndromedaSiteId) {
                        throw "Settings.AndromedaSiteId is undefined";
                    }
                    //throttle input for 1 second. ie search will resume after the user stops typing.
                    this.Search
                        .throttle(1000)
                        .subscribe(function (value) { return _this.SearcInternal(value); });
                }
                WebOrderingThemeWebApiService.prototype.GetThemeDataSource = function () {
                    var _this = this;
                    var route = kendo.format('/api/AndroWebOrderingTheme/{0}/List', WebOrdering.Settings.AndromedaSiteId);
                    this.dataSource = new kendo.data.DataSource({
                        transport: {
                            read: route
                        }
                    });
                    this.dataSource.bind("requestStart", function () { _this.IsBusy.onNext(true); });
                    this.dataSource.bind("requestEnd", function () { _this.IsBusy.onNext(false); });
                    return this.dataSource;
                };
                WebOrderingThemeWebApiService.prototype.SearchText = function (value) {
                    this.Search.onNext(value);
                };
                WebOrderingThemeWebApiService.prototype.SearcInternal = function (value) {
                    value || (value = "");
                    value = value.trim();
                    if (value.length === 0) {
                        this.dataSource.filter([]);
                        return;
                    }
                    var op = "contains";
                    var filterFileName = {
                        field: "FileName",
                        operator: op,
                        value: value
                    };
                    var filterInterName = {
                        field: "InternalName",
                        operator: op,
                        value: value
                    };
                    var filterGroup = {
                        filters: [filterFileName, filterInterName],
                        logic: "or"
                    };
                    this.dataSource.filter([
                        filterGroup
                    ]);
                };
                WebOrderingThemeWebApiService.Name = "WebOrderingThemeWebApiService";
                return WebOrderingThemeWebApiService;
            })();
            Services.WebOrderingThemeWebApiService = WebOrderingThemeWebApiService;
        })(Services = WebOrdering.Services || (WebOrdering.Services = {}));
    })(WebOrdering = MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
})(MyAndromeda || (MyAndromeda = {}));
/// <reference path="MyAndromeda.WebOrdering.App.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var WebOrdering;
    (function (WebOrdering) {
        var Services;
        (function (Services) {
            WebOrdering.Angular.ServicesInitilizations.push(function (app) {
                app.service(WebOrderingWebApiService.Name, WebOrderingWebApiService);
            });
            var WebOrderingWebApiService = (function () {
                function WebOrderingWebApiService($http, contextService) {
                    var _this = this;
                    this.Context = contextService;
                    this.IsPublishLiveBusy = new Rx.BehaviorSubject(false);
                    this.IsPublishPreviewBusy = new Rx.BehaviorSubject(false);
                    this.IsLoading = new Rx.BehaviorSubject(false);
                    this.IsSaving = new Rx.BehaviorSubject(false);
                    if (!WebOrdering.Settings.AndromedaSiteId) {
                        throw "Settings.AndromedaSiteId is undefined";
                    }
                    var readWebsiteSettings = kendo.format(WebOrdering.Settings.ReadRoute, WebOrdering.Settings.AndromedaSiteId, WebOrdering.Settings.WebSiteId);
                    var readStores = kendo.format(WebOrdering.Settings.ReadStoreRoute, WebOrdering.Settings.AndromedaSiteId, WebOrdering.Settings.WebSiteId);
                    this.IsLoading.onNext(true);
                    this.watcher = new Rx.Subject();
                    this.watcher.distinctUntilChanged(function (x) { return x.WebSiteId; }).subscribe(function (settings) {
                        _this.Context.ModelSubject.onNext(settings);
                    });
                    this.Context.ModelSubject
                        .where(function (e) { return e !== null; })
                        .subscribe(function (v) { console.log(v.WebSiteName); });
                    var promise = $http.get(readWebsiteSettings);
                    promise.then(function (result) {
                        //set defaults. 
                        var nullOrUndefined = function (path) {
                            return typeof (path) === "undefined" || path === null;
                            return true;
                        };
                        if (!result.data.MenuPageSettings) {
                            result.data.MenuPageSettings = {
                                IsSingleToppingsOnlyEnabled: false,
                                IsQuantityDropdownEnabled: true,
                                IsThumbnailsEnabled: true
                            };
                        }
                        if (nullOrUndefined(result.data.GeneralSettings.EnableDelivery)) {
                            result.data.GeneralSettings.EnableDelivery = true;
                        }
                        if (nullOrUndefined(result.data.GeneralSettings.EnableCollection)) {
                            result.data.GeneralSettings.EnableCollection = true;
                        }
                        return result;
                    }).then(function (result) {
                        //transfer pence to decimal so that the editors can work easier. 
                        if (result.data.GeneralSettings.MinimumDeliveryAmount) {
                            result.data.GeneralSettings.MinimumDeliveryAmount /= 100;
                        }
                        if (result.data.GeneralSettings.DeliveryCharge) {
                            result.data.GeneralSettings.DeliveryCharge /= 100;
                        }
                        if (result.data.GeneralSettings.OptionalFreeDeliveryThreshold) {
                            result.data.GeneralSettings.OptionalFreeDeliveryThreshold /= 100;
                        }
                        if (result.data.GeneralSettings.CardCharge) {
                            result.data.GeneralSettings.CardCharge /= 100;
                        }
                        return result;
                    }).then(function (result) {
                        //may get rid of this bit some point soon. 
                        var observable = kendo.observable(result.data);
                        _this.Context.Model = observable;
                        _this.Context.ModelSubject.onNext(observable);
                        return result;
                    }).then(function (result) {
                        var storePromise = $http.get(readStores);
                        storePromise.success(function (stores) {
                            contextService.StoreSubject.onNext(stores);
                        });
                        return result;
                    });
                    promise.finally(function () {
                        _this.IsLoading.onNext(false);
                    });
                }
                WebOrderingWebApiService.prototype.UpdateThemeSettings = function (settings) {
                    var s = settings;
                    var m = this.Context.Model;
                    m.set("ThemeSettings", settings);
                    this.Update();
                };
                WebOrderingWebApiService.prototype.Publish = function () {
                    var _this = this;
                    var publish = kendo.format(WebOrdering.Settings.PublishRoute, WebOrdering.Settings.AndromedaSiteId, WebOrdering.Settings.WebSiteId);
                    if (!this.Context.Model.CustomerAccountSettings.get("IsEnable")) {
                        if (!confirm("Are you sure you want to continue with customer account settings disabled? Continue to accept this responsibility, or cancel to apply that setting.")) {
                            this.Context.Model.CustomerAccountSettings.set("IsEnable", true);
                        }
                    }
                    var raw = this.GetRawModel();
                    var promise = $.ajax({
                        url: publish,
                        type: "POST",
                        contentType: 'application/json',
                        dataType: "json",
                        data: raw
                    });
                    this.IsPublishLiveBusy.onNext(true);
                    promise.always(function () {
                        _this.IsPublishLiveBusy.onNext(false);
                    });
                };
                WebOrderingWebApiService.prototype.Preview = function () {
                    var _this = this;
                    var preview = kendo.format(WebOrdering.Settings.PreviewRoute, WebOrdering.Settings.AndromedaSiteId, WebOrdering.Settings.WebSiteId);
                    var raw = this.GetRawModel();
                    var promise = $.ajax({
                        url: preview,
                        type: "POST",
                        contentType: 'application/json',
                        dataType: "json",
                        data: raw
                    });
                    this.IsPublishPreviewBusy.onNext(true);
                    promise.always(function () {
                        _this.IsPublishPreviewBusy.onNext(false);
                    });
                };
                WebOrderingWebApiService.prototype.Update = function () {
                    var _this = this;
                    var update = kendo.format(WebOrdering.Settings.UpdateRoute, WebOrdering.Settings.AndromedaSiteId, WebOrdering.Settings.WebSiteId);
                    console.log("sync");
                    var raw = this.GetRawModel();
                    var promise = $.ajax({
                        url: update,
                        type: "POST",
                        contentType: 'application/json',
                        dataType: "json",
                        data: raw
                    });
                    this.IsSaving.onNext(true);
                    promise.always(function () {
                        _this.IsSaving.onNext(true);
                    });
                };
                WebOrderingWebApiService.prototype.GetRawModel = function () {
                    var newObject = JSON.parse(JSON.stringify(this.Context.Model));
                    //jQuery.extend(true, {}, this.Context.Model);
                    if (newObject.GeneralSettings.MinimumDeliveryAmount) {
                        newObject.GeneralSettings.MinimumDeliveryAmount *= 100;
                    }
                    if (newObject.GeneralSettings.DeliveryCharge) {
                        newObject.GeneralSettings.DeliveryCharge *= 100;
                    }
                    if (newObject.GeneralSettings.OptionalFreeDeliveryThreshold) {
                        newObject.GeneralSettings.OptionalFreeDeliveryThreshold *= 100;
                    }
                    if (newObject.GeneralSettings.CardCharge) {
                        newObject.GeneralSettings.CardCharge *= 100;
                    }
                    var raw = JSON.stringify(newObject);
                    return raw;
                };
                WebOrderingWebApiService.Name = "webOrderingWebApiService";
                return WebOrderingWebApiService;
            })();
            Services.WebOrderingWebApiService = WebOrderingWebApiService;
        })(Services = WebOrdering.Services || (WebOrdering.Services = {}));
    })(WebOrdering = MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
})(MyAndromeda || (MyAndromeda = {}));
/// <reference path="MyAndromeda.WebOrdering.App.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var WebOrdering;
    (function (WebOrdering) {
        var Settings = (function () {
            function Settings() {
            }
            Settings.AndromedaSiteId = 0;
            Settings.WebSiteId = 0;
            //api/{AndromedaSiteId}/AndroWebOrdering/{webOrderingWebsiteId}/Read
            Settings.ReadRoute = "/api/{0}/AndroWebOrdering/{1}/Read";
            //api/{AndromedaSiteId}/AndroWebOrdering/{webOrderingWebsiteId}/Update
            Settings.UpdateRoute = "/api/{0}/AndroWebOrdering/{1}/Update";
            //api/{AndromedaSiteId}/AndroWebOrdering/{webOrderingWebsiteId}/Publish
            Settings.PublishRoute = "/api/{0}/AndroWebOrdering/{1}/Publish";
            //api/{AndromedaSiteId}/AndroWebOrdering/{webOrderingWebsiteId}/Preview
            Settings.PreviewRoute = "/api/{0}/AndroWebOrdering/{1}/Preview";
            //api/{AndromedaSiteId}/AndroWebOrdering/{webOrderingWebsiteId}/Stores/Read
            Settings.ReadStoreRoute = "/api/{0}/AndroWebOrdering/{1}/Stores/Read";
            return Settings;
        })();
        WebOrdering.Settings = Settings;
        ;
    })(WebOrdering = MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var WebOrdering;
    (function (WebOrdering) {
        var UpSelling;
        (function (UpSelling) {
            var upSellModule = angular.module("androweb-upsell-module", []);
            upSellModule.controller("upSellController", function ($scope, $timeout, contextService, upSellDataService, webOrderingWebApiService) {
                var menuPromise = upSellDataService.GetMenu();
                var getMultiSelect = function () {
                    var multiSelect = $scope.UpSellMultiSelect;
                    return multiSelect;
                };
                var dataSource = new kendo.data.DataSource({
                    autoSync: false
                });
                var multiselectOptions = {
                    placeholder: "Select a menu section",
                    autoBind: false,
                    dataTextField: "Name",
                    dataValueField: "Id",
                    dataSource: dataSource
                };
                $scope.MultiselectOptions = multiselectOptions;
                //$scope.SelectedDisplayCategories = [];
                $scope.DisplayCategoryDataSource = dataSource;
                $scope.SaveChanges = function () {
                    if ($scope.UpSellingValidator.validate()) {
                        webOrderingWebApiService.Update();
                    }
                };
                $scope.Settings = null;
                var menuObservable = Rx.Observable.fromPromise(menuPromise);
                var settingsObservable = contextService.ModelSubject
                    .where(function (e) { return e !== null; });
                var both = Rx.Observable.combineLatest(menuObservable, settingsObservable, function (menuResponse, settings) {
                    return {
                        Menu: menuResponse.data,
                        AndroWebSettings: settings
                    };
                });
                var bothSubscription = both.subscribe(function (settings) {
                    //var multiSelect = getMultiSelect();
                    //$scope.SelectedDisplayCategories = settings.AndroWebSettings.UpSelling.DisplayCategories;
                    if (!settings.AndroWebSettings.UpSelling) {
                        settings.AndroWebSettings.UpSelling = {
                            Enabled: false,
                            DisplayCategories: []
                        };
                    }
                    $timeout(function () {
                        $scope.Settings = settings.AndroWebSettings;
                        dataSource.data(settings.Menu.DisplayCategories);
                    });
                });
                $scope.$on('$destroy', function () {
                    bothSubscription.dispose();
                });
            });
            var UpSellDataService = (function () {
                function UpSellDataService($http) {
                    this.$http = $http;
                }
                UpSellDataService.prototype.GetMenu = function () {
                    var promise = this.$http.get(UpSellDataService.GetMemuRoute);
                    return promise;
                };
                //set by cshtml 
                UpSellDataService.GetMemuRoute = "";
                return UpSellDataService;
            })();
            UpSelling.UpSellDataService = UpSellDataService;
            upSellModule.service("upSellDataService", UpSellDataService);
        })(UpSelling = WebOrdering.UpSelling || (WebOrdering.UpSelling = {}));
    })(WebOrdering = MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
})(MyAndromeda || (MyAndromeda = {}));
//createDriver(driverUpdate)
var MyAndromeda;
(function (MyAndromeda) {
    var Data;
    (function (Data) {
        var Services;
        (function (Services) {
            var m = angular.module("MyAndromeda.Data.Drivers", []);
            var DriverService = (function () {
                function DriverService($http) {
                    this.$http = $http;
                }
                DriverService.prototype.AddToOrder = function (andromedaSiteId, orderId, driver) {
                    var route = kendo.format("/data/{0}/orders/{1}/addDriver", andromedaSiteId, orderId);
                    var promise = this.$http.post(route, driver);
                    return promise;
                };
                return DriverService;
            })();
            Services.DriverService = DriverService;
            m.service("driverService", DriverService);
        })(Services = Data.Services || (Data.Services = {}));
    })(Data = MyAndromeda.Data || (MyAndromeda.Data = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var Data;
    (function (Data) {
        var Services;
        (function (Services) {
            var m = angular.module("MyAndromeda.Data.Orders", []);
            var OrderService = (function () {
                function OrderService($http) {
                    this.$http = $http;
                }
                OrderService.prototype.ListOrdersForMap = function (andromedaSiteId, start, end) {
                    var _this = this;
                    var read = kendo.format("/data/{0}/orders/map", andromedaSiteId);
                    var sort = {
                        field: "OrderPlacedTime",
                        dir: "desc"
                    };
                    var dataSource = new kendo.data.DataSource({
                        transport: {
                            read: function (options) {
                                if (!options.data) {
                                    options.data = {};
                                }
                                var f = kendo.toString(start, "u");
                                var t = kendo.toString(end, "u");
                                MyAndromeda.Logger.Notify(f);
                                MyAndromeda.Logger.Notify(t);
                                var data = $.extend({}, options.data, {
                                    From: f,
                                    To: t
                                });
                                var promise = _this.$http.post(read, data);
                                Rx.Observable
                                    .fromPromise(promise)
                                    .subscribe(function (r) { options.success(r.data); }, function (ex) { });
                            }
                        },
                        //data: "Data",
                        //pageSize: 10,
                        //page: 1,
                        serverPaging: true,
                        serverSorting: true,
                        schema: {
                            //data: "Data",
                            //total: "Total",
                            model: {
                                id: "Id",
                                fields: {
                                    OrderPlacedTime: { "type": "date" },
                                    OrderWantedTime: { "type": "date" },
                                    CustomerGeoLocation: function () {
                                        var model = this;
                                        var lat = model.Customer.Latitude;
                                        var long = model.Customer.Longitude;
                                        return [0, 0];
                                        if (!lat) {
                                            return null;
                                        }
                                        return [lat, long];
                                    }
                                },
                            }
                        },
                    });
                    dataSource.bind("change", function () {
                        MyAndromeda.Logger.Notify(dataSource.data());
                    });
                    return dataSource;
                };
                OrderService.prototype.ListOrders = function (andromedaSiteId) {
                    var _this = this;
                    var read = kendo.format("/data/{0}/debug-orders", andromedaSiteId);
                    var sort = {
                        field: "OrderPlacedTime",
                        dir: "desc"
                    };
                    var dataSource = new kendo.data.DataSource({
                        transport: {
                            read: function (options) {
                                MyAndromeda.Logger.Notify("read: options");
                                MyAndromeda.Logger.Notify(options);
                                var data = options.data;
                                var a = {
                                    aggregate: data.aggregate,
                                    filter: data.filter,
                                    filters: data.filter,
                                    group: data.group,
                                    groups: data.group,
                                    models: data.models,
                                    page: data.page,
                                    pageSize: data.pageSize,
                                    skip: data.skip,
                                    sort: data.sort,
                                    sorts: data.sort,
                                    take: data.take
                                };
                                var promise = _this.$http.post(read, a);
                                Rx.Observable
                                    .fromPromise(promise)
                                    .subscribe(function (r) {
                                    options.success(r.data);
                                }, function (ex) { });
                            },
                            parameterMap: function (data, type) {
                                //return kendo.stringify(data);
                                var a = {
                                    aggregate: data.aggregate,
                                    filter: data.filter,
                                    filters: data.filter,
                                    group: data.group,
                                    groups: data.group,
                                    models: data.models,
                                    page: data.page,
                                    pageSize: data.pageSize,
                                    skip: data.skip,
                                    sort: data.sort,
                                    sorts: data.sort,
                                    take: data.take
                                };
                                MyAndromeda.Logger.Notify("param map");
                                MyAndromeda.Logger.Notify(a);
                                return kendo.stringify(a);
                            }
                        },
                        //data: "Data",
                        pageSize: 10,
                        page: 1,
                        serverPaging: true,
                        serverSorting: true,
                        schema: {
                            data: "Data",
                            total: function (response) {
                                return response.Total;
                            },
                            model: {
                                id: "Id",
                                fields: {
                                    OrderPlacedTime: { "type": "date" },
                                    OrderWantedTime: { "type": "date" },
                                    CustomerGeoLocation: function () {
                                        var model = this;
                                        var lat = model.Customer.Latitude;
                                        var long = model.Customer.Longitude;
                                        return [0, 0];
                                        if (!lat) {
                                            return null;
                                        }
                                        return [lat, long];
                                    }
                                },
                            }
                        },
                        sort: sort
                    });
                    return dataSource;
                };
                OrderService.prototype.ChangeOrderStatus = function (andromedaSiteId, orderId, change) {
                    var route = kendo.format("/data/{0}/orders/{1}/updateStatus", andromedaSiteId, orderId);
                    return this.$http.post(route, change);
                };
                return OrderService;
            })();
            Services.OrderService = OrderService;
            m.service("orderService", OrderService);
        })(Services = Data.Services || (Data.Services = {}));
    })(Data = MyAndromeda.Data || (MyAndromeda.Data = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var Chain;
    (function (Chain) {
        var Services;
        (function (Services) {
            var chainService = (function () {
                function chainService(chainServiceRoutes) {
                    this.chainServiceRoutes = chainServiceRoutes;
                }
                chainService.prototype.get = function (id, callback) {
                    var internal = this, route = {
                        type: "POST",
                        dataType: "json",
                        data: { id: id },
                        success: function (data) {
                            callback(data);
                        }
                    };
                    $.ajax($.extend({}, route, {
                        url: internal.chainServiceRoutes.getById
                    }));
                };
                return chainService;
            })();
            Services.chainService = chainService;
        })(Services = Chain.Services || (Chain.Services = {}));
    })(Chain = MyAndromeda.Chain || (MyAndromeda.Chain = {}));
})(MyAndromeda || (MyAndromeda = {}));
/// <reference path="../../Scripts/typings/linqjs/linq.d.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var Chain;
    (function (Chain) {
        var Services;
        (function (Services) {
            var TreeviewMapService = (function () {
                function TreeviewMapService(kendoTreeView) {
                    var internal = this;
                    this.viewModel = kendo.observable({
                        stores: []
                    });
                    var bindingElement = $("#mapData");
                    kendo.bind(bindingElement, this.viewModel);
                    this.viewModel.bind("change", function () {
                        internal.AddMarkers();
                    });
                    this.kendoMap = $("#map").data("kendoMap");
                }
                TreeviewMapService.prototype.AddMarkers = function () {
                    var map = this.kendoMap, markers = map.markers;
                    map.markers.clear();
                    var stores = this.viewModel.get("stores");
                    var viableStores = Enumerable.from(stores).where(function (e) { return e.latitude !== null && e.longitude !== null; });
                    //zoom out
                    if (viableStores.count() === 0) {
                        map.zoom(1);
                        map.center([0, 0]);
                        return;
                    }
                    var centeredPosition = function () {
                        //console.log(viableStores.toArray());
                        var avgLat = viableStores.select(function (e) { return parseFloat(e.latitude); }).average();
                        var avgLong = viableStores.select(function (e) { return parseFloat(e.longitude); }).average();
                        var c = [avgLat, avgLong];
                        return c;
                    };
                    viableStores.forEach(function (store, index) {
                        if (store.longitude && store.latitude) {
                            var location = [store.latitude, store.longitude];
                            var addition = {
                                shape: "pin",
                                store: store,
                                tooltip: {
                                    animation: {
                                        close: {
                                            effects: "fade:out"
                                        }
                                    },
                                    autoHide: true,
                                    position: "right",
                                    showOn: "mouseenter",
                                    template: $("#map-tooltip-template").html()
                                },
                                location: location
                            };
                            map.markers.add(addition);
                        }
                    });
                    var centerdLocation = centeredPosition();
                    map.center(centeredPosition());
                    map.zoom(3);
                };
                return TreeviewMapService;
            })();
            var TreeviewChainService = (function () {
                function TreeviewChainService(data) {
                    var internal = this;
                    this.data = data;
                    this.rootDataSource = new kendo.data.HierarchicalDataSource({
                        transport: {
                            read: function (options) {
                                options.success(internal.data);
                            }
                        },
                        schema: {
                            model: {
                                id: "id",
                                children: "items"
                            }
                        }
                    });
                    this.rootDataSource.read();
                    var treeVm = kendo.observable({
                        chains: this.rootDataSource
                    });
                    var treeviewElement = $("#TreeviewChains").kendoTreeView({
                        template: kendo.template($('#StoreNode').html()),
                        dataSource: this.rootDataSource,
                        loadOnDemand: false,
                        dataTextField: "name"
                    });
                    this.kendoTreeView = treeviewElement.data("kendoTreeView");
                    treeviewElement.on("click", ".k-button-show-stores", function (e) {
                        e.preventDefault();
                        var uid = $(this).closest(".k-item").data("uid");
                        var element = internal.rootDataSource.getByUid(uid);
                        internal.mapService.viewModel.set("stores", element.stores);
                    });
                    kendo.bind($("#treeviewdata"), treeVm);
                    this.mapService = new TreeviewMapService(this.kendoTreeView);
                }
                return TreeviewChainService;
            })();
            Services.TreeviewChainService = TreeviewChainService;
        })(Services = Chain.Services || (Chain.Services = {}));
    })(Chain = MyAndromeda.Chain || (MyAndromeda.Chain = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var Debug;
    (function (Debug) {
        var gridApp = angular.module("MyAndromeda.Debug.OrdersApp", [
            "kendo.directives",
            "MyAndromeda.Resize",
            "MyAndromeda.Data.Orders",
            "MyAndromeda.Data.Drivers"
        ]);
        gridApp.run(function ($templateCache) {
            MyAndromeda.Logger.Notify("OrdersApp Started");
            angular.element('script[type="text/template"]').each(function (i, element) {
                $templateCache.put(element.id, element.innerHTML);
            });
        });
        gridApp.controller("startController", function ($scope) {
            $scope.onSelect = function (e) {
                //e.preventDefault();
            };
        });
        var gridTempaltes = {
            orderId: "<grid-order-id></grid-order-id>",
            OrderItemCount: "<grid-order-item-count></<grid-order-item-count>",
            OrderPlacedTime: "<grid-order-placed-time></grid-order-placed-time>",
            OrderWantedTime: "<grid-order-wanted-time></grid-order-wanted-time>",
            OrderCustomer: "<grid-order-customer></grid-order-customer>"
        };
        gridApp.directive("ordersGrid", function () {
            return {
                name: "ordersGrid",
                restrict: "E",
                scope: {
                    andromedaSiteId: "@"
                },
                templateUrl: "GridTemplate.html",
                transclude: true,
                link: function ($scope, instanceElement, instanceAttributes, controller, transclude) {
                    transclude($scope, function (clone, scope) {
                        instanceElement.append(clone);
                    });
                },
                controller: function ($scope, orderService, driverService) {
                    var andromedaSiteId = $scope.andromedaSiteId;
                    var dataSource = orderService.ListOrders(andromedaSiteId);
                    var rowTemplate = "<tr orders-grid-row-template></tr>";
                    var detailTemplate = "<order-detail-template></order-detail-template>";
                    var gridOptions = {
                        dataSource: dataSource,
                        sortable: true,
                        filterable: true,
                        pageable: {
                            numeric: true,
                            refresh: true,
                            pageSizes: [10, 25, 50],
                            previousNext: true,
                            input: false,
                            info: false
                        },
                        columns: [
                            { title: "Id", field: "Id", template: gridTempaltes.orderId },
                            { title: "Items", field: "ItemCount", template: gridTempaltes.OrderItemCount },
                            { title: "Final Price", field: "FinalPrice" },
                            { title: "Placed Time", field: "OrderPlacedTime", template: gridTempaltes.OrderPlacedTime },
                            { title: "Wanted Time", field: "OrderWantedTime", template: gridTempaltes.OrderWantedTime },
                            { title: "Customer Name", field: "Customer.Name", template: gridTempaltes.OrderCustomer }
                        ],
                        detailTemplate: kendo.template(detailTemplate)
                    };
                    var mapOptions = {
                        center: [0, 0],
                        zoom: 2,
                        layers: [
                            {
                                type: "tile",
                                urlTemplate: "http://#= subdomain #.tile.openstreetmap.org/#= zoom #/#= x #/#= y #.png",
                                subdomains: ["a", "b", "c"],
                                attribution: "&copy; <a href='http://osm.org/copyright'>OpenStreetMap contributors</a>"
                            },
                            {
                                type: "marker",
                                dataSource: dataSource,
                                locationField: "Customer.GeoLocation"
                            }
                        ],
                    };
                    $scope.createDriver = function (orderId, driver) {
                        driverService.AddToOrder(andromedaSiteId, orderId, driver);
                    };
                    $scope.changeStatus = function (orderId, statusId) {
                        orderService.ChangeOrderStatus(andromedaSiteId, orderId, {
                            StatusId: statusId
                        });
                    };
                    $scope.gridOptions = gridOptions;
                    $scope.mapOptions = mapOptions;
                }
            };
        });
        var orderColumn = function (name, template) {
            var a = {
                name: name,
                transclude: true,
                link: function ($scope, instanceElement, instanceAttributes, controller, transclude) {
                    transclude($scope, function (clone, scope) {
                        instanceElement.append(clone);
                    });
                },
                controller: null,
                //controller: ($scope) => {
                //},
                templateUrl: template
            };
            return function () { return a; };
        };
        var orderIdColumn = function () {
            var col = orderColumn("gridOrderId", "order-id-column.html");
            var r = col();
            r.controller = function ($scope, $element) {
                var target = $element.find(".hasMenu");
                $scope.target = target;
                $scope.openMenu = function ($event) {
                    var menu = $scope.rowMenu;
                    menu.open($event.clientX, $event.clientY);
                    MyAndromeda.Logger.Notify($event);
                };
            };
            return function () { return r; };
        };
        gridApp.directive("gridOrderId", orderIdColumn());
        gridApp.directive("gridOrderItemCount", orderColumn("gridOrderItemCount", "order-item-count-column.html"));
        gridApp.directive("gridOrderPlacedTime", orderColumn("gridOrderPlacedTime", "order-placed-time-column.html"));
        gridApp.directive("gridOrderWantedTime", orderColumn("gridOrderWantedTime", "order-wanted-time-column.html"));
        gridApp.directive("gridOrderCustomer", orderColumn("gridOrderCustomer", "order-customer-column.html"));
        gridApp.directive("orderDetailTemplate", function () {
            return {
                name: "orderDetailTemplate",
                transclude: true,
                link: function ($scope, instanceElement, instanceAttributes, controller, transclude) {
                    transclude($scope, function (clone, scope) {
                        instanceElement.append(clone);
                    });
                },
                controller: null,
                //controller: ($scope) => {
                //},
                templateUrl: "order-detail-template.html"
            };
        });
        //order-wanted-time-column.html
        //order-customer-column.html
        //gridApp.directive("ordersGridRowTemplate", () => {
        //    return {
        //        name: "ordersGridRowTemplate",
        //        transclude: true,
        //        link: ($scope,
        //            instanceElement: ng.IAugmentedJQuery,
        //            instanceAttributes: ng.IAttributes,
        //            controller: any,
        //            transclude: ng.ITranscludeFunction
        //            ) => {
        //            transclude($scope, (clone, scope) => {
        //                instanceElement.append(clone);
        //            });
        //        },
        //        //controller: ($scope) => {
        //        //},
        //        templateUrl: "GridRowTemplate.html"
        //    };   
        //});
        //gridApp.directive("")
        Debug.gridAppSetup = function (id) {
            var element = document.getElementById(id);
            angular.bootstrap(element, ["MyAndromeda.Debug.OrdersApp"]);
        };
    })(Debug = MyAndromeda.Debug || (MyAndromeda.Debug = {}));
})(MyAndromeda || (MyAndromeda = {}));
/// <reference path="../Menu/MyAndromeda.Menu.Logger.ts" />
/// <reference path="../../Scripts/typings/angularjs/angular-route.d.ts" />
/// <reference path="../../Scripts/typings/angularjs/angular.d.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var DeliveryZonesByRadius;
    (function (DeliveryZonesByRadius) {
        "use strict";
        var DeliveryZonesByRadiusApp = (function () {
            function DeliveryZonesByRadiusApp() {
            }
            DeliveryZonesByRadiusApp.ApplicationName = "DeliveryZonesByRadius";
            return DeliveryZonesByRadiusApp;
        })();
        DeliveryZonesByRadius.DeliveryZonesByRadiusApp = DeliveryZonesByRadiusApp;
        var Angular = (function () {
            function Angular() {
            }
            Angular.Init = function () {
                DeliveryZonesByRadius.Logger.Notify("bootstrap-Deliveryzonesbyradius");
                var element = document.getElementById("DeliveryZonesByRadius");
                var app = angular.module(DeliveryZonesByRadiusApp.ApplicationName, [
                    "kendo.directives",
                    //"ngRoute",
                    "ngAnimate"
                ]);
                DeliveryZonesByRadius.Logger.Notify("Assign " + Angular.ServicesInitilizations.length + " services");
                Angular.ServicesInitilizations.forEach(function (value) {
                    value(app);
                });
                DeliveryZonesByRadius.Logger.Notify("Assign " + Angular.ControllersInitilizations.length + " controllers");
                Angular.ControllersInitilizations.forEach(function (value) {
                    value(app);
                });
                angular.bootstrap(element, [DeliveryZonesByRadiusApp.ApplicationName]);
                DeliveryZonesByRadius.Logger.Notify("bootstrap-Deliveryzonesbyradius-complete");
            };
            Angular.ServicesInitilizations = [];
            Angular.ControllersInitilizations = [];
            return Angular;
        })();
        DeliveryZonesByRadius.Angular = Angular;
    })(DeliveryZonesByRadius = MyAndromeda.DeliveryZonesByRadius || (MyAndromeda.DeliveryZonesByRadius = {}));
})(MyAndromeda || (MyAndromeda = {}));
/// <reference path="MyAndromeda.DeliveryZonesByRadius.App.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var DeliveryZonesByRadius;
    (function (DeliveryZonesByRadius) {
        var Controllers;
        (function (Controllers) {
            DeliveryZonesByRadius.Angular.ControllersInitilizations.push(function (app) {
                app.controller(DeliveryZoneNamesController.Name, [
                    '$scope', '$timeout',
                    DeliveryZonesByRadius.Services.ContextService.Name,
                    DeliveryZonesByRadius.Services.DeliveryZonesByRadiusWebApiService.Name,
                    function ($scope, $timeout, contextService, deliveryZonesByRadiusApiService) {
                        DeliveryZoneNamesController.OnLoad($scope, $timeout, contextService, deliveryZonesByRadiusApiService);
                        DeliveryZoneNamesController.SetupSubscriptions($scope, $timeout, contextService, deliveryZonesByRadiusApiService);
                    }
                ]);
            });
            var DeliveryZoneNamesController = (function () {
                function DeliveryZoneNamesController() {
                }
                DeliveryZoneNamesController.OnLoad = function ($scope, $timeout, contextService, deliveryZonesByRadiusApiService) {
                    $scope.SaveChanges = function () {
                        if ($scope.PostCodeValidator.validate()) {
                            deliveryZonesByRadiusApiService.Update();
                            $timeout(function () {
                                var vm = $scope.ViewModel;
                                var postCodeOptions = vm.PostCodeSectors;
                                $scope.PostCodeOptionsListView.dataSource.data(postCodeOptions);
                                var unselectedItems = contextService.Model.PostCodeSectors.filter(function (e) { return !e.IsSelected; });
                                $scope.SelectAll = unselectedItems.length === 0 ? true : false;
                            });
                        }
                    };
                    $scope.GeneratePostCodeSectors = function () {
                        var validInput = $scope.PostCodeValidator.validate();
                        if (!validInput) {
                            alert("Correct the input.");
                            return;
                        }
                        if ($scope.ViewModel.Id == 0 || confirm("The existing post code sectors selection will be lost and reset.Are you sure you want to Regenerate the post code sectors?")) {
                            if ($scope.PostCodeValidator.validate()) {
                                $("#loader").removeClass('hidden');
                                $scope.ViewModel.HasPostCodes = false;
                                deliveryZonesByRadiusApiService.GeneratePostCodes();
                            }
                        }
                    };
                    $scope.SelectAllChange = function () {
                        var selectAll = $scope.SelectAll;
                        $timeout(function () {
                            var data = $scope.PostCodeOptionsListView.dataSource.data();
                            data.forEach(function (item) {
                                item.IsSelected = selectAll;
                            });
                        });
                        //var data = $scope.PostCodeOptionsListView.dataSource.data();
                        //contextService.Model.PostCodeSectors = data;
                    };
                    $scope.UpdateSelectAll = function () {
                        console.log("update select all");
                        var data = $scope.PostCodeOptionsListView.dataSource.data();
                        //contextService.Model.PostCodeSectors = data;
                        var unselectedItems = data.filter(function (e) { return !e.IsSelected; });
                        if (unselectedItems.length === 0) {
                            $scope.SelectAll = true;
                        }
                        else if (unselectedItems.length < data.length) {
                            $scope.SelectAll = false;
                        }
                        else {
                            $scope.SelectAll = false;
                        }
                    };
                };
                DeliveryZoneNamesController.SetupSubscriptions = function ($scope, $timeout, contextService, deliveryZonesByRadiusApiService) {
                    $scope.IsSaveBusy = false;
                    var settingsSubscription = contextService.ModelSubject
                        .subscribe(function (deliveryZoneByRadius) {
                        $timeout(function () {
                            $scope.ViewModel = deliveryZoneByRadius;
                            $scope.PostCodeOptionsListView.dataSource.data($scope.ViewModel.PostCodeSectors);
                            $scope.ViewModel.HasPostCodes = deliveryZoneByRadius.PostCodeSectors.length === 0 ? false : true;
                            var unselectedItems = contextService.Model.PostCodeSectors.filter(function (e) { return !e.IsSelected; });
                            $scope.SelectAll = unselectedItems.length === 0 ? true : false;
                        });
                    });
                    var settingsPostcodeSubscription = contextService.PostcodeModels
                        .subscribe(function (newDeliveryOptions) {
                        $scope.ViewModel.HasPostCodes = newDeliveryOptions.length === 0 ? false : true;
                        $timeout(function () {
                            var vm = $scope.ViewModel;
                            var postCodeOptions = vm.PostCodeSectors;
                            postCodeOptions = new kendo.data.ObservableArray(newDeliveryOptions);
                            vm.PostCodeSectors = postCodeOptions;
                            $scope.PostCodeOptionsListView.dataSource.data(postCodeOptions);
                            var unselectedItems = contextService.Model.PostCodeSectors.filter(function (e) { return !e.IsSelected; });
                            $scope.SelectAll = unselectedItems.length === 0 ? true : false;
                            $scope.ViewModel.HasPostCodes = (contextService.Model.PostCodeSectors == null || contextService.Model.PostCodeSectors.length === 0) ? false : true;
                        });
                    });
                    var savingSubscription = deliveryZonesByRadiusApiService.IsSavingBusy.distinctUntilChanged(function (e) { return e; }).subscribe(function (value) {
                        $timeout(function () {
                            $scope.IsSaveBusy = value;
                        });
                    });
                    $scope.$on('$destroy', function () {
                        settingsSubscription.dispose();
                        settingsPostcodeSubscription.dispose();
                        savingSubscription.dispose();
                    });
                };
                DeliveryZoneNamesController.Name = "DeliveryZoneNamesController";
                return DeliveryZoneNamesController;
            })();
            Controllers.DeliveryZoneNamesController = DeliveryZoneNamesController;
        })(Controllers = DeliveryZonesByRadius.Controllers || (DeliveryZonesByRadius.Controllers = {}));
    })(DeliveryZonesByRadius = MyAndromeda.DeliveryZonesByRadius || (MyAndromeda.DeliveryZonesByRadius = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var DeliveryZonesByRadius;
    (function (DeliveryZonesByRadius) {
        "use strict";
        var Logger = (function () {
            function Logger() {
                this.UseNotify = true;
                this.UseDebug = true;
                this.UseError = true;
            }
            Logger.Notify = function (o) {
                if (logger.UseNotify) {
                    console.log(o);
                }
            };
            Logger.Debug = function (o) {
                if (logger.UseDebug) {
                    console.log(o);
                }
            };
            Logger.Error = function (o) {
                if (logger.UseError) {
                    console.log(o);
                }
            };
            Logger.SettingUpController = function (name, state) {
                if (logger.UseNotify) {
                    console.log("setting up controller - " + name + " : " + state);
                }
            };
            Logger.SettingUpService = function (name, state) {
                if (logger.UseNotify) {
                    console.log("setting up service - " + name + " : " + state);
                }
            };
            Logger.AllowDebug = function (value) {
                logger.UseDebug = value;
            };
            Logger.AllowError = function (value) {
                logger.UseError = value;
            };
            return Logger;
        })();
        DeliveryZonesByRadius.Logger = Logger;
        var logger = new Logger();
    })(DeliveryZonesByRadius = MyAndromeda.DeliveryZonesByRadius || (MyAndromeda.DeliveryZonesByRadius = {}));
})(MyAndromeda || (MyAndromeda = {}));
/// <reference path="MyAndromeda.DeliveryZonesByRadius.App.ts" />
/// <reference path="MyAndromeda.DeliveryZonesByRadius.App.ts" />
/// <reference path="MyAndromeda.DeliveryZonesByRadius.App.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var DeliveryZonesByRadius;
    (function (DeliveryZonesByRadius) {
        var Services;
        (function (Services) {
            DeliveryZonesByRadius.Angular.ServicesInitilizations.push(function (app) {
                app.factory(ContextService.Name, [
                    function () {
                        var instnance = new ContextService();
                        return instnance;
                    }
                ]);
            });
            var ContextService = (function () {
                function ContextService() {
                    this.Model = null;
                    this.ModelSubject = new Rx.Subject();
                    this.PostcodeModels = new Rx.Subject();
                }
                ContextService.Name = "ContextService";
                return ContextService;
            })();
            Services.ContextService = ContextService;
        })(Services = DeliveryZonesByRadius.Services || (DeliveryZonesByRadius.Services = {}));
    })(DeliveryZonesByRadius = MyAndromeda.DeliveryZonesByRadius || (MyAndromeda.DeliveryZonesByRadius = {}));
})(MyAndromeda || (MyAndromeda = {}));
/// <reference path="../../scripts/typings/rx/rx.all.d.ts" />
/// <reference path="MyAndromeda.DeliveryZonesByRadius.App.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var DeliveryZonesByRadius;
    (function (DeliveryZonesByRadius) {
        var Services;
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
                    if (!DeliveryZonesByRadius.Settings.AndromedaSiteId) {
                        throw "Settings.AndromedaSiteId is undefined";
                    }
                    this.Context = context;
                    this.IsDeliveryZonesBusy = new Rx.BehaviorSubject(false);
                    this.IsSavingBusy = new Rx.BehaviorSubject(false);
                    this.Search = new Rx.Subject();
                    this.DataSource = new kendo.data.DataSource({});
                    this.IsDeliveryZonesBusy.onNext(true);
                    var read = kendo.format(DeliveryZonesByRadius.Settings.ReadRoute, DeliveryZonesByRadius.Settings.AndromedaSiteId);
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
                    this.IsSavingBusy.onNext(true);
                    this.IsDeliveryZonesBusy.onNext(true);
                    var promise = $.ajax({
                        url: update,
                        type: "POST",
                        contentType: 'application/json',
                        dataType: "json",
                        data: raw,
                    });
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
                    });
                    promise.always(function () {
                        _this.IsDeliveryZonesBusy.onNext(false);
                        _this.IsSavingBusy.onNext(false);
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
                        var postcodes = result.map(function (postcode) { return {
                            PostCodeSector: postcode,
                            IsSelected: true
                        }; });
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
        })(Services = DeliveryZonesByRadius.Services || (DeliveryZonesByRadius.Services = {}));
    })(DeliveryZonesByRadius = MyAndromeda.DeliveryZonesByRadius || (MyAndromeda.DeliveryZonesByRadius = {}));
})(MyAndromeda || (MyAndromeda = {}));
/// <reference path="MyAndromeda.DeliveryZonesByRadius.App.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var DeliveryZonesByRadius;
    (function (DeliveryZonesByRadius) {
        var Settings = (function () {
            function Settings() {
            }
            Settings.AndromedaSiteId = 0;
            //public static WebSiteId: number = 0;
            //api/{AndromedaSiteId}/DeliveryZonesByRadius/Read
            Settings.ReadRoute = "/api/{0}/DeliveryZonesByRadius/Read";
            //api/{AndromedaSiteId}/DeliveryZonesByRadius/GeneratePostCodeSectors
            Settings.ReadPostCodesRoute = "/api/{0}/DeliveryZonesByRadius/GeneratePostCodeSectors";
            //api/{AndromedaSiteId}/DeliveryZonesByRadius/Update
            Settings.UpdateRoute = "/api/{0}/DeliveryZonesByRadius/Update";
            return Settings;
        })();
        DeliveryZonesByRadius.Settings = Settings;
        ;
    })(DeliveryZonesByRadius = MyAndromeda.DeliveryZonesByRadius || (MyAndromeda.DeliveryZonesByRadius = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var KendoExtensions;
    (function (KendoExtensions) {
        var EditorTools;
        (function (EditorTools) {
            var emailEditorService = (function () {
                //public static editorOptions = {
                //    tools: [
                //        "bold", "italic", "underline",
                //        "justifyLeft", "justifyCenter", "justifyRight", "justifyFull",
                //        "insertUnorderedList", "insertOrderedList",
                //        "indent", "outdent",
                //        "createLink", "unlink",
                //        {
                //            name: "tokenTool",
                //            tooltip: "Add tokens",
                //            template  
                //        }
                //    ]
                //};
                function emailEditorService(options) {
                    this.options = options;
                    this.element = $(options.elementId);
                    //document.getElementById(options.elementId);
                    this.fieldsTemplate = $(options.fieldsTemplateId);
                    //document.getElementById(options.fieldsTemplateId);
                }
                //confirm all the elements are set
                emailEditorService.prototype.checkIntegrity = function () {
                    if (!this.element) {
                        throw new Error("The element for the editor is not known");
                    }
                    if (!this.fieldsTemplate) {
                        throw new Error("the template for the field selection is not known");
                    }
                    if (!this.options.editorOptions) {
                        throw new Error("the editor options are not set");
                    }
                };
                emailEditorService.prototype.eventChangeTokenSelected = function (dropwDown, e) {
                    var dataItem = dropwDown.dataItem(e.item.index());
                    this.currentTemplate = dataItem.value;
                };
                emailEditorService.prototype.setupFieldsTemplate = function () {
                    var internal = this;
                    var dropDown = $("#ToolsInsertToken").kendoDropDownList({
                        dataSource: this.options.tokenOptions,
                        dataTextField: "text",
                        dataValueField: "value",
                        optionLabel: "Select"
                    }).data("kendoDropDownList");
                    dropDown.bind("select", function (e) {
                        internal.eventChangeTokenSelected(dropDown, e);
                        //$.proxy(internal, "eventChangeTokenSelected", [dropDown, e]);
                    });
                };
                emailEditorService.prototype.insertTemplate = function () {
                    var template = this.currentTemplate + "&nbsp;";
                    if (!template) {
                        alert("Please select a token first");
                    }
                    this.getEditor().paste(template, {});
                };
                emailEditorService.prototype.setupInsertButton = function () {
                    var internal = this;
                    var button = $(".k-insert-token-button").on("click", function (e) {
                        e.preventDefault();
                        internal.insertTemplate();
                    });
                };
                emailEditorService.prototype.manageSelection = function (e) {
                    //var range = this.getEditor().getRange();
                    //var selection = this.getEditor().getSelection();
                    var r = this.getEditor();
                    var a = 0;
                };
                emailEditorService.prototype.setupEditor = function () {
                    var internal = this;
                    var editorElememt = $(this.element);
                    var editor = editorElememt.kendoEditor(this.options.editorOptions).data("kendoEditor");
                    editor.bind("select", function (e) {
                        internal.manageSelection(e);
                    });
                };
                emailEditorService.prototype.init = function () {
                    //validate internal
                    this.checkIntegrity();
                    this.setupEditor();
                    this.setupFieldsTemplate();
                    this.setupInsertButton();
                };
                emailEditorService.prototype.getEditor = function () {
                    return $(this.element).data("kendoEditor");
                };
                emailEditorService.prototype.selected = function () {
                    return this.getEditor().selectedHtml();
                };
                //about range http://www.quirksmode.org/dom/range_intro.html
                emailEditorService.prototype.selectedRange = function () {
                    return this.getEditor().getRange();
                };
                return emailEditorService;
            })();
            EditorTools.emailEditorService = emailEditorService;
        })(EditorTools = KendoExtensions.EditorTools || (KendoExtensions.EditorTools = {}));
    })(KendoExtensions = MyAndromeda.KendoExtensions || (MyAndromeda.KendoExtensions = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var Services;
    (function (Services) {
        var services = angular.module("MyAndromeda.Core", []);
        var UUIdService = (function () {
            function UUIdService() {
            }
            UUIdService.prototype.GenerateUUID = function () {
                var d = new Date().getTime();
                if (window.performance && typeof window.performance.now === "function") {
                    d += performance.now(); //use high-precision timer if available
                }
                var uuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
                    var r = (d + Math.random() * 16) % 16 | 0;
                    d = Math.floor(d / 16);
                    return (c == 'x' ? r : (r & 0x3 | 0x8)).toString(16);
                });
                return uuid;
            };
            return UUIdService;
        })();
        Services.UUIdService = UUIdService;
        services.service("uuidService", UUIdService);
    })(Services = MyAndromeda.Services || (MyAndromeda.Services = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    "use strict";
    var Logger = (function () {
        function Logger() {
            this.UseNotify = true;
            this.UseDebug = true;
            this.UseError = true;
        }
        Logger.Notify = function (o) {
            if (logger.UseNotify) {
                console.log(o);
            }
        };
        Logger.Debug = function (o) {
            if (logger.UseDebug) {
                console.log(o);
            }
        };
        Logger.Error = function (o) {
            if (logger.UseError) {
                console.log(o);
            }
        };
        Logger.SettingUpController = function (name, state) {
            if (logger.UseNotify) {
                console.log("setting up controller - " + name + " : " + state);
            }
        };
        Logger.SettingUpService = function (name, state) {
            if (logger.UseNotify) {
                console.log("setting up service - " + name + " : " + state);
            }
        };
        Logger.AllowDebug = function (value) {
            logger.UseDebug = value;
        };
        Logger.AllowError = function (value) {
            logger.UseError = value;
        };
        return Logger;
    })();
    MyAndromeda.Logger = Logger;
    var logger = new Logger();
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var Services;
    (function (Services) {
        var m = angular.module("MyAndromeda.Progress", []);
        var ProgressService = (function () {
            function ProgressService() {
            }
            ProgressService.prototype.Create = function ($element) {
                this.$element = $element;
                return this;
            };
            ProgressService.prototype.Show = function () {
                kendo.ui.progress($(this.$element), true);
            };
            ProgressService.prototype.ShowProgress = function ($element) {
                kendo.ui.progress($($element), true);
            };
            ProgressService.prototype.Hide = function () {
                kendo.ui.progress($(this.$element), false);
            };
            ProgressService.prototype.HideProgress = function ($element) {
                kendo.ui.progress($($element), false);
            };
            return ProgressService;
        })();
        Services.ProgressService = ProgressService;
        m.factory("progressService", function () {
            return new ProgressService();
        });
    })(Services = MyAndromeda.Services || (MyAndromeda.Services = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var Services;
    (function (Services) {
        var m = angular.module("MyAndromeda.Resize", []);
        var ResizeService = (function () {
            function ResizeService() {
                var r = new Rx.Subject();
                this.ResizeObservable = r.throttle(250);
                $(window).resize(function (e) {
                    var width = $(window).width();
                    var height = $(window).height();
                    r.onNext({
                        height: height,
                        width: width
                    });
                });
            }
            return ResizeService;
        })();
        Services.ResizeService = ResizeService;
        m.service("resizeService", ResizeService);
        m.run(function (resizeService) {
            resizeService.ResizeObservable.subscribe(function (e) {
                MyAndromeda.Logger.Notify(kendo.format("Resize: {0}x{1}", e.width, e.height));
            });
        });
        m.directive('ngRightClick', function ($parse) {
            return function (scope, element, attrs) {
                var fn = $parse(attrs.ngRightClick);
                element.bind('contextmenu', function (event) {
                    scope.$apply(function () {
                        event.preventDefault();
                        fn(scope, { $event: event });
                    });
                });
            };
        });
    })(Services = MyAndromeda.Services || (MyAndromeda.Services = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var GridExport;
    (function (GridExport) {
        var Services;
        (function (Services) {
            var KendoGridExcelExporter = (function () {
                function KendoGridExcelExporter(options) {
                    this.options = options;
                }
                KendoGridExcelExporter.prototype.getGrid = function () {
                    return $(this.options.gridSelector).data("kendoGrid");
                };
                KendoGridExcelExporter.prototype.setupExcelEvents = function () {
                    var internal = this;
                    $(this.options.downloadSelector).on("click", function (e) {
                        var grid = internal.getGrid();
                        var currentGroup = grid.dataSource.group();
                        if (currentGroup && currentGroup.length > 0) {
                            alert(KendoGridExcelExporter.notSurportedMessage);
                            e.preventDefault();
                            return;
                        }
                        internal.exportExcel();
                    });
                    //ignore this.
                    $(".k-button-show-orderType-then-payType").on("click", function (e) {
                        var grid = internal.getGrid();
                        var currentGroup = grid.dataSource.group();
                        if (currentGroup && currentGroup.length > 0) {
                            alert(KendoGridExcelExporter.alreadyGroupedItems);
                            e.preventDefault();
                            return;
                        }
                        grid.dataSource.group({
                            field: "PayType", dir: "asc", aggregates: [
                                { field: "PayType", aggregate: "count" },
                                { field: "FinalPrice", aggregate: "min" },
                                { field: "FinalPrice", aggregate: "max" },
                                { field: "FinalPrice", aggregate: "sum" },
                                { field: "FinalPrice", aggregate: "average" }
                            ]
                        });
                    });
                };
                KendoGridExcelExporter.prototype.exportExcel = function () {
                    var internal = this, grid = this.getGrid();
                    var schemaTruncated = grid.columns.map(function (column, index) {
                        return {
                            title: column.title,
                            field: column.field
                        };
                    });
                    var title = this.options.title, model = JSON.stringify(schemaTruncated), data = JSON.stringify(grid.dataSource.view());
                    //get the the pager 
                    $(this.options.titleSelector).val(title);
                    $(this.options.modelSelector).val(model);
                    $(this.options.dataSelector).val(data);
                };
                KendoGridExcelExporter.prototype.init = function () {
                    this.setupExcelEvents();
                };
                KendoGridExcelExporter.notSurportedMessage = "Please remove all groups before exporting. This feature is not supported.";
                KendoGridExcelExporter.alreadyGroupedItems = "There the grid is already grouped by an item";
                return KendoGridExcelExporter;
            })();
            Services.KendoGridExcelExporter = KendoGridExcelExporter;
        })(Services = GridExport.Services || (GridExport.Services = {}));
    })(GridExport = MyAndromeda.GridExport || (MyAndromeda.GridExport = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var Hr;
    (function (Hr) {
        var app = angular.module("MyAndromeda.Hr.Config", [
            "MyAndromeda.Hr.Controllers",
            "MyAndromeda.Hr.Services",
            "MyAndromeda.Hr.Services.Scheduler",
            "MyAndromeda.Hr.Directives",
            "MyAndromeda.Hr.Directives.Scheduler"
        ]);
        app.config(function ($stateProvider, $urlRouterProvider) {
            var hr = {
                abstract: true,
                url: '/hr/:chainId',
                template: '<div id="masterUI" ui-view="main"></div>'
            };
            var hrStoreList = {
                url: "/list/store/:andromedaSiteId",
                views: {
                    "main": {
                        templateUrl: "employee-list.html",
                        controller: "employeeListController"
                    },
                },
                onEnter: function () {
                    MyAndromeda.Logger.Notify("Entering employee list");
                },
                cache: false
            };
            var hrEmployeeList = {
                url: "/employees",
                views: {
                    "store-employee-view": {
                        templateUrl: "store-employee-list.html"
                    }
                }
            };
            var hrStoreScheduler = {
                url: "/schedule",
                views: {
                    "store-employee-view": {
                        templateUrl: "store-employee-scheduler.html"
                    }
                }
            };
            var hrStoreEmployeeEdit = {
                url: "/edit/:id",
                views: {
                    //use the 'main' view area of the 'hr' state. 
                    "main@hr": {
                        templateUrl: "employee-edit.html",
                        controller: "employeeEditController"
                    }
                },
                onEnter: function () {
                    MyAndromeda.Logger.Notify("Entering employee edit");
                },
                cache: false
            };
            var hrStoreEmployeEditDetails = {
                url: "/details",
                views: {
                    "editor-main": {
                        templateUrl: "employee-edit-details.html"
                    }
                },
                cache: false
            };
            var hrStoreEmployeeDocuments = {
                url: "/documents",
                views: {
                    "editor-main": {
                        templateUrl: "hr.store-list.edit-employee.documents.html"
                    }
                }
            };
            var hrStoreEmployeeEditScheduler = {
                url: "/schedule",
                views: {
                    "editor-main": {
                        templateUrl: "employee-edit-schedule.html",
                        controller: "employeeEditSchedulerController"
                    }
                },
                onEnter: function () {
                    MyAndromeda.Logger.Notify("Edit person's schedule.");
                },
                cache: false
            };
            var hrStoreEmployeeCreate = {
                url: "/create/",
                views: {
                    //use the 'main' view area of the 'hr' state. 
                    "main@hr": {
                        templateUrl: "employee-edit.html",
                        controller: "employeeEditController"
                    }
                },
                onEnter: function () {
                    MyAndromeda.Logger.Notify("Entering employee create");
                },
                cache: false
            };
            MyAndromeda.Logger.Notify("set hr states");
            // route: /hr-store
            $stateProvider.state("hr", hr);
            $stateProvider.state("hr.store-list", hrStoreList);
            $stateProvider.state("hr.store-list.employee-list", hrEmployeeList);
            $stateProvider.state("hr.store-list.scheduler", hrStoreScheduler);
            $stateProvider.state("hr.store-list.edit-employee", hrStoreEmployeeEdit);
            $stateProvider.state("hr.store-list.edit-employee.details", hrStoreEmployeEditDetails);
            $stateProvider.state("hr.store-list.edit-employee.schedule", hrStoreEmployeeEditScheduler);
            $stateProvider.state("hr.store-list.edit-employee.documents", hrStoreEmployeeDocuments);
            $stateProvider.state("hr.store-list.create-employee", hrStoreEmployeeCreate);
        });
        app.run(function ($rootScope) {
            $rootScope.$on('$stateChangeStart', function (event, toState, toParams, fromState, fromParams) {
                MyAndromeda.Logger.Notify("$stateChangeStart");
            });
            $rootScope.$on('$stateNotFound', function (event, unfoundState, fromState, fromParams) {
                MyAndromeda.Logger.Notify("$stateNotFound");
            });
            $rootScope.$on('$stateChangeSuccess', function (event, toState, toParams, fromState, fromParams) {
                MyAndromeda.Logger.Notify("$stateChangeSuccess");
            });
        });
    })(Hr = MyAndromeda.Hr || (MyAndromeda.Hr = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var Hr;
    (function (Hr) {
        var Controllers;
        (function (Controllers) {
            var app = angular.module("MyAndromeda.Hr.Controllers", ["kendo.directives", "oitozero.ngSweetAlert"]);
            app.controller("employeeListController", function ($scope, $element, $timeout, $stateParams, SweetAlert, employeeService, employeeServiceState, employeeSchedulerService, progressService) {
                $scope.$stateParams = $stateParams;
                employeeServiceState.ChainId.onNext($stateParams.chainId);
                employeeServiceState.AndromedaSiteId.onNext($stateParams.andromedaSiteId);
                employeeService.Saved.subscribe(function (saved) {
                    if (saved) {
                        SweetAlert.swal("Saved!", "", "success");
                    }
                });
                var employeeGridDataSource = employeeService.StoreEmployeeDataSource;
                var headerTemplate = $("#employee-list-header-template").html();
                var actionsTemplate = $("#employee-list-row-template").html();
                var chainId = $stateParams.chainId;
                var andromedaSiteId = $stateParams.andromedaSiteId;
                var employeePicTemplate = "<employee-pic employee='dataItem'></employee-pic>";
                employeePicTemplate = kendo.format(employeePicTemplate, chainId, andromedaSiteId);
                var employeeGridOptions = {
                    dataSource: employeeGridDataSource,
                    autoBind: true,
                    filterable: true,
                    sortable: true,
                    groupable: true,
                    resizable: true,
                    toolbar: kendo.template(headerTemplate),
                    columns: [
                        { field: "Department", title: "Department", width: 100, minScreenWidth: 400 },
                        {
                            title: "Contact",
                            columns: [
                                {
                                    field: "Name",
                                    title: "Name",
                                    width: 400,
                                    template: employeePicTemplate
                                },
                                { field: "Phone", title: "Phone", width: 100 },
                                { field: "Email", title: "Email", width: 200, minScreenWidth: 400 }
                            ]
                        },
                        {
                            title: "Actions",
                            width: 100,
                            template: actionsTemplate,
                        }
                    ]
                };
                var storeIdChanged = employeeServiceState.AndromedaSiteId.where(function (e) { return e !== null; }).map(function (e) {
                    var r = employeeService.GetStore(chainId, e);
                    return r;
                }).flatMap(function (e) { return e; });
                storeIdChanged.subscribe(function (stores) {
                    MyAndromeda.Logger.Notify("stores: ");
                    MyAndromeda.Logger.Notify(stores);
                    $timeout(function () {
                        var schedulerOptions = employeeSchedulerService.GetStoreEmployeeScheduler(stores);
                        $scope.schedulerOptions = schedulerOptions;
                    });
                });
                var savingSubscription = employeeService.SavingSchedule.where(function (e) { return e; }).subscribe(function (saving) {
                    MyAndromeda.Logger.Notify("Scheduler saving.... show progress");
                    progressService.ShowProgress($element);
                });
                var savedSubscription = employeeService.SavingSchedule.where(function (e) { return !e; }).subscribe(function (notSaving) {
                    MyAndromeda.Logger.Notify("Scheduler saved!.... hide progress");
                    progressService.HideProgress($element);
                });
                $scope.$on('$destroy', function () {
                    savingSubscription.dispose();
                    savedSubscription.dispose();
                });
                $scope.employeeGridOptions = employeeGridOptions;
            });
            app.controller("employeeEditController", function ($element, $scope, $stateParams, $timeout, SweetAlert, progressService, employeeService, employeeServiceState, uuidService) {
                MyAndromeda.Logger.Notify("stateParams");
                MyAndromeda.Logger.Notify($stateParams);
                var chainId = $stateParams.chainId;
                var andromedaSiteId = $stateParams.andromedaSiteId;
                employeeServiceState.ChainId.onNext(chainId);
                employeeServiceState.AndromedaSiteId.onNext(andromedaSiteId);
                var status = {
                    uploading: false,
                    random: uuidService.GenerateUUID()
                };
                var getOrCreateEmployee = function () {
                    var employeeId = $stateParams.id;
                    if (!employeeId) {
                        var modelCreator = kendo.data.Model.define(Hr.Models.employeeDataSourceSchema);
                        var newEmployee = new modelCreator({
                            Id: uuidService.GenerateUUID(),
                            Name: "",
                            PrimaryRole: "",
                            Roles: [],
                            ShiftStatus: {}
                        });
                        return newEmployee;
                    }
                    MyAndromeda.Logger.Notify("employee id:" + employeeId);
                    var employee = employeeService.StoreEmployeeDataSource.get(employeeId);
                    MyAndromeda.Logger.Notify(employee);
                    MyAndromeda.Logger.Notify("uid: " + employee.uid);
                    return employee;
                };
                var dataSource = employeeService.StoreEmployeeDataSource;
                MyAndromeda.Logger.Notify("data-source length: " + dataSource.data().length);
                var noData = dataSource.data().length === 0;
                var setEmployee = function () {
                    var employee = getOrCreateEmployee();
                    MyAndromeda.Logger.Notify(employee);
                    $scope.employee = employee;
                    employeeServiceState.EditEmployee.onNext(employee);
                };
                if (noData && !employeeService.IsLoading) {
                    MyAndromeda.Logger.Notify("Load employees");
                    var promise = employeeService.StoreEmployeeDataSource.read();
                    promise.then(function () {
                        setEmployee();
                    });
                }
                else if (noData && employeeService.IsLoading) {
                    var loadingSubscription = employeeService.Loading.where(function (e) { return !e; }).subscribe(function () {
                        setEmployee();
                        loadingSubscription.dispose();
                    });
                }
                else {
                    setEmployee();
                }
                var save = function (employee) {
                    MyAndromeda.Logger.Notify("saved called");
                    //do i need these anymore? 
                    //employee.dirty = true;
                    employee.set("DirtyHack", true);
                    var validator = $scope.validator;
                    var valid = validator.validate();
                    if (!valid) {
                        MyAndromeda.Logger.Notify("validation failed.");
                        return;
                    }
                    progressService.ShowProgress($element);
                    var sync = employeeService.Save(employee);
                    sync.then(function () {
                        progressService.HideProgress($element);
                        MyAndromeda.Logger.Notify("Sync done");
                        var name = employee.get("ShortName");
                        SweetAlert.swal("Saved!", name + " has been saved.", "success");
                        employeeServiceState.EmployeeUpdated.onNext(employee);
                    });
                };
                var getProfilePic = function (employee) {
                    var route = "/content/profile-picture.jpg";
                    if (employee) {
                        route = employeeService.GetEmployeePictureUrl($stateParams.chainId, $stateParams.andromedaSiteId, employee.Id);
                        route += "?r=" + status.random;
                    }
                    return route;
                };
                $scope.status = status;
                $scope.saveRoute = function (employee) {
                    MyAndromeda.Logger.Notify("save route");
                    var id = "";
                    if (employee) {
                        id = employee.Id;
                    }
                    var route = employeeService.GetUploadRouteUrl($stateParams.chainId, $stateParams.andromedaSiteId, $scope.employee.Id);
                    return route;
                };
                $scope.onUploading = function () {
                    MyAndromeda.Logger.Notify("uploading profile pic");
                    status.uploading = true;
                    status.random = undefined;
                };
                $scope.onUploadSuccess = function (e) {
                    MyAndromeda.Logger.Notify("uploaded profile pic");
                    $timeout(function () {
                        status.uploading = false;
                        status.random = uuidService.GenerateUUID();
                    });
                };
                $scope.onSelect = function (e) {
                    var message = $.map(e.files, function (file) { return file.name; }).join(", ");
                    console.log(message);
                };
                //uploadSettings
                $scope.save = save;
                $scope.profilePicture = getProfilePic;
            });
            app.controller("employeeEditSchedulerController", function ($element, $scope, $timeout, employeeService, employeeServiceState, employeeSchedulerService) {
                var loadRelatedStoresObservable = employeeServiceState.EditEmployee
                    .where(function (e) { return e !== null; })
                    .map(function (employee) {
                    var loadObservable = employeeService.GetStoreListByEmployee(employeeServiceState.CurrentChainId, employeeServiceState.CurrentAndromedaSiteId, employee.Id);
                    return loadObservable;
                }).flatMap(function (e) { return e; });
                var editEmployeeObservable = employeeServiceState.EditEmployee
                    .where(function (e) { return e !== null; });
                var merged = Rx.Observable.combineLatest(loadRelatedStoresObservable, editEmployeeObservable, function (stores, employee) {
                    MyAndromeda.Logger.Notify("Stores: ");
                    MyAndromeda.Logger.Notify(stores);
                    MyAndromeda.Logger.Notify("Employee: " + employee.ShortName);
                    return {
                        stores: stores,
                        employee: employee
                    };
                });
                var editSubscription = merged.subscribe(function (data) {
                    MyAndromeda.Logger.Notify("stores+ employees available");
                    $timeout(function () {
                        var schedulerOptions = employeeSchedulerService.GetSingleEmployeeScheduler(data.stores, data.employee);
                        $scope.schedulerOptions = schedulerOptions;
                    });
                });
                $scope.$on('$destroy', function () {
                    editSubscription.dispose();
                });
            });
        })(Controllers = Hr.Controllers || (Hr.Controllers = {}));
    })(Hr = MyAndromeda.Hr || (MyAndromeda.Hr = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var Hr;
    (function (Hr) {
        var Directives;
        (function (Directives) {
            var app = angular.module("MyAndromeda.Hr.Directives.Scheduler", []);
            app.directive("rotaTaskEditor", function () {
                return {
                    name: "rotaTaskEditor",
                    scope: {
                        task: "=task",
                    },
                    templateUrl: "rotaTaskEditor.html",
                    controller: function ($scope, employeeService, employeeServiceState) {
                        var task = $scope.task;
                        MyAndromeda.Logger.Notify("rota task started");
                        var storeEmployeeDataSource = employeeService.StoreEmployeeDataSource;
                        var taskTypeDataSource = Hr.Models.taskTypes; //Models.
                        var dataSources = {
                            storeEmployeeDataSource: storeEmployeeDataSource,
                            storeTaskTypeDataSource: taskTypeDataSource,
                        };
                        $scope.dataSources = dataSources;
                    }
                };
            });
            app.directive("workingTask", function () {
                return {
                    name: "workingTask",
                    templateUrl: "working-task.html",
                    scope: {
                        task: "=task",
                        timeLineMode: "=timeLineMode"
                    },
                    controller: function ($element, $scope, employeeService) {
                        var task = $scope.task;
                        var employee = employeeService.StoreEmployeeDataSource.get(task.EmployeeId);
                        if (employee === null) {
                            MyAndromeda.Logger.Notify("cant find the person");
                        }
                        $scope.employee = employee;
                        var topElement = $($element).closest(".k-event");
                        var borderStyle = "";
                        switch (employee.Department) {
                            case "Front of house":
                                borderStyle = 'task-front-of-house';
                                break;
                            case "Kitchen":
                                borderStyle = 'task-kitchen';
                                break;
                            case "Management":
                                borderStyle = 'task-management';
                                break;
                            case "Delivery":
                                borderStyle = 'task-delivery';
                                break;
                        }
                        topElement.addClass("task-border");
                        topElement.addClass(borderStyle);
                        var status = {
                            clone: null
                        };
                        var popover = topElement.popover({
                            title: "Task preview",
                            placement: "auto",
                            html: true,
                            content: "please wait",
                            trigger: "click"
                        }).on("show.bs.popover", function () {
                            var html = topElement.html();
                            popover.attr('data-content', html);
                            var current = $(this);
                            setTimeout(function () { current.popover('hide'); }, 5000);
                            $scope.$on('$destroy', function () {
                                //current.fadeOut();
                            });
                        });
                        $scope.$on('$destroy', function () {
                            popover.hide();
                        });
                        var extra = {
                            hours: Math.abs(task.end.getTime() - task.start.getTime()) / 36e5,
                            startTime: kendo.toString(task.start, "HH:mm"),
                            endTime: kendo.toString(task.end, "HH:mm")
                        };
                        $scope.extra = extra;
                    }
                };
            });
            app.directive("employeeTask", function () {
                return {
                    name: "employeeTask",
                    templateUrl: "employee-task.html",
                    scope: {
                        task: "=task",
                        timeLineMode: "=timeLineMode"
                    },
                    controller: function ($element, $scope, employeeService) {
                        var task = $scope.task;
                        var employee = employeeService.StoreEmployeeDataSource.get(task.EmployeeId);
                        if (employee === null) {
                            MyAndromeda.Logger.Notify("cant find the person");
                        }
                        $scope.employee = employee;
                        var topElement = $($element).closest(".k-event");
                        var borderStyle = "";
                        switch (employee.Department) {
                            case "Front of house":
                                borderStyle = 'task-front-of-house';
                                break;
                            case "Kitchen":
                                borderStyle = 'task-kitchen';
                                break;
                            case "Management":
                                borderStyle = 'task-management';
                                break;
                            case "Delivery":
                                borderStyle = 'task-delivery';
                                break;
                        }
                        topElement.addClass("task-border");
                        topElement.addClass(borderStyle);
                        var status = {
                            clone: null
                        };
                        //var popover = topElement.popover({
                        //    title: "Task preview",
                        //    placement: "auto",
                        //    html: true,
                        //    content: "please wait",
                        //    trigger: "hover"
                        //}).on("show.bs.popover", function() {
                        //    let html = topElement.html();
                        //    popover.attr('data-content', html);
                        //    var current = $(this); 
                        //    setTimeout(() => { current.popover('hide'); }, 5000)
                        //});
                        //topElement.on("hover", function (e) {
                        //    Logger.Notify("animate .k-event");
                        //});
                        var popover = topElement.popover({
                            title: "Task preview",
                            placement: "auto",
                            html: true,
                            content: "please wait",
                            trigger: "click"
                        }).on("show.bs.popover", function () {
                            var html = topElement.html();
                            popover.attr('data-content', html);
                            var current = $(this);
                            setTimeout(function () { current.popover('hide'); }, 5000);
                            $scope.$on('$destroy', function () {
                                //current.fadeOut();
                            });
                        });
                        $scope.$on('$destroy', function () {
                            popover.hide();
                        });
                        var extra = {
                            hours: Math.abs(task.end.getTime() - task.start.getTime()) / 36e5,
                            startTime: kendo.toString(task.start, "HH:mm"),
                            endTime: kendo.toString(task.end, "HH:mm")
                        };
                        $scope.extra = extra;
                    }
                };
            });
        })(Directives = Hr.Directives || (Hr.Directives = {}));
    })(Hr = MyAndromeda.Hr || (MyAndromeda.Hr = {}));
})(MyAndromeda || (MyAndromeda = {}));
/// <reference path="../../scripts/typings/bootstrap/bootstrap.d.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var Hr;
    (function (Hr) {
        var Directives;
        (function (Directives) {
            var app = angular.module("MyAndromeda.Hr.Directives", []);
            app.directive("employeePic", function () {
                return {
                    name: "employeePic",
                    templateUrl: "employee-pic.html",
                    restrict: "EA",
                    transclude: true,
                    scope: {
                        employeeId: '=id',
                        employee: '=employee',
                        showShortName: "=showShortName",
                        showFullName: "=showFullName",
                        showWorkStatus: "=showWorkStatus"
                    },
                    controller: function ($scope, $timeout, employeeService, employeeServiceState, uuidService) {
                        if (!$scope.employee) {
                            MyAndromeda.Logger.Notify("I have a employee Id: " + $scope.employeeId);
                            MyAndromeda.Logger.Notify($scope);
                        }
                        var dataItem = $scope.employee;
                        var getValueOrDefault = function (source, defaultValue) {
                            var v = source;
                            var k = typeof (v);
                            if (k == "undefined") {
                                return defaultValue;
                            }
                            return v;
                        };
                        var options = {
                            showShortName: getValueOrDefault($scope.showShortName, false),
                            //typeof ($scope.showShortName) == "undefined" ? true : $scope.showShortName,
                            showFullName: getValueOrDefault($scope.showFullName, false),
                            //typeof($scope.showFullName) == "undefined" ? true : $scope.showFullName,
                            showWorkStatus: getValueOrDefault($scope.showWorkStatus, false)
                        };
                        $scope.options = options;
                        var state = {
                            random: uuidService.GenerateUUID()
                        };
                        $scope.$watch('showShortName', function (newValue, old) {
                            $timeout(function () { options.showShortName = getValueOrDefault(newValue, true); });
                        });
                        $scope.$watch('showFullName', function (newValue, oldValue) {
                            $timeout(function () { options.showFullName = getValueOrDefault(newValue, true); });
                        });
                        $scope.$watch('showWorkStatus', function (newValue, oldValue) {
                            $timeout(function () { options.showWorkStatus = getValueOrDefault(newValue, true); });
                        });
                        var updates = employeeServiceState.EmployeeUpdated.where(function (e) { return e.Id == dataItem.Id; }).subscribe(function (change) {
                            $timeout(function () {
                                MyAndromeda.Logger.Notify(dataItem.ShortName + " updated");
                                //just run ... not nothing to do. 
                                state.random = uuidService.GenerateUUID();
                            });
                        });
                        ;
                        $scope.state = state;
                        $scope.profilePicture = function () {
                            //var profilePicture = "/content/profile-picture.jpg";
                            var chainId = employeeServiceState.CurrentChainId;
                            var andromedaSiteId = employeeServiceState.CurrentAndromedaSiteId;
                            var route = employeeService.GetEmployeePictureUrl(chainId, andromedaSiteId, dataItem.Id);
                            route = route + "?r=" + state.random;
                            return {
                                'background-image': 'url(' + route + ')'
                            };
                        };
                        $scope.$on('$destroy', function () {
                            updates.dispose();
                        });
                    }
                };
            });
            app.directive("employeeDocs", function () {
                return {
                    name: "employeeDocs",
                    templateUrl: "employee-documents.html",
                    restrict: "EA",
                    scope: {
                        dataItem: '=employee',
                        save: "=save"
                    },
                    controller: function ($element, $scope, $timeout, SweetAlert, progressService, employeeService, employeeServiceState, uuidService) {
                        var dataItem = $scope.dataItem;
                        var save = function () {
                            MyAndromeda.Logger.Notify("save - from docs");
                            var employee = $scope.dataItem;
                            var promise = employeeService.Save(employee);
                            progressService.ShowProgress($element);
                            promise.then(function () {
                                SweetAlert.swal("Saved!", name + " has been saved.", "success");
                                progressService.HideProgress($element);
                            });
                        };
                        $scope.save = save;
                        MyAndromeda.Logger.Notify("dataItem: " + dataItem);
                        MyAndromeda.Logger.Notify($scope);
                        var status = {
                            uploading: false,
                            random: uuidService.GenerateUUID()
                        };
                        if (!dataItem.Documents) {
                            dataItem.Documents = [];
                        }
                        else {
                            dataItem.Documents = dataItem.Documents.filter(function () { return true; });
                        }
                        $scope.status = status;
                        $scope.uploadRoute = function () {
                            var chainId = employeeServiceState.CurrentAndromedaSiteId, andromedaSiteId = employeeServiceState.CurrentAndromedaSiteId, document = $scope.document;
                            var route = employeeService.GetDocumentUploadRoute(chainId, andromedaSiteId, dataItem.Id, document.Id);
                            return route;
                        };
                        $scope.onUploading = function (e) {
                            MyAndromeda.Logger.Notify("upload started");
                            status.uploading = true;
                            status.random = undefined;
                        };
                        $scope.onUploadSuccess = function (e) {
                            MyAndromeda.Logger.Notify("upload success");
                            MyAndromeda.Logger.Notify(e);
                            var document = $scope.document;
                            e.files.forEach(function (item) {
                                document.Files.push({
                                    FileName: item.name
                                });
                            });
                            //save();
                            $timeout(function () {
                                status.uploading = false;
                                status.random = uuidService.GenerateUUID();
                            });
                        };
                        $scope.onUploadComplete = function (e) {
                            MyAndromeda.Logger.Notify("upload complete:");
                            MyAndromeda.Logger.Notify(e);
                            $timeout(function () {
                                status.uploading = false;
                                status.random = uuidService.GenerateUUID();
                            });
                        };
                        $scope.removeAll = function () {
                            var r = function () {
                                dataItem.Documents = [];
                                save();
                            };
                            SweetAlert.swal({
                                title: "Are you sure?",
                                text: "You will not be able to recover these files!",
                                type: "warning",
                                showCancelButton: true,
                                confirmButtonColor: "#DD6B55",
                                confirmButtonText: "Yes, delete it!",
                                closeOnConfirm: false
                            }, function (isConfirm) {
                                MyAndromeda.Logger.Notify("confirm:" + isConfirm);
                                if (isConfirm) {
                                    var editing = $scope.document;
                                    r();
                                    MyAndromeda.Logger.Notify("alert removed");
                                    SweetAlert.swal("Deleted!", "Your file has been deleted.", "success");
                                }
                                else {
                                    MyAndromeda.Logger.Notify("alert cancel");
                                }
                            });
                        };
                        $scope.new = function () {
                            var doc = {
                                Id: uuidService.GenerateUUID(),
                                Name: null,
                                Files: []
                            };
                            MyAndromeda.Logger.Notify("Add to documents");
                            dataItem.Documents.push(doc);
                            MyAndromeda.Logger.Notify("set $scope.document");
                            $scope.document = doc;
                        };
                        $scope.select = function (doc) {
                            MyAndromeda.Logger.Notify(doc);
                            if (!doc.Files) {
                                doc.Files = [];
                            }
                            $timeout(function () {
                                $scope.document = undefined;
                            }).then(function () {
                                $timeout(function () {
                                    $scope.document = doc;
                                }, 100);
                            });
                        };
                        $scope.clear = function () {
                            $scope.document = undefined;
                        };
                        $scope.remove = function (document) {
                            var removeItem = function () {
                                var editing = $scope.document;
                                if (editing && editing.Id == document.Id) {
                                    $scope.document = undefined;
                                }
                                dataItem.Documents = dataItem.Documents.filter(function (item) {
                                    return item.Id !== document.Id;
                                });
                                //save();
                            };
                            SweetAlert.swal({
                                title: "Are you sure?",
                                text: "You will not be able to recover this file!",
                                type: "warning",
                                showCancelButton: true,
                                confirmButtonColor: "#DD6B55",
                                confirmButtonText: "Yes, delete it!",
                                closeOnConfirm: false
                            }, function (isConfirm) {
                                MyAndromeda.Logger.Notify("confirm:" + isConfirm);
                                if (isConfirm) {
                                    var editing = $scope.document;
                                    removeItem();
                                    MyAndromeda.Logger.Notify("alert removed");
                                    SweetAlert.swal("Deleted!", "Your file has been deleted.", "success");
                                }
                                else {
                                    MyAndromeda.Logger.Notify("alert cancel");
                                }
                            });
                        };
                        $scope.removeFile = function (document, file) {
                            var removeItem = function () {
                                document.Files = document.Files.filter(function (item) { return item.FileName !== file.FileName; });
                                //save();
                            };
                            SweetAlert.swal({
                                title: "Are you sure?",
                                text: "You will not be able to recover this file!",
                                type: "warning",
                                showCancelButton: true,
                                confirmButtonColor: "#DD6B55",
                                confirmButtonText: "Yes, delete it!",
                                closeOnConfirm: false
                            }, function (isConfirm) {
                                MyAndromeda.Logger.Notify("confirm:" + isConfirm);
                                if (isConfirm) {
                                    var editing = $scope.document;
                                    removeItem();
                                    MyAndromeda.Logger.Notify("alert removed");
                                    SweetAlert.swal("Deleted!", "Your file has been deleted.", "success");
                                }
                                else {
                                    MyAndromeda.Logger.Notify("alert cancel");
                                }
                            });
                        };
                        $scope.getEmployeeDocumentImage = function (document, file) {
                            var chainId = employeeServiceState.CurrentAndromedaSiteId, andromedaSiteId = employeeServiceState.CurrentAndromedaSiteId;
                            var route = employeeService.GetDocumentRouteUrl(chainId, andromedaSiteId, dataItem.Id, document.Id, file.FileName);
                            route = route + "?r=" + status.random;
                            return route;
                        };
                        $scope.downloadDocumentFile = function (document, file) {
                            var chainId = employeeServiceState.CurrentAndromedaSiteId, andromedaSiteId = employeeServiceState.CurrentAndromedaSiteId;
                            var route = employeeService.GetDocumentDownloadRouteUrl(chainId, andromedaSiteId, dataItem.Id, document.Id, file.FileName);
                            return route;
                        };
                        $scope.dataItem = dataItem;
                    }
                };
            });
        })(Directives = Hr.Directives || (Hr.Directives = {}));
    })(Hr = MyAndromeda.Hr || (MyAndromeda.Hr = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var Hr;
    (function (Hr) {
        var KendoThings;
        (function (KendoThings) {
            var extend = $.extend, Logger = MyAndromeda.Logger, k = kendo, ui = kendo.ui, kData = kendo.data, kDate = k.date, kAttr = k.attr, kGetter = k.getter, SchedulerTimelineView = ui.TimelineView, SchedulerMonthView = ui.MonthView, MS_PER_DAY = kDate.MS_PER_DAY, MS_PER_MINUTE = kDate.MS_PER_MINUTE, SchedulerView = ui.SchedulerView, NS = ".kendoTimelineWeekView";
            function customCreateLayoutConfiguration(name, resources, inner, something) {
                var resource = resources[0];
                if (resource) {
                    var configuration = [];
                    var data = resource.dataSource.view();
                    for (var dataIndex = 0; dataIndex < data.length; dataIndex++) {
                        var defaultText = kendo.htmlEncode(k.getter(resource.dataTextField)(data[dataIndex]));
                        var dataItem = data[dataIndex];
                        MyAndromeda.Logger.Notify(dataItem);
                        var templateText = "\n                    <div>{0}</div>\n                    <employee-pic id=\"'{0}'\"></employee-pic>";
                        templateText = kendo.format(templateText, dataItem.Id);
                        var template = kendo.template("<a href='javascript: void(0)'>#=data.Email#</a>");
                        var template2 = kendo.template("<a href='javascript: void(0)'>{{dataItem.Email}}</a>");
                        var obj = {
                            text: template2(dataItem),
                            className: "k-slot-cell",
                            data: dataItem,
                            dataItem: dataItem
                        };
                        //var element = $(template2(data));
                        //var scope = something.$angular_scope; //scope from _createColumnsLayout
                        //scope('
                        //this.angular is not defined :( 
                        //this.angular('compile', function () {
                        //    return {
                        //        elements: element,
                        //        data: [{ dataItem: data }]
                        //    };
                        //});
                        //obj[name] = customCreateLayoutConfiguration(name, resources.slice(1), inner);
                        //text version
                        configuration.push(obj);
                    }
                    return configuration;
                }
                return inner;
            }
            function shiftArray(array, idx) {
                return array.slice(idx).concat(array.slice(0, idx));
            }
            function createLayoutConfiguration(name, resources, inner, template) {
                var resource = resources[0];
                if (resource) {
                    var configuration = [];
                    var data = resource.dataSource.view();
                    data = data.sort(function (a, b) {
                        var aValue = a.Department ? a.Department : "NA", bValue = b.Department ? b.Department : "NA";
                        return aValue.length - bValue.length;
                    });
                    for (var dataIndex = 0; dataIndex < data.length; dataIndex++) {
                        var dataItem = data[dataIndex];
                        var things = Hr.Models.departments;
                        var searchDepartments = things.filter(function (e) { return e.text === dataItem.Department; });
                        var department = { text: 'None', majorColour: "#000", minorColour: "#000" };
                        if (searchDepartments.length > 0) {
                            department = searchDepartments[0];
                        }
                        var obj = {
                            text: template({
                                text: kendo.htmlEncode(kGetter(resource.dataTextField)(data[dataIndex])),
                                majorColor: department.majorColour,
                                minorColor: department.minorColour,
                                employee: dataItem,
                                field: resource.field,
                                title: resource.title,
                                name: resource.name,
                                value: kGetter(resource.dataValueField)(data[dataIndex])
                            }),
                            className: 'k-slot-cell k-thingy'
                        };
                        obj[name] = createLayoutConfiguration(name, resources.slice(1), inner, template);
                        configuration.push(obj);
                    }
                    return configuration;
                }
                return inner;
            }
            /**
             * ***********************************************************
             * Timeline view
             * ***********************************************************
             */
            var timeLineOptions = {
                nextDate: function () {
                    return kDate.nextDay(this.startDate());
                },
                calculateDateRange: function () {
                    var selectedDate = this.options.date, start = selectedDate, end = kDate, dates = [];
                    for (var index = 0, length_1 = 7; index < length_1; index++) {
                        dates.push(start);
                        start = kDate.nextDay(start);
                    }
                    this._render(dates);
                },
                _createEventElement: function (event) {
                    var options = this.options;
                    var editable = options.editable;
                    var isMobile = this._isMobile();
                    event.showDelete = editable && editable.destroy !== false && !isMobile;
                    event.resizable = false; //editable && editable.resize !== false && !isMobile;
                    event.ns = kendo.ns;
                    event.resources = this.eventResources(event);
                    event.inverseColor = event.resources && event.resources[0] ? this._shouldInverseResourceColor(event.resources[0]) : false;
                    var element = $(this.eventTemplate(event));
                    this.angular('compile', function () {
                        return {
                            elements: element,
                            data: [{ dataItem: event }]
                        };
                    });
                    return element;
                },
                _render: function (dates) {
                    var that = this;
                    dates = dates || [];
                    that._dates = dates;
                    that._startDate = dates[0];
                    that._endDate = dates[dates.length - 1 || 0];
                    that._calculateSlotRanges();
                    that.createLayout(that._layout(dates));
                    that._content(dates);
                    that._footer();
                    that._setContentWidth();
                    that.refreshLayout();
                    that.datesHeader.on('click' + NS, '.k-nav-day', function (e) {
                        var th = $(e.currentTarget).closest('th');
                        var slot = that._slotByPosition(th.offset().left, that.content.offset().top);
                        that.trigger('navigate', {
                            view: 'timeline',
                            date: slot.startDate()
                        });
                    });
                    that.timesHeader.find('table tr:last').hide();
                    that.datesHeader.find('table tr:last').hide();
                },
                _positionEvent: function (eventObject) {
                    var slotsCollection = eventObject.slotRange.collection;
                    //var eventWidth = 100;
                    var eventHeight = this.options.eventHeight + 2;
                    var rect = eventObject.slotRange.innerRect(eventObject.start, eventObject.end, false);
                    var left = this._adjustLeftPosition(rect.left);
                    var width = rect.right - rect.left - 2;
                    if (width < 0) {
                        width = 0;
                    }
                    if (width < this.options.eventMinWidth) {
                        var lastSlot = slotsCollection._slots[slotsCollection._slots.length - 1];
                        var offsetRight = lastSlot.offsetLeft + lastSlot.offsetWidth;
                        width = this.options.eventMinWidth;
                        if (offsetRight < left + width) {
                            width = offsetRight - rect.left - 2;
                        }
                    }
                    eventObject.element.css({
                        top: eventObject.slotRange.start.offsetTop + eventObject.rowIndex * (eventHeight + 2) + 'px',
                        left: left,
                        height: 50,
                        width: width //slot.clientWidth - 2
                    });
                    $(eventObject.element).hover(function () {
                        $(this).animate({ width: 100 });
                    }, function () {
                        $(this).animate({ width: width });
                    });
                },
                _renderEvents: function (events, groupIndex, eventGroup) {
                    var event;
                    var idx;
                    var length;
                    for (idx = 0, length = events.length; idx < length; idx++) {
                        event = events[idx];
                        if (this._isInDateSlot(event)) {
                            var isMultiDayEvent = event.isAllDay || event.end.getTime() - event.start.getTime() >= MS_PER_DAY;
                            var container = this.content;
                            if (isMultiDayEvent || this._isInTimeSlot(event)) {
                                var adjustedEvent = this._adjustEvent(event);
                                var group = this.groups[groupIndex];
                                if (!group._continuousEvents) {
                                    group._continuousEvents = [];
                                }
                                var ranges = group.slotRanges(adjustedEvent.occurrence, false);
                                var range = ranges[0];
                                var element;
                                if (this._isInTimeSlot(adjustedEvent.occurrence)) {
                                    element = this._createEventElement(adjustedEvent.occurrence, event, range.head || adjustedEvent.head, range.tail || adjustedEvent.tail);
                                    element.appendTo(container).css({
                                        top: 0,
                                        height: this.options.eventHeight
                                    });
                                    var eventObject = {
                                        start: adjustedEvent.occurrence._startTime || adjustedEvent.occurrence.start,
                                        end: adjustedEvent.occurrence._endTime || adjustedEvent.occurrence.end,
                                        element: element,
                                        uid: event.uid,
                                        slotRange: range,
                                        rowIndex: 0,
                                        offsetTop: 0
                                    };
                                    eventGroup.events[event.uid] = eventObject;
                                    this.addContinuousEvent(group, range, element, event.isAllDay);
                                    this._arrangeRows(eventObject, range, eventGroup);
                                }
                            }
                        }
                    }
                },
            };
            var SchedulerTimelineWeekView = SchedulerTimelineView.extend(timeLineOptions);
            extend(true, ui, {
                SchedulerTimelineWeekView: SchedulerTimelineWeekView
            });
        })(KendoThings = Hr.KendoThings || (Hr.KendoThings = {}));
    })(Hr = MyAndromeda.Hr || (MyAndromeda.Hr = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var Hr;
    (function (Hr) {
        var KendoThings;
        (function (KendoThings) {
            var extend = $.extend, Logger = MyAndromeda.Logger, k = kendo, ui = kendo.ui, kData = kendo.data, kDate = k.date, kAttr = k.attr, kGetter = k.getter, kTemplate = k.Template, KUserEvents = k.UserEvents, getDate = k.date.getDate, 
            //SchedulerMonthView = ui.MonthView,
            SchedulerView = ui.SchedulerView, MS_PER_DAY = kDate.MS_PER_DAY, MS_PER_MINUTE = kDate.MS_PER_MINUTE, NS = ".kendoTimelineWeekView";
            function shiftArray(array, idx) {
                return array.slice(idx).concat(array.slice(0, idx));
            }
            function firstVisibleMonthDay(date, calendarInfo) {
                var firstDay = calendarInfo.firstDay, firstVisibleDay = new Date(date.getFullYear(), date.getMonth(), 0, date.getHours(), date.getMinutes(), date.getSeconds(), date.getMilliseconds());
                while (firstVisibleDay.getDay() != firstDay) {
                    k.date.setTime(firstVisibleDay, -1 * MS_PER_DAY);
                }
                return firstVisibleDay;
            }
            function isInDateRange(value, min, max) {
                var msMin = min, msMax = max, msValue;
                msValue = value;
                return msValue >= msMin && msValue <= msMax;
            }
            function createLayoutConfiguration(name, resources, inner, template) {
                var resource = resources[0];
                if (resource) {
                    var configuration = [];
                    var data = resource.dataSource.view();
                    data = data.sort(function (a, b) {
                        var aValue = a.Department ? a.Department : "NA", bValue = b.Department ? b.Department : "NA";
                        return aValue.length - bValue.length;
                    });
                    for (var dataIndex = 0; dataIndex < data.length; dataIndex++) {
                        var dataItem = data[dataIndex];
                        var things = Hr.Models.departments;
                        var searchDepartments = things.filter(function (e) { return e.text === dataItem.Department; });
                        var department = { text: 'None', majorColour: "#000", minorColour: "#000" };
                        if (searchDepartments.length > 0) {
                            department = searchDepartments[0];
                        }
                        Logger.Notify("department:");
                        Logger.Notify(department);
                        var obj = {
                            text: template({
                                text: kendo.htmlEncode(kGetter(resource.dataTextField)(data[dataIndex])),
                                majorColor: department.majorColour,
                                minorColor: department.minorColour,
                                employee: dataItem,
                                field: resource.field,
                                title: resource.title,
                                name: resource.name,
                                value: kGetter(resource.dataValueField)(data[dataIndex])
                            }),
                            className: 'k-slot-cell k-thingy'
                        };
                        obj[name] = createLayoutConfiguration(name, resources.slice(1), inner, template);
                        configuration.push(obj);
                    }
                    return configuration;
                }
                return inner;
            }
            /**
             * ***********************************************************
             * Month view
             * ***********************************************************
             */
            var NUMBER_OF_COLUMNS = 7, NUMBER_OF_ROWS = 1, MORE_BUTTON_TEMPLATE = kendo.template("<div style=\"width:#=width#px;left:#=left#px;top:#=top#px;height:20px\" class=\"k-more-events k-button\">\n    <span style=\"height:20px; margin-top:0\" class=\"label label-success\">#=count#...</span>\n</div>"), DAY_TEMPLATE = kendo.template('<span class="k-link k-nav-day">#:kendo.toString(date, "dd")#</span>'), EVENT_WRAPPER_STRING = '<div role="gridcell" aria-selected="false" data-#=ns#uid="#=uid#"' + '#if (resources[0]) { #' + 'style="background-color:#=resources[0].color #; border-color: #=resources[0].color#"' + 'class="k-event#=inverseColor ? " k-event-inverse" : ""#"' + '#} else {#' + 'class="k-event"' + '#}#' + '>' + '<span class="k-event-actions">' + '# if(data.tail || data.middle) {#' + '<span class="k-icon k-i-arrow-w"></span>' + '#}#' + '# if(data.isException()) {#' + '<span class="k-icon k-i-exception"></span>' + '# } else if(data.isRecurring()) {#' + '<span class="k-icon k-i-refresh"></span>' + '#}#' + '</span>' + '{0}' + '<span class="k-event-actions">' + '#if (showDelete) {#' + '<a href="\\#" class="k-link k-event-delete"><span class="k-icon k-si-close"></span></a>' + '#}#' + '# if(data.head || data.middle) {#' + '<span class="k-icon k-i-arrow-e"></span>' + '#}#' + '</span>' + '# if(resizable && !data.tail && !data.middle) {#' + '<span class="k-resize-handle k-resize-w"></span>' + '#}#' + '# if(resizable && !data.head && !data.middle) {#' + '<span class="k-resize-handle k-resize-e"></span>' + '#}#' + '</div>', EVENT_TEMPLATE = kendo.template('<div title="#=title.replace(/"/g,"&\\#34;")#">' + '<div class="k-event-template">#:title#</div>' + '</div>');
            var monthViewOptions = {
                //calculateDateRange: function () {
                //    var selectedDate = this.options.date,
                //        start = selectedDate,
                //        dates = [];
                //    for (let index = 0, length = 7; index < length; index++) {
                //        dates.push(start);
                //        start = kDate.nextDay(start);
                //    }
                //    this._render(dates);
                //},
                init: function (element, options) {
                    var that = this;
                    SchedulerView.fn.init.call(that, element, options);
                    that.title = that.options.title;
                    that._templates();
                    that._editable();
                    that._renderLayout(that.options.date);
                    that._groups();
                },
                name: 'WeekOverView',
                _updateDirection: function (selection, ranges, multiple, reverse, vertical) {
                    if (multiple) {
                        var startSlot = ranges[0].start;
                        var endSlot = ranges[ranges.length - 1].end;
                        var isSameSlot = startSlot.index === endSlot.index;
                        var isSameCollection = startSlot.collectionIndex === endSlot.collectionIndex;
                        var updateDirection;
                        if (vertical) {
                            updateDirection = isSameSlot && isSameCollection || isSameCollection;
                        }
                        else {
                            updateDirection = isSameSlot && isSameCollection;
                        }
                        if (updateDirection) {
                            selection.backward = reverse;
                        }
                    }
                },
                _changeViewPeriod: function (selection, reverse, vertical) {
                    var pad = vertical ? 7 : 1;
                    if (reverse) {
                        pad *= -1;
                    }
                    selection.start = kDate.addDays(selection.start, pad);
                    selection.end = kDate.addDays(selection.end, pad);
                    if (!vertical || vertical && this._isVerticallyGrouped()) {
                        selection.groupIndex = reverse ? this.groups.length - 1 : 0;
                    }
                    selection.events = [];
                    return true;
                },
                _continuousSlot: function (selection, ranges, reverse) {
                    var index = selection.backward ? 0 : ranges.length - 1;
                    var group = this.groups[selection.groupIndex];
                    return group.continuousSlot(ranges[index].start, reverse);
                },
                _changeGroupContinuously: function (selection, continuousSlot, multiple, reverse) {
                    if (!multiple) {
                        var groupIndex = selection.groupIndex;
                        var lastGroupIndex = this.groups.length - 1;
                        var vertical = this._isVerticallyGrouped();
                        var group = this.groups[groupIndex];
                        if (!continuousSlot && vertical) {
                            continuousSlot = group[reverse ? 'lastSlot' : 'firstSlot']();
                            groupIndex += reverse ? -1 : 1;
                        }
                        else if (continuousSlot && !vertical) {
                            groupIndex = reverse ? lastGroupIndex : 0;
                        }
                        if (groupIndex < 0 || groupIndex > lastGroupIndex) {
                            groupIndex = reverse ? lastGroupIndex : 0;
                            continuousSlot = null;
                        }
                        selection.groupIndex = groupIndex;
                    }
                    return continuousSlot;
                },
                _createRow: function (startDate, startIdx, cellsPerRow, groupIndex) {
                    var that = this;
                    var min = that._firstDayOfMonth;
                    var max = that._lastDayOfMonth;
                    var content = that.dayTemplate;
                    var classes = '';
                    var html = '';
                    var resources = function () {
                        return that._resourceBySlot({ groupIndex: groupIndex });
                    };
                    for (var cellIdx = 0; cellIdx < cellsPerRow; cellIdx++) {
                        classes = '';
                        if (kDate.isToday(startDate)) {
                            classes += 'k-today';
                        }
                        if (!kDate.isInDateRange(startDate, min, max)) {
                            classes += ' k-other-month';
                        }
                        html += '<td ';
                        if (classes !== '') {
                            html += 'class="' + classes + '"';
                        }
                        html += '>';
                        html += content({
                            date: startDate,
                            resources: resources
                        });
                        html += '</td>';
                        that._slotIndices[kDate.getDate(startDate).getTime()] = startIdx + cellIdx;
                        startDate = kDate.nextDay(startDate);
                    }
                    return html;
                },
                _createRowsLayout: function (resources, inner, template) {
                    return createLayoutConfiguration('rows', resources, inner, template);
                },
                _normalizeHorizontalSelection: function (selection, ranges, reverse) {
                    var slot;
                    if (reverse) {
                        slot = ranges[0].start;
                    }
                    else {
                        slot = ranges[ranges.length - 1].end;
                    }
                    return slot;
                },
                _normalizeVerticalSelection: function (selection, ranges) {
                    var slot;
                    if (selection.backward) {
                        slot = ranges[0].start;
                    }
                    else {
                        slot = ranges[ranges.length - 1].end;
                    }
                    return slot;
                },
                _templates: function () {
                    var options = this.options, settings = extend({}, kTemplate, options.templateSettings);
                    this.eventTemplate = this._eventTmpl(options.eventTemplate, EVENT_WRAPPER_STRING);
                    this.dayTemplate = kendo.template(options.dayTemplate, settings);
                    this.groupHeaderTemplate = kendo.template(options.groupHeaderTemplate, settings);
                },
                dateForTitle: function () {
                    return kendo.format(this.options.selectedDateFormat, this._firstDayOfMonth, this._lastDayOfMonth);
                },
                shortDateForTitle: function () {
                    return kendo.format(this.options.selectedShortDateFormat, this._firstDayOfMonth, this._lastDayOfMonth);
                },
                previousDate: function () {
                    var now = this.startDate();
                    var yesterday = kDate.previousDay(now); //kendo.date.previousDay(this._firstDayOfMonth);
                    return yesterday;
                },
                nextDate: function () {
                    var now = this.startDate();
                    var tomorrow = kDate.nextDay(now);
                    return tomorrow;
                },
                startDate: function () {
                    return this._startDate;
                },
                endDate: function () {
                    return this._endDate;
                },
                _renderLayout: function (date) {
                    var that = this;
                    this._firstDayOfMonth = date;
                    this._lastDayOfMonth = kDate.addDays(date, 7);
                    this._startDate = date;
                    this._endDate = kDate.addDays(date, 7);
                    this.createLayout(this._layout());
                    this._content();
                    this.refreshLayout();
                    this.content.on('click' + NS, '.k-nav-day,.k-more-events', function (e) {
                        var offset = $(e.currentTarget).offset();
                        var slot = that._slotByPosition(offset.left, offset.top);
                        e.preventDefault();
                        //alert("popup :)");
                        //console.log(slot);
                        //let slotElement = slot.element;
                        //var popover = $(slotElement).popover({
                        //    title: "Tasks",
                        //    placement: "auto",
                        //    html: true,
                        //    content: "please wait"
                        //}).on("show.bs.popover", function () {
                        //    console.log("show popover");
                        //    let html = $(slotElement).contents().html();
                        //    popover.attr('data-content', html);
                        //    var current = $(this);
                        //    setTimeout(() => { current.popover('hide'); }, 5000);
                        //});
                        //popover.popover("show");
                        that.trigger('navigate', {
                            view: 'day',
                            date: slot.startDate()
                        });
                    });
                },
                _editable: function () {
                    if (this.options.editable && !this._isMobilePhoneView()) {
                        if (this._isMobile()) {
                            this._touchEditable();
                        }
                        else {
                            this._mouseEditable();
                        }
                    }
                },
                _mouseEditable: function () {
                    var that = this;
                    that.element.on('click' + NS, '.k-scheduler-WeekOverViewview .k-event a:has(.k-si-close)', function (e) {
                        that.trigger('remove', { uid: $(this).closest('.k-event').attr(kAttr('uid')) });
                        e.preventDefault();
                    });
                    if (that.options.editable.create !== false) {
                        that.element.on('dblclick' + NS, '.k-scheduler-WeekOverViewview .k-scheduler-content td', function (e) {
                            Logger.Notify("edit mode");
                            var offset = $(e.currentTarget).offset();
                            var slot = that._slotByPosition(offset.left, offset.top);
                            if (slot) {
                                var resourceInfo = that._resourceBySlot(slot);
                                that.trigger('add', {
                                    eventInfo: extend({
                                        //isAllDay: true,
                                        start: slot.startDate(),
                                        end: slot.startDate()
                                    }, resourceInfo)
                                });
                            }
                            e.preventDefault();
                        });
                    }
                    if (that.options.editable.update !== false) {
                        that.element.on('dblclick' + NS, '.k-scheduler-WeekOverViewview .k-event', function (e) {
                            that.trigger('edit', { uid: $(this).closest('.k-event').attr(kAttr('uid')) });
                            e.preventDefault();
                        });
                    }
                },
                _touchEditable: function () {
                    var that = this;
                    var threshold = 0;
                    if (k.support.mobileOS.android) {
                        threshold = 5;
                    }
                    if (that.options.editable.create !== false) {
                        that._addUserEvents = new KUserEvents(that.element, {
                            threshold: threshold,
                            filter: '.k-scheduler-WeekOverViewview .k-scheduler-content td',
                            tap: function (e) {
                                var offset = $(e.target).offset();
                                var slot = that._slotByPosition(offset.left, offset.top);
                                if (slot) {
                                    var resourceInfo = that._resourceBySlot(slot);
                                    that.trigger('add', {
                                        eventInfo: extend({
                                            isAllDay: true,
                                            start: slot.startDate(),
                                            end: slot.startDate()
                                        }, resourceInfo)
                                    });
                                }
                                e.preventDefault();
                            }
                        });
                    }
                    if (that.options.editable.update !== false) {
                        that._editUserEvents = new k.UserEvents(that.element, {
                            threshold: threshold,
                            filter: '.k-scheduler-WeekOverViewview .k-event',
                            tap: function (e) {
                                if ($(e.event.target).closest('a:has(.k-si-close)').length === 0) {
                                    that.trigger('edit', { uid: $(e.target).closest('.k-event').attr(k.attr('uid')) });
                                    e.preventDefault();
                                }
                            }
                        });
                    }
                },
                selectionByElement: function (cell) {
                    var offset = $(cell).offset();
                    return this._slotByPosition(offset.left, offset.top);
                },
                _columnCountForLevel: function (level) {
                    var columnLevel = this.columnLevels[level];
                    return columnLevel ? columnLevel.length : 0;
                },
                _rowCountForLevel: function (level) {
                    var rowLevel = this.rowLevels[level];
                    return rowLevel ? rowLevel.length : 0;
                },
                _content: function () {
                    var html = '<tbody>';
                    var verticalGroupCount = 1;
                    var resources = this.groupedResources;
                    if (resources.length) {
                        if (this._isVerticallyGrouped()) {
                            verticalGroupCount = this._rowCountForLevel(resources.length - 1);
                        }
                    }
                    for (var verticalGroupIdx = 0; verticalGroupIdx < verticalGroupCount; verticalGroupIdx++) {
                        html += this._createCalendar(verticalGroupIdx);
                    }
                    html += '</tbody>';
                    this.content.find('table').html(html);
                },
                _createCalendar: function (verticalGroupIndex) {
                    var start = this.startDate();
                    var cellCount = NUMBER_OF_COLUMNS * NUMBER_OF_ROWS;
                    var cellsPerRow = NUMBER_OF_COLUMNS;
                    var weekStartDates = [start];
                    var html = '';
                    var horizontalGroupCount = 1;
                    var isVerticallyGrouped = this._isVerticallyGrouped();
                    var resources = this.groupedResources;
                    if (resources.length) {
                        if (!isVerticallyGrouped) {
                            horizontalGroupCount = this._columnCountForLevel(resources.length - 1);
                        }
                    }
                    this._slotIndices = {};
                    for (var rowIdx = 0, length = 1; rowIdx < length; rowIdx++) {
                        html += '<tr>';
                        weekStartDates.push(start);
                        var startIdx = rowIdx * cellsPerRow;
                        for (var groupIdx = 0; groupIdx < horizontalGroupCount; groupIdx++) {
                            html += this._createRow(start, startIdx, cellsPerRow, isVerticallyGrouped ? verticalGroupIndex : groupIdx);
                        }
                        start = kDate.addDays(start, cellsPerRow);
                        html += '</tr>';
                    }
                    this._weekStartDates = weekStartDates;
                    this._endDate = kDate.previousDay(start);
                    return html;
                },
                _layout: function () {
                    var calendarInfo = this.calendarInfo();
                    var weekDayNames = this._isMobile() ? calendarInfo.days.namesShort : calendarInfo.days.names;
                    var names = shiftArray(weekDayNames, calendarInfo.firstDay);
                    //var columns = $.map(names, function (value) {
                    //    return { text: value };
                    //});
                    var columns = [];
                    var columnDate = this.startDate();
                    for (var i = 0; i < 7; i++) {
                        var m = kendo.toString(columnDate, 'm'), d = kendo.toString(columnDate, 'ddd');
                        columns.push({
                            text: d + ", " + m
                        });
                        columnDate = kDate.nextDay(columnDate);
                    }
                    var resources = this.groupedResources;
                    Logger.Notify("Grouped resources:");
                    Logger.Notify(resources);
                    var rows;
                    if (resources.length) {
                        if (this._isVerticallyGrouped()) {
                            var inner = [];
                            for (var idx = 0; idx < 1; idx++) {
                                inner.push({
                                    text: '<div>&nbsp;</div>',
                                    className: 'k-hidden k-slot-cell'
                                });
                            }
                            rows = this._createRowsLayout(resources, inner, this.groupHeaderTemplate);
                            console.log("rows: ");
                            console.log(rows);
                        }
                        else {
                            columns = this._createColumnsLayout(resources, columns, this.groupHeaderTemplate);
                        }
                    }
                    return {
                        columns: columns,
                        rows: rows
                    };
                },
                _createEventElement: function (event) {
                    var options = this.options;
                    var editable = options.editable;
                    var isMobile = this._isMobile();
                    event.showDelete = editable && editable.destroy !== false && !isMobile;
                    event.resizable = editable && editable.resize !== false && !isMobile;
                    event.ns = kendo.ns;
                    event.resources = this.eventResources(event);
                    event.inverseColor = event.resources && event.resources[0] ? this._shouldInverseResourceColor(event.resources[0]) : false;
                    var element = $(this.eventTemplate(event));
                    this.angular('compile', function () {
                        return {
                            elements: element,
                            data: [{ dataItem: event }]
                        };
                    });
                    return element;
                },
                _isInDateSlot: function (event) {
                    var groups = this.groups[0];
                    var slotStart = groups.firstSlot().start;
                    var slotEnd = groups.lastSlot().end - 1;
                    var startTime = kDate.toUtcTime(event.start);
                    var endTime = kDate.toUtcTime(event.end);
                    return (isInDateRange(startTime, slotStart, slotEnd)
                        || isInDateRange(endTime, slotStart, slotEnd)
                        || isInDateRange(slotStart, startTime, endTime)
                        || isInDateRange(slotEnd, startTime, endTime))
                        && (!isInDateRange(endTime, slotStart, slotStart)
                            || isInDateRange(endTime, startTime, startTime) || event.isAllDay);
                },
                _slotIndex: function (date) {
                    return this._slotIndices[getDate(date).getTime()];
                },
                _positionMobileEvent: function (slotRange, element, group) {
                    var startSlot = slotRange.start;
                    if (slotRange.start.offsetLeft > slotRange.end.offsetLeft) {
                        startSlot = slotRange.end;
                    }
                    var startIndex = slotRange.start.index;
                    var endIndex = startIndex;
                    var eventCount = 3;
                    var events = SchedulerView.collidingEvents(slotRange.events(), startIndex, endIndex);
                    events.push({
                        element: element,
                        start: startIndex,
                        end: endIndex
                    });
                    var rows = SchedulerView.createRows(events);
                    var slot = slotRange.collection.at(startIndex);
                    var container = slot.container;
                    if (!container) {
                        container = $(kendo.format('<div class="k-events-container" style="top:{0};left:{1};width:{2}"/>', startSlot.offsetTop + startSlot.firstChildTop + startSlot.firstChildHeight - 3 + 'px', startSlot.offsetLeft + 'px', startSlot.offsetWidth + 'px'));
                        slot.container = container;
                        this.content[0].appendChild(container[0]);
                    }
                    if (rows.length <= eventCount) {
                        slotRange.addEvent({
                            element: element,
                            start: startIndex,
                            end: endIndex,
                            groupIndex: startSlot.groupIndex
                        });
                        group._continuousEvents.push({
                            element: element,
                            uid: element.attr(kAttr('uid')),
                            start: slotRange.start,
                            end: slotRange.end
                        });
                        container[0].appendChild(element[0]);
                    }
                },
                _positionEvent: function (slotRange, element, group) {
                    var eventHeight = this.options.eventHeight;
                    var startSlot = slotRange.start;
                    if (slotRange.start.offsetLeft > slotRange.end.offsetLeft) {
                        startSlot = slotRange.end;
                    }
                    var startIndex = slotRange.start.index;
                    var endIndex = slotRange.end.index;
                    var eventCount = startSlot.eventCount;
                    var events = SchedulerView.collidingEvents(slotRange.events(), startIndex, endIndex);
                    var rightOffset = startIndex !== endIndex ? 5 : 4;
                    events.push({
                        element: element,
                        start: startIndex,
                        end: endIndex
                    });
                    var rows = SchedulerView.createRows(events);
                    for (var idx = 0, length = Math.min(rows.length, eventCount); idx < length; idx++) {
                        var rowEvents = rows[idx].events;
                        var eventTop = startSlot.offsetTop + startSlot.firstChildHeight + idx * eventHeight + 3 * idx + 'px';
                        for (var j = 0, eventLength = rowEvents.length; j < eventLength; j++) {
                            rowEvents[j].element[0].style.top = eventTop;
                        }
                    }
                    if (rows.length > eventCount) {
                        for (var slotIndex = startIndex; slotIndex <= endIndex; slotIndex++) {
                            var collection = slotRange.collection;
                            var slot = collection.at(slotIndex);
                            if (slot.more) {
                                return;
                            }
                            slot.more = $(MORE_BUTTON_TEMPLATE({
                                ns: kendo.ns,
                                start: slotIndex,
                                end: slotIndex,
                                count: eventCount,
                                width: slot.clientWidth - 2,
                                left: slot.offsetLeft + 2,
                                top: slot.offsetTop + slot.firstChildHeight + eventCount * eventHeight + 3 * eventCount
                            }));
                            this.content[0].appendChild(slot.more[0]);
                        }
                    }
                    else {
                        slotRange.addEvent({
                            element: element,
                            start: startIndex,
                            end: endIndex,
                            groupIndex: startSlot.groupIndex
                        });
                        element[0].style.width = slotRange.innerWidth() - rightOffset + 'px';
                        element[0].style.left = startSlot.offsetLeft + 2 + 'px';
                        element[0].style.height = eventHeight + 'px';
                        group._continuousEvents.push({
                            element: element,
                            uid: element.attr(k.attr('uid')),
                            start: slotRange.start,
                            end: slotRange.end
                        });
                        element.appendTo(this.content);
                    }
                },
                //_slotByPosition: function (x, y) {
                //    let content = this.content;
                //    if (!content) {
                //        content = $("div.k-scheduler-content");
                //    }
                //    var offset = content.offset();
                //    x -= offset.left;
                //    y -= offset.top;
                //    y += content[0].scrollTop;
                //    x += content[0].scrollLeft;
                //    x = Math.ceil(x);
                //    y = Math.ceil(y);
                //    for (var groupIndex = 0; groupIndex < this.groups.length; groupIndex++) {
                //        var slot = this.groups[groupIndex].daySlotByPosition(x, y);
                //        if (slot) {
                //            return slot;
                //        }
                //    }
                //    return null;
                //},
                _slotByPosition: function (x, y) {
                    var offset = this.content.offset();
                    x -= offset.left;
                    y -= offset.top;
                    y += this.content[0].scrollTop;
                    x += this.content[0].scrollLeft;
                    x = Math.ceil(x);
                    y = Math.ceil(y);
                    for (var groupIndex = 0; groupIndex < this.groups.length; groupIndex++) {
                        var slot = this.groups[groupIndex].daySlotByPosition(x, y);
                        if (slot) {
                            return slot;
                        }
                    }
                    return null;
                },
                _createResizeHint: function (range) {
                    var left = range.startSlot().offsetLeft;
                    var top = range.start.offsetTop;
                    var width = range.innerWidth();
                    var height = range.start.clientHeight - 2;
                    var hint = SchedulerView.fn._createResizeHint.call(this, left, top, width, height);
                    hint.appendTo(this.content);
                    this._resizeHint = this._resizeHint.add(hint);
                },
                _updateResizeHint: function (event, groupIndex, startTime, endTime) {
                    this._removeResizeHint();
                    var group = this.groups[groupIndex];
                    var ranges = group.ranges(startTime, endTime, true, event.isAllDay);
                    for (var rangeIndex = 0; rangeIndex < ranges.length; rangeIndex++) {
                        this._createResizeHint(ranges[rangeIndex]);
                    }
                    this._resizeHint.find('.k-label-top,.k-label-bottom').text('');
                    this._resizeHint.first().addClass('k-first').find('.k-label-top').text(kendo.toString(k.timezone.toLocalDate(startTime), 'M/dd'));
                    this._resizeHint.last().addClass('k-last').find('.k-label-bottom').text(kendo.toString(k.timezone.toLocalDate(endTime), 'M/dd'));
                },
                _updateMoveHint: function (event, groupIndex, distance) {
                    var start = k.date.toUtcTime(event.start) + distance;
                    var end = start + event.duration();
                    var group = this.groups[groupIndex];
                    var ranges = group.ranges(start, end, true, event.isAllDay);
                    this._removeMoveHint();
                    for (var rangeIndex = 0; rangeIndex < ranges.length; rangeIndex++) {
                        var range = ranges[rangeIndex];
                        var startSlot = range.startSlot();
                        var endSlot = range.endSlot();
                        var hint = this._createEventElement(event.clone({
                            head: range.head,
                            tail: range.tail
                        }));
                        hint.css({
                            left: startSlot.offsetLeft + 2,
                            top: startSlot.offsetTop + startSlot.firstChildHeight,
                            height: this.options.eventHeight,
                            width: range.innerWidth() - (startSlot.index !== endSlot.index ? 5 : 4)
                        });
                        hint.addClass('k-event-drag-hint');
                        hint.appendTo(this.content);
                        this._moveHint = this._moveHint.add(hint);
                    }
                },
                //_positionEvent: function (slotRange, element, group) {
                //    var eventHeight = this.options.eventHeight;
                //    var startSlot = slotRange.start;
                //    if (slotRange.start.offsetLeft > slotRange.end.offsetLeft) {
                //        startSlot = slotRange.end;
                //    }
                //    var startIndex = slotRange.start.index;
                //    var endIndex = slotRange.end.index;
                //    var eventCount = startSlot.eventCount;
                //    var events = SchedulerView.collidingEvents(slotRange.events(), startIndex, endIndex);
                //    var rightOffset = startIndex !== endIndex ? 5 : 4;
                //    events.push({
                //        element: element,
                //        start: startIndex,
                //        end: endIndex
                //    });
                //    var rows = SchedulerView.createRows(events);
                //    for (var idx = 0, length = Math.min(rows.length, eventCount); idx < length; idx++) {
                //        var rowEvents = rows[idx].events;
                //        var eventTop = startSlot.offsetTop + startSlot.firstChildHeight + idx * eventHeight + 3 * idx + 'px';
                //        for (var j = 0, eventLength = rowEvents.length; j < eventLength; j++) {
                //            rowEvents[j].element[0].style.top = eventTop;
                //        }
                //    }
                //    if (rows.length > eventCount) {
                //        for (var slotIndex = startIndex; slotIndex <= endIndex; slotIndex++) {
                //            var collection = slotRange.collection;
                //            var slot = collection.at(slotIndex);
                //            if (slot.more) {
                //                return;
                //            }
                //            slot.more = $(MORE_BUTTON_TEMPLATE({
                //                ns: kendo.ns,
                //                start: slotIndex,
                //                end: slotIndex,
                //                width: slot.clientWidth - 2,
                //                left: slot.offsetLeft + 2,
                //                top: slot.offsetTop + slot.firstChildHeight + eventCount * eventHeight + 3 * eventCount,
                //                count: rows.length
                //            }));
                //            this.content[0].appendChild(slot.more[0]);
                //        }
                //    } else {
                //        slotRange.addEvent({
                //            element: element,
                //            start: startIndex,
                //            end: endIndex,
                //            groupIndex: startSlot.groupIndex
                //        });
                //        element[0].style.width = slotRange.innerWidth() - rightOffset + 'px';
                //        element[0].style.left = startSlot.offsetLeft + 2 + 'px';
                //        element[0].style.height = eventHeight + 'px';
                //        group._continuousEvents.push({
                //            element: element,
                //            uid: element.attr(kAttr('uid')),
                //            start: slotRange.start,
                //            end: slotRange.end
                //        });
                //        element.appendTo(this.content);
                //    }
                //},
                _groups: function () {
                    var groupCount = this._groupCount();
                    var columnCount = NUMBER_OF_COLUMNS;
                    var rowCount = NUMBER_OF_ROWS;
                    this.groups = [];
                    for (var idx = 0; idx < groupCount; idx++) {
                        this._addResourceView(idx);
                    }
                    var tableRows = this.content[0].getElementsByTagName('tr');
                    var startDate = this.startDate();
                    for (var groupIndex = 0; groupIndex < groupCount; groupIndex++) {
                        var cellCount = 0;
                        var rowMultiplier = 0;
                        if (this._isVerticallyGrouped()) {
                            rowMultiplier = groupIndex;
                        }
                        for (var rowIndex = rowMultiplier * rowCount; rowIndex < (rowMultiplier + 1) * rowCount; rowIndex++) {
                            var group = this.groups[groupIndex];
                            var collection = group.addDaySlotCollection(kDate.addDays(startDate, cellCount), kDate.addDays(this.startDate(), cellCount + columnCount));
                            var tableRow = tableRows[rowIndex];
                            var cells = tableRow.children;
                            var cellMultiplier = 0;
                            Logger.Notify("group: ");
                            Logger.Notify(group);
                            Logger.Notify("collection: " + collection);
                            Logger.Notify(collection);
                            tableRow.setAttribute('role', 'row');
                            if (!this._isVerticallyGrouped()) {
                                cellMultiplier = groupIndex;
                            }
                            for (var cellIndex = cellMultiplier * columnCount; cellIndex < (cellMultiplier + 1) * columnCount; cellIndex++) {
                                var cell = cells[cellIndex];
                                var clientHeight = cell.clientHeight;
                                var firstChildHeight = cell.children.length ? cell.children[0].offsetHeight + 3 : 0;
                                var start = kDate.addDays(startDate, cellCount);
                                var end = kDate.MS_PER_DAY;
                                if (startDate.getHours() !== start.getHours()) {
                                    end += (startDate.getHours() - start.getHours()) * kDate.MS_PER_HOUR;
                                }
                                start = kDate.toUtcTime(start);
                                end += start;
                                cellCount++;
                                //var eventCount = 20; 
                                var overflows = Math.floor((clientHeight - firstChildHeight - this.options.moreButtonHeight) / (this.options.eventHeight + 3));
                                //console.log("show events: " + eventCount);
                                //let majorColour = "#fff";
                                //let minorColour = "#465298";
                                //minorColour = ""; 
                                //let background = "repeating-linear-gradient(90deg, {0}, {0} 10px, {1} 10px, {1} 20px)";
                                //background = kendo.format(background, majorColour, minorColour);
                                //$(cell).css({
                                //    "background": background
                                //});
                                cell.setAttribute('role', 'gridcell');
                                cell.setAttribute('aria-selected', false);
                                collection.addDaySlot(cell, start, end, overflows);
                            }
                        }
                    }
                },
                render: function (events) {
                    this.content.children('.k-event,.k-more-events,.k-events-container').remove();
                    this._groups();
                    events = new kData.Query(events).sort([
                        {
                            field: 'start',
                            dir: 'asc'
                        },
                        {
                            field: 'end',
                            dir: 'desc'
                        }
                    ]).toArray();
                    var resources = this.groupedResources;
                    if (resources.length) {
                        this._renderGroups(events, resources, 0, 1);
                    }
                    else {
                        this._renderEvents(events, 0);
                    }
                    this.refreshLayout();
                    this.trigger('activate');
                },
                _renderEvents: function (events, groupIndex) {
                    var event;
                    var idx;
                    var length;
                    var isMobilePhoneView = this._isMobilePhoneView();
                    //console.log("_renderEvents: per row - event.length" +  events.length);
                    for (idx = 0, length = events.length; idx < length; idx++) {
                        event = events[idx];
                        var dataInSlot = this._isInDateSlot(event);
                        if (dataInSlot) {
                            var group = this.groups[groupIndex];
                            if (!group._continuousEvents) {
                                group._continuousEvents = [];
                            }
                            var ranges = group.slotRanges(event, true);
                            var rangeCount = ranges.length;
                            for (var rangeIndex = 0; rangeIndex < rangeCount; rangeIndex++) {
                                var range = ranges[rangeIndex];
                                var start = event.start;
                                var end = event.end;
                                //find max ? 
                                if (rangeCount > 1) {
                                    if (rangeIndex === 0) {
                                        end = range.end.endDate();
                                    }
                                    else if (rangeIndex == rangeCount - 1) {
                                        start = range.start.startDate();
                                    }
                                    else {
                                        start = range.start.startDate();
                                        end = range.end.endDate();
                                    }
                                }
                                var occurrence = event.clone({
                                    start: start,
                                    end: end,
                                    head: range.head,
                                    tail: range.tail
                                });
                                //find max ? 
                                if (isMobilePhoneView) {
                                    this._positionMobileEvent(range, this._createEventElement(occurrence), group);
                                }
                                else {
                                    this._positionEvent(range, this._createEventElement(occurrence), group);
                                }
                            }
                        }
                    }
                },
                _renderGroups: function (events, resources, offset, columnLevel) {
                    var resource = resources[0];
                    if (resource) {
                        var view = resource.dataSource.view();
                        Logger.Notify("resource view");
                        Logger.Notify(view);
                        /* sort by department */
                        view = view.sort(function (a, b) {
                            var aValue = a.Department ? a.Department : "NA", bValue = b.Department ? b.Department : "NA";
                            return aValue.length - bValue.length;
                        });
                        for (var itemIdx = 0; itemIdx < view.length; itemIdx++) {
                            var value = this._resourceValue(resource, view[itemIdx]);
                            //MyAndromeda.Logger.Notify("group operator:");
                            //MyAndromeda.Logger.Notify(SchedulerView.groupEqFilter(value));
                            var tmp = new kData.Query(events).filter({
                                field: resource.field,
                                operator: SchedulerView.groupEqFilter(value)
                            }).toArray();
                            //tmp = tmp.sort((a, b) => {
                            //    let aValue = a.Department, bValue = b.Department;
                            //    return aValue.length - bValue.length;
                            //});
                            if (resources.length > 1) {
                                offset = this._renderGroups(tmp, resources.slice(1), offset++, columnLevel + 1);
                            }
                            else {
                                this._renderEvents(tmp, offset++);
                            }
                        }
                    }
                    return offset;
                },
                _groupCount: function () {
                    var resources = this.groupedResources;
                    if (resources.length) {
                        if (this._isVerticallyGrouped()) {
                            return this._rowCountForLevel(resources.length - 1);
                        }
                        else {
                            return this._columnCountForLevel(resources.length) / this._columnOffsetForResource(resources.length);
                        }
                    }
                    return 1;
                },
                _columnOffsetForResource: function (index) {
                    return this._columnCountForLevel(index) / this._columnCountForLevel(index - 1);
                },
                destroy: function () {
                    if (this.table) {
                        this.table.removeClass('k-scheduler-WeekOverViewview');
                    }
                    if (this.content) {
                        this.content.off(NS);
                    }
                    if (this.element) {
                        this.element.off(NS);
                    }
                    SchedulerView.fn.destroy.call(this);
                    if (this._isMobile() && !this._isMobilePhoneView() && this.options.editable) {
                        if (this.options.editable.create !== false) {
                            this._addUserEvents.destroy();
                        }
                        if (this.options.editable.update !== false) {
                            this._editUserEvents.destroy();
                        }
                    }
                },
                events: [
                    'remove',
                    'add',
                    'edit',
                    'navigate'
                ],
                options: {
                    title: 'Month',
                    name: 'month',
                    //eventHeight: 25,
                    moreButtonHeight: 13,
                    editable: true,
                    selectedDateFormat: '{0:y}',
                    selectedShortDateFormat: '{0:y}',
                    groupHeaderTemplate: '#=text#',
                    dayTemplate: DAY_TEMPLATE,
                    eventTemplate: EVENT_TEMPLATE
                }
            };
            var SchedulerMonthWeekView = SchedulerView.extend(monthViewOptions);
            MyAndromeda.Logger.Notify("Defining SchedulerTimelineWeekView");
            extend(true, ui, {
                MonthTimeWeekView: SchedulerMonthWeekView
            });
        })(KendoThings = Hr.KendoThings || (Hr.KendoThings = {}));
    })(Hr = MyAndromeda.Hr || (MyAndromeda.Hr = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var Hr;
    (function (Hr) {
        var Models;
        (function (Models) {
            Models.taskTypes = [
                {
                    text: "Normal Shift",
                    value: "Shift",
                    color: "#ffffff"
                },
                {
                    text: "Need cover",
                    value: "Need cover",
                    color: "#d9534f"
                },
                {
                    text: "Covering Shift",
                    value: "Covering Shift",
                    color: "#d9edf7"
                },
                {
                    text: "Unplanned leave",
                    value: "Unplanned",
                    color: "#f2dede"
                },
                {
                    text: "Planned leave",
                    value: "Holiday",
                    color: "#fcf8e3"
                }
            ];
            Models.departments = [
                { text: 'Front of house', majorColour: "#AA6C39", minorColour: "#FFD0AA" },
                { text: 'Kitchen', majorColour: "#2D882D", minorColour: "#87CC87" },
                { text: 'Management', majorColour: "#AA3939", minorColour: "#FFAAAA" },
                { text: 'Delivery', majorColour: "#483d8b", minorColour: "#938CC5" }
            ];
            Models.getSchedulerDataSourceSchema = function (andromedaSiteId, employeeService, employeeId) {
                var employeePart = function () {
                    var employee = {
                        type: "string",
                        //defaultValue: employeeId,
                        nullable: false,
                        validation: {
                            required: true
                        }
                    };
                    if (employeeId) {
                        employee.defaultValue = employeeId;
                    }
                    return employee;
                };
                var model = {
                    id: "Id",
                    fields: {
                        Id: {
                            type: "string",
                            nullable: true
                        },
                        title: { from: "Title", defaultValue: "No title", validation: { required: true } },
                        start: { type: "date", from: "Start" },
                        end: { type: "date", from: "End" },
                        startTimezone: { from: "StartTimezone" },
                        endTimezone: { from: "EndTimezone" },
                        description: { from: "Description" },
                        recurrenceId: { from: "RecurrenceId" },
                        recurrenceRule: { from: "RecurrenceRule" },
                        recurrenceException: { from: "RecurrenceException" },
                        isAllDay: { type: "boolean", from: "IsAllDay" },
                        EmployeeId: employeePart(),
                        AndromedaSiteId: {
                            type: "number",
                            defaultValue: andromedaSiteId,
                            nullable: false,
                            validation: {
                                required: true
                            }
                        },
                        TaskType: {
                            type: "string",
                            defaultValue: "Shift",
                            nullable: false,
                            validation: {
                                required: true
                            }
                        },
                        Department: {
                            type: "string",
                            nullable: true
                        }
                    }
                };
                return model;
            };
            Models.employeeDataSourceSchema = {
                id: "Id",
                fields: {
                    Id: {
                        type: "string",
                        nullable: true
                    },
                    Deleted: {
                        type: "boolean",
                        defaultValue: false,
                        nullable: false
                    },
                    ShortName: {
                        type: "string",
                        nullable: false
                    },
                    Phone: {
                        type: "string",
                        nullable: false
                    },
                    DirtyHack: {
                        type: "string",
                        nullable: true
                    }
                }
            };
        })(Models = Hr.Models || (Hr.Models = {}));
    })(Hr = MyAndromeda.Hr || (MyAndromeda.Hr = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var Hr;
    (function (Hr) {
        var Services;
        (function (Services) {
            var app = angular.module("MyAndromeda.Hr.Services", []);
            var EmployeeServiceState = (function () {
                function EmployeeServiceState() {
                    var _this = this;
                    this.AndromedaSiteId = new Rx.BehaviorSubject(null);
                    this.ChainId = new Rx.BehaviorSubject(null);
                    this.EditEmployee = new Rx.BehaviorSubject(null);
                    this.EmployeeUpdated = new Rx.Subject();
                    this.ChainId.where(function (e) { return e !== null; }).subscribe(function (e) {
                        MyAndromeda.Logger.Notify("new chain id: " + e);
                        _this.CurrentChainId = e;
                    });
                    this.AndromedaSiteId.where(function (e) { return e !== null; }).subscribe(function (e) {
                        MyAndromeda.Logger.Notify("new Andromeda site id: " + e);
                        _this.CurrentAndromedaSiteId = e;
                    });
                }
                return EmployeeServiceState;
            })();
            Services.EmployeeServiceState = EmployeeServiceState;
            var EmployeeService = (function () {
                function EmployeeService($http, employeeServiceState, uuidService) {
                    var _this = this;
                    this.$http = $http;
                    this.employeeServiceState = employeeServiceState;
                    this.uuidService = uuidService;
                    this.Loading = new Rx.Subject();
                    this.IsLoading = false;
                    this.SavingSchedule = new Rx.Subject();
                    this.Saved = new Rx.Subject();
                    this.Error = new Rx.Subject();
                    this.ChainEmployeeDataSource = new kendo.data.DataSource({
                        schema: {
                            model: Hr.Models.employeeDataSourceSchema
                        }
                    });
                    this.StoreEmployeeDataSource = new kendo.data.DataSource({
                        batch: false,
                        schema: {
                            model: Hr.Models.employeeDataSourceSchema,
                        },
                        transport: {
                            read: function (options) {
                                var route = "hr/{0}/employees/{1}/list";
                                route = kendo.format(route, _this.chainId, _this.andromedaSiteId);
                                var promise = _this.$http.get(route);
                                _this.Loading.onNext(true);
                                Rx.Observable.fromPromise(promise).subscribe(function (callback) {
                                    options.success(callback.data);
                                    _this.Loading.onNext(false);
                                });
                            },
                            update: function (e) {
                                MyAndromeda.Logger.Notify("Update employee records");
                                var model = e.data;
                                _this.SavingSchedule.onNext(true);
                                _this.Update(model, function (data) {
                                    _this.SavingSchedule.onNext(false);
                                    e.success(data);
                                }, function (data) {
                                    _this.SavingSchedule.onNext(false);
                                    e.error(data);
                                });
                            },
                            create: function (e) {
                                MyAndromeda.Logger.Notify("Create employee record");
                                var data = e.data;
                                _this.SavingSchedule.onNext(true);
                                _this.Create(data, function (model) {
                                    _this.SavingSchedule.onNext(false);
                                    e.success(model);
                                }, function (data) {
                                    _this.SavingSchedule.onNext(false);
                                    e.error(data);
                                });
                            }
                        },
                        sort: { field: "ShortName", dir: "desc" }
                    });
                    this.employeeServiceState.AndromedaSiteId.where(function (e) { return e !== null; }).distinctUntilChanged(function (e) { return e; }).subscribe(function (id) {
                        MyAndromeda.Logger.Notify("new Andromeda site id : " + id);
                        _this.andromedaSiteId = id;
                        _this.StoreEmployeeDataSource.read();
                    });
                    this.employeeServiceState.ChainId.where(function (e) { return e !== null; }).distinctUntilChanged(function (e) { return e; }).subscribe(function (id) {
                        MyAndromeda.Logger.Notify("new chain id : " + id);
                        _this.chainId = id;
                        _this.ChainEmployeeDataSource.read();
                    });
                    this.Loading.subscribe(function (e) {
                        _this.IsLoading = e;
                    });
                }
                EmployeeService.prototype.List = function (chainId, andromedaSiteId) {
                    var route = "";
                    var pomise = this.$http.get(route);
                    return pomise;
                };
                EmployeeService.prototype.Save = function (employee) {
                    employee.set("DirtyHack", true);
                    var exists = this.StoreEmployeeDataSource.data().filter(function (item) {
                        return item.Id == employee.id;
                    });
                    if (exists.length == 0) {
                        MyAndromeda.Logger.Notify("Add employee");
                        this.StoreEmployeeDataSource.add(employee);
                    }
                    MyAndromeda.Logger.Notify("sync");
                    var sync = this.StoreEmployeeDataSource.sync();
                    return sync;
                };
                EmployeeService.prototype.Update = function (model, onSuccess, onError) {
                    var _this = this;
                    var route = "hr/{0}/employees/{1}/update";
                    route = kendo.format(route, this.chainId, this.andromedaSiteId);
                    var promise = this.$http.post(route, model);
                    this.Saved.onNext(false);
                    Rx.Observable.fromPromise(promise).subscribe(function (callback) {
                        var callBackData = callback.data;
                        MyAndromeda.Logger.Notify("result: ");
                        MyAndromeda.Logger.Notify(callBackData);
                        onSuccess(callBackData);
                        _this.Saved.onNext(true);
                    }, function (error) {
                        MyAndromeda.Logger.Error(error);
                        _this.Error.onNext("Updating Failed");
                    });
                };
                EmployeeService.prototype.Create = function (model, onSuccess, onError) {
                    var _this = this;
                    var route = "hr/{0}/employees/{1}/create";
                    route = kendo.format(route, this.chainId, this.andromedaSiteId);
                    var promise = this.$http.post(route, model);
                    this.Saved.onNext(false);
                    Rx.Observable.fromPromise(promise).subscribe(function (callback) {
                        var callBackData = callback.data;
                        MyAndromeda.Logger.Notify("result: ");
                        MyAndromeda.Logger.Notify(callBackData);
                        onSuccess(model);
                        _this.Saved.onNext(true);
                    }, function (error) {
                        MyAndromeda.Logger.Error(error);
                        _this.Error.onNext("Creating Failed");
                        onError(error);
                    });
                };
                EmployeeService.prototype.GetStore = function (chainId, andromedaSiteId) {
                    var route = "hr/{0}/employees/{1}/get-store";
                    route = kendo.format(route, chainId, andromedaSiteId);
                    var promise = this.$http.get(route);
                    var map = Rx.Observable.fromPromise(promise).map(function (s) { return s.data; });
                    return map;
                };
                EmployeeService.prototype.GetStoreListByEmployee = function (chainId, andromedaSiteId, employeeId) {
                    var route = "hr/{0}/employees/{1}/list-stores/{2}";
                    route = kendo.format(route, chainId, andromedaSiteId, employeeId);
                    var promise = this.$http.get(route);
                    var map = Rx.Observable.fromPromise(promise).map(function (s) { return s.data; });
                    return map;
                };
                EmployeeService.prototype.GetEmployeePictureUrl = function (chainId, andromedaSiteId, employeeId) {
                    //"hr/{{$stateParams.chainId}}/employees/{{$stateParams.andromedaSiteId}}/resources/{{employee.Id}}"
                    var path = "hr/{0}/employees/{1}/resources/{2}/profile-pic";
                    path = kendo.format(path, chainId, andromedaSiteId, employeeId);
                    //let r = this.uuidService.GenerateUUID();
                    //path = path + "?r=" + r;
                    return path;
                };
                EmployeeService.prototype.GetUploadRouteUrl = function (chainId, andromedaSiteId, employeeId) {
                    var path = "hr/{0}/employees/{1}/resources/{2}/update-profile-pic";
                    path = kendo.format(path, chainId, andromedaSiteId, employeeId);
                    return path;
                };
                EmployeeService.prototype.GetDocumentUploadRoute = function (chainId, andromedaSiteId, employeeId, documentId) {
                    var path = "hr/{0}/employees/{1}/resources/{2}/update-document/{3}";
                    path = kendo.format(path, chainId, andromedaSiteId, employeeId, documentId);
                    return path;
                };
                EmployeeService.prototype.GetDocumentRouteUrl = function (chainId, andromedaSiteId, employeeId, documentId, fileName) {
                    var path = "hr/{0}/employees/{1}/resources/{2}/document/{3}/{4}";
                    path = kendo.format(path, chainId, andromedaSiteId, employeeId, documentId, fileName);
                    return path;
                };
                EmployeeService.prototype.GetDocumentDownloadRouteUrl = function (chainId, andromedaSiteId, employeeId, documentId, fileName) {
                    var path = "hr/{0}/employees/{1}/resources/{2}/document/{3}/download/{4}";
                    path = kendo.format(path, chainId, andromedaSiteId, employeeId, documentId, fileName);
                    return path;
                };
                EmployeeService.prototype.GetDataSourceForStoreScheduler = function (chainId, andromedaSiteId) {
                    var _this = this;
                    var schema = {
                        data: "Data",
                        total: "Total",
                        model: Hr.Models.getSchedulerDataSourceSchema(andromedaSiteId, this, undefined)
                    };
                    var dataSource = new kendo.data.SchedulerDataSource({
                        batch: false,
                        transport: {
                            read: function (options) {
                                var route = _this.GetStoreEmployeeSchedulerReadRoute(chainId, andromedaSiteId);
                                var promise = _this.$http.post(route, options.data);
                                promise.then(function (callback) {
                                    options.success(callback.data);
                                });
                            },
                            update: function (options) {
                                MyAndromeda.Logger.Notify("Scheduler update");
                                var route = _this.GetEmployeeSchedulerUpdateRoute(chainId, andromedaSiteId);
                                var promise = _this.$http.post(route, options.data);
                                promise.then(function (callback) {
                                    options.success();
                                });
                            },
                            create: function (options) {
                                MyAndromeda.Logger.Notify("Scheduler create");
                                MyAndromeda.Logger.Notify(options.data);
                                var route = _this.GetEmployeeSchedulerUpdateRoute(chainId, andromedaSiteId);
                                var promise = _this.$http.post(route, options.data);
                                promise.then(function (callback) {
                                    MyAndromeda.Logger.Notify("Create response:");
                                    MyAndromeda.Logger.Notify(callback.data);
                                    options.success(callback.data);
                                });
                            },
                            destroy: function (options) {
                                MyAndromeda.Logger.Notify("GetEmployeeSchedulerDestroyRoute");
                                MyAndromeda.Logger.Notify(options.data);
                                var route = _this.GetEmployeeSchedulerDestroyRoute(chainId, andromedaSiteId);
                                var promise = _this.$http.post(route, options.data);
                                promise.then(function (callback) {
                                    MyAndromeda.Logger.Notify("destroy response:");
                                    MyAndromeda.Logger.Notify(callback.data);
                                    options.success(callback.data);
                                });
                            }
                        },
                        //sort: [
                        //    { field: "Department", dir: "asc" }
                        //],
                        schema: schema
                    });
                    dataSource.sort({
                        field: "EmployeeId",
                        dir: "desc"
                    });
                    return dataSource;
                };
                EmployeeService.prototype.GetDataSourceForEmployeeScheduler = function (chainId, andromedaSiteId, employeeId) {
                    var _this = this;
                    var schema = {
                        data: "Data",
                        total: "Total",
                        model: Hr.Models.getSchedulerDataSourceSchema(andromedaSiteId, this, employeeId)
                    };
                    var dataSource = new kendo.data.SchedulerDataSource({
                        batch: false,
                        transport: {
                            read: function (options) {
                                var route = _this.GetEmployeeSchedulerReadRoute(chainId, andromedaSiteId, employeeId);
                                var promise = _this.$http.post(route, options.data);
                                promise.then(function (callback) {
                                    options.success(callback.data);
                                });
                            },
                            update: function (options) {
                                MyAndromeda.Logger.Notify("Scheduler update");
                                var route = _this.GetEmployeeSchedulerUpdateRoute(chainId, andromedaSiteId);
                                var promise = _this.$http.post(route, options.data);
                                promise.then(function (callback) {
                                    options.success();
                                });
                            },
                            create: function (options) {
                                MyAndromeda.Logger.Notify("Scheduler create");
                                MyAndromeda.Logger.Notify(options.data);
                                var route = _this.GetEmployeeSchedulerUpdateRoute(chainId, andromedaSiteId);
                                var promise = _this.$http.post(route, options.data);
                                promise.then(function (callback) {
                                    MyAndromeda.Logger.Notify("Create response:");
                                    MyAndromeda.Logger.Notify(callback.data);
                                    options.success(callback.data);
                                });
                            },
                            destroy: function (options) {
                                MyAndromeda.Logger.Notify("GetEmployeeSchedulerDestroyRoute");
                                MyAndromeda.Logger.Notify(options.data);
                                var route = _this.GetEmployeeSchedulerDestroyRoute(chainId, andromedaSiteId);
                                var promise = _this.$http.post(route, options.data);
                                promise.then(function (callback) {
                                    MyAndromeda.Logger.Notify("destroy response:");
                                    MyAndromeda.Logger.Notify(callback.data);
                                    options.success(callback.data);
                                });
                            }
                        },
                        schema: schema
                    });
                    return dataSource;
                };
                EmployeeService.prototype.GetStoreEmployeeSchedulerReadRoute = function (chainId, andromedaSiteId) {
                    var path = "/hr/{0}/employees/{1}/schedule/store-list";
                    path = kendo.format(path, chainId, andromedaSiteId);
                    return path;
                };
                EmployeeService.prototype.GetEmployeeSchedulerReadRoute = function (chainId, andromedaSiteId, employeeId) {
                    var path = "/hr/{0}/employees/{1}/schedule/list/{2}";
                    path = kendo.format(path, chainId, andromedaSiteId, employeeId);
                    return path;
                };
                EmployeeService.prototype.GetEmployeeSchedulerUpdateRoute = function (chainId, andromedaSiteId) {
                    var path = "/hr/{0}/employees/{1}/schedule/update";
                    path = kendo.format(path, chainId, andromedaSiteId);
                    return path;
                };
                EmployeeService.prototype.GetEmployeeSchedulerDestroyRoute = function (chainId, andromedaSiteId) {
                    var path = "/hr/{0}/employees/{1}/schedule/destroy";
                    path = kendo.format(path, chainId, andromedaSiteId);
                    return path;
                };
                return EmployeeService;
            })();
            Services.EmployeeService = EmployeeService;
            app.service("employeeService", EmployeeService);
            app.service("employeeServiceState", EmployeeServiceState);
        })(Services = Hr.Services || (Hr.Services = {}));
    })(Hr = MyAndromeda.Hr || (MyAndromeda.Hr = {}));
})(MyAndromeda || (MyAndromeda = {}));
/// <reference path="hr.services.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var Hr;
    (function (Hr) {
        var Services;
        (function (Services) {
            var EmployeeAvailabilityTestService = (function () {
                function EmployeeAvailabilityTestService(scheduler) {
                    this.scheduler = scheduler;
                }
                EmployeeAvailabilityTestService.prototype.GetTasksInRange = function (start, end) {
                    var occurences = this.scheduler.occurrencesInRange(start, end);
                    return occurences;
                };
                EmployeeAvailabilityTestService.prototype.CheckTasksByEmployee = function (start, end, task) {
                    var context = {
                        start: start,
                        end: end,
                        task: task
                    };
                    var startCheck = start.toLocaleTimeString();
                    var endCheck = end.toLocaleTimeString();
                    //only interested in current employee, which is not the current task
                    var currentTasks = this.GetTasksInRange(start, end);
                    MyAndromeda.Logger.Notify("Tasks in range: " + currentTasks.length);
                    MyAndromeda.Logger.Notify(currentTasks);
                    currentTasks = currentTasks.filter(function (e) { return e.id !== task.id; });
                    MyAndromeda.Logger.Notify("Tasks in range after removing self: " + currentTasks.length);
                    currentTasks = currentTasks.filter(function (e) { return e.EmployeeId === task.EmployeeId; });
                    MyAndromeda.Logger.Notify("Tasks in range - by employee: " + currentTasks.length);
                    MyAndromeda.Logger.Notify("startCheck : " + startCheck + " | endCheck: " + endCheck);
                    MyAndromeda.Logger.Notify(context);
                    MyAndromeda.Logger.Notify("Tasks in range: " + currentTasks.length);
                    return currentTasks.length === 0;
                };
                EmployeeAvailabilityTestService.prototype.IsDurationValid = function (start, end) {
                    //hours
                    var duration = Math.abs(end.getTime() - start.getTime()) / 36e5;
                    if (duration < 0.1) {
                        return false;
                    }
                    return true;
                };
                EmployeeAvailabilityTestService.prototype.IsWorkAvailable = function (start, end, task) {
                    return this.CheckTasksByEmployee(start, end, task);
                };
                return EmployeeAvailabilityTestService;
            })();
            Services.EmployeeAvailabilityTestService = EmployeeAvailabilityTestService;
        })(Services = Hr.Services || (Hr.Services = {}));
    })(Hr = MyAndromeda.Hr || (MyAndromeda.Hr = {}));
})(MyAndromeda || (MyAndromeda = {}));
/// <reference path="hr.services.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var Hr;
    (function (Hr) {
        var Services;
        (function (Services) {
            var app = angular.module("MyAndromeda.Hr.Services.Scheduler", [
                "MyAndromeda.Hr.Services"
            ]);
            var EmployeeSchedulerService = (function () {
                function EmployeeSchedulerService(employeeServiceState, employeeService, SweetAlert) {
                    this.employeeServiceState = employeeServiceState;
                    this.employeeService = employeeService;
                    this.SweetAlert = SweetAlert;
                    this.saving = new Rx.Subject();
                }
                EmployeeSchedulerService.prototype.GetResources = function (stores, employee) {
                    var _this = this;
                    var employeePart = function () {
                        var part = {
                            field: "EmployeeId",
                            dataTextField: "ShortName",
                            dataValueField: "Id",
                            title: "Employee",
                            name: "Employee",
                            dataSource: []
                        };
                        if (employee) {
                            part.dataSource = [employee];
                            return part;
                        }
                        var employees = _this.employeeService.StoreEmployeeDataSource;
                        part.dataSource = employees;
                        return part;
                    };
                    var departmentAvailable = false;
                    if (employee) {
                        if (employee.Department) {
                            MyAndromeda.Logger.Notify("department: " + employee.Department);
                            departmentAvailable = true;
                        }
                    }
                    //= employee && employee.Department;
                    var resources = [
                        {
                            title: "Task",
                            field: "TaskType",
                            dataSource: Hr.Models.taskTypes
                        },
                        //{
                        //    name: "Department",
                        //    field: "Department",
                        //    dataValueField: "text",
                        //    dataTextField: "text",
                        //    dataSource: Models.departments
                        //    //dataSource: departmentAvailable
                        //    //    ? [{ text: employee.Department }]
                        //    //    : [{ text: "NA" }]
                        //},
                        employeePart(),
                        {
                            name: "Store",
                            title: "Store",
                            field: "AndromedaSiteId",
                            dataSource: stores,
                            dataValueField: "AndromedaSiteId",
                            dataTextField: "Name"
                        }
                    ];
                    return resources;
                };
                EmployeeSchedulerService.prototype.GetStoreEmployeeScheduler = function (stores) {
                    var _this = this;
                    var start = new Date();
                    var end = new Date();
                    start.setHours(0);
                    end.setHours(24);
                    var chainId = this.employeeServiceState.CurrentChainId, andromedaSiteId = this.employeeServiceState.CurrentAndromedaSiteId, currentStore = stores[0], dataSource = this.employeeService.GetDataSourceForStoreScheduler(chainId, andromedaSiteId);
                    var employeeGroupTemplate = "\n                <div>\n                    #=text#\n                </div>\n                <div>\n                    <span class=\"label\" style=\"background-color:#=majorColor#\">#=employee.Department #</span>\n                </div>\n                <div>\n                    <span class=\"label\" style=\"background-color:#=minorColor#\">#=employee.PrimaryRole #</span>\n                </div>\n            ";
                    var schedulerOptions = {
                        date: new Date(),
                        workDayStart: start,
                        workDayEnd: end,
                        majorTick: 60,
                        minorTickCount: 1,
                        workWeekStart: 0,
                        workWeekEnd: 6,
                        allDaySlot: true,
                        dataSource: dataSource,
                        timezone: "Europe/London",
                        currentTimeMarker: {
                            useLocalTimezone: false
                        },
                        editable: {
                            template: "<rota-task-editor task='dataItem'></rota-task-editor>"
                        },
                        pdf: {
                            fileName: currentStore.Name + " schedule",
                            title: "Schedule"
                        },
                        //groupHeaderTemplate: "<div>#=text#</div>",
                        groupHeaderTemplate: employeeGroupTemplate,
                        toolbar: ["pdf"],
                        showWorkHours: false,
                        resources: this.GetResources(stores),
                        views: [
                            {
                                type: "day", showWorkHours: false,
                                eventTemplate: "<employee-task task='dataItem'></employee-task>"
                            },
                            //{ type: "week", selected: true, showWorkHours: false },
                            //{ type: "month", showWorkHours: false },
                            //{ type: "timeline", showWorkHours: false },
                            //{
                            //    title: "Week thing",
                            //    eventHeight: 100,
                            //    slotTemplate: "<div style='background-color:\\#FFF'; height: 100%;width: 100%;'></div>",
                            //    selected: true,
                            //    majorTick: 1440,
                            //    minorTickCount: 1,
                            //    type: "kendo.ui.SchedulerTimelineWeekView", showWorkHours: false,
                            //    group: {
                            //        orientation: "vertical",
                            //        resources: ["Employee"]
                            //    }
                            //},
                            {
                                title: "Week Overview",
                                selected: true,
                                eventHeight: 20,
                                type: "kendo.ui.MonthTimeWeekView",
                                eventTemplate: "<working-task task='dataItem'></working-task>",
                                //dayTemplate: "",
                                dayTemplate: '#:kendo.toString(date, "dd")#',
                                group: {
                                    orientation: "vertical",
                                    resources: ["Employee"]
                                }
                            }
                        ],
                        resize: function (e) {
                            MyAndromeda.Logger.Notify("resize");
                            MyAndromeda.Logger.Notify(e);
                            var checker = new Services.EmployeeAvailabilityTestService(e.sender);
                            var valid = checker.IsWorkAvailable(e.start, e.end, e.event);
                            if (!valid) {
                                this.wrapper.find(".k-marquee-color").addClass("invalid-slot");
                                e.preventDefault();
                            }
                        },
                        resizeEnd: function (e) {
                            MyAndromeda.Logger.Notify("resize-end");
                            MyAndromeda.Logger.Notify(e);
                            var tester = new Services.EmployeeAvailabilityTestService(e.sender);
                            if (tester.IsWorkAvailable(e.start, e.end, e.event)) {
                                return;
                            }
                            MyAndromeda.Logger.Notify("cancel resize");
                            _this.SweetAlert.swal("Sorry", "The employee already has a job in this range", "error");
                            e.preventDefault();
                        },
                        move: function (e) {
                            MyAndromeda.Logger.Notify("move");
                            MyAndromeda.Logger.Notify(e);
                            var tester = new Services.EmployeeAvailabilityTestService(e.sender);
                            if (tester.IsWorkAvailable(e.start, e.end, e.event)) {
                                return;
                            }
                            this.wrapper.find(".k-event-drag-hint").addClass("invalid-slot");
                        },
                        moveEnd: function (e) {
                            MyAndromeda.Logger.Notify("move-end");
                            MyAndromeda.Logger.Notify(e);
                            var tester = new Services.EmployeeAvailabilityTestService(e.sender);
                            if (tester.IsWorkAvailable(e.start, e.end, e.event)) {
                                return;
                            }
                            MyAndromeda.Logger.Notify("cancel move");
                            _this.SweetAlert.swal("Sorry", "The employee already has a job in this range", "error");
                            e.preventDefault();
                        },
                        add: function (e) {
                            MyAndromeda.Logger.Notify("add");
                            MyAndromeda.Logger.Notify(e);
                            var tester = new Services.EmployeeAvailabilityTestService(e.sender);
                            if (!tester.IsWorkAvailable(e.event.start, e.event.end, e.event)) {
                                MyAndromeda.Logger.Notify("cancel add");
                                _this.SweetAlert.swal("Sorry", "The employee already has a job in this range", "error");
                                e.preventDefault();
                            }
                        },
                        save: function (e) {
                            MyAndromeda.Logger.Notify("save");
                            MyAndromeda.Logger.Notify(e);
                            var tester = new Services.EmployeeAvailabilityTestService(e.sender);
                            if (!tester.IsWorkAvailable(e.event.start, e.event.end, e.event)) {
                                MyAndromeda.Logger.Notify("cancel save");
                                _this.SweetAlert.swal("Sorry", "The employee already has a job in this range", "error");
                                e.preventDefault();
                            }
                        }
                    };
                    return schedulerOptions;
                };
                EmployeeSchedulerService.prototype.GetSingleEmployeeScheduler = function (stores, employee) {
                    var _this = this;
                    var start = new Date();
                    var end = new Date();
                    start.setHours(0);
                    end.setHours(24);
                    var chainId = this.employeeServiceState.CurrentChainId, andromedaSiteId = this.employeeServiceState.CurrentAndromedaSiteId, dataSource = this.employeeService.GetDataSourceForEmployeeScheduler(chainId, andromedaSiteId, employee.Id);
                    var schedulerOptions = {
                        date: new Date(),
                        workDayStart: start,
                        workDayEnd: end,
                        majorTick: 60,
                        minorTickCount: 1,
                        workWeekStart: 0,
                        workWeekEnd: 6,
                        allDaySlot: true,
                        dataSource: dataSource,
                        timezone: "Europe/London",
                        currentTimeMarker: {
                            useLocalTimezone: false
                        },
                        editable: true,
                        pdf: {
                            fileName: employee.ShortName + " schedule",
                            title: "Schedule"
                        },
                        eventTemplate: "<employee-task task='dataItem'></employee-task>",
                        toolbar: ["pdf"],
                        showWorkHours: false,
                        resources: this.GetResources(stores, employee),
                        views: [
                            { type: "day", showWorkHours: false },
                            { type: "week", selected: true, showWorkHours: false },
                            { type: "month", showWorkHours: false },
                            { type: "timeline", showWorkHours: false }
                        ],
                        resize: function (e) {
                            MyAndromeda.Logger.Notify("resize");
                            MyAndromeda.Logger.Notify(e);
                            var checker = new Services.EmployeeAvailabilityTestService(e.sender);
                            var valid = checker.IsWorkAvailable(e.start, e.end, e.event);
                            if (!valid) {
                                this.wrapper.find(".k-marquee-color").addClass("invalid-slot");
                                e.preventDefault();
                            }
                        },
                        resizeEnd: function (e) {
                            MyAndromeda.Logger.Notify("resize-end");
                            MyAndromeda.Logger.Notify(e);
                            var tester = new Services.EmployeeAvailabilityTestService(e.sender);
                            if (!tester.IsWorkAvailable(e.start, e.end, e.event)) {
                                MyAndromeda.Logger.Notify("cancel resize");
                                _this.SweetAlert.swal("Sorry", "The employee already has a job in this range", "error");
                                e.preventDefault();
                            }
                        },
                        move: function (e) {
                            MyAndromeda.Logger.Notify("move");
                            MyAndromeda.Logger.Notify(e);
                            var tester = new Services.EmployeeAvailabilityTestService(e.sender);
                            if (!tester.IsWorkAvailable(e.start, e.end, e.event)) {
                                this.wrapper.find(".k-event-drag-hint").addClass("invalid-slot");
                            }
                        },
                        moveEnd: function (e) {
                            MyAndromeda.Logger.Notify("move-end");
                            MyAndromeda.Logger.Notify(e);
                            var tester = new Services.EmployeeAvailabilityTestService(e.sender);
                            if (!tester.IsWorkAvailable(e.start, e.end, e.event)) {
                                MyAndromeda.Logger.Notify("cancel move");
                                _this.SweetAlert.swal("Sorry", "The employee already has a job in this range", "error");
                                e.preventDefault();
                            }
                        },
                        add: function (e) {
                            MyAndromeda.Logger.Notify("add");
                            MyAndromeda.Logger.Notify(e);
                            var tester = new Services.EmployeeAvailabilityTestService(e.sender);
                            if (!tester.IsWorkAvailable(e.event.start, e.event.end, e.event)) {
                                MyAndromeda.Logger.Notify("cancel add");
                                //SweetAlert.swal("Sorry!", name + " has been saved.", "success");
                                _this.SweetAlert.swal("Sorry", "The employee already has a job in this range", "error");
                                e.preventDefault();
                            }
                        },
                        save: function (e) {
                            MyAndromeda.Logger.Notify("save");
                            MyAndromeda.Logger.Notify(e);
                            var tester = new Services.EmployeeAvailabilityTestService(e.sender);
                            if (!tester.IsWorkAvailable(e.event.start, e.event.end, e.event)) {
                                MyAndromeda.Logger.Notify("cancel save");
                                _this.SweetAlert.swal("Sorry", "The employee already has a job in this range", "error");
                                e.preventDefault();
                            }
                        }
                    };
                    return schedulerOptions;
                };
                EmployeeSchedulerService.prototype.IsEmployeeFree = function () {
                    //var occurrences = occurrencesInRangeByResource(start, end, "attendee", event, resources);
                };
                EmployeeSchedulerService.prototype.OccurrencesInRangeByResource = function () {
                };
                return EmployeeSchedulerService;
            })();
            Services.EmployeeSchedulerService = EmployeeSchedulerService;
            app.service("employeeSchedulerService", EmployeeSchedulerService);
        })(Services = Hr.Services || (Hr.Services = {}));
    })(Hr = MyAndromeda.Hr || (MyAndromeda.Hr = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var Hr;
    (function (Hr) {
        var app = angular.module("MyAndromeda.Hr", [
            "MyAndromeda.Core",
            "MyAndromeda.Hr.Config",
            "MyAndromeda.Resize",
            "MyAndromeda.Progress",
            "ngAnimate",
            "ui.bootstrap"
        ]);
        app.run(function () {
            MyAndromeda.Logger.Notify("HR module is running");
        });
    })(Hr = MyAndromeda.Hr || (MyAndromeda.Hr = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var Hubs;
    (function (Hubs) {
        var MyAndromedaHubConnection = (function () {
            function MyAndromedaHubConnection(options) {
                this.options = options;
                this.connect();
            }
            MyAndromedaHubConnection.GetInstance = function (options) {
                if (MyAndromedaHubConnection._instance) {
                    return MyAndromedaHubConnection._instance;
                }
                return (MyAndromedaHubConnection._instance = new MyAndromedaHubConnection(options));
            };
            MyAndromedaHubConnection.prototype.connect = function () {
                MyAndromeda.Logger.Notify("Hub: Connect called");
                if (this.connected) {
                    MyAndromeda.Logger.Notify("already connected");
                    return;
                }
                if (this.connecting) {
                    MyAndromeda.Logger.Notify("already connecting");
                    return;
                }
                if (this.setup) {
                    MyAndromeda.Logger.Notify("already setup");
                    return;
                }
                //$.connection.hub.logging = true;
                var internal = this, hubConnection = $.connection.hub;
                //if (this.hubConnection)
                //{
                //    return this.hubConnection;
                //}
                this.hubConnection = hubConnection;
                //setup route parameters for MyAndromeda 
                if (!this.setupQueryString()) {
                    return hubConnection;
                }
                ;
                hubConnection.starting(function () {
                    internal.connecting = true;
                    MyAndromedaHubConnection.log("hub connection starting");
                });
                var transportType = "longPolling";
                //if(document.URL.indexOf("localhost") >= 0){
                //    transportType = "webSockets";
                //}
                hubConnection.start({
                    transport: transportType //['webSockets', 'longPolling'] 
                }).done(function () {
                    internal.connecting = false;
                    internal.connected = true;
                    MyAndromedaHubConnection.log("hub connection started!");
                });
                this.setup = true;
                return hubConnection;
            };
            MyAndromedaHubConnection.prototype.setupQueryString = function () {
                var connection = this.hubConnection;
                if (!this.options.chainId) {
                    return false;
                }
                connection.qs = {
                    'externalSiteId': this.options.externalSiteId,
                    'chainId': this.options.chainId
                };
                return true;
            };
            MyAndromedaHubConnection.log = function (data) {
                if (console && console.log) {
                    try {
                        console.log(data);
                    }
                    catch (e) { }
                }
            };
            return MyAndromedaHubConnection;
        })();
        Hubs.MyAndromedaHubConnection = MyAndromedaHubConnection;
    })(Hubs = MyAndromeda.Hubs || (MyAndromeda.Hubs = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var Hubs;
    (function (Hubs) {
        var Services;
        (function (Services) {
            var MenuHubService = (function () {
                function MenuHubService(options) {
                    var internal = this;
                    this.options = options;
                    this.hub = Hubs.StoreHub.GetInstance(options); //new StoreHub(options);
                    this.viewModel = kendo.observable({
                        siteName: "",
                        menuVersion: "",
                        lastUpdated: "",
                        updates: []
                    });
                }
                MenuHubService.prototype.init = function () {
                    kendo.bind(this.options.id, this.viewModel);
                };
                return MenuHubService;
            })();
            Services.MenuHubService = MenuHubService;
        })(Services = Hubs.Services || (Hubs.Services = {}));
    })(Hubs = MyAndromeda.Hubs || (MyAndromeda.Hubs = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var Hubs;
    (function (Hubs) {
        var StoreHub = (function () {
            function StoreHub(options) {
                this.menuItemChangeEvents = [];
                this.options = options;
                this.connect();
                if (StoreHub._storeHubInstance) {
                    throw Error("The class has already been initialized. Use StoreHub.GetInstance");
                }
                var hubs = this.myAndromedaHubConnection.hubConnection.proxies, menuHub = hubs.storehub;
                this.eventMap = {};
                this.menuHub = menuHub;
                this.setupEvents();
            }
            StoreHub.GetInstance = function (options) {
                MyAndromeda.Logger.Notify("Get Store Hub");
                if (StoreHub._storeHubInstance) {
                    return StoreHub._storeHubInstance;
                }
                StoreHub._storeHubInstance = new StoreHub(options);
                return StoreHub._storeHubInstance;
            };
            StoreHub.prototype.connect = function () {
                this.myAndromedaHubConnection = Hubs.MyAndromedaHubConnection.GetInstance(this.options);
                this.myAndromedaHubConnection.connect();
            };
            StoreHub.prototype.setupEvents = function () {
                var internal = this;
                this.menuHub.client.user = function (user) {
                    internal.user = user;
                    StoreHub.log("User:" + internal.user.Username);
                };
                this.menuHub.client.transactionLog = function (message) {
                    if ($("#MenuFtpTransactionLog").length > 0) {
                        $("#MenuFtpTransactionLog").append("<div>" + message + "</div>");
                    }
                };
                this.menuHub.client.getStoreMenuVersion = function (data) {
                    StoreHub.log("GetStoreMenuVersion");
                    StoreHub.log(data);
                };
                this.menuHub.client.ping = function (data) {
                    StoreHub.log(data);
                };
                /* valid changes have been sent from the server */
                this.menuHub.client.updateLocalItems = function (data) {
                    StoreHub.log("local items need changing?");
                    StoreHub.log(data);
                    internal.menuItemChangeEvents.forEach(function (value, index) {
                        value(data);
                    });
                };
                this.menuHub.client.logFtp = function (data) {
                    StoreHub.log("logftp");
                    StoreHub.log(data);
                };
                this.menuHub.client.storeInfo = function (data) {
                    StoreHub.log(data);
                };
                this.menuHub.client.menuInfo = function (data) {
                    StoreHub.log(data);
                };
                this.menuHub.client.onDebug = this.createEventMapping("onDebug");
                this.menuHub.client.onNotifierDebug = this.createEventMapping("onNotifierDebug");
                this.menuHub.client.onError = this.createEventMapping("onError");
                this.menuHub.client.onNotifierError = this.createEventMapping("onNotifierError");
                this.menuHub.client.onNotifyMail = this.createEventMapping("onNotifyMail");
                this.menuHub.client.onNotify = this.createEventMapping("onNotify");
                this.menuHub.client.onNotifierNotify = this.createEventMapping("onNotifierNotify");
                this.menuHub.client.onNotifierSuccess = this.createEventMapping("onNotifierSuccess");
                this.menuHub.client.checkingDatabaseEvent = function (data) {
                    StoreHub.log("1. checkingDatabaseEvent");
                    StoreHub.log(data);
                };
                this.menuHub.client.downloadingDatabaseEvent = function (data) {
                    StoreHub.log("2. downloadingDatabaseEvent");
                    StoreHub.log(data);
                };
                this.menuHub.client.downloadedDatabaseEvent = function (data) {
                    StoreHub.log("3. downloadingDatabaseEvent");
                    StoreHub.log(data);
                };
                this.menuHub.client.extractedDatabaseEvent = function (data) {
                    StoreHub.log("4. extractedDatabaseEvent");
                    StoreHub.log(data);
                };
                //opened a connection to both to compare altered data and versions
                this.menuHub.client.comparingDatabaseEvent = function (data) {
                    StoreHub.log("5. comparingDatabaseEvent");
                    StoreHub.log(data);
                };
                //database altered or version number is the same or lower 
                this.menuHub.client.notChangedDatabaseEvent = function (data) {
                    StoreHub.log("5.1  notChangedDatabaseEvent");
                    StoreHub.log(data);
                };
                //database is newer - taking the ftp one. 
                this.menuHub.client.copiedDatabaseEvent = function (data) {
                    StoreHub.log("5.2. copiedDatabaseEvent");
                };
            };
            StoreHub.prototype.createEventMapping = function (key) {
                this.eventMap[key] = new Array();
                var internal = this, action = function (data) {
                    var dispatch = internal.eventMap[key];
                    dispatch.forEach(function (listener) {
                        listener(data);
                    });
                };
                return action;
            };
            StoreHub.log = function (data) {
                if (console && console.log) {
                    try {
                        console.log(data);
                    }
                    catch (e) { }
                }
            };
            StoreHub.prototype.bind = function (types, listener) {
                var _this = this;
                if (types === StoreHub.MenuItemChangeEvent) {
                    this.menuItemChangeEvents.push(listener);
                    return;
                }
                types.split(" ").forEach(function (type) {
                    var collection = _this.eventMap[type];
                    if (collection) {
                        collection.push(listener);
                    }
                    else {
                        StoreHub.log("There is no type: " + type + " to bind to");
                    }
                });
            };
            StoreHub.prototype.getStoreMenuVersion = function (handler) {
                this.menuHub.server.getStoreMenuVersion().done(function (data) {
                    handler(data);
                });
            };
            StoreHub.MenuItemChangeEvent = "MenuItemChangeEvent";
            return StoreHub;
        })();
        Hubs.StoreHub = StoreHub;
    })(Hubs = MyAndromeda.Hubs || (MyAndromeda.Hubs = {}));
})(MyAndromeda || (MyAndromeda = {}));
/// <reference path="../../scripts/typings/signalr/signalr.d.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var Hubs;
    (function (Hubs) {
        var SynchronizationHub = (function () {
            function SynchronizationHub(options) {
                this.options = options;
                this.connect();
                var hubs = $.connection, hub = hubs.hub, cloudHub = hubs.cloudSynchronization;
                cloudHub.client.ping = function (date) {
                    SynchronizationHub.log("ping fired");
                    SynchronizationHub.log(date);
                };
                cloudHub.client.startedSynchronization = function (data) {
                    SynchronizationHub.log("started fired");
                };
                cloudHub.client.completedSynchronization = function (data) {
                    SynchronizationHub.log("completed fired");
                };
                cloudHub.client.errorSynchronization = function (data) {
                    SynchronizationHub.log("error fired");
                };
                cloudHub.client.skippedSynchronization = function (data) {
                    SynchronizationHub.log("skip fired");
                };
                if (!cloudHub) {
                    cloudHub = this.produceConnection();
                }
                this.client = cloudHub.client;
                //hub.start()
                //    .done(function () {
                //        SynchronizationHub.log("connected");
                //    })
                //    .fail(function () {
                //        SynchronizationHub.log("connection failed");
                //    });
            }
            SynchronizationHub.log = function (data) {
                if (console && console.log) {
                    try {
                        console.log(data);
                    }
                    catch (e) { }
                }
            };
            SynchronizationHub.prototype.connect = function () {
                this.myAndromedaHubConnection = Hubs.MyAndromedaHubConnection.GetInstance(this.options);
                this.myAndromedaHubConnection.connect();
            };
            SynchronizationHub.prototype.produceConnection = function () {
                var connection = $.hubConnection();
                var proxy = connection.createHubProxy("CloudSynchronization");
                //proxy.on("startedSynchronization", (msg: any) => {
                //    this.client.startedSynchronization(msg);
                //});
                //proxy.on("completedSynchronization", (msg: any) => {
                //});
                //proxy.on("completedSynchronization", function (d) {
                //    this.client.completedSynchronization(d);
                //});
                //proxy.on("errorSynchronization", function (d) {
                //    this.client.errorSynchronization(d);
                //});
                connection.start()
                    .done(function () { SynchronizationHub.log("Connected"); })
                    .fail(function () { SynchronizationHub.log("Could not connect"); });
                return proxy;
            };
            SynchronizationHub.STARTED = "started";
            SynchronizationHub.COMPLETED = "completed";
            SynchronizationHub.ERROR = "error";
            return SynchronizationHub;
        })();
        Hubs.SynchronizationHub = SynchronizationHub;
        var SynchronizationHubService = (function () {
            function SynchronizationHubService(options) {
                this.options = options;
                this.hub = new SynchronizationHub(options);
                this.viewModel = kendo.observable({
                    started: [],
                    completed: [],
                    errors: []
                });
            }
            SynchronizationHubService.prototype.initEvents = function () {
                var internal = this, hub = this.hub.myAndromedaHubConnection.hubConnection.proxies.cloudsynchronization, client = hub.client;
                client.startedSynchronization = function (data) {
                    SynchronizationHub.log("started fired");
                    var models = internal.viewModel.get("started");
                    models.push(data);
                };
                client.completedSynchronization = function (data) {
                    SynchronizationHub.log("completed fired");
                    var models = internal.viewModel.get("completed");
                    models.push(data);
                };
                client.errorSynchronization = function (data) {
                    SynchronizationHub.log("error fired");
                    var models = internal.viewModel.get("errors");
                    models.push(data);
                };
                client.tasks = function (data) {
                    var models = internal.viewModel.get("started");
                    models.push({
                        Note: "Checking how many tasks",
                        TimeStamp: new Date(),
                        Count: data
                    });
                };
            };
            SynchronizationHubService.prototype.init = function () {
                kendo.bind(this.options.id, this.viewModel);
                this.initEvents();
            };
            return SynchronizationHubService;
        })();
        Hubs.SynchronizationHubService = SynchronizationHubService;
    })(Hubs = MyAndromeda.Hubs || (MyAndromeda.Hubs = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var Loyalty;
    (function (Loyalty) {
        Loyalty.ServicesName = "LoyaltyServices";
        var servicesModule = angular.module(Loyalty.ServicesName, []);
        var Services;
        (function (Services) {
            var LoyaltyService = (function () {
                function LoyaltyService($http) {
                    this.$http = $http;
                    /* data messaging */
                    // all types that have not been used yet. 
                    this.AllLoyaltyTypeList = new Rx.Subject();
                    // store loyalty types that have been used 
                    this.StoreLoyalties = new Rx.Subject();
                    this.LoyaltyProvider = new Rx.Subject();
                    /* monitor network */
                    this.ListLoyaltyTypesBusy = new Rx.BehaviorSubject(false);
                    this.ListBusy = new Rx.BehaviorSubject(false);
                    this.GetBusy = new Rx.BehaviorSubject(false);
                    this.UpdateBusy = new Rx.BehaviorSubject(false);
                }
                LoyaltyService.prototype.ListLoyaltyTypes = function () {
                    var _this = this;
                    var partialRoute = "/api/{0}/loyalty/types";
                    var route = kendo.format(partialRoute, Loyalty.Settings.AndromedaSiteId);
                    var promise = this.$http.get(route);
                    this.ListLoyaltyTypesBusy.onNext(true);
                    promise.success(function (data) {
                        _this.AllLoyaltyTypeList.onNext(data);
                    });
                    promise.finally(function () {
                        _this.ListLoyaltyTypesBusy.onNext(false);
                    });
                };
                LoyaltyService.prototype.List = function () {
                    var _this = this;
                    var partialRoute = "/api/{0}/loyalty/list";
                    var route = kendo.format(partialRoute, Loyalty.Settings.AndromedaSiteId);
                    var promise = this.$http.get(route);
                    this.ListBusy.onNext(true);
                    promise.success(function (data) {
                        _this.StoreLoyalties.onNext(data);
                    });
                    promise.finally(function () {
                        _this.ListBusy.onNext(false);
                    });
                };
                LoyaltyService.prototype.Get = function (name) {
                    var _this = this;
                    var partialRoute = "/api/{0}/loyalty/get/{1}";
                    var route = kendo.format(partialRoute, Loyalty.Settings.AndromedaSiteId, name);
                    var promise = this.$http.get(route);
                    this.GetBusy.onNext(true);
                    promise.success(function (data) {
                        _this.LoyaltyProvider.onNext(data);
                    });
                    promise.finally(function () { _this.GetBusy.onNext(false); });
                };
                LoyaltyService.prototype.Update = function (model) {
                    var _this = this;
                    var partialRoute = "/api/{0}/loyalty/update/{1}";
                    var route = kendo.format(partialRoute, Loyalty.Settings.AndromedaSiteId, model.ProviderName);
                    var promise = this.$http.post(route, model);
                    this.UpdateBusy.onNext(true);
                    promise.success(function (data) { _this.UpdateBusy.onNext(false); });
                    promise.finally(function () { _this.UpdateBusy.onNext(false); });
                };
                return LoyaltyService;
            })();
            Services.LoyaltyService = LoyaltyService;
            var loyaltyService = "loyaltyService";
            servicesModule.service(loyaltyService, LoyaltyService);
        })(Services = Loyalty.Services || (Loyalty.Services = {}));
    })(Loyalty = MyAndromeda.Loyalty || (MyAndromeda.Loyalty = {}));
})(MyAndromeda || (MyAndromeda = {}));
/// <reference path="Loyalty.Services.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var Loyalty;
    (function (Loyalty) {
        Loyalty.ControllersName = "LoyaltyControllers";
        var controllersModule = angular.module(Loyalty.ControllersName, [
            Loyalty.ServicesName
        ]);
        Loyalty.StartController = "StartController";
        controllersModule.controller(Loyalty.StartController, function ($scope, $timeout, loyaltyService) {
            loyaltyService.ListLoyaltyTypes();
            loyaltyService.List();
            $scope.ShowAddList = false;
            $scope.ShowEditList = false;
            var allTypesSubscription = loyaltyService.AllLoyaltyTypeList.subscribe(function (list) {
                $timeout(function () {
                    $scope.AvailableLoyaltyTypeNames = list;
                    $scope.ShowAddList = list.length > 0;
                });
            });
            var currentStoreLoyalties = loyaltyService.StoreLoyalties.subscribe(function (list) {
                $timeout(function () {
                    $scope.CurrentLoyaltyTypes = list;
                    $scope.ShowEditList = list.length > 0;
                });
            });
            $scope.$on("$destroy", function () {
                allTypesSubscription.dispose();
                currentStoreLoyalties.dispose();
            });
        });
        Loyalty.EditLoyaltyController = "EditLoyaltyController";
        controllersModule.controller(Loyalty.EditLoyaltyController, function ($scope, $timeout, $routeParams, loyaltyService) {
            $scope.SaveBusy = false;
            $scope.Currency = kendo.toString(1.00, "c");
            $scope.Currency10 = kendo.toString(10.00, "c");
            //lets explicitly look for this one:
            $scope.ProviderAvailable = false;
            loyaltyService.Get($routeParams.providerName);
            var modelAvailableSubscription = loyaltyService.LoyaltyProvider.subscribe(function (provider) {
                $timeout(function () {
                    $scope.Name = provider.ProviderName;
                    $scope.ProviderLoyalty = provider;
                    $scope.Model = provider.Configuration;
                    $scope.ProviderAvailable = true;
                    if ($scope.Model.MinimumPointsBeforeAvailable > $scope.Model.MaximumObtainablePoints &&
                        $scope.Model.MinimumPointsBeforeAvailable && $scope.Model.MaximumObtainablePoints) {
                        alert("Minimum points needed before spending is available cannot be lower than the maximum points that a user can have. Correcting.");
                        $scope.Model.MinimumPointsBeforeAvailable = $scope.Model.MaximumObtainablePoints;
                    }
                });
            });
            var saveBusySubscription = loyaltyService.UpdateBusy.subscribe(function (value) {
                $timeout(function () {
                    $scope.SaveBusy = value;
                });
            });
            $scope.SetDefaults = function () {
                $scope.Model || ($scope.Model = {
                    RoundUp: true
                });
                var m = $scope.Model;
                if (!confirm("Setting defaults will wipe any existing change")) {
                    return;
                }
                m.Enabled = true;
                m.AutoSpendPointsOverThisPeak = null;
                m.AwardOnRegiration = 500;
                m.AwardPointsPerPoundSpent = 10;
                m.MaximumPercentThatCanBeClaimed = 1; /* 100% */
                m.MaximumValueThatCanBeClaimed = 10.00; /* 10.00 */
                m.MinimumPointsBeforeAvailable = null;
                m.PointValue = 100;
                //more defaults 
                m.MaximumObtainablePoints = null;
                m.MinimumOrderTotalAfterLoyalty = null;
            };
            $scope.IsAutoSpendPointsOverThisPeakEnabled = function () {
                if (typeof ($scope.Model) === 'undefined' || $scope.Model === null) {
                    return false;
                }
                var m = $scope.Model;
                if (m.AutoSpendPointsOverThisPeak === null) {
                    return false;
                }
                var t = (m.AutoSpendPointsOverThisPeak >= 0);
                return t;
            };
            $scope.IsMinimumPointsBeforeAvailableEnabled = function () {
                if (typeof ($scope.Model) === 'undefined' || $scope.Model === null) {
                    return false;
                }
                var m = $scope.Model;
                if (m.MinimumPointsBeforeAvailable === null) {
                    return false;
                }
                var t = m.MinimumPointsBeforeAvailable >= 0;
                return t;
            };
            $scope.IsMaximumObtainablePointsEnabled = function () {
                if (typeof ($scope.Model) === 'undefined' || $scope.Model === null) {
                    return false;
                }
                var m = $scope.Model;
                if (m.MaximumObtainablePoints === null) {
                    return false;
                }
                var t = m.MaximumObtainablePoints >= 0;
                return t;
            };
            $scope.Save = function () {
                var validator = $scope.AndromedaLoyaltyValidator;
                if (validator.validate()) {
                    loyaltyService.Update($scope.ProviderLoyalty);
                }
            };
            $scope.IsMaximumAndMinimumRulesInvalid = function () {
                var maxValue = $scope.MaximumObtainablePointsNumericTextBox.value();
                var minValue = $scope.MinimumPointsBeforeAvailableNumericTextBox.value();
                if (!maxValue || !minValue) {
                    return false;
                }
                if (minValue <= maxValue) {
                    return false;
                }
                return true;
            };
            $scope.IsMaximumAndMinimumRulesEqual = function () {
                var maxValue = $scope.MaximumObtainablePointsNumericTextBox.value();
                var minValue = $scope.MinimumPointsBeforeAvailableNumericTextBox.value();
                if (!maxValue || !minValue) {
                    return false;
                }
                if (maxValue == minValue) {
                    return true;
                }
                return false;
            };
            $scope.$watch("Model.MaximumObtainablePoints", function () {
                MyAndromeda.Logger.Notify("Model.MaximumObtainablePoints changed");
                if (!$scope.MaximumObtainablePointsNumericTextBox) {
                    MyAndromeda.Logger.Notify("cant find numeric text box");
                    return;
                }
                if ($scope.MinimumPointsBeforeAvailableNumericTextBox) {
                    var value = $scope.MaximumObtainablePointsNumericTextBox.value();
                    if ($scope.MinimumPointsBeforeAvailableNumericTextBox.value() >
                        $scope.MaximumObtainablePointsNumericTextBox.value()) {
                        $scope.MaximumObtainablePointsNumericTextBox.value($scope.MinimumPointsBeforeAvailableNumericTextBox.value());
                    }
                    MyAndromeda.Logger.Notify("set max");
                    if (value) {
                        $scope.MinimumPointsBeforeAvailableNumericTextBox.max(value);
                    }
                    else {
                        $scope.MinimumPointsBeforeAvailableNumericTextBox.max(null);
                    }
                }
            });
            //$scope.$watch("Model.MinimumPointsBeforeAvailable", () => {
            //    Logger.Notify("Model.MinimumPointsBeforeAvailable changed");
            //    if (!$scope.MinimumPointsBeforeAvailableNumericTextBox) {
            //        Logger.Notify("cant find numeric text box");
            //        return;
            //    }
            //    if ($scope.Model.MaximumObtainablePoints) {
            //        Logger.Notify("set max");
            //        var minimumPointsBeforeAvailableNumericTextBox = $scope.MinimumPointsBeforeAvailableNumericTextBox;
            //        var maximumObtainablePointsNumericTextBox = $scope.MaximumObtainablePointsNumericTextBox;
            //        if (minimumPointsBeforeAvailableNumericTextBox.value()) {
            //            maximumObtainablePointsNumericTextBox.max(minimumPointsBeforeAvailableNumericTextBox.value());
            //        } else {
            //            maximumObtainablePointsNumericTextBox.max(null);
            //        }
            //    }
            //});
            $scope.$watch("Model.PointValue", function () {
                if (!$scope.AwardingPointsNumericTextBox) {
                    return;
                }
                if ($scope.Model.PointValue) {
                    console.log("update points");
                    var numericPicker = $scope.AwardingPointsNumericTextBox;
                    numericPicker.max($scope.Model.PointValue);
                }
            });
            $scope.ValueOf = function (points) {
                points || (points = 0);
                if (!$scope.Model) {
                    return "";
                }
                //£1 = pointValue
                var pointValue = $scope.Model.PointValue;
                if (!pointValue) {
                    return "";
                }
                var worth = (1.00 / pointValue) * points;
                return kendo.toString(worth, "c"); //$1,234.57
            };
            $scope.DisplayAsCurrency = function (monies) {
                monies || (monies = 0);
                return kendo.toString(monies, "c");
            };
            $scope.$on("$destroy", function () {
                modelAvailableSubscription.dispose();
                saveBusySubscription.dispose();
            });
        });
    })(Loyalty = MyAndromeda.Loyalty || (MyAndromeda.Loyalty = {}));
})(MyAndromeda || (MyAndromeda = {}));
/// <reference path="Loyalty.Services.ts" />
/// <reference path="Loyalty.Controllers.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var Loyalty;
    (function (Loyalty) {
        Loyalty.LoyaltyName = "MyAndromedaLoyalty";
        Loyalty.Settings = {
            AndromedaSiteId: ""
        };
        var loyaltyModule = angular.module(Loyalty.LoyaltyName, [
            'ngRoute',
            'ngAnimate',
            "kendo.directives",
            Loyalty.ServicesName,
            Loyalty.ControllersName
        ]);
        loyaltyModule.config(function ($routeProvider) {
            $routeProvider.when('/', {
                templateUrl: "start.html",
                controller: Loyalty.StartController
            });
            $routeProvider.when("/edit/:providerName", {
                templateUrl: "edit.html",
                controller: Loyalty.EditLoyaltyController
            });
        });
        loyaltyModule.run(function ($templateCache) {
            console.log("Loyalty Started");
            angular.element('script[type="text/template"]').each(function (i, element) {
                $templateCache.put(element.id, element.innerHTML);
            });
        });
        Loyalty.Start = function (element) {
            angular.bootstrap(element, [Loyalty.LoyaltyName]);
        };
    })(Loyalty = MyAndromeda.Loyalty || (MyAndromeda.Loyalty = {}));
})(MyAndromeda || (MyAndromeda = {}));
/// <reference path="../general/resizemodule.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var Marketing;
    (function (Marketing) {
        MyAndromeda.Logger.Notify("MyAndromeda.MarketingThing");
        Marketing.moduleName = "MyAndromeda.MarketingThing";
        Marketing.m = angular.module(Marketing.moduleName, [
            "MyAndromeda.Resize",
            "MyAndromeda.Progress",
            "ngAnimate",
            "kendo.directives",
            "ui.bootstrap",
            "oitozero.ngSweetAlert"
        ]);
        Marketing.m.run(function ($templateCache) {
            MyAndromeda.Logger.Notify("WebHooks Started");
            angular
                .element('script[type="text/template"]')
                .each(function (i, element) {
                $templateCache.put(element.id, element.innerHTML);
            });
        });
        Marketing.Routes = {
            ContactRoute: "/marketing/{0}/marketing/contact",
            RegisteredAndInactiveRoute: "/marketing/{0}/marketing/noorders",
            InactiveForSevenDaysRoute: "/marketing/{0}/marketing/oneweek",
            InactiveForOneMonthRoute: "/marketing/{0}/marketing/onemonth",
            InactiveForThreeMonthsRoute: "/marketing/{0}/marketing/threemonth",
            TestType: "/marketing/{0}/marketing/test",
            Save: "/marketing/{0}/marketing/saveevent",
            Preview: "/marketing/{0}/marketing/preview",
            SendNow: "/marketing/{0}/marketing/sendnow",
            PreviewRecipients: "/marketing/{0}/marketing/previewRecipients"
        };
        function SetupMaketingEvents(id) {
            var element = document.getElementById(id);
            angular.bootstrap(element, [Marketing.moduleName]);
        }
        Marketing.SetupMaketingEvents = SetupMaketingEvents;
    })(Marketing = MyAndromeda.Marketing || (MyAndromeda.Marketing = {}));
})(MyAndromeda || (MyAndromeda = {}));
/// <reference path="myandromeda.marketing.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var Marketing;
    (function (Marketing) {
        Marketing.m.controller("StartController", function ($scope, $timeout, resizeService, progressService) {
            MyAndromeda.Logger.Notify("start");
            var resizeSubscription = resizeService.ResizeObservable.subscribe(function (e) {
                var appTabStrip = $scope.appTabStrip;
                appTabStrip.resize(true);
            });
            $scope.$on('$destroy', function iVeBeenDismissed() {
                resizeSubscription.dispose();
            });
            //var element = document.getElementById("EventDrivenMarketing");
            //progressService.Create(element).Show();
        });
    })(Marketing = MyAndromeda.Marketing || (MyAndromeda.Marketing = {}));
})(MyAndromeda || (MyAndromeda = {}));
/// <reference path="myandromeda.marketing.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var Marketing;
    (function (Marketing) {
        Marketing.m.controller('UibTabsetAdvancedController', function ($scope, SweetAlert) {
            var ctrl = this, tabs = ctrl.tabs = $scope.tabs = [];
            ctrl.select = function (selectedTab) {
                angular.forEach(tabs, function (tab) {
                    if (tab.active && tab !== selectedTab) {
                        tab.active = false;
                        tab.onDeselect();
                        selectedTab.selectCalled = false;
                    }
                });
                selectedTab.active = true;
                // only call select if it has not already been called
                if (!selectedTab.selectCalled) {
                    selectedTab.onSelect();
                    selectedTab.selectCalled = true;
                }
            };
            ctrl.addTab = function addTab(tab) {
                tabs.push(tab);
                // we can't run the select function on the first tab
                // since that would select it twice
                if (tabs.length === 1 && tab.active !== false) {
                    tab.active = true;
                }
                else if (tab.active) {
                    ctrl.select(tab);
                }
                else {
                    tab.active = false;
                }
            };
            ctrl.removeTab = function removeTab(tab) {
                var index = tabs.indexOf(tab);
                //Select a new tab if the tab to be removed is selected and not destroyed
                if (tab.active && tabs.length > 1 && !destroyed) {
                    //If this is the last tab, select the previous tab. else, the next tab.
                    var newActiveIndex = index == tabs.length - 1 ? index - 1 : index + 1;
                    ctrl.select(tabs[newActiveIndex]);
                }
                tabs.splice(index, 1);
            };
            var destroyed;
            $scope.$on('$destroy', function () {
                destroyed = true;
            });
        });
        Marketing.m.directive("uibTabsetExtension", function () {
            return {
                restrict: 'EA',
                transclude: true,
                replace: true,
                scope: {
                    type: '@'
                },
                controller: 'UibTabsetAdvancedController',
                templateUrl: 'template/tabs/tabset.html',
                link: function (scope, element, attrs) {
                    scope.vertical = angular.isDefined(attrs.vertical) ? scope.$parent.$eval(attrs.vertical) : false;
                    scope.justified = angular.isDefined(attrs.justified) ? scope.$parent.$eval(attrs.justified) : false;
                }
            };
        });
        Marketing.m.directive("recipientTemplate", function () {
            return {
                name: "recipientTemplate",
                restrict: "E",
                transclude: true,
                link: function ($scope, element, attrs, controller, transclude) {
                    transclude($scope, function (clone, scope) {
                        element.append(clone);
                    });
                },
                controller: function ($scope, recipientService) {
                    var andromedaSiteId = $scope.andromedaSiteId;
                    var dataSource = new kendo.data.DataSource({
                        transport: {
                            read: function (options) {
                                var model = $scope.model;
                                var promise = recipientService.LoadRecipients(andromedaSiteId, model);
                                promise.then(function (success) {
                                    var data = success.data;
                                    options.success(data);
                                });
                            }
                        },
                        serverSorting: false,
                        serverGrouping: false,
                        serverFiltering: false,
                        serverAggregates: false,
                        serverPaging: false
                    });
                    var gridOptions;
                    gridOptions = {
                        //selectable: "multiple cell",
                        //allowCopy: true,
                        columns: [
                            { title: "email", field: "email" },
                            { title: "name", field: "name" }
                        ],
                        dataSource: dataSource,
                        filterable: true,
                        sortable: true,
                        height: '100%',
                        autoBind: false,
                        pageable: {
                            refresh: true,
                            pageSizes: true,
                            buttonCount: 5,
                            pageSize: 100
                        }
                    };
                    $scope.mainGridOptions = gridOptions;
                },
                templateUrl: "recipientListTemplate.html"
            };
        });
        Marketing.m.directive("updateBodyContent", function () {
            return {
                name: "updateBodyContent",
                restrict: "A",
                link: function ($scope, element, attrs) {
                    var $body = $(element).contents().find('body');
                    $(element).load(function () {
                        $body = $(element).contents().find('body');
                    });
                    attrs.$observe("dynamicContent", function (val) {
                        $body.html(val);
                    });
                }
            };
        });
        Marketing.m.directive("contactTemplate", function () {
            return {
                name: "contactTemplate",
                restrict: "E",
                "require": [
                    "^andromedaSiteId"
                ],
                scope: {
                    andromedaSiteId: "@"
                },
                controller: function ($scope, $element, SweetAlert, marketingEventService, progressService) {
                    var andromedaSiteId = $scope.andromedaSiteId;
                    var request = marketingEventService.LoadContactDetails(andromedaSiteId);
                    var save = function () {
                        var validator = $scope.validator;
                        if (!validator.validate()) {
                            return;
                        }
                        progressService.ShowProgress($element);
                        var promise = marketingEventService.SaveContact(andromedaSiteId, $scope.contact);
                        Rx.Observable
                            .fromPromise(promise)
                            .subscribe(function (result) {
                            MyAndromeda.Logger.Notify("saved");
                            progressService.HideProgress($element);
                            SweetAlert.swal("Saved!", "", "success");
                        }, function (ex) {
                            SweetAlert.swal("A error happened!", "Try again?", "error");
                        }, function () { });
                    };
                    var mainToolbarOptions = {
                        resizable: true,
                        items: [
                            {
                                type: "button",
                                text: "Save",
                                click: save
                            }
                        ]
                    };
                    Rx.Observable.fromPromise(request)
                        .subscribe(function (result) {
                        var data = result.data;
                        $scope.contact = data;
                    }, function (ex) { }, function () { });
                    $scope.mainToolbarOptions = mainToolbarOptions;
                },
                templateUrl: "contact-template.html"
            };
        });
        Marketing.m.directive("eventTemplate", function () {
            return {
                name: "eventTemplate",
                restrict: "E",
                "require": [
                    "^andromedaSiteId",
                    "^marketingType"
                ],
                scope: {
                    andromedaSiteId: "@",
                    marketingType: "@",
                    container: "@",
                    externalSiteId: "@",
                    showSendingButton: "@",
                    description: "@"
                },
                controller: function ($scope, $element, SweetAlert, marketingEventService, progressService) {
                    var andromedaSiteId = $scope.andromedaSiteId;
                    var container = $scope.container;
                    var externalSiteId = $scope.externalSiteId;
                    var showSendingButton = $scope.showSendingButton;
                    var load = function () {
                        var promise;
                        switch ($scope.marketingType) {
                            case "no orders": {
                                promise = marketingEventService.LoadUnRegistered(andromedaSiteId);
                                break;
                            }
                            case "no orders in a week": {
                                promise = marketingEventService.LoadSevenDays(andromedaSiteId);
                                break;
                            }
                            case "no orders in a month": {
                                promise = marketingEventService.LoadOneMonthSettings(andromedaSiteId);
                                break;
                            }
                            case "no orders in three months": {
                                promise = marketingEventService.LoadThreeMonthSettings(andromedaSiteId);
                                break;
                            }
                            case "test marketing type": {
                                promise = marketingEventService.LoadTestSettings(andromedaSiteId);
                                break;
                            }
                            default: {
                                alert("marketing type is not setup for this section: " + $scope.marketingType);
                                return;
                            }
                        }
                        return promise;
                    };
                    var save = function () {
                        progressService.ShowProgress($element);
                        switch ($scope.marketingType) {
                            case "no orders": break;
                            case "no orders in a week": break;
                            case "no orders in a month": break;
                            case "no orders in three months": break;
                            case "test marketing type": break;
                            default: {
                                alert("marketing type is not setup for this section");
                                return;
                            }
                        }
                        var promise = marketingEventService.SaveEvent(andromedaSiteId, $scope.model);
                        Rx.Observable.fromPromise(promise).subscribe(function (result) {
                            progressService.HideProgress($element);
                            SweetAlert.swal("Saved!", "", "success");
                        }, function (ex) {
                            SweetAlert.swal("Error!", "Try again?", "error");
                        }, function () { });
                    };
                    var sendNow = function () {
                        progressService.ShowProgress($element);
                        var promise = marketingEventService.SendNow(andromedaSiteId, $scope.model);
                        Rx.Observable.fromPromise(promise).subscribe(function (result) {
                            progressService.HideProgress($element);
                        });
                    };
                    var openPreivew = function () {
                        var popup = $scope.createPreviewWindow;
                        var p = popup;
                        p.center();
                        popup.open();
                    };
                    var sendPreview = function () {
                        var validator = $scope.previewValidator;
                        if (!validator.validate()) {
                            return;
                        }
                        var popup = $scope.createPreviewWindow;
                        popup.close();
                        var promise = marketingEventService.PreviewEmail(andromedaSiteId, {
                            Model: $scope.model,
                            Preview: $scope.previewSettings
                        });
                        Rx.Observable.fromPromise(promise).subscribe(function (result) {
                            $scope.model = result.data;
                            var popup = $scope.showPreviewWindow;
                            var p = popup;
                            //p.content(result.data.Preview);
                            p.center();
                            popup.open();
                        }, function (ex) { });
                    };
                    var preview = function () {
                        var popup = $scope.createPreviewWindow;
                        var p = popup;
                        p.center();
                        popup.open();
                    };
                    var toolbarOptions = {
                        resizable: true,
                        items: [
                            { template: "<label>SMS:{{smsCount}}</label>" }
                        ]
                    };
                    var mainToolbarOptions = {
                        items: [
                            {
                                //template: "<a class='k-button' ng-click='save()'>Save</a>"
                                text: "Save",
                                click: save,
                                type: "button"
                            },
                            {
                                //template: "<a class='k-button' ng-click='preview()'>Preview</a>"
                                text: "Preview",
                                click: preview,
                                type: "button"
                            },
                            {
                                type: "button",
                                text: "Recipients",
                                click: function () {
                                    var window = $scope.showRecipientWindow;
                                    window.center();
                                    window.open();
                                    var grid = $scope.recipientList;
                                    grid.dataSource.read();
                                }
                            }
                        ]
                    };
                    if (showSendingButton) {
                        mainToolbarOptions.items.push({
                            type: "button",
                            click: sendNow,
                            text: "Launch"
                        });
                    }
                    var kendoTools = ['insertImage', 'insertFile', 'bold', 'italic', 'underline', 'strikethrough', 'justifyLeft', 'justifyCenter', 'justifyRight', 'justifyFull', 'outdent', 'indent',
                        'createLink', 'unlink', 'insertImage', 'insertFile', 'subScript', 'superScript', 'tableEdititing', 'viewHTml', 'formatting', 'cleanFormatting',
                        'fontName', 'fontSize', 'fontColor', 'backColor', 'viewHtml',
                        {
                            name: 'tokentool',
                            tooltip: 'Add tokens',
                            template: '<token-template></token-template>'
                        }
                    ];
                    var fileLocation = function (filePath) {
                        MyAndromeda.Logger.Notify("filePath:");
                        MyAndromeda.Logger.Notify(filePath);
                        var url = "http://cdn.myandromedaweb.co.uk/{0}/stores/{1}/{2}";
                        url = kendo.format(url, container, externalSiteId, filePath).toLowerCase();
                        return url;
                    };
                    var thumbPath = function (folder, file) {
                        var url = "http://cdn.myandromedaweb.co.uk/{0}/stores/{1}/{2}";
                        url = kendo.format(url, container, externalSiteId, folder + file).toLowerCase();
                        return url;
                    };
                    var imageBrowserSettings = {
                        messages: {
                            dropFilesHere: "Drop files here"
                        },
                        transport: {
                            read: kendo.format("/api/{0}/files/ImageBrowser/Read", andromedaSiteId),
                            destroy: {
                                url: kendo.format("/api/{0}/files/ImageBrowser/Destroy", andromedaSiteId),
                                type: "POST"
                            },
                            create: {
                                url: kendo.format("/api/{0}/files/ImageBrowser/Create", andromedaSiteId),
                                type: "POST"
                            },
                            //thumbnailUrl: kendo.format("/api/{0}/files/ImageBrowser/Thumbnail", andromedaSiteId),
                            uploadUrl: kendo.format("/api/{0}/files/ImageBrowser/Upload", andromedaSiteId),
                            imageUrl: fileLocation,
                            fileUrl: fileLocation,
                            thumbnailUrl: thumbPath
                        },
                        path: "/"
                    };
                    var fileBrowserSettings = {
                        messages: {
                            dropFilesHere: "Drop files here"
                        },
                        transport: {
                            read: kendo.format("/api/{0}/files/FileBrowser/Read", andromedaSiteId),
                            destroy: kendo.format("/api/{0}/files/FileBrowser/Destroy", andromedaSiteId),
                            create: kendo.format("/api/{0}/files/FileBrowser/CreateDirectory", andromedaSiteId),
                            uploadUrl: kendo.format("/api/{0}/files/FileBrowser/Upload", andromedaSiteId),
                            imageUrl: fileLocation,
                            fileUrl: fileLocation
                        },
                        path: "/"
                    };
                    var promise = load();
                    Rx.Observable.fromPromise(promise).subscribe(function (result) {
                        var data = result.data;
                        $scope.model = data;
                    });
                    //editor settings 
                    $scope.editorTools = kendoTools;
                    $scope.imageBrowser = imageBrowserSettings;
                    $scope.fileBrowser = fileBrowserSettings;
                    $scope.mainToolbarOptions = mainToolbarOptions;
                    $scope.toolbarOptions = toolbarOptions;
                    $scope.smsCount = 0;
                    $scope.previewSettings = {
                        To: "",
                        Send: false
                    };
                    $scope.save = save;
                    $scope.preview = preview;
                    $scope.sendPreview = sendPreview;
                    $scope.sendNow = sendNow;
                },
                templateUrl: "event-template.html"
            };
        });
        Marketing.m.directive("tokenTemplate", function () {
            return {
                name: "tokenTemplate",
                restrict: "E",
                controller: function ($scope, tokenDataService) {
                    var selected = null;
                    var getTools = function () {
                        var tokenPicker = $scope.tokenPicker;
                        return tokenPicker;
                    };
                    var select = function () {
                        var editor = $scope.EmailTemplateEditor;
                        var tokenPicker = getTools();
                        var selected = tokenPicker.dataItem();
                        editor.paste(selected.Value, {});
                    };
                    $scope.tokenDataSource = tokenDataService.dataSource;
                    $scope.selectToken = select;
                },
                link: function ($scope, element, attrs, controller, transclude) {
                    transclude($scope, function (clone, scope) {
                        element.append(clone);
                    });
                },
                templateUrl: "emailTokenTemplate.html",
                transclude: true
            };
        });
    })(Marketing = MyAndromeda.Marketing || (MyAndromeda.Marketing = {}));
})(MyAndromeda || (MyAndromeda = {}));
/// <reference path="myandromeda.marketing.ts" />
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
/// <reference path="../../Scripts/typings/angularjs/angular-route.d.ts" />
/// <reference path="../../Scripts/typings/angularjs/angular.d.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var Menu;
    (function (Menu) {
        "use strict";
        var MenuApp = (function () {
            function MenuApp() {
            }
            MenuApp.ApplicationName = "MenuApplication";
            return MenuApp;
        })();
        Menu.MenuApp = MenuApp;
        var Angular = (function () {
            function Angular() {
            }
            Angular.Init = function () {
                Menu.Logger.Notify("bootstrap");
                var element = document.getElementById("MenuApp");
                var app = angular.module(MenuApp.ApplicationName, [
                    "kendo.directives",
                    "ngRoute",
                    "ngAnimate"
                ]);
                Menu.Logger.Notify("Assign " + Angular.ServicesInitilizations.length + " services");
                Angular.ServicesInitilizations.forEach(function (value) {
                    value(app);
                });
                Menu.Logger.Notify("Assign " + Angular.ControllersInitilizations.length + " controllers");
                Angular.ControllersInitilizations.forEach(function (value) {
                    value(app);
                });
                app.config(['$routeProvider', function ($routeProvider) {
                        /* route: / */
                        $routeProvider.when(Menu.Controllers.ToppingsController.Route, {
                            template: Menu.Controllers.ToppingsController.Template(),
                            controller: Menu.Controllers.ToppingsController.Name
                        });
                        $routeProvider.otherwise({ redirectTo: "/" });
                    }]);
                angular.bootstrap(element, [MenuApp.ApplicationName]);
                Menu.Logger.Notify("bootstrap-complete");
            };
            Angular.ServicesInitilizations = [];
            Angular.ControllersInitilizations = [];
            return Angular;
        })();
        Menu.Angular = Angular;
    })(Menu = MyAndromeda.Menu || (MyAndromeda.Menu = {}));
})(MyAndromeda || (MyAndromeda = {}));
/// <reference path="MyAndromeda.Menu.App.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var Menu;
    (function (Menu) {
        var Controllers;
        (function (Controllers) {
            var FtpController = (function () {
                function FtpController() {
                }
                FtpController.SetupScope = function ($scope, $timeout, ftpService) {
                    $scope.DeleteLocalFile = function () {
                        var c = confirm("Are you sure you want to delete the current version? Thumbnails will not be effected.");
                        if (c) {
                            ftpService.DeleteLocalFile();
                        }
                    };
                    $scope.DownloadFromFtp = function () { return ftpService.StartFtpDownload(); };
                    $scope.UploadToFtp = function () { return ftpService.StartFtpUpload(); };
                    $scope.CheckDbVersion = function () { return ftpService.GetVersion(); };
                    //FTP download status 
                    ftpService.FtpDownloadBusy.subscribe(function (busy) {
                        $timeout(function () { $scope.DownloadBusy = busy; });
                    });
                    ftpService.FtpDownloadErrors.subscribe(function (error) {
                        $timeout(function () { $scope.Errors = error; });
                    });
                    //FTP upload status
                    ftpService.FtpUploadBusy.subscribe(function (busy) {
                        $timeout(function () { $scope.UploadBusy = busy; });
                    });
                    ftpService.FtpUploadErrors.subscribe(function (error) {
                        $timeout(function () { $scope.Errors = error; });
                    });
                    //FTP access status
                    ftpService.LocalAccessVersion.subscribe(function (versionModel) {
                        $timeout(function () {
                            $scope.Version = versionModel.Version;
                            $scope.UpdatedOn = versionModel.UpdatedOn;
                            $scope.LastDownloaded = versionModel.LastDownloaded;
                        });
                    });
                    var a = ftpService.FtpUploadBusy, b = ftpService.FtpDownloadBusy, c = ftpService.LocalAccessVersionBusy, d = ftpService.DeleteBusy;
                    var anyBusy = Rx.Observable.combineLatest(a, b, c, d, function (o1, o2, o3, o4) {
                        return o1 || o2 || o3 || o4;
                    });
                    anyBusy.subscribe(function (busy) {
                        $timeout(function () {
                            $scope.BlockAccess = busy;
                        });
                    });
                    $scope.CheckDbVersion();
                };
                FtpController.Name = "FtpController";
                return FtpController;
            })();
            Controllers.FtpController = FtpController;
        })(Controllers = Menu.Controllers || (Menu.Controllers = {}));
    })(Menu = MyAndromeda.Menu || (MyAndromeda.Menu = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var Menu;
    (function (Menu) {
        var Services;
        (function (Services) {
            var FtpService = (function () {
                function FtpService($http) {
                    this.$http = $http;
                    this.FtpDownloadBusy = new Rx.BehaviorSubject(false);
                    this.FtpDownloadErrors = new Rx.Subject();
                    this.FtpUploadBusy = new Rx.BehaviorSubject(false);
                    this.FtpUploadErrors = new Rx.Subject();
                    this.LocalAccessVersion = new Rx.Subject();
                    this.LocalAccessVersionBusy = new Rx.BehaviorSubject(false);
                    this.DeleteBusy = new Rx.BehaviorSubject(false);
                }
                FtpService.prototype.ValidateRoute = function (route) {
                    route || (route = "");
                    if (route.length === 0) {
                        throw "The route locations have not been set.";
                    }
                };
                FtpService.prototype.GetVersion = function () {
                    var _this = this;
                    var route = Menu.Settings.Routes.Ftp.Version;
                    this.ValidateRoute(route);
                    var promise = this.$http.post(route, {});
                    this.LocalAccessVersionBusy.onNext(true);
                    promise.success(function (data, status, headers, config) {
                        _this.LocalAccessVersion.onNext(data);
                    });
                    promise.finally(function () { _this.LocalAccessVersionBusy.onNext(false); });
                    return promise;
                };
                FtpService.prototype.StartFtpDownload = function () {
                    var _this = this;
                    var route = Menu.Settings.Routes.Ftp.DownloadMenu;
                    this.ValidateRoute(route);
                    this.FtpDownloadBusy.onNext(true);
                    var promise = this.$http.post(route, {});
                    promise.then(function (result) {
                        var versionPromise = _this.GetVersion();
                        return versionPromise;
                    }).finally(function () {
                        _this.FtpDownloadBusy.onNext(false);
                    });
                };
                FtpService.prototype.StartFtpUpload = function () {
                    var _this = this;
                    var route = Menu.Settings.Routes.Ftp.UploadMenu;
                    this.ValidateRoute(route);
                    this.FtpUploadBusy.onNext(true);
                    var promise = this.$http.post(route, {});
                    promise.finally(function () {
                        _this.FtpUploadBusy.onNext(false);
                    });
                };
                FtpService.prototype.DeleteLocalFile = function () {
                    var _this = this;
                    var route = Menu.Settings.Routes.Ftp.Delete;
                    this.ValidateRoute(route);
                    this.DeleteBusy.onNext(true);
                    var promise = this.$http.post(route, {});
                    promise.finally(function () {
                        _this.DeleteBusy.onNext(false);
                    });
                };
                FtpService.Name = "FtpControllerService";
                return FtpService;
            })();
            Services.FtpService = FtpService;
        })(Services = Menu.Services || (Menu.Services = {}));
    })(Menu = MyAndromeda.Menu || (MyAndromeda.Menu = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var Menu;
    (function (Menu) {
        var Controllers;
        (function (Controllers) {
            Menu.Angular.ControllersInitilizations.push(function (app) {
                app.controller(MenuItemsController.Name, [
                    '$scope',
                    function ($scope) {
                        MenuItemsController.OnLoad($scope);
                        MenuItemsController.SetupScope($scope);
                    }
                ]);
            });
            var MenuItemsController = (function () {
                function MenuItemsController() {
                }
                MenuItemsController.OnLoad = function ($scope) {
                    $scope.$on('$destroy', function () { });
                };
                MenuItemsController.SetupScope = function ($scope) {
                    $scope.$on('$destroy', function () { });
                };
                MenuItemsController.Template = "Templates/MenuItems";
                MenuItemsController.Name = "MenuItemsController";
                MenuItemsController.Route = "/MenuItems";
                return MenuItemsController;
            })();
            Controllers.MenuItemsController = MenuItemsController;
        })(Controllers = Menu.Controllers || (Menu.Controllers = {}));
    })(Menu = MyAndromeda.Menu || (MyAndromeda.Menu = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var Menu;
    (function (Menu) {
        var Controllers;
        (function (Controllers) {
            Menu.Angular.ControllersInitilizations.push(function (app) {
                app.controller(MenuNavigationController.Name, [
                    '$scope',
                    Menu.Services.MenuNavigationService.Name,
                    Menu.Services.PublishingService.Name,
                    function ($scope, menuNavigationService, publishingService) {
                        Menu.Logger.Notify("Menu navigation controller loaded");
                        MenuNavigationController.ToolbarOptions($scope, publishingService);
                        MenuNavigationController.OnLoad($scope, menuNavigationService);
                    }
                ]);
            });
            var MenuNavigationController = (function () {
                function MenuNavigationController() {
                }
                MenuNavigationController.ToolbarOptions = function ($scope, publishingService) {
                    publishingService.init();
                    $scope.Publish = function () { publishingService.openWindow(); };
                    $scope.ToolbarOptions = {
                        items: [
                            //{ type: "button", text: "Menu Items" },
                            //{ type: "button", text: "Menu Sequencing" },
                            //{ type: "button", text: "Toppings" },
                            //{ type: "separator" },
                            { type: "button", text: "Publish", click: function () { publishingService.openWindow(); } }
                        ]
                    };
                };
                MenuNavigationController.OnLoad = function ($scope, menuNavigationService) {
                    $scope.$on("$destroy", function () { });
                };
                MenuNavigationController.Name = "MenuNavigationController";
                return MenuNavigationController;
            })();
            Controllers.MenuNavigationController = MenuNavigationController;
        })(Controllers = Menu.Controllers || (Menu.Controllers = {}));
    })(Menu = MyAndromeda.Menu || (MyAndromeda.Menu = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var Menu;
    (function (Menu) {
        var Controllers;
        (function (Controllers) {
            Menu.Angular.ControllersInitilizations.push(function (app) {
                app.controller(ToppingsController.Name, [
                    '$scope',
                    Menu.Services.MenuToppingsService.Name,
                    function ($scope, menuToppingsService) {
                        Menu.Logger.Debug("Start: Setting up Toppings controller");
                        ToppingsController.OnLoad($scope);
                        ToppingsController.SetupScope($scope);
                        ToppingsController.SetupItemTemplate($scope);
                        ToppingsController.SetupDataSource($scope, menuToppingsService);
                        Menu.Logger.Debug("Complete: Setting up Toppings controller");
                    }
                ]);
            });
            var ToppingsController = (function () {
                function ToppingsController() {
                }
                ToppingsController.OnLoad = function ($scope) {
                    Menu.Logger.Notify("Toppings Controller Loaded");
                };
                ToppingsController.SetupScope = function ($scope) {
                };
                ToppingsController.SetupItemTemplate = function ($scope) {
                    $scope.MenuToppingsListViewTemplate = $("#MenuToppingsListViewTemplate").html();
                    $scope.MenuToppingsEditListViewTemplate = $("#MenuToppingsEditListViewTemplate").html();
                };
                ToppingsController.SetupDataSource = function ($scope, service) {
                    var dataSource = service.GetDataSource();
                    $scope.DataSource = dataSource;
                    var start = new Rx.Subject();
                    var end = new Rx.Subject();
                    var startLoading = start.subscribe(function (e) {
                        Menu.Logger.Debug("Start Loading");
                        kendo.ui.progress($("body"), true);
                    });
                    var endLoading = end.subscribe(function (e) {
                        Menu.Logger.Debug("Loading complete");
                        kendo.ui.progress($("body"), false);
                    });
                    dataSource.bind("requestStart", function () { start.onNext(true); });
                    dataSource.bind("requestEnd", function () { end.onNext(true); });
                    $scope.$on("$destroy", function () {
                        startLoading.dispose();
                        endLoading.dispose();
                    });
                };
                ToppingsController.Name = "ToppingsController";
                ToppingsController.Route = "/";
                ToppingsController.Template = function () { return $("#MenuTemplate").html(); };
                return ToppingsController;
            })();
            Controllers.ToppingsController = ToppingsController;
        })(Controllers = Menu.Controllers || (Menu.Controllers = {}));
    })(Menu = MyAndromeda.Menu || (MyAndromeda.Menu = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var Menu;
    (function (Menu) {
        var Controllers;
        (function (Controllers) {
            Menu.Angular.ControllersInitilizations.push(function (app) {
                app.controller(ToppingsFilterController.Name, [
                    '$scope',
                    '$timeout',
                    Menu.Services.MenuToppingsService.Name,
                    Menu.Services.MenuToppingsFilterService.Name,
                    function ($scope, $timeout, menuToppingsService, menuToppingsFilterService) {
                        Menu.Logger.Debug("Setting up ToppingsFilterController");
                        ToppingsFilterController.OnLoad($scope);
                        ToppingsFilterController.SetupScope($scope, $timeout, menuToppingsFilterService);
                        Menu.Logger.Debug("Set up ToppingsFilterController");
                    }
                ]);
            });
            var ToppingsFilterController = (function () {
                function ToppingsFilterController() {
                }
                ToppingsFilterController.OnLoad = function ($scope) {
                    Menu.Logger.Notify("Toppings Filter Controller Loaded");
                };
                ToppingsFilterController.SetupScope = function ($scope, $timeout, menuToppingsFilterService) {
                    $scope.Name = menuToppingsFilterService.GetName();
                    $scope.$watch("Name", function (newValue, olderValue) {
                        Menu.Logger.Debug("ToppingsFilterController : Name changed");
                        if (newValue === olderValue) {
                            return;
                        }
                        menuToppingsFilterService.ChangeNameFilter(newValue);
                    });
                    $scope.ResetFilters = function () {
                        Menu.Logger.Debug("Reset button clicked");
                        menuToppingsFilterService.ResetFilters();
                    };
                    var observable = menuToppingsFilterService.ResetFiltersObservable.subscribe(function () {
                        $timeout(function () {
                            $scope.Name = "";
                        }, 0);
                    });
                    $scope.$on("$destroy", function () {
                        observable.dispose();
                    });
                };
                ToppingsFilterController.Name = "ToppingsFilterController";
                return ToppingsFilterController;
            })();
            Controllers.ToppingsFilterController = ToppingsFilterController;
        })(Controllers = Menu.Controllers || (Menu.Controllers = {}));
    })(Menu = MyAndromeda.Menu || (MyAndromeda.Menu = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var Menu;
    (function (Menu) {
        var Models;
        (function (Models) {
            var thumbItem = (function () {
                function thumbItem() {
                }
                return thumbItem;
            })();
            Models.thumbItem = thumbItem;
        })(Models = Menu.Models || (Menu.Models = {}));
    })(Menu = MyAndromeda.Menu || (MyAndromeda.Menu = {}));
})(MyAndromeda || (MyAndromeda = {}));
/// <reference path="MyAndromeda.Menu.Controllers.FtpController.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var Menu;
    (function (Menu) {
        var Modules;
        (function (Modules) {
            Modules.MenuFtpModuleName = "MenuFtpModule";
            var FtpModule = angular.module(Modules.MenuFtpModuleName, [
                "kendo.directives"
            ]);
            FtpModule.factory(Menu.Services.FtpService.Name, [
                '$http',
                function ($http) {
                    var instance = new Menu.Services.FtpService($http);
                    return instance;
                }
            ]);
            FtpModule.controller(Menu.Controllers.FtpController.Name, [
                '$scope',
                '$timeout',
                Menu.Services.FtpService.Name,
                function ($scope, $timeout, ftpService) {
                    Menu.Controllers.FtpController.SetupScope($scope, $timeout, ftpService);
                }
            ]);
        })(Modules = Menu.Modules || (Menu.Modules = {}));
    })(Menu = MyAndromeda.Menu || (MyAndromeda.Menu = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var Menu;
    (function (Menu) {
        var Services;
        (function (Services) {
            var CategoryService = (function () {
                function CategoryService(categories) {
                    this.categories = categories;
                }
                CategoryService.prototype.findById = function (id) {
                    for (var i = 0; i < this.categories.length; i++) {
                        var c = this.categories[i];
                        if (c.Id === id) {
                            return c;
                        }
                    }
                    return null;
                };
                CategoryService.prototype.findCategoriesByParentId = function (id) {
                    var elements = $.grep(this.categories, function (category, index) {
                        return category.ParentId === id;
                    });
                    return elements;
                };
                return CategoryService;
            })();
            Services.CategoryService = CategoryService;
        })(Services = Menu.Services || (Menu.Services = {}));
    })(Menu = MyAndromeda.Menu || (MyAndromeda.Menu = {}));
})(MyAndromeda || (MyAndromeda = {}));
/// <reference path="../../Scripts/typings/kendo/kendo.all.d.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var Menu;
    (function (Menu) {
        var Services;
        (function (Services) {
            var MenuControllerService = (function () {
                function MenuControllerService(options) {
                    this.options = options;
                    this.menuService = new Services.MenuService(options);
                    this.publishingService = new Services.PublishingService(options, this.menuService);
                }
                MenuControllerService.prototype.initPager = function () {
                    $(this.options.ids.pagerId).kendoPager({
                        dataSource: this.menuService.menuItemService.dataSource
                    });
                };
                MenuControllerService.prototype.initListView = function () {
                    var _this = this;
                    var internal = this, dataSource = this.menuService.menuItemService.dataSource;
                    this.listView = $(this.options.ids.listViewId).kendoListView({
                        dataSource: dataSource,
                        template: kendo.template(this.options.listview.template),
                        editTemplate: kendo.template(this.options.listview.editTemplate),
                        save: function (e) {
                            console.log("save");
                            var listView = internal.listView, dataSource = internal.menuService.menuItemService.dataSource, item = e.item, model = e.model;
                            //prevent the sync being called on the data source
                            e.preventDefault();
                            //close the list view editor
                            _this.closeEditItem.onNext(model);
                            dataSource.trigger("change");
                            //d.anyDirtyItems();
                        },
                        edit: function (e) {
                            console.log("edit");
                            //physical HTML element
                            var menuItemElement = $(e.item), 
                            //data-id attribute 
                            id = menuItemElement.data("id"), 
                            //unique name encase batch mode is enabled
                            uploadId = "#files" + id, advancedUploadId = "#cropFiles" + id;
                            //need to find the observable model
                            var menuItem = internal.menuService.menuItemService.findById(id);
                            _this.openEditItem.onNext(menuItem);
                            internal.initEditListViewItem(uploadId, advancedUploadId, menuItem);
                        },
                        cancel: function (e) {
                            var model = e.model;
                            _this.closeEditItem.onNext(model);
                        }
                    }).data("kendoListView");
                    //public openEditItem: Rx.BehaviorSubject<Models.IMenuItemObservable>;
                    //public closeEditItem: Rx.BehaviorSubject<Models.IMenuItemObservable>;
                    this.openEditItem = new Rx.BehaviorSubject(null);
                    this.closeEditItem = new Rx.BehaviorSubject(null);
                    this.closeEditItem.where(function (e) { return e !== null; }).subscribe(function (e) {
                        var listView = _this.listView;
                        listView._closeEditable(true);
                    });
                    var changeHandler = function (e) {
                        var menuItem = this, webNameField = "WebName", descriptionField = "WebDescription", webSequenceField = "WebSequence";
                        console.log("change k");
                        console.log(e);
                        var relatedItems = internal.menuService.menuItemService.getRelatedItems([menuItem]);
                        console.log("related = " + relatedItems.length);
                        if (relatedItems.length === 0) {
                            //don't need to care... run away
                            return;
                        }
                        if (e.field === webNameField) {
                            var newVal = menuItem.get(webNameField);
                            relatedItems.forEach(function (item, index) {
                                item.set(webNameField, newVal);
                            });
                        }
                        if (e.field === descriptionField) {
                            var newVal = menuItem.get(descriptionField);
                            relatedItems.forEach(function (item, index) {
                                item.set(descriptionField, newVal);
                            });
                        }
                        if (e.field === webSequenceField) {
                            var newVal = menuItem.get(webSequenceField);
                            relatedItems.forEach(function (item, index) {
                                item.set(webSequenceField, newVal);
                            });
                        }
                    };
                    var altered = Rx.Observable.combineLatest(this.openEditItem, this.closeEditItem, function (v1, v2) {
                        return {
                            opened: v1,
                            closed: v2
                        };
                    });
                    altered.subscribe(function (c) {
                        console.log("opened / closed item");
                        if (c.opened === null) {
                            return;
                        }
                        if (c.closed !== null) {
                            c.closed.unbind("change", changeHandler);
                        }
                        console.log("add change handler");
                        c.opened.bind("change", changeHandler);
                    });
                };
                //list view item events: 
                MenuControllerService.prototype.initListViewItemEvents = function () {
                    var _this = this;
                    var listViewId = this.options.ids.listViewId;
                    var internal = this;
                    //watch for the edit button clicked.
                    $(listViewId).on("click", ".k-button-edit", function (e) {
                        e.preventDefault();
                        console.log("watching edits");
                        var row = $(this).closest(".menu-item");
                        //check if any editor rows are active
                        var editorRow = internal.listView.element.find(".k-edit-item");
                        if (editorRow.length == 0) {
                            //internal.listView.cancel();
                            internal.listView.edit(row);
                            return;
                        }
                        var menuItemElement = editorRow, 
                        //data-id attribute 
                        id = menuItemElement.data("id");
                        var menuItem = internal.menuService.menuItemService.findById(id);
                        internal.closeEditItem.onNext(menuItem);
                        internal.listView.edit(row);
                    });
                    $(listViewId).on("click", ".k-button-enable", function (e) {
                        e.preventDefault();
                        var row = $(this).closest(".menu-item");
                        var id = row.data("id");
                        var menuItem = internal.menuService.menuItemService.findById(id);
                        menuItem.Enable();
                    });
                    $(listViewId).on("click", ".k-button-disable", function (e) {
                        e.preventDefault();
                        var row = $(this).closest(".menu-item");
                        var id = row.data("id");
                        var menuItem = internal.menuService.menuItemService.findById(id);
                        menuItem.Disable();
                    });
                    //watch for any save event
                    $(listViewId).on("click", ".k-button-save", function (e) {
                        e.preventDefault();
                        //save changes
                        _this.listView.save();
                    });
                    //watch for any cancel event
                    $(listViewId).on("click", ".k-button-cancel", function (e) {
                        e.preventDefault();
                        _this.listView.cancel();
                    });
                };
                MenuControllerService.prototype.initEditListItemThumbnailUpload = function (thumbnailUploadElementId, folder, menuItem) {
                    var _this = this;
                    var uploadBox = $(thumbnailUploadElementId).kendoUpload({
                        async: {
                            saveUrl: Menu.Settings.Routes.MenuItems.SaveImageUrl + folder,
                            //removeUrl: removeFile,
                            autoUpload: true,
                            batch: false
                        },
                        showFileList: false
                    }).data("kendoUpload");
                    //when the service comes back and says its complete + succeeded 
                    uploadBox.bind("success", function (e) {
                        //effectively the JSON response. 
                        var response = e.response;
                        var thumbData = new kendo.data.ObservableArray([]);
                        //response will contain all of the thumb elements;
                        for (var i = 0; i < e.response.length; i++) {
                            var thumb = e.response[i];
                            thumbData.push(thumb);
                        }
                        //menu item is an observable, ergo updating the array should make it happy. 
                        menuItem.set("Thumbs", thumbData);
                        var relatedItems = _this.menuService.menuItemService.getRelatedItems([menuItem]);
                        if (relatedItems.length > 0) {
                            var message = kendo.format("Add this image to {0} other records?", relatedItems.length);
                            var assignToSimilarItems = confirm(message);
                            if (assignToSimilarItems) {
                                relatedItems.forEach(function (item) {
                                    item.set("Thumbs", thumbData);
                                });
                            }
                        }
                    });
                };
                MenuControllerService.prototype.initEditListViewAdvancedThumbnail = function (advancedThumbnailUploadElementId, folder) {
                    var advancedUploadBox;
                    var onSelect = function (selectArgs) {
                        var fileList = selectArgs.files;
                        fileList.forEach(function (value, index) {
                            var fileReader = new FileReader();
                            fileReader.onload = function (onLoadArgs) {
                                var image = $('<img />').attr("src", fileReader.result);
                                var imageHolder = advancedUploadBox.element.closest(".k-widget").find(".imageHolder");
                                imageHolder.append(image);
                            };
                            fileReader.readAsDataURL(value.rawFile);
                        });
                    };
                    advancedUploadBox = $(advancedThumbnailUploadElementId).kendoUpload({
                        async: {
                            saveUrl: Menu.Settings.Routes.MenuItems.SaveImageUrl + folder,
                            //removeUrl: removeFile,
                            autoUpload: false,
                            batch: false
                        },
                        template: $("#thumbnails-crop-template").html(),
                        select: onSelect
                    }).data("kendoUpload");
                };
                MenuControllerService.prototype.initEditListViewRemoveThumbnails = function (removeFile, menuItem) {
                    var _this = this;
                    //remove thumbnail
                    $(this.options.ids.listViewId).on("click", ".k-button-remove-thumb", function () {
                        var itemUrl = $(_this).closest(".thumb").data("fileName");
                        var call = $.ajax({
                            url: removeFile,
                            type: "POST",
                            dataType: "json",
                            data: { fileName: itemUrl }
                        });
                        call.done(function (e) {
                            _this.menuService.menuItemService.removeThumb(menuItem, itemUrl);
                        });
                        call.fail(function (e) {
                            alert("There was an error processing the request. Please try again");
                        });
                    });
                    //remove all thumbnails
                    $(this.options.ids.listViewId).on("click", ".k-button-removeall-thumb", function () {
                        //var itemUrl = $(this).closest(".thumb").data("fileName");
                        //if(!confirm("Are you sure you want to remove all of this items thumbnails"))
                        //{
                        //    return false;
                        //}
                        var removeFrom = [menuItem];
                        var relatedItems = _this.menuService.menuItemService.getRelatedItems([menuItem]);
                        if (relatedItems.length > 0) {
                            var message = kendo.format("remove this image from {0} other records? You need to cancel all changes if you want to undo!", relatedItems.length);
                            var assignToSimilarItems = confirm(message);
                            if (assignToSimilarItems) {
                                relatedItems.forEach(function (item) {
                                    removeFrom.push(item);
                                });
                            }
                        }
                        removeFrom.forEach(function (item) {
                            while (item.Thumbs.length > 0) {
                                item.Thumbs.pop();
                            }
                        });
                    });
                };
                MenuControllerService.prototype.initEditListViewFlocking = function (menuItem) {
                    menuItem.bind("change", function (e) { });
                };
                MenuControllerService.prototype.initEditListViewItem = function (thumbnailUploadElementId, advancedThumbnailUploadElementId, menuItem) {
                    var folder = "?folderPath=" + menuItem.Id; //folder to store the image in 
                    var removeFile = Menu.Settings.Routes.MenuItems.RemoveImageUrl + folder; // this.options.actionUrls.removeImageUrl + folder;
                    /* setup thumbnails */
                    this.initEditListItemThumbnailUpload(thumbnailUploadElementId, folder, menuItem);
                    this.initEditListViewAdvancedThumbnail(advancedThumbnailUploadElementId, folder);
                    this.initEditListViewRemoveThumbnails(removeFile, menuItem);
                    /* setup item edit */
                    this.initEditListViewFlocking(menuItem);
                };
                MenuControllerService.prototype.initHubChanges = function () {
                    var internal = this, hub = MyAndromeda.Hubs.StoreHub.GetInstance(this.options.routeParameters);
                    hub.bind(MyAndromeda.Hubs.StoreHub.MenuItemChangeEvent, function (context) {
                        console.log("i have changes :)");
                        //console.log(context);
                        internal.menuService.extendMenuItemData(context.EditedItems);
                        var updateDataSource = context.Section === "Sequence";
                        internal.menuService.menuItemService.updateItems(context.EditedItems, updateDataSource);
                        if (!$.isEmptyObject(Services.menuFilterController.SORTFILTER)) {
                            var currentSort = internal.menuService.menuItemService.dataSource.sort();
                            if (currentSort != Services.menuFilterController.SORTFILTER) {
                                internal.menuService.menuItemService.dataSource.sort(Services.menuFilterController.SORTFILTER);
                            }
                        }
                    });
                };
                MenuControllerService.prototype.setEditorDefaultBehaviour = function () {
                    var editor = kendo.ui.Editor, tools = editor.defaultTools;
                    tools["insertLineBreak"].options.shift = true;
                    tools["insertParagraph"].options.shift = true;
                };
                MenuControllerService.prototype.initMenuStatusView = function () {
                    var id = this.options.ids.statusViewId, status = this.menuService.menuItemService.menuStatus;
                    if (id) {
                        kendo.bind($(id), status.observable);
                    }
                };
                MenuControllerService.prototype.init = function () {
                    this.setEditorDefaultBehaviour();
                    this.menuService.init();
                    //setup ui elements & events
                    this.initListView();
                    //set in the initialization 
                    //this.initListViewEvents();
                    this.initListViewItemEvents();
                    this.initPager();
                    this.initMenuStatusView();
                    this.initHubChanges();
                };
                MenuControllerService.MaxLengthMessage = "The max length for the field is {0} characters. Current count: {1}";
                MenuControllerService.EditingNewItemMessage = "You are navigating away from the current item, please save changes or press cancel to the current item.";
                return MenuControllerService;
            })();
            Services.MenuControllerService = MenuControllerService;
        })(Services = Menu.Services || (Menu.Services = {}));
    })(Menu = MyAndromeda.Menu || (MyAndromeda.Menu = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var Menu;
    (function (Menu) {
        var Services;
        (function (Services) {
            var menuFilterController = (function () {
                function menuFilterController(filterIds, target) {
                    this.filterIds = filterIds;
                    this.target = target;
                    this.viewModel = kendo.observable({
                        ItemName: "",
                        SelectedDisplayCategory: "",
                        SelectedCategory1: "",
                        SelectedCategory2: "",
                        ShowGetStartedMessage: true,
                        Filtered: false,
                        FilteredBy: kendo.observable({
                            Name: "",
                            DisplayCategory: "",
                            Category1: "",
                            Category2: ""
                        }),
                        Sorted: false,
                        SortedBy: ""
                    });
                }
                //setup display category data
                menuFilterController.prototype.InitDisplayCategoryDataSource = function () {
                    var source = this.displayCategoryService.categories;
                    this.displayCategoryDataSource = new kendo.data.DataSource({
                        data: source
                    });
                    this.viewModel.set("DisplayCategory", this.displayCategoryDataSource);
                    this.viewModel.set("SelectedDisplayCategory", null);
                };
                //setup category 1 data
                menuFilterController.prototype.InitCategory1DataSource = function () {
                    var source = this.category1Service.categories;
                    this.category1DataSource = new kendo.data.DataSource({
                        data: source
                    });
                    this.viewModel.set("Category1", this.category1DataSource);
                };
                //setup category 2 data
                menuFilterController.prototype.InitCategory2DataSource = function () {
                    var source = this.category2Service.categories;
                    this.category2DataSource = new kendo.data.DataSource({
                        data: source
                    });
                    this.viewModel.set("Category2", this.category2DataSource);
                };
                //setup all local data sources
                menuFilterController.prototype.InitDataSources = function () {
                    console.log("setting category data sources");
                    this.InitDisplayCategoryDataSource();
                    this.InitCategory1DataSource();
                    this.InitCategory2DataSource();
                };
                menuFilterController.prototype.InitSearchBox = function () {
                    var internal = this;
                    this.itemSearchBox = $(this.filterIds.itemNameId);
                };
                menuFilterController.prototype.BindViewModel = function () {
                    kendo.bind(this.filterIds.toolBarId, this.viewModel);
                };
                menuFilterController.prototype.InitDisplayCategoryAndEvents = function () {
                    var internal = this;
                    this.displayCategoryCombo = $(this.filterIds.displayCategoryId).data("kendoComboBox");
                    this.displayCategoryCombo.bind("select", function (e) {
                        internal.viewModel.set(menuFilterController.SHOWGETSTARTEDMESSAGE, false);
                    });
                };
                menuFilterController.prototype.InitFields = function () {
                    this.BindViewModel();
                    this.InitSearchBox();
                    this.InitDisplayCategoryAndEvents();
                    this.category1Combo = $(this.filterIds.category1Id).data("kendoComboBox");
                    this.category2Combo = $(this.filterIds.category2Id).data("kendoComboBox");
                };
                menuFilterController.prototype.buildFilterForMainList = function () {
                    var filters = [];
                    var values = {
                        itemName: this.viewModel.get(menuFilterController.ITEMNAME).trim(),
                        displayCategoryId: this.displayCategoryCombo.value(),
                        category1Id: this.category1Combo.value(),
                        category2Id: this.category2Combo.value()
                    };
                    if (values.itemName) {
                        var nameFilters = {
                            logic: "or",
                            filters: [
                                { field: "Name", operator: "contains", value: values.itemName },
                                { field: "WebName", operator: "contains", value: values.itemName },
                                { field: "WebDescription", operator: "contains", value: values.itemName }
                            ]
                        };
                        filters.push(nameFilters);
                    }
                    //value needs to be the same as the query item. 
                    if (typeof (values.displayCategoryId) !== "undefined" && values.displayCategoryId !== "") {
                        filters.push({ field: "DisplayCategoryId", operator: "eq", value: parseInt(values.displayCategoryId) });
                    }
                    var category1Selected = typeof (values.category1Id) !== "undefined" && values.category1Id !== "";
                    var category2Selected = typeof (values.category2Id) !== "undefined" && values.category2Id !== "";
                    if (category1Selected) {
                        filters.push({ field: "CategoryId1", operator: "eq", value: parseInt(values.category1Id) });
                    }
                    if (category2Selected) {
                        filters.push({ field: "CategoryId2", operator: "eq", value: parseInt(values.category2Id) });
                    }
                    return filters;
                };
                menuFilterController.prototype.initFilterChanges = function () {
                    var internal = this;
                    $(this.filterIds.toolBarId).on("keyup change", function (e) {
                        //if(internal.itemSearchBox.context.activeElement.value){
                        //}
                        var filters = internal.buildFilterForMainList();
                        internal.target.filter(filters);
                        internal.target.sort(menuFilterController.SORTFILTER);
                    });
                };
                menuFilterController.prototype.initDisplayFilterAndSortMessageCue = function () {
                    var internal = this;
                    internal.target.bind("change", function (e) {
                        var dsFilter = internal.target.filter(), dsSort = internal.target.sort(), vm = internal.viewModel;
                        vm.set(menuFilterController.FILTERED, dsFilter && dsFilter.filters && dsFilter.filters.length > 0 && internal.target.data().length > 0);
                        var filteredBy = vm.get(menuFilterController.FILTEREDBY);
                        filteredBy.set("Name", internal.itemSearchBox.val());
                        filteredBy.set("DisplayCategory", internal.displayCategoryCombo.text());
                        filteredBy.set("Category1", internal.category1Combo.text());
                        filteredBy.set("Category2", internal.category2Combo.text());
                        var sorting = dsSort.length > 0;
                        vm.set(menuFilterController.SORTED, sorting);
                        vm.set(menuFilterController.SORTEDBY, sorting ? dsSort[0].field : "");
                    });
                };
                menuFilterController.prototype.initFilterReset = function () {
                    var internal = this;
                    $(this.filterIds.resetId).on("click", function (e) {
                        e.preventDefault();
                        internal.viewModel.set(menuFilterController.DISPLAYCATEGORY, "");
                        internal.viewModel.set("SelectedDisplayCategory", null);
                        internal.viewModel.set(menuFilterController.CATEGORY1, "");
                        internal.viewModel.set(menuFilterController.CATEGORY2, "");
                        internal.viewModel.set(menuFilterController.ITEMNAME, "");
                        internal.viewModel.set(menuFilterController.SHOWGETSTARTEDMESSAGE, true);
                        internal.target.filter(menuFilterController.RESETFILTER);
                    });
                };
                menuFilterController.prototype.init = function (displayCategoryService, category1Service, category2Service) {
                    this.displayCategoryService = displayCategoryService;
                    this.category1Service = category1Service;
                    this.category2Service = category2Service;
                    //create data sources from the data 
                    this.InitDataSources();
                    //create ui controls to manage the filters 
                    this.InitFields();
                    this.initFilterChanges();
                    this.initFilterReset();
                    this.initDisplayFilterAndSortMessageCue();
                };
                menuFilterController.ITEMNAME = "ItemName";
                menuFilterController.DISPLAYCATEGORY = "SelectedDisplayCategory";
                menuFilterController.CATEGORY1 = "SelectedCategory1";
                menuFilterController.CATEGORY2 = "SelectedCategory2";
                menuFilterController.SHOWGETSTARTEDMESSAGE = "ShowGetStartedMessage";
                menuFilterController.FILTERED = "Filtered";
                menuFilterController.FILTEREDBY = "FilteredBy";
                menuFilterController.SORTED = "Sorted";
                menuFilterController.SORTEDBY = "SortedBy";
                menuFilterController.RESETFILTER = [];
                menuFilterController.SORTFILTER = [];
                return menuFilterController;
            })();
            Services.menuFilterController = menuFilterController;
        })(Services = Menu.Services || (Menu.Services = {}));
    })(Menu = MyAndromeda.Menu || (MyAndromeda.Menu = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var Menu;
    (function (Menu) {
        var Services;
        (function (Services) {
            Menu.Angular.ServicesInitilizations.push(function (app) {
            });
            var MenuItemService = (function () {
                function MenuItemService(dataSource) {
                    this.dataSource = dataSource;
                    this.menuStatus = new Services.MenuStatus(dataSource);
                }
                //find elements based on the id
                MenuItemService.prototype.findById = function (id) {
                    return this.dataSource.get(id);
                };
                //find elements based on the name
                MenuItemService.prototype.getRelatedItems = function (menuItems) {
                    var internal = this;
                    var data = internal.dataSource.data();
                    //var filtered = data.filter((value: Models.IMenuItemObservable) => {
                    //    var left = menuItems.filter((item) => {
                    //        if (item.Id === value.Id) { return false; }
                    //        if (item.Name === value.Name) { return true; }
                    //    });
                    //    return 
                    //});
                    var series = Enumerable.from(menuItems).selectMany(function (x) {
                        return internal.dataSource.data().filter(function (value, index, array) {
                            return value.Name == x.Name && value.Id !== x.Id;
                        });
                    }).toArray();
                    return series;
                };
                MenuItemService.prototype.removeThumb = function (menuItem, filename) {
                    var thumbs = menuItem.get("Thumbs");
                    var thumb = thumbs.find(function (item, index, array) {
                        return item.FileName === filename;
                    });
                    thumbs.remove(thumb);
                };
                MenuItemService.prototype.updateItems = function (menuItems, updateDataSource) {
                    var internal = this;
                    for (var i = 0; i < menuItems.length; i++) {
                        var item = menuItems[i];
                        var dataSourceItem = internal.findById(item.Id), dataItem = dataSourceItem;
                        if (!dataSourceItem) {
                            continue;
                        }
                        var acceptParams = {
                            Name: item.Name,
                            WebDescription: item.WebDescription,
                            WebName: item.WebName,
                            WebSequence: item.WebSequence,
                            Prices: item.Prices
                        };
                        dataItem.accept(acceptParams);
                        if (!updateDataSource) {
                            dataItem.trigger("change");
                        }
                    }
                    if (updateDataSource) {
                        this.dataSource.trigger("change");
                    }
                    //this.dataSource.trigger("change");
                };
                MenuItemService.prototype.anyDirtyItems = function () {
                    var internal = this;
                    return this.dataSource.hasChanges();
                };
                return MenuItemService;
            })();
            Services.MenuItemService = MenuItemService;
        })(Services = Menu.Services || (Menu.Services = {}));
    })(Menu = MyAndromeda.Menu || (MyAndromeda.Menu = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var Menu;
    (function (Menu) {
        var Services;
        (function (Services) {
            Menu.Angular.ServicesInitilizations.push(function (app) {
                app.factory(MenuNavigationService.Name, [
                    function () {
                        var instance = new MenuNavigationService();
                        return instance;
                    }
                ]);
            });
            var MenuNavigationService = (function () {
                function MenuNavigationService() {
                }
                MenuNavigationService.Name = "MenuNavigationService";
                return MenuNavigationService;
            })();
            Services.MenuNavigationService = MenuNavigationService;
        })(Services = Menu.Services || (Menu.Services = {}));
    })(Menu = MyAndromeda.Menu || (MyAndromeda.Menu = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var Menu;
    (function (Menu) {
        var Services;
        (function (Services) {
            Menu.Angular.ServicesInitilizations.push(function (app) {
            });
            var Positioning = (function () {
                function Positioning() {
                }
                Positioning.BEFORE = "before";
                Positioning.AFTER = "after";
                Positioning.INTO = "into";
                Positioning.REPLACE = "replace";
                return Positioning;
            })();
            var MenuOrderingControllerService = (function () {
                function MenuOrderingControllerService(options) {
                    var internal = this;
                    this.options = options;
                    this.menuService = new Services.MenuService(this.options);
                    this.publishingService = new Services.PublishingService(this.options, this.menuService);
                    this.positionWindowViewModel = kendo.observable({
                        length: function () {
                            var items = internal.menuService.menuItemService.dataSource.view();
                            var total = items.length;
                            return total;
                        },
                        onMoveToFirst: function () {
                            var item = this.get("item");
                            var firstItem = internal.menuService.menuItemService.dataSource.view()[0];
                            internal.movePlaces(firstItem, item, Positioning.BEFORE);
                            internal.updateWindowUi();
                        },
                        onMoveToLast: function () {
                            var menuItemService = internal.menuService.menuItemService;
                            var view = internal.menuService.menuItemService.dataSource.view();
                            var item = this.get("item");
                            var lastItem = view[this.get("length") - 1];
                            internal.movePlaces(lastItem, item, Positioning.AFTER);
                            internal.updateWindowUi();
                        },
                        onMoveItemDown: function () {
                            var item = this.get("item"), index = parseInt(this.get("index")), view = internal.menuService.menuItemService.dataSource.view(), vm = internal.positionWindowViewModel;
                            vm.set("index", index < view.length ? index + 1 : index);
                            internal.moveItemByTextBox();
                        },
                        onMoveItemUp: function () {
                            var item = this.get("item"), index = parseInt(this.get("index")), vm = internal.positionWindowViewModel;
                            vm.set("index", index > 1 ? index - 1 : index);
                            internal.moveItemByTextBox();
                        },
                        onMoveToPositionClick: function () {
                            var item = this.get("item"), index = this.get("index"), view = internal.menuService.menuItemService.dataSource.view(), vm = internal.positionWindowViewModel;
                            internal.moveItemByTextBox();
                        },
                        saveChanges: function () {
                            internal.closeWindow(true);
                        },
                        cancelChanges: function () {
                            internal.closeWindow(false);
                            internal.menuService.menuItemService.dataSource.cancelChanges();
                        },
                        displayTest: function () {
                            var value = this.get("item");
                            if (value === null) {
                                return "nothing";
                            }
                            return value.DisplayName();
                        },
                        item: null,
                        itemName: "-",
                        itemIndex: 0,
                        //index of the numeric input box
                        index: 0
                    });
                }
                MenuOrderingControllerService.prototype.updateWindowUi = function () {
                    var internal = this;
                    internal.normalize();
                    internal.menuService.menuItemService.dataSource.sort({ field: "WebSequence", dir: "asc" });
                    var vm = this.positionWindowViewModel;
                    var item = vm.get("item");
                    vm.set("itemName", item.DisplayName());
                    vm.set("itemIndex", item.Index());
                    vm.set("index", item.Index());
                };
                MenuOrderingControllerService.prototype.moveItemByTextBox = function () {
                    var vm = this.positionWindowViewModel, item = vm.get("item"), internal = this, view = this.menuService.menuItemService.dataSource.view();
                    var value = $("#numericIndexSelector").val();
                    if (value < 0) {
                        return;
                    }
                    if (value == vm.get("itemIndex")) {
                        return;
                    }
                    vm.set("index", parseInt(value));
                    var a = view[value - 1];
                    this.movePlaces(a, item, Positioning.INTO);
                    this.updateWindowUi();
                };
                MenuOrderingControllerService.prototype.initListView = function () {
                    var internal = this;
                    this.listView = $(this.options.ids.listViewId).kendoListView({
                        autoBind: false,
                        dataSource: this.menuService.menuItemService.dataSource,
                        template: kendo.template(this.options.listview.template),
                        editTemplate: kendo.template(this.options.listview.editTemplate)
                    }).data("kendoListView");
                };
                MenuOrderingControllerService.prototype.highlighOriginalItem = function (item) {
                    $(item)
                        .animate({ "margin-left": "10px", "opacity": 0.4 }, 100)
                        .animate({ "margin-left": "0px" }, 100)
                        .css({ "border-color": "#E7001B", "border-width": "4px" });
                };
                MenuOrderingControllerService.prototype.UnhighlightOriginalItem = function (item) {
                    $(item).css({ "border-width": "0px" });
                };
                MenuOrderingControllerService.prototype.initListViewEvents = function () {
                    var internal = this;
                    var ds = internal.menuService.menuItemService.dataSource;
                    var draggableSettings = {
                        filter: ".menu-item",
                        hint: internal.generateHint,
                        group: "listViewItems",
                        //container: "html",
                        holdToDrag: true,
                        hold: function (e) {
                            if (e.which === 2) {
                                e.preventDefault();
                            }
                            internal.highlighOriginalItem(e.currentTarget);
                        },
                        dragcancel: function (e) {
                            internal.UnhighlightOriginalItem(e.currentTarget);
                        },
                        dragstart: function (e) {
                        },
                        dragend: function (e) {
                            internal.UnhighlightOriginalItem(e.currentTarget);
                        },
                        drag: function (e) {
                        }
                    };
                    $(internal.options.ids.listViewId).kendoDraggable(draggableSettings);
                    var dropTargetAreaOptions = {
                        filter: ".menu-item",
                        group: "listViewItems",
                        hint: internal.generateHint,
                        cancel: internal.destroyDraggable,
                        dragenter: function (e) {
                            var $e = e.dropTarget;
                            //if ($e.is(".moving")) { return; }
                            $($e).css({ "border-color": "#A5E400" });
                            $($e).animate({
                                "margin-left": "40px",
                                "opacity": 0.9,
                                "border-width": 4
                            }, 200);
                        },
                        dragleave: function (e) {
                            var $e = e.dropTarget;
                            $($e).animate({
                                "margin-left": "0px",
                                "opacity": 1,
                                "border-width": 0
                            }, 200);
                        },
                        drop: function (e) {
                            var draggableThing = e.draggable;
                            var hint = draggableThing.hint;
                            var draggableDataItem = ds.getByUid(hint.data("uid")), //ds.getByUid(e.draggable.hint.data("uid")),
                            dropTargetDataItem = ds.getByUid(e.dropTarget.data("uid"));
                            internal.movePlaces(dropTargetDataItem, draggableDataItem, Positioning.BEFORE);
                            internal.normalize();
                            ds.sort({ field: "WebSequence", dir: "asc" });
                            internal.menuService.menuItemService.dataSource.sync();
                        }
                    };
                    $(internal.options.ids.listViewId).kendoDropTargetArea(dropTargetAreaOptions);
                };
                /*todo write a handler to allow the page to scroll with drag */
                MenuOrderingControllerService.prototype.movePage = function (mouseMoveEvent) {
                    var m = mouseMoveEvent;
                };
                MenuOrderingControllerService.prototype.movePlaces = function (target, moveItem, switchPosition) {
                    switchPosition || (switchPosition = Positioning.BEFORE);
                    var internal = this, ds = internal.menuService.menuItemService.dataSource, items = ds.view(), max = items.length - 1;
                    var draggedItem = { id: moveItem.id, item: moveItem, webSesquence: moveItem.get("WebSequence"), position: items.indexOf(moveItem) };
                    var droppedOnItem = { id: target.id, item: target, webSesquence: target.get("WebSequence"), position: items.indexOf(target) };
                    var switchPositions = draggedItem.position - droppedOnItem.position;
                    switchPositions *= switchPositions;
                    if (switchPositions === 1) {
                        moveItem.set("WebSequence", droppedOnItem.webSesquence);
                        target.set("WebSequence", draggedItem.webSesquence);
                    }
                    else if (droppedOnItem.position === 0) {
                        //move item to position 1
                        var moveItemPosition = droppedOnItem.webSesquence / 2;
                        moveItem.set("WebSequence", moveItemPosition);
                    }
                    else if (droppedOnItem.position === max) {
                        //move item before or after last position
                        var moveItemPosition = 0;
                        switch (switchPosition) {
                            case Positioning.BEFORE:
                                moveItemPosition = droppedOnItem.webSesquence + items[droppedOnItem.position - 1];
                                break;
                            case Positioning.INTO:
                                moveItemPosition = droppedOnItem.webSesquence * 2 + 100;
                                break;
                            case Positioning.AFTER:
                                moveItemPosition = droppedOnItem.webSesquence * 2 + 100;
                                break;
                        }
                        moveItemPosition = moveItemPosition / 2;
                        draggedItem.item.set("WebSequence", moveItemPosition);
                    }
                    else {
                        var fitBetweenItem = xIndex === 0 ? target : items[droppedOnItem.position - 1];
                        if (Positioning.INTO === switchPosition && draggedItem.position < droppedOnItem.position) {
                            fitBetweenItem = droppedOnItem.position === 0 ? droppedOnItem.item : items[droppedOnItem.position + 1];
                        }
                        //if (Positioning.INTO === switchPosition && draggedItem.position > droppedOnItem.position)
                        //{
                        //    fitBetweenItem = droppedOnItem.position === 0 ? droppedOnItem.item : items[droppedOnItem.position - 1]
                        //}
                        var xIndex = droppedOnItem.position;
                        var zPosition = droppedOnItem.webSesquence + fitBetweenItem.get("WebSequence");
                        zPosition = zPosition / 2;
                        draggedItem.item.set("WebSequence", zPosition);
                    }
                };
                MenuOrderingControllerService.prototype.normalize = function () {
                    var internal = this, ds = internal.menuService.menuItemService.dataSource, view = ds.view();
                    var start = 100;
                    var linq = Enumerable.from(view);
                    var group = linq
                        .groupBy(function (x) { return x.Name; }, function (x) { return x; }, function (key, result) {
                        return {
                            key: key,
                            items: result.toArray()
                        };
                    })
                        .toArray();
                    group.forEach(function (item) {
                        //group should end up in the same order as the first element found for each key. 
                        //ergo we can apply it sequentially. 
                        item.items.forEach(function (menuItem) {
                            menuItem.set("WebSequence", start);
                        });
                        start += 100;
                    });
                    //view.forEach((item: any, index, source) => {
                    //    item.set("WebSequence", start);
                    //    start += 100;
                    //});
                    internal.positionWindowViewModel.trigger("change");
                    ds.trigger("change");
                };
                MenuOrderingControllerService.prototype.closeWindow = function (saveChanges) {
                    var kendoWindow = $("#positionWindow").data("kendoWindow");
                    var ds = this.menuService.menuItemService.dataSource;
                    if (saveChanges) {
                        ds.sync();
                    }
                    else {
                        ds.cancelChanges();
                    }
                    kendoWindow.close();
                };
                MenuOrderingControllerService.prototype.displayWindow = function () {
                    var internal = this, wrapper = $("#positionWindowWrapper");
                    kendo.bind(wrapper, this.positionWindowViewModel);
                    $(internal.options.ids.listViewId).on("click", ".k-button-edit-position", function (e) {
                        e.preventDefault();
                        var itemId = $(this).closest(".menu-item").data("id"), item = internal.menuService.menuItemService.findById(itemId), items = internal.menuService.menuItemService.dataSource.view(), vm = internal.positionWindowViewModel;
                        var kendoWindows = $("#positionWindow").data("kendoWindow");
                        vm.set("item", item);
                        vm.set("itemName", item.DisplayName());
                        vm.set("itemIndex", item.Index());
                        vm.set("length", items.length);
                        vm.set("index", item.Index());
                        kendoWindows.open();
                        kendoWindows.center();
                    });
                };
                MenuOrderingControllerService.prototype.generateHint = function (element) {
                    var hint = element.clone();
                    hint.css({
                        "width": element.width(),
                        "height": element.height()
                    });
                    $(hint).css({ "border-color": "#600BA2", "border-width": "4px" });
                    $(element).animate({ "opacity": 0.8 });
                    //return hint;
                };
                MenuOrderingControllerService.prototype.initHubChanges = function () {
                    var internal = this, hub = MyAndromeda.Hubs.StoreHub.GetInstance(internal.options.routeParameters);
                    hub.bind(MyAndromeda.Hubs.StoreHub.MenuItemChangeEvent, function (context) {
                        console.log("i have changes :)");
                        console.log(context);
                        internal.menuService.extendMenuItemData(context.EditedItems);
                        internal.menuService.menuItemService.updateItems(context.EditedItems, true);
                        internal.menuService.menuItemService.dataSource.sort({ field: "WebSequence", dir: "asc" });
                    });
                };
                MenuOrderingControllerService.prototype.destroyDraggable = function (e) {
                    var o = this;
                    e.currentTarget.css("opacity", 1);
                    e.currentTarget.removeClass("draggable");
                    o.destroy();
                };
                MenuOrderingControllerService.prototype.init = function () {
                    Services.menuFilterController.RESETFILTER = [{ field: "DisplayCategoryId", operator: "eq", value: -19232131 }];
                    Services.menuFilterController.SORTFILTER = [{ field: "WebSequence", dir: "asc" }];
                    this.menuService.init();
                    this.menuService.menuItemService.dataSource.pageSize(1000);
                    this.initListView();
                    this.initListViewEvents();
                    this.initHubChanges();
                    this.displayWindow();
                };
                return MenuOrderingControllerService;
            })();
            Services.MenuOrderingControllerService = MenuOrderingControllerService;
        })(Services = Menu.Services || (Menu.Services = {}));
    })(Menu = MyAndromeda.Menu || (MyAndromeda.Menu = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var Menu;
    (function (Menu) {
        var Services;
        (function (Services) {
            Menu.Angular.ServicesInitilizations.push(function (app) {
                app.factory(MenuService.Name, [
                    function () {
                        //var instance = new MenuService();
                        //return instance;
                    }
                ]);
            });
            var MenuService = (function () {
                function MenuService(options) {
                    this.enableDetailMenuEditing = false;
                    this.options = options;
                }
                MenuService.prototype.setupFilterHelper = function () {
                    this.menuFilterHelper = new Services.menuFilterController(this.options.filterIds, this.menuItemService.dataSource);
                };
                MenuService.prototype.createCategoryService = function (category1, category2, displayCategories) {
                    this.displayCategoriesService = new Services.CategoryService(displayCategories);
                    this.category1Service = new Services.CategoryService(category1);
                    this.category2Service = new Services.CategoryService(category2);
                    //pass along the three services to the filter helper
                    this.menuFilterHelper.init(this.displayCategoriesService, this.category1Service, this.category2Service);
                };
                MenuService.prototype.extendMenuItemData = function (data) {
                    var internal = this;
                    for (var i = 0; i < data.length; i++) {
                        var menuItem = data[i];
                        //look up names 
                        var category1 = this.category1Service.findById(menuItem.CategoryId1);
                        var category2 = this.category2Service.findById(menuItem.CategoryId2);
                        menuItem.isNew = function () { return false; };
                        menuItem.Category1Name = category1 ? category1.Name : "";
                        menuItem.Category2Name = category2 ? category2.Name : "";
                        menuItem.DisplayName = function () {
                            var self = this, //observable 
                            webName = self.get("WebName"), name = self.get("Name");
                            if (webName && webName.length > 0) {
                                return webName;
                            }
                            else {
                                return name;
                            }
                        };
                        menuItem.CanEditNameAndDescription = function () {
                            return internal.enableDetailMenuEditing;
                        };
                        menuItem.ShowCantEditMessage = function () {
                            return !this.CanEditNameAndDescription();
                        };
                        menuItem.WebNameCount = function () {
                            var name = this.get("WebName");
                            var length = 0;
                            if (name && name.length && name.length > 0) {
                                length = name.length;
                            }
                            return kendo.format("{0}/{1} characters used", length, 255);
                        };
                        menuItem.WebDescriptionCount = function () {
                            var description = this.get("WebDescription");
                            var length = 0;
                            if (description && description.length && description.length > 0) {
                                length = description.length;
                            }
                            return kendo.format("{0}/{1} characters used", length, 255);
                        };
                        menuItem.Index = function () {
                            return internal.menuItemService.dataSource.view().indexOf(this) + 1;
                        };
                        menuItem.ColorStatus = function () {
                            if (this.dirty) {
                                return "#F29A00";
                            }
                            return "#A4E400";
                        };
                        menuItem.ClearWebName = function () {
                            var self = this;
                            self.set("WebName", "");
                        };
                        menuItem.ClearDescription = function () {
                            var self = this;
                            self.set("WebDescription", "");
                        };
                        if (menuItem.Thumbs == null) {
                            menuItem.Thumbs = new kendo.data.ObservableArray([]);
                        }
                    }
                };
                MenuService.prototype.initAppDataSource = function () {
                    var internal = this;
                    var t = {};
                    var dataSource = new kendo.data.DataSource({
                        batch: true,
                        filter: Services.menuFilterController.RESETFILTER,
                        sort: Services.menuFilterController.SORTFILTER,
                        transport: {
                            read: function (options) {
                                var op = $.ajax({
                                    url: Menu.Settings.Routes.MenuItems.ListMenuItems,
                                    dataType: "json",
                                    type: "POST"
                                });
                                op.done(function (result) {
                                    //create the category data sources
                                    var c1 = result.Categories1, c2 = result.Categories2, display = result.DisplayCategories;
                                    internal.enableDetailMenuEditing = result.DetailEditable;
                                    internal.createCategoryService(c1, c2, display);
                                    //extend the JSON items 
                                    internal.extendMenuItemData(result.MenuItems);
                                    //get the data source to continue is daily, more usual life 
                                    options.success(result);
                                });
                                op.fail(function (result) {
                                    options.error(result);
                                });
                            },
                            update: function (options) {
                                var data = options.data, 
                                //find the related menu items
                                menuItem = null, //internal.menuItemService.findById(data.Id),
                                similarItems = null; //internal.menuItemService.getRelatedItems([menuItem]),
                                kendo.ui.progress($("body"), true);
                                var jData = JSON.stringify(data);
                                var promise = $.ajax({
                                    url: Menu.Settings.Routes.MenuItems.SaveMenuItems,
                                    data: jData,
                                    type: "POST",
                                    dataType: "json",
                                    contentType: "application/json; charset=utf-8"
                                });
                                promise.done(function (result, textStatus, jqXHR) {
                                    options.success(result, textStatus, jqXHR);
                                    //update additional items ? 
                                    //many items should have been updated with options.success but some reason isn't ... manual update.
                                    //internal.menuItemService.updateItems(result, false);
                                    kendo.ui.progress($("body"), false);
                                });
                                promise.fail(function (jqXHR, textStatus, errorThrown) {
                                    //options.error(data, textStatus, jqXHR);
                                    alert("There was a an error saving your request. Please try again");
                                    kendo.ui.progress($("body"), false);
                                });
                            }
                        },
                        schema: {
                            data: function (data) {
                                return data.MenuItems;
                            },
                            total: function (data) {
                                return data.MenuItems.length;
                            },
                            errors: "Errors",
                            model: {
                                id: "Id",
                                fields: {
                                    WebName: {
                                        validation: {}
                                    },
                                    WebDescription: {
                                        validation: {
                                            maxLength: function (context) {
                                                var $i = $(context);
                                                if ($i.is("[name=WebDescription]") || $i.is("[name=WebName]")) {
                                                    var currentLenght = $i.val().length;
                                                    $i.attr("data-maxlength-msg", kendo.format(Services.MenuControllerService.MaxLengthMessage, 255, currentLenght));
                                                    return $i.val().length <= 255;
                                                }
                                                return true;
                                            }
                                        }
                                    }
                                },
                                Enable: function () {
                                    Menu.Logger.Notify("Enable");
                                    this.set("Enabled", true);
                                },
                                Disable: function () {
                                    Menu.Logger.Notify("Disable");
                                    this.set("Enabled", false);
                                }
                            }
                        },
                        serverPaging: false,
                        serverFiltering: false,
                        serverSorting: false,
                        pageSize: 10
                    });
                    this.menuItemService = new Services.MenuItemService(dataSource);
                };
                MenuService.prototype.init = function () {
                    //setup own data source
                    this.initAppDataSource();
                    //setup filters
                    this.setupFilterHelper();
                };
                MenuService.Name = "MenuService";
                return MenuService;
            })();
            Services.MenuService = MenuService;
        })(Services = Menu.Services || (Menu.Services = {}));
    })(Menu = MyAndromeda.Menu || (MyAndromeda.Menu = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var Menu;
    (function (Menu) {
        var Services;
        (function (Services) {
            var MenuStatus = (function () {
                function MenuStatus(dataSource) {
                    this.dataSource = dataSource;
                    this.observable = kendo.observable({
                        Message: MenuStatus.EditedMessage,
                        HasChanges: false,
                        CanPublish: false,
                        SaveChanges: function () {
                            dataSource.sync();
                        },
                        CancelChanges: function () {
                            if (confirm("All changes will be lost, continue?")) {
                                dataSource.cancelChanges();
                            }
                        }
                    });
                    this.intitDataSource();
                    this.initPageEvents();
                }
                MenuStatus.prototype.intitDataSource = function () {
                    var internal = this;
                    this.dataSource.bind("change", function () {
                        internal.observable.set(MenuStatus.HasChangesField, internal.dataSource.hasChanges());
                        internal.observable.set(MenuStatus.CanPublishField, !internal.dataSource.hasChanges());
                    });
                };
                MenuStatus.prototype.initPageEvents = function () {
                    var _this = this;
                    $(window).on("beforeunload", function (e) {
                        if (_this.dataSource.hasChanges()) {
                            //return MenuStatus.LeaveMesssage;
                            e.preventDefault();
                        }
                    });
                };
                MenuStatus.Name = "MenuStatusService";
                MenuStatus.LeaveMesssage = "There are unsaved changes to the menu. Are you sure you want to leave?";
                MenuStatus.EditedMessage = "There are unsaved changes to the menu. Don't forget to save changes as you go along.";
                MenuStatus.HasChangesField = "HasChanges";
                MenuStatus.CanPublishField = "CanPublish";
                MenuStatus.MessageField = "Message";
                return MenuStatus;
            })();
            Services.MenuStatus = MenuStatus;
        })(Services = Menu.Services || (Menu.Services = {}));
    })(Menu = MyAndromeda.Menu || (MyAndromeda.Menu = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var Menu;
    (function (Menu) {
        var Services;
        (function (Services) {
            Menu.Angular.ServicesInitilizations.push(function (app) {
                app.factory(PublishingService.Name, [
                    function () {
                        return new PublishingService({
                            publishPanel: {
                                mainButtonId: "#doesnotexist",
                                publishPanelId: "#publishPanel",
                                publishUrlPath: Menu.Settings.Routes.Publish
                            }
                        }, null);
                    }
                ]);
            });
            var PublishingService = (function () {
                function PublishingService(options, menuservice) {
                    var _this = this;
                    var internal = this;
                    this.options = options;
                    this.menuService = menuservice;
                    this.model = kendo.observable({
                        publishThumbnails: true,
                        publishMenu: true,
                        publishNow: true,
                        publishLater: false,
                        publishOn: new Date(),
                        minDate: new Date(),
                        cancel: function () {
                            $(options.publishPanel.publishPanelId).kendoMobileModalView("close");
                        },
                        publishNowClick: function () {
                            internal.publishNow(true, true, new Date());
                        },
                        publishLaterClick: function () {
                            var publishThumbs = internal.model.get("publishThumbnails"), publishMenu = internal.model.get("publishMenu"), publishOn = internal.model.get("publishOn");
                            internal.publishNow(publishMenu, publishThumbs, publishOn);
                        },
                        publishThumbnailsClick: function () {
                            internal.publishNow(false, true, new Date());
                        }
                    });
                    this.model.bind("change", function (e) {
                        if (e.field !== 'publishNow')
                            return;
                        var now = _this.model.get("publishNow");
                        _this.model.set("publishLater", now === "later");
                    });
                    this.init();
                }
                PublishingService.prototype.publishNow = function (menu, thumbnails, date) {
                    var internal = this, data = {
                        menu: menu,
                        thumbnails: thumbnails,
                        dateUtc: null
                    };
                    if (this.menuService !== null && this.menuService.menuItemService !== null && this.menuService.menuItemService.anyDirtyItems()) {
                        internal.closeWindow();
                        alert("Please save the items before publish");
                        return;
                    }
                    if (date > new Date()) {
                        //toJson will change a local date time to UTC format that is accepted by the json parser in .NET
                        data.dateUtc = date.toJSON();
                    }
                    $.ajax({
                        url: internal.options.publishPanel.publishUrlPath,
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        cache: false,
                        data: JSON.stringify(data),
                        success: function (data) {
                            internal.closeWindow();
                        }
                    });
                };
                PublishingService.prototype.openWindow = function () {
                    var options = this.options.publishPanel;
                    $(options.publishPanelId).kendoMobileModalView("open");
                };
                PublishingService.prototype.closeWindow = function () {
                    var options = this.options.publishPanel;
                    $(options.publishPanelId).kendoMobileModalView("close");
                };
                PublishingService.prototype.init = function () {
                    var internal = this, options = this.options.publishPanel;
                    $(options.publishPanelId).kendoMobileModalView();
                    kendo.bind(options.publishPanelId, this.model);
                    $(options.mainButtonId).on("click", function (e) {
                        e.preventDefault();
                        internal.openWindow();
                    });
                };
                PublishingService.Name = "PublishingService";
                return PublishingService;
            })();
            Services.PublishingService = PublishingService;
        })(Services = Menu.Services || (Menu.Services = {}));
    })(Menu = MyAndromeda.Menu || (MyAndromeda.Menu = {}));
})(MyAndromeda || (MyAndromeda = {}));
/// <reference path="../../scripts/typings/rx/rx.all.d.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var Menu;
    (function (Menu) {
        var Services;
        (function (Services) {
            Menu.Angular.ServicesInitilizations.push(function (app) {
                app.factory(MenuToppingsFilterService.Name, [
                    Services.MenuToppingsService.Name,
                    function (menuToppingsService) {
                        var instnance = new MenuToppingsFilterService(menuToppingsService);
                        return instnance;
                    }
                ]);
            });
            var MenuToppingsFilterService = (function () {
                function MenuToppingsFilterService(menuToppingsService) {
                    var _this = this;
                    this.menuToppingsService = menuToppingsService;
                    this.dataSource = this.menuToppingsService.GetDataSource();
                    this.ResetFiltersObservable = new Rx.Subject();
                    this.model = {
                        Name: "",
                        ResetFilters: function () { _this.ResetFilters(); }
                    };
                }
                MenuToppingsFilterService.prototype.ChangeNameFilter = function (name) {
                    this.model.Name = name;
                    this.Filter();
                };
                MenuToppingsFilterService.prototype.GetName = function () {
                    return this.model.Name;
                };
                MenuToppingsFilterService.prototype.GetNameFilter = function () {
                    var filter = {
                        field: "Name",
                        operator: "contains",
                        value: this.model.Name
                    };
                    return filter;
                };
                MenuToppingsFilterService.prototype.Filter = function () {
                    var filters = [];
                    if (this.model.Name !== "") {
                        var nameFilter = this.GetNameFilter();
                        filters.push(nameFilter);
                    }
                    this.dataSource.filter(filters);
                };
                MenuToppingsFilterService.prototype.ResetFilters = function () {
                    this.dataSource.filter([]);
                    this.ResetFiltersObservable.onNext(true);
                };
                MenuToppingsFilterService.Name = "MenuToppingsFilterService ";
                return MenuToppingsFilterService;
            })();
            Services.MenuToppingsFilterService = MenuToppingsFilterService;
        })(Services = Menu.Services || (Menu.Services = {}));
    })(Menu = MyAndromeda.Menu || (MyAndromeda.Menu = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var Menu;
    (function (Menu) {
        var Services;
        (function (Services) {
            Menu.Angular.ServicesInitilizations.push(function (app) {
                app.factory(MenuToppingsService.Name, [
                    function () {
                        var instnance = new MenuToppingsService();
                        return instnance;
                    }
                ]);
            });
            var MenuToppingsService = (function () {
                function MenuToppingsService() {
                }
                MenuToppingsService.prototype.GetDataSource = function () {
                    var internal = this;
                    if (this.dataSource) {
                        return this.dataSource;
                    }
                    var routes = Menu.Settings.Routes.Toppings;
                    var dataSourceGroup = [
                        { field: "Name", dir: "" }
                    ];
                    this.dataSource = new kendo.data.DataSource({
                        type: "aspnetmvc-ajax",
                        batch: true,
                        pageSize: 10,
                        //group: dataSourceGroup,
                        transport: {
                            read: { url: routes.List },
                            update: { url: routes.Update }
                        },
                        schema: {
                            data: "Data",
                            total: "Total",
                            errors: "Errors",
                            model: {
                                id: "Id",
                                UpdateAllDelivery: function (confirm) {
                                    var item = this;
                                    item.ToppingVarients.forEach(function (varient) {
                                        var c = item.get("CollectionPrice"), d = item.get("DeliveryPrice"), s = item.get("DineInPrice");
                                        varient.set("DeliveryPrice", d);
                                        varient.trigger("change");
                                    });
                                    item.trigger("change");
                                },
                                UpdateAllCollection: function (confirm) {
                                    var item = this;
                                    item.ToppingVarients.forEach(function (varient) {
                                        var c = item.get("CollectionPrice");
                                        varient.set("CollectionPrice", c);
                                        varient.trigger("change");
                                    });
                                },
                                UpdateAllDineIn: function (confirm) {
                                    var item = this;
                                    item.ToppingVarients.forEach(function (varient) {
                                        var s = item.get("DineInPrice");
                                        varient.set("DineInPrice", s);
                                        varient.trigger("change");
                                    });
                                },
                                UpdateAllToppingPrices: function (confirm) {
                                    var item = this;
                                    console.log(item);
                                    item.ToppingVarients.forEach(function (varient) {
                                        var c = item.get("CollectionPrice"), d = item.get("DeliveryPrice"), s = item.get("DineInPrice");
                                        varient.set("CollectionPrice", c);
                                        varient.set("DeliveryPrice", d);
                                        varient.set("DineInPrice", s);
                                        varient.trigger("change");
                                    });
                                    item.trigger("change");
                                },
                                ColorStatus: function () {
                                    var item = this;
                                    if (this.dirty) {
                                        return "#F29A00";
                                    }
                                    return "#A4E400";
                                }
                            }
                        }
                    });
                    return this.dataSource;
                };
                MenuToppingsService.prototype.FindTopppings = function (predicate) {
                    var data = this.dataSource.data();
                    var filtered = data.filter(predicate);
                    return filtered;
                };
                MenuToppingsService.Name = "ToppingsService";
                return MenuToppingsService;
            })();
            Services.MenuToppingsService = MenuToppingsService;
        })(Services = Menu.Services || (Menu.Services = {}));
    })(Menu = MyAndromeda.Menu || (MyAndromeda.Menu = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var Menu;
    (function (Menu) {
        var Settings;
        (function (Settings) {
            var Routes = (function () {
                function Routes() {
                }
                Routes.Toppings = {
                    List: "",
                    Update: ""
                };
                Routes.MenuItems = {
                    ListMenuItems: "",
                    SaveMenuItems: "",
                    SaveImageUrl: "",
                    RemoveImageUrl: "",
                };
                Routes.Ftp = {
                    DownloadMenu: "",
                    UploadMenu: "",
                    Version: "",
                    Delete: ""
                };
                Routes.Publish = "";
                return Routes;
            })();
            Settings.Routes = Routes;
            ;
            ;
            ;
        })(Settings = Menu.Settings || (Menu.Settings = {}));
    })(Menu = MyAndromeda.Menu || (MyAndromeda.Menu = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var Reporting;
    (function (Reporting) {
        var Models;
        (function (Models) {
            var Result = (function () {
                function Result(key, data) {
                    this.key = key;
                    if (data instanceof kendo.data.DataSource) {
                        this.data = data;
                    }
                    else {
                        this.CreateDataSource(data);
                    }
                }
                Result.prototype.CreateDataSource = function (data) {
                    var model = { data: [] };
                    if (!data) {
                        throw new Error("Data is missing");
                    }
                    if (data.length) {
                        model.data = data;
                    }
                    else if (data.data) {
                        //suggests that data is a data source scaffold instead. 
                        model = data;
                    }
                    this.data = new kendo.data.DataSource(model);
                };
                Result.prototype.Bind = function (eventName, handler) {
                    this.data.bind(eventName, handler);
                };
                return Result;
            })();
            Models.Result = Result;
        })(Models = Reporting.Models || (Reporting.Models = {}));
    })(Reporting = MyAndromeda.Reporting || (MyAndromeda.Reporting = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var Reporting;
    (function (Reporting) {
        var Services;
        (function (Services) {
            var chainDailySummaryService = (function () {
                function chainDailySummaryService(options) {
                    this.loadDelay = 250;
                    this.options = options;
                    var internal = this;
                    this.graphs = [];
                }
                chainDailySummaryService.prototype.generateGridChartActions = function () {
                    var internal = this, grid = $(this.options.gridElement), dataSource = this.getDataSource(), selectedTabItem = internal.getSelctedTab();
                    if (!this.isSelectedTab() || this.loaded)
                        return;
                    internal.graphs.length = 0;
                    $(internal.options.gridElement).find(".chain-sparkline").each(function (index, element) {
                        var e = $(element);
                        var uid = e.closest("tr").data("uid");
                        var dataRow = dataSource.getByUid(uid);
                        var dataSet = dataRow;
                        var data = dataRow.Data;
                        if (data.length == 0 || data.length == 1) {
                            return;
                        }
                        var descriptiveData = new kendo.data.DataSource({
                            data: data,
                            schema: {
                                model: {
                                    fields: {
                                        "Date": { "type": "date" }
                                    }
                                }
                            }
                        });
                        var action = null;
                        var showType = e.data("rep");
                        if (showType === "sales") {
                            action = function () {
                                internal.generateGridCharts(e, "Total Sales", "Combined.Sales", "area", descriptiveData, "#: kendo.toString(value/100, 'c') #");
                            };
                        }
                        if (showType === "orders") {
                            action = function () {
                                internal.generateGridCharts(e, "Total Orders", "Combined.OrderCount", "column", descriptiveData, "#: value #");
                            };
                        }
                        if (showType === "avgSpend") {
                            action = function () {
                                internal.generateGridCharts(e, "Avg Spend", "Combined.AvgSale", "area", descriptiveData, "#: kendo.toString(value/100, 'c') #");
                            };
                        }
                        if (showType === "otd") {
                            action = function () {
                                internal.generateGridCharts(e, "OTD", "Performance.AvgOutTheDoor", "line", descriptiveData, "#: value #");
                            };
                        }
                        if (showType === "ttd") {
                            action = function () {
                                internal.generateGridCharts(e, "TTD", "Performance.AvgDoorTime", "line", descriptiveData, "#: value #");
                            };
                        }
                        internal.graphs.push(action);
                    });
                    if (internal.graphs.length > 0) {
                        if (internal.graphs.length > 25) {
                            internal.graphs.forEach(function (act) {
                                setTimeout(function () { act(); }, internal.loadDelay);
                            });
                        }
                        else {
                            internal.graphs.forEach(function (act) {
                                act();
                            });
                        }
                    }
                    this.loaded = true;
                };
                chainDailySummaryService.prototype.generateGridCharts = function (element, name, field, type, data, format) {
                    var e = $(element);
                    format || (format = "#: value #");
                    e.kendoSparkline({
                        theme: "bootstrap",
                        renderAs: kendo.support.mobileOS ? "canvas" : "svg",
                        series: [{
                                name: name, field: field, type: type
                            }],
                        dataSource: data,
                        tooltip: {
                            template: "<div>#: kendo.toString(dataItem.Date, 'd')# (#: kendo.toString(dataItem.Date, 'ddd') #)</div><div>" + format + "</div>"
                        },
                        chartArea: {
                            background: "transparent"
                        }
                    });
                };
                chainDailySummaryService.prototype.getGrid = function () {
                    return $(this.options.gridElement).data("kendoGrid");
                };
                chainDailySummaryService.prototype.getDataSource = function () {
                    var grid = this.getGrid(), dataSource = grid.dataSource;
                    return dataSource;
                };
                chainDailySummaryService.prototype.getTabStrip = function () {
                    var tabStrip = $(this.options.tabStripElement).data("kendoTabStrip");
                    return tabStrip;
                };
                chainDailySummaryService.prototype.getSelctedTab = function () {
                    var selectedElement = $(this.options.tabStripElement).find(".k-tabstrip-items").find(".k-state-active");
                    var name = selectedElement.data("tabName");
                    return name;
                };
                chainDailySummaryService.prototype.isSelectedTab = function () {
                    return this.getSelctedTab() === this.options.tabStripItemName;
                };
                chainDailySummaryService.prototype.runJob = function () {
                    this.graphs.forEach(function (value) {
                        value();
                    });
                };
                chainDailySummaryService.prototype.init = function () {
                    $(this.options.elementId).data("Reporting.chainDailySummaryService", this);
                    var internal = this;
                    var grid = this.getGrid();
                    grid.bind("detailInit", function (e) {
                        var actions = e.detailRow.find(".actions");
                        var content = e.detailRow.find(".content");
                        var rowData = e.data;
                        var detailService = new Services.chainGridDetailSumarryService(internal, content, actions, rowData);
                    });
                    grid.dataSource.bind("change", function () {
                        console.log("datasource change");
                        if (internal.loaded) {
                            internal.loadDelay = 0;
                        }
                        internal.loaded = false;
                    });
                    grid.bind("dataBound", $.proxy(internal.generateGridChartActions, internal));
                    var tabStrip = this.getTabStrip();
                    tabStrip.bind("activate", function (e) {
                        if (internal.isSelectedTab()) {
                            //$.proxy(internal.runJob, internal);
                            //console.log("run: " + internal.getSelctedTab()); 
                            internal.generateGridChartActions();
                        }
                    });
                };
                chainDailySummaryService.prototype.fetch = function () {
                };
                return chainDailySummaryService;
            })();
            Services.chainDailySummaryService = chainDailySummaryService;
        })(Services = Reporting.Services || (Reporting.Services = {}));
    })(Reporting = MyAndromeda.Reporting || (MyAndromeda.Reporting = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var Reporting;
    (function (Reporting) {
        var Services;
        (function (Services) {
            var chainGridDetailSumarryService = (function () {
                function chainGridDetailSumarryService(parent, contentElement, actionsElement, data) {
                    this.parent = parent;
                    this.contentElement = contentElement;
                    this.actionsElement = actionsElement;
                    this.data = data;
                    this.url = parent.options.hourlyUrl.replace("EXTERNALSITEID", this.data.ExternalSiteId);
                    this.contentViewModel = kendo.observable({
                        Data: null
                    });
                    this.setupActionsViewModel();
                    this.setupContentViewModel();
                }
                chainGridDetailSumarryService.prototype.setupActionsViewModel = function () {
                    var internal = this;
                    this.actionsViewModel = kendo.observable({
                        visibleSales: true,
                        visibleOrderCount: true,
                        visiblePerformance: true
                    });
                    this.actionsViewModel.bind("change", function (e) {
                        //console.log("ive changed");
                        internal.onActionsChange(e);
                        //$.proxy(internal.onActionsChange, internal)
                    });
                    kendo.bind(this.actionsElement, this.actionsViewModel);
                };
                chainGridDetailSumarryService.prototype.setupContentViewModel = function () {
                    var grid = this.parent.getGrid(), dataSource = this.parent.getDataSource();
                    if (this.data.Data && this.data.Data.length > 1) {
                        this.setupContentViewModelFromExisting();
                    }
                    else {
                        this.LoadHourly();
                    }
                    kendo.bind(this.contentElement, this.contentViewModel);
                };
                chainGridDetailSumarryService.prototype.setupContentViewModelFromExisting = function () {
                    var chartData = this.data.Data;
                    var dataSource = new kendo.data.DataSource({
                        data: chartData,
                        schema: {
                            model: {
                                fields: {
                                    "Date": { "type": "date" }
                                }
                            }
                        }
                    });
                    this.contentViewModel.set("Data", dataSource);
                };
                chainGridDetailSumarryService.prototype.LoadHourly = function () {
                    var hourlyDataSource = new kendo.data.DataSource({
                        transport: {
                            read: {
                                data: {},
                                type: "POST",
                                dataType: "json",
                                url: this.url
                            }
                        },
                        schema: {
                            model: {
                                fields: {
                                    "Date": { type: "date" }
                                }
                            },
                            data: function (result) {
                                return result.Data;
                            },
                            total: function (result) {
                                return result.Data.length;
                            }
                        }
                    });
                    this.contentViewModel.set("Data", hourlyDataSource);
                    hourlyDataSource.read();
                };
                chainGridDetailSumarryService.prototype.onActionsChange = function (e) {
                    //console.log("ive changed");
                    var chart = this.getGridDetailChart(this.contentElement);
                    var series = chart.options.series;
                    var valueAxis = chart.options.valueAxis;
                    series.forEach(function (seriesSet) {
                        var a = seriesSet;
                        if (e.field === "visibleOrderCount" && seriesSet.axis === "orderCount") {
                            a.visible = !a.visible;
                        }
                        if (e.field === "visibleSales" && seriesSet.axis === "sales") {
                            a.visible = !a.visible;
                        }
                        if (e.field === "visiblePerformance" && (seriesSet.axis === "otd" || seriesSet.axis === "ttd")) {
                            a.visible = !a.visible;
                        }
                    });
                    valueAxis.forEach(function (axis) {
                        if (e.field === "visibleOrderCount" && axis.name === "orderCount") {
                            axis.visible = !axis.visible;
                        }
                        if (e.field === "visibleSales" && axis.name === "sales") {
                            axis.visible = !axis.visible;
                        }
                        if (e.field === "visiblePerformance" && (axis.name === "otd" || axis.name === "ttd")) {
                            axis.visible = !axis.visible;
                        }
                    });
                    chart.redraw();
                };
                chainGridDetailSumarryService.prototype.getGridDetailChart = function (content) {
                    var contentChart = $(content).find(".k-chart-detail").data("kendoChart");
                    return contentChart;
                };
                return chainGridDetailSumarryService;
            })();
            Services.chainGridDetailSumarryService = chainGridDetailSumarryService;
        })(Services = Reporting.Services || (Reporting.Services = {}));
    })(Reporting = MyAndromeda.Reporting || (MyAndromeda.Reporting = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var Reporting;
    (function (Reporting) {
        var Services;
        (function (Services) {
            var dailySummaryDataService = (function () {
                function dailySummaryDataService(options) {
                    this.options = options;
                    this.initDataSource();
                }
                dailySummaryDataService.prototype.initDataSource = function () {
                    var internal = this;
                    this.dataSource = new kendo.data.DataSource({
                        transport: {
                            read: {
                                url: internal.options.url,
                                data: internal.options.data,
                                type: "POST",
                                dataType: "json"
                            }
                        },
                        schema: {
                            model: {
                                fields: {
                                    "Date": { "type": "date" }
                                }
                            },
                            data: function (result) {
                                return $.map(result.Data, function (element, index) {
                                    var p = element.Performance.NumOrdersLT30Mins;
                                    var t = element.Combined.OrderCount;
                                    element.Performance.NumOrdersLT30MinsPercentage = p / t;
                                    return element;
                                });
                            },
                            total: function (result) { return result.Data.length; }
                        },
                        aggregate: [
                            { field: "Combined.OrderCount", aggregate: "sum" },
                            { field: "Combined.OrderCount", aggregate: "average" },
                            { field: "Combined.OrderCount", aggregate: "max" },
                            { field: "Combined.OrderCount", aggregate: "min" },
                            { field: "Performance.AvgDoorTime", aggregate: "average" },
                            { field: "Performance.AvgDoorTime", aggregate: "max" },
                            { field: "Performance.AvgDoorTime", aggregate: "min" },
                            { field: "Performance.AvgOutTheDoor", aggregate: "average" },
                            { field: "Performance.AvgOutTheDoor", aggregate: "max" },
                            { field: "Performance.AvgOutTheDoor", aggregate: "min" },
                            { field: "Performance.AvgMakeTime", aggregate: "average" },
                            { field: "Performance.AvgMakeTime", aggregate: "max" },
                            { field: "Performance.AvgMakeTime", aggregate: "min" },
                            { field: "Performance.AvgRackTime", aggregate: "average" },
                            { field: "Performance.AvgRackTime", aggregate: "max" },
                            { field: "Performance.AvgRackTime", aggregate: "min" },
                            { field: "Performance.NumOrdersLT30Mins", aggregate: "sum" },
                            { field: "Performance.NumOrdersLT30Mins", aggregate: "average" }
                        ]
                    });
                };
                dailySummaryDataService.prototype.bind = function (eventName, handler) {
                    this.dataSource.bind(eventName, handler);
                };
                return dailySummaryDataService;
            })();
            Services.dailySummaryDataService = dailySummaryDataService;
        })(Services = Reporting.Services || (Reporting.Services = {}));
    })(Reporting = MyAndromeda.Reporting || (MyAndromeda.Reporting = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var Reporting;
    (function (Reporting) {
        var Services;
        (function (Services) {
            var dailyPerformanceService = (function () {
                function dailyPerformanceService(options) {
                    //console.log("new dailyPerformanceService");
                    this.options = options;
                    this.dashboardSalesService = new Services.dailySummaryDataService(options);
                    this.bound = false;
                }
                dailyPerformanceService.prototype.bindViewModel = function () {
                    //console.log("try bind");
                    if (this.bound) {
                        return;
                    }
                    var element = $(this.options.elementId);
                    //console.log("bind");
                    kendo.bind(element, this.viewModel);
                    this.bound = true;
                };
                dailyPerformanceService.prototype.setupResults = function () {
                    var dataSource = this.dashboardSalesService.dataSource, result = this.dashboardSalesService.result;
                    var orderCount = null, doorTime = null, outDoorTime = null, makeTime = null, rackTime = null, less30Mins = null;
                    try {
                        var aggs = this.dashboardSalesService.dataSource.aggregates();
                        orderCount = aggs["Combined.OrderCount"];
                        doorTime = aggs["Performance.AvgDoorTime"];
                        outDoorTime = aggs["Performance.AvgOutTheDoor"];
                        makeTime = aggs["Performance.AvgMakeTime"];
                        rackTime = aggs["Performance.AvgRackTime"];
                        less30Mins = aggs["Performance.NumOrdersLT30Mins"];
                        less30Mins.percent = function () {
                            var sum = less30Mins.sum;
                            var total = orderCount.sum;
                            return sum / total;
                        };
                    }
                    catch (e) {
                        console.log(e);
                    }
                    this.viewModel = kendo.observable({
                        dataSource: dataSource,
                        orderCount: orderCount,
                        doorTime: doorTime,
                        outDoorTime: outDoorTime,
                        makeTime: makeTime,
                        rackTime: rackTime,
                        less30Mins: less30Mins,
                        showCharts: function () {
                            return dataSource.total() > 1;
                        }
                    });
                    this.bindViewModel();
                    kendo.ui.progress($(this.options.elementId), false);
                };
                dailyPerformanceService.prototype.init = function () {
                    var internal = this;
                    var element = $(this.options.elementId);
                    kendo.ui.progress(element, true);
                    element.data("Reporting.DailySummaryService", this);
                    internal.dashboardSalesService.dataSource.fetch($.proxy(this.setupResults, internal));
                };
                return dailyPerformanceService;
            })();
            Services.dailyPerformanceService = dailyPerformanceService;
        })(Services = Reporting.Services || (Reporting.Services = {}));
    })(Reporting = MyAndromeda.Reporting || (MyAndromeda.Reporting = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var Reporting;
    (function (Reporting) {
        var Services;
        (function (Services) {
            var Filter = (function () {
                function Filter(from, to, dayRange) {
                    this.from = from;
                    this.to = to;
                    this.dayRange = dayRange;
                }
                return Filter;
            })();
            var ReportingSalesResult = (function () {
                function ReportingSalesResult(name, from, to, dayRange, routeData) {
                    this.name = name;
                    this.filter = new Filter(from, to, dayRange);
                    this.routeData = routeData;
                }
                ReportingSalesResult.prototype.InitDataSource = function () {
                    var internal = this;
                    this.dataSource = new kendo.data.DataSource({
                        transport: {
                            read: function (options) {
                                var url = internal.routeData.urlFormat, filter = internal.filter;
                                var destination = kendo.format(url, filter.from.toJSON(), filter.to.toJSON());
                            }
                        },
                        schema: {
                            model: {
                                fields: {
                                    "Day": { "type": "date" },
                                    "Total": { "type": "number" },
                                    "Count": { "type": "number" },
                                    "Average": { "type": "number" },
                                    "Min": { "type": "number" },
                                    "Max": { "type": "number" }
                                }
                            }
                        }
                    });
                };
                ReportingSalesResult.prototype.ReactToDateChange = function () {
                    var internal = this, service = $("#dashboardreport").data("ReportingService");
                    service.Bind("change", function (e) {
                        if (e.field !== "from") {
                            return;
                        }
                        var dateChange = service.Get("from");
                        var from = new Date();
                        var to = dateChange;
                        from.setDate(dateChange.getDate() - internal.filter.dayRange);
                        internal.filter.from = from;
                        internal.filter.to = to;
                    });
                };
                ReportingSalesResult.prototype.InitReactions = function () {
                    this.ReactToDateChange();
                };
                ReportingSalesResult.prototype.Init = function () {
                    this.InitDataSource();
                    this.InitReactions();
                };
                return ReportingSalesResult;
            })();
            Services.ReportingSalesResult = ReportingSalesResult;
        })(Services = Reporting.Services || (Reporting.Services = {}));
    })(Reporting = MyAndromeda.Reporting || (MyAndromeda.Reporting = {}));
})(MyAndromeda || (MyAndromeda = {}));
///// <reference path="../../Scripts/typings/kendo/kendo.all.d.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var Reporting;
    (function (Reporting) {
        var Services;
        (function (Services) {
            "use strict";
            var ReportingServiceWatcher = (function () {
                function ReportingServiceWatcher(options) {
                    var internal = this;
                    this.options = options;
                    if (!options.id) {
                        throw new Error("Requires 'id' for the view model to bind to the element");
                    }
                    if (options.id) {
                        this.element = $(options.id);
                    }
                    if (options.results) {
                        this.results = options.results;
                    }
                    else {
                        this.results = [];
                    }
                    this.viewModel = kendo.observable({
                        resultCount: 0,
                        max: new Date()
                    });
                    this.viewModel.bind("change", this.viewModelChagned);
                }
                ReportingServiceWatcher.prototype.viewModelChagned = function (e) {
                    console.log("change on field :" + e.field);
                };
                ReportingServiceWatcher.prototype.Init = function () {
                    var internal = this, element = $(this.element);
                    if (this.results && this.results.length > 0) {
                        this.results.forEach(function (item) {
                            internal.AddResultToViewModel(item.key, item);
                        });
                    }
                    if (!element || element.length == 0) {
                        throw new Error("the element is not found:" + this.options.id);
                    }
                    //kendo.bind(element[0], this.viewModel);
                    element.data("ReportingService", this);
                    this.SetupChangeQueryOptions();
                };
                ReportingServiceWatcher.prototype.AddResult = function (key, result) {
                    //already the type we want
                    if (result instanceof Reporting.Models.Result) {
                        this.AddResultToViewModel(key, result);
                    }
                    else {
                        result = new Reporting.Models.Result(key, result);
                        this.AddResultToViewModel(key, result);
                    }
                    return result;
                };
                ReportingServiceWatcher.prototype.TickAction = function () {
                    var value = new Date();
                    this.viewModel.set("today", value);
                };
                ReportingServiceWatcher.prototype.Tick = function () {
                    var internal = this;
                    this.liveUpdate = setInterval(function () {
                        internal.TickAction();
                    }, 1000);
                };
                ReportingServiceWatcher.prototype.StopTick = function () {
                    clearInterval(this.liveUpdate);
                };
                ReportingServiceWatcher.prototype.SetupChangeQueryOptions = function () {
                    var internal = this;
                    $(this.element).on("click", ".k-button-change-date", function () {
                        alert("change");
                    });
                };
                ReportingServiceWatcher.prototype.Get = function (key) {
                    return this.viewModel.get(key);
                };
                ReportingServiceWatcher.prototype.Set = function (key, value) {
                    this.viewModel.set(key, value);
                };
                ReportingServiceWatcher.prototype.Bind = function (eventName, handler) {
                    this.viewModel.bind(eventName, handler);
                };
                ReportingServiceWatcher.prototype.AddResultToViewModel = function (key, result) {
                    var vm = this.viewModel;
                    this.results.push(result);
                    vm.set(key, result);
                    vm.set("resultCount", this.results.length);
                };
                return ReportingServiceWatcher;
            })();
            Services.ReportingServiceWatcher = ReportingServiceWatcher;
        })(Services = Reporting.Services || (Reporting.Services = {}));
    })(Reporting = MyAndromeda.Reporting || (MyAndromeda.Reporting = {}));
})(MyAndromeda || (MyAndromeda = {}));
//module MyAndromeda {
//    module Reporting {
//        export class SalesListDataService {
//        }
//        export class SalesListService
//        {
//            constructor() {
//            }
//        }
//    }
//}
var MyAndromeda;
(function (MyAndromeda) {
    var Reports;
    (function (Reports) {
        var ChainDashboard;
        (function (ChainDashboard) {
            var app = angular.module("ChainDashboard.Config", [
                "ui.router",
                "ngAnimate"
            ]);
            app.config(function ($stateProvider, $urlRouterProvider) {
                var app = {
                    abstract: true,
                    url: "/",
                    templateUrl: "dashboard-app-template.html"
                };
                var appChainSalesDashboard = {
                    url: "/sales-dashboard",
                    views: {
                        "summary": {
                            templateUrl: "chain-sales-dashboard-summary.html",
                            controller: "chainSalesDashboardSummaryController"
                        },
                        "detail": {
                            templateUrl: "chain-sales-dashboard-detail.html",
                            controller: "chainSalesDashboardDetailController"
                        }
                    }
                };
                var appStoreSalesDashboard = {
                    url: "/store-sales-dashboard/:storeId",
                    views: {
                        "summary": {
                            templateUrl: "store-sales-dashboard-summary.html",
                            controller: "storeSalesDashboardSummaryController"
                        },
                        "detail": {
                            templateUrl: "store-sales-dashboard-detail.html",
                            controller: "storeSalesDashboardDetailController"
                        }
                    },
                    onEnter: function ($stateParams, groupedStoreResultsService) {
                        groupedStoreResultsService.LoadStore($stateParams.storeId);
                    },
                    cache: false
                };
                var appChainSalesDataWarehouse = {
                    url: "/chain-sales-live",
                    views: {
                        "summary": {
                            templateUrl: "store-sales-day-dashboard-summary.html",
                            controller: "chainTodaysSalesSummaryController"
                        },
                        "detail": {
                            templateUrl: "store-sales-day-dashboard-detail.html",
                            controller: "chainTodaysSalesDetailController"
                        }
                    }
                };
                var appStoreSalesLive = {
                    url: "/store-sales-live/:storeId",
                    views: {
                        "summary": {
                            templateUrl: "store-sales-day-dashboard-summary.html",
                            controller: "storeSalesDayDashboardSummaryController"
                        },
                    },
                    onEnter: function ($stateParams, groupedStoreResultsService) {
                        groupedStoreResultsService.LoadStore($stateParams.storeId);
                    },
                    cache: false
                };
                var storeMapDashboard = {
                    url: "/store-map-live/:storeId",
                    views: {
                        "summary": {
                            templateUrl: "store-sales-map.summary.html",
                            controller: "storeMapDashboardSummaryController"
                        },
                        "detail": {
                            templateUrl: "store-sales-map.detail.html",
                            controller: "storeMapDashboardDetailController"
                        }
                    },
                    onEnter: function ($stateParams, groupedStoreResultsService) {
                        groupedStoreResultsService.LoadStore($stateParams.storeId);
                    },
                    cache: false
                };
                //$stateProvider.state("app", app);
                $stateProvider.state("chain-sales-dashboard", appChainSalesDashboard);
                $stateProvider.state("chain-sales-data-warehouse", appChainSalesDataWarehouse);
                $stateProvider.state("store-sales-dashboard", appStoreSalesDashboard);
                $stateProvider.state("store-sales-live", appStoreSalesLive);
                $stateProvider.state("store-map-live", storeMapDashboard);
                $urlRouterProvider.otherwise("/sales-dashboard");
            });
            app.run(function ($templateCache) {
                MyAndromeda.Logger.Notify("WebHooks Started");
                angular
                    .element('script[type="text/ng-template"]')
                    .each(function (i, element) {
                    $templateCache.put(element.id, element.innerHTML);
                });
            });
        })(ChainDashboard = Reports.ChainDashboard || (Reports.ChainDashboard = {}));
    })(Reports = MyAndromeda.Reports || (MyAndromeda.Reports = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var Reports;
    (function (Reports) {
        var ChainDashboard;
        (function (ChainDashboard) {
            var app = angular.module("ChainDashboard.StoreMap", ["ChainDashboard.Services"]);
            app.controller("storeMapDashboardSummaryController", function ($scope, $timeout, $stateParams, groupedStoreResultsService, groupedStoreResultsDataService, dashboardQueryContext, chartOptions, valueFormater) {
                MyAndromeda.Logger.Notify("Starting storeSalesDayDashboardSummaryController");
                $scope.currency = valueFormater.Currency;
                $scope.store = groupedStoreResultsService.StoreData;
                groupedStoreResultsService.StoreObservable.subscribe(function (store) {
                    MyAndromeda.Logger.Notify("set store");
                    $timeout(function () {
                        $scope.store = store;
                    });
                });
                if (groupedStoreResultsService.StoreData == null) {
                    MyAndromeda.Logger.Notify("load store data");
                    groupedStoreResultsService.LoadStore($stateParams.storeId);
                }
            });
            app.controller("storeMapDashboardDetailController", function ($scope, $timeout, $stateParams, orderService, dashboardQueryContext, valueFormater) {
                $scope.currency = valueFormater.CurrencyDecimal;
                //var dataSource = orderService.ListOrders($stateParams.storeId);
                var dataSource = orderService.ListOrdersForMap(1885, dashboardQueryContext.Query.FromObj, dashboardQueryContext.Query.ToObj);
                dashboardQueryContext.Changed.throttle(300).subscribe(function () {
                    dataSource.read();
                });
                var getMap = function () {
                    return $scope.myMap;
                };
                var selectMarker = function (e) {
                    var marker = e.marker;
                    var dataItem = marker.dataItem;
                    MyAndromeda.Logger.Notify(dataItem);
                    $timeout(function () {
                        $scope.highlightedOrder = dataItem;
                    });
                };
                var mapOptions = {
                    markerClick: selectMarker,
                    center: [0, 0],
                    zoom: 2,
                    layers: [
                        {
                            type: "tile",
                            urlTemplate: "http://#= subdomain #.tile.openstreetmap.org/#= zoom #/#= x #/#= y #.png",
                            subdomains: ["a", "b", "c"],
                            attribution: "&copy; <a href='http://osm.org/copyright'>OpenStreetMap contributors</a>"
                        },
                        {
                            type: "marker",
                            dataSource: dataSource,
                            locationField: "Customer.GeoLocation"
                        }
                    ],
                };
                $scope.mapOptions = mapOptions;
            });
        })(ChainDashboard = Reports.ChainDashboard || (Reports.ChainDashboard = {}));
    })(Reports = MyAndromeda.Reports || (MyAndromeda.Reports = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var Reports;
    (function (Reports) {
        var ChainDashboard;
        (function (ChainDashboard) {
            var app = angular.module("ChainDashboard.Services", []);
            var DashboardQueryContext = (function () {
                function DashboardQueryContext() {
                    this.Changed = new Rx.BehaviorSubject(false);
                    var today = new Date();
                    var thirtyDaysAgo = new Date();
                    this.Query = {
                        FromObj: new Date(thirtyDaysAgo.setDate(-30)),
                        ToObj: today
                    };
                    this.Changed.onNext(true);
                }
                return DashboardQueryContext;
            })();
            ChainDashboard.DashboardQueryContext = DashboardQueryContext;
            var GroupedDataWarehouseStoreResultsService = (function () {
                function GroupedDataWarehouseStoreResultsService($http) {
                    this.ChainData = null;
                    this.ChainDataObservable = new Rx.BehaviorSubject(null);
                    this.$http = $http;
                }
                GroupedDataWarehouseStoreResultsService.prototype.LoadChain = function (chainId, query) {
                    var _this = this;
                    var route = kendo.format("/chain-data-warehouse/{0}", chainId);
                    var data = query !== null ? this.CreateQueryObj(query) : {};
                    var promise = this.$http.post(route, data);
                    promise.then(function (result) {
                        _this.ChainData = result.data;
                        _this.ChainDataObservable.onNext(_this.ChainData);
                    });
                    return this.ChainDataObservable.asObservable();
                };
                GroupedDataWarehouseStoreResultsService.prototype.CreateQueryObj = function (query) {
                    var f = kendo.toString(query.FromObj, "u");
                    var t = kendo.toString(query.ToObj, "u");
                    MyAndromeda.Logger.Notify(f);
                    MyAndromeda.Logger.Notify(t);
                    return {
                        From: f,
                        To: t
                    };
                };
                GroupedDataWarehouseStoreResultsService.prototype.CreaateTotals = function (name, allOrders) {
                    var totals = {
                        Cancelled: 0,
                        CancelledValue: 0,
                        Completed: 0,
                        CompletedValue: 0,
                        OutForDelivery: 0,
                        OutForDeliveryValue: 0,
                        Oven: 0,
                        OvenValue: 0,
                        ReadyToDispatch: 0,
                        ReadyToDispatchValue: 0,
                        Received: 0,
                        ReceivedValue: 0,
                        Total: 0,
                        TotalValue: 0,
                        FutureOrder: 0,
                        FutureOrderValue: 0,
                        /* pie chart values */
                        Collection: 0,
                        CollectionValue: 0,
                        Delivery: 0,
                        DeliveryValue: 0,
                        InStore: 0,
                        InStoreValue: 0,
                        /* */
                        Name: "",
                        Data: new kendo.data.DataSource(),
                        AcsApplicationData: new kendo.data.DataSource({
                            group: [
                                {
                                    field: "ApplicationId"
                                }
                            ]
                        }),
                        OccasionData: new kendo.data.DataSource()
                    };
                    var k = [];
                    allOrders.subscribe(function (order) {
                        k.push(order);
                        totals.Total++;
                        totals.TotalValue += order.FinalPrice;
                        totals.Name = name;
                        switch (order.OrderType.toLocaleLowerCase()) {
                            case "collection": {
                                totals.Collection++;
                                totals.CollectionValue += order.FinalPrice;
                                break;
                            }
                            case "delivery": {
                                totals.Delivery++;
                                totals.DeliveryValue += order.FinalPrice;
                                break;
                            }
                            default: {
                                totals.InStore++;
                                totals.InStoreValue += order.FinalPrice;
                            }
                        }
                        switch (order.Status) {
                            //Order has been received by the store
                            case 1: {
                                totals.Received++;
                                totals.ReceivedValue += order.FinalPrice;
                                break;
                            }
                            //Order is in oven
                            case 2: {
                                totals.Oven++;
                                totals.OvenValue += order.FinalPrice;
                                break;
                            }
                            //Order is ready for dispatch
                            case 3: {
                                totals.ReadyToDispatch++;
                                totals.ReadyToDispatchValue += order.FinalPrice;
                                break;
                            }
                            //Order is out for delivery
                            case 4: {
                                totals.OutForDelivery++;
                                totals.OutForDeliveryValue += order.FinalPrice;
                                break;
                            }
                            //Order has been completed
                            case 5: {
                                totals.Completed++;
                                totals.CompletedValue += order.FinalPrice;
                                break;
                            }
                            //Order has been canceled
                            case 6: {
                                totals.Cancelled++;
                                totals.CancelledValue += order.FinalPrice;
                                break;
                            }
                            //Future Order
                            case 8: {
                                totals.FutureOrder++;
                                totals.FutureOrderValue += order.FinalPrice;
                                break;
                            }
                        }
                    });
                    totals.AcsApplicationData.data(k);
                    totals.Data.data(k);
                    totals.OccasionData.data([
                        { OrderType: "Collection", Count: totals.Collection, Sum: totals.CollectionValue },
                        { OrderType: "Delivery", Count: totals.Delivery, Sum: totals.DeliveryValue },
                        { OrderType: "Dine in", Count: totals.InStore, Sum: totals.InStoreValue }
                    ]);
                    return totals;
                };
                return GroupedDataWarehouseStoreResultsService;
            })();
            ChainDashboard.GroupedDataWarehouseStoreResultsService = GroupedDataWarehouseStoreResultsService;
            var GroupedStoreResultsService = (function () {
                function GroupedStoreResultsService(groupedStoreResultsDataService, dashboardQueryContext) {
                    var _this = this;
                    this.ChainData = null;
                    this.StoreData = null;
                    this.dashboardQueryContext = dashboardQueryContext;
                    this.groupedStoreResultsDataService = groupedStoreResultsDataService;
                    this.ChainDataObservable = new Rx.BehaviorSubject(null);
                    this.StoreObservable = new Rx.BehaviorSubject(null);
                    var throttled = dashboardQueryContext.Changed.where(function (e) { return e; }).throttle(700);
                    throttled.select(function () {
                        var promise = _this.groupedStoreResultsDataService.GetDailyAllStoreData(ChainDashboard.settings.chainId, _this.dashboardQueryContext.Query);
                        return promise;
                    }).subscribe(function (e) {
                        //load stuff; 
                        e.then(function (dataResult) {
                            _this.ChainData = dataResult.data;
                            _this.ChainDataObservable.onNext(dataResult.data);
                        });
                    });
                    this.ChainDataObservable.subscribe(function (data) {
                    });
                    throttled.where(function () { return _this.StoreData !== null; }).select(function () {
                        var promise = _this.groupedStoreResultsDataService.GetDailySingleStoreData(ChainDashboard.settings.chainId, _this.StoreData.StoreId, _this.dashboardQueryContext.Query);
                        return promise;
                    }).subscribe(function (e) {
                        e.then(function (dataResult) {
                            _this.StoreData = dataResult.data;
                            _this.StoreObservable.onNext(_this.StoreData);
                        });
                    });
                }
                GroupedStoreResultsService.prototype.LoadStore = function (andromedaSiteId) {
                    var _this = this;
                    if (this.ChainData == null) {
                        var promise = this.groupedStoreResultsDataService
                            .GetDailySingleStoreData(ChainDashboard.settings.chainId, andromedaSiteId, this.dashboardQueryContext.Query);
                        promise.then(function (dataResult) {
                            _this.StoreData = dataResult.data;
                            _this.StoreObservable.onNext(_this.StoreData);
                        });
                    }
                    else {
                        var store = this.ChainData.Data.filter(function (e) { return e.StoreId == andromedaSiteId; });
                        this.StoreData = store[0];
                        this.StoreObservable.onNext(this.StoreData);
                    }
                };
                return GroupedStoreResultsService;
            })();
            ChainDashboard.GroupedStoreResultsService = GroupedStoreResultsService;
            var GroupedStoreResultsDataService = (function () {
                function GroupedStoreResultsDataService($http) {
                    this.$http = $http;
                }
                GroupedStoreResultsDataService.prototype.CreateQueryObj = function (query) {
                    var f = kendo.toString(query.FromObj, "u");
                    var t = kendo.toString(query.ToObj, "u");
                    MyAndromeda.Logger.Notify(f);
                    MyAndromeda.Logger.Notify(t);
                    return {
                        From: f,
                        To: t
                    };
                };
                GroupedStoreResultsDataService.prototype.GetDailyAllStoreData = function (chainId, query) {
                    var route = kendo.format("/chain-data/{0}", chainId);
                    var queryBody = this.CreateQueryObj(query);
                    var promise = this.$http.post(route, queryBody);
                    return promise;
                };
                GroupedStoreResultsDataService.prototype.GetDailySingleStoreData = function (chainId, andromedaSiteId, query) {
                    var route = kendo.format("/chain-data/{0}/store/{1}", chainId, andromedaSiteId);
                    var queryBody = this.CreateQueryObj(query);
                    var promise = this.$http.post(route, queryBody);
                    return promise;
                };
                return GroupedStoreResultsDataService;
            })();
            ChainDashboard.GroupedStoreResultsDataService = GroupedStoreResultsDataService;
            var ChartOptions = (function () {
                function ChartOptions() {
                }
                ChartOptions.prototype.DataWareHouseOccasionChart = function () {
                    var options = {
                        theme: "bootstrap",
                        legend: {
                            position: "top"
                        },
                        seriesDefaults: {
                            labels: {
                                template: "#= category # - #= kendo.format('{0:P}', percentage)#",
                                //position: "outsideEnd",
                                visible: true,
                                background: "transparent"
                            },
                            type: "pie"
                        },
                        series: [
                            {
                                name: "Count", categoryField: "OrderType", field: "Count",
                                labels: {
                                    template: "#=category# - #= dataItem.Count# order(s)\n#= kendo.toString(dataItem.Sum, 'c') #"
                                }
                            }
                        ]
                    };
                    return options;
                };
                ChartOptions.prototype.DataWareHouseAcsApplicationChart = function () {
                    var options = {
                        theme: "bootstrap",
                        legend: {
                            position: "top"
                        },
                        categoryAxis: [
                            {
                                field: "WantedTime",
                                type: "date",
                                baseUnit: "months"
                            }
                        ],
                        valueAxis: [
                            {
                                title: "Sales",
                                name: "Sales",
                                labels: {
                                    template: "#= kendo.toString(value, 'c') #"
                                },
                            },
                        ],
                        series: [
                            { name: "Sales", field: "FinalPrice", type: "column", axis: "Sales", aggregate: "sum", stack: { group: "Sales" } },
                        ]
                    };
                    return options;
                };
                ChartOptions.prototype.WeekChart = function () {
                    var options = {
                        theme: "bootstrap",
                        legend: {
                            position: "top"
                        },
                        categoryAxis: [
                            {
                                field: "WeekOfYear",
                                baseUnitStep: "auto",
                            }
                        ],
                        valueAxis: [
                            {
                                title: "Sales",
                                name: "Sales",
                                labels: {
                                    template: "#= kendo.toString(value / 100, 'c') #"
                                }
                            },
                            { title: "Orders", name: "Orders" }
                        ],
                        //seriesDefaults: { type: "radarLine" },
                        series: [
                            { name: "Sales", field: "Total.NetSales", type: "area", axis: "Sales" },
                            { name: "Order Count", field: "Total.OrderCount", type: "column", axis: "Orders" },
                            { name: "Delivery", field: "Delivery.NetSales", type: "area", axis: "Sales", aggregate: "sum" },
                            { name: "Collection", field: "Collection.NetSales", type: "area", axis: "Sales", aggregate: "sum" },
                            { name: "Carry out", field: "CarryOut.NetSales", type: "area", axis: "Sales", aggregate: "sum" }
                        ]
                    };
                    return options;
                };
                ChartOptions.prototype.DayChart = function (baseUnit) {
                    var options = {
                        theme: "bootstrap",
                        legend: {
                            position: "top"
                        },
                        categoryAxis: [
                            {
                                field: "CreateTimeStamp",
                                //baseUnitStep: "auto",
                                type: "date",
                                baseUnit: baseUnit,
                                baseUnitStep: 1,
                                autoBaseUnitSteps: {
                                    days: [1],
                                    weeks: [],
                                    months: []
                                },
                                weekStartDay: 1,
                            }
                        ],
                        valueAxis: [
                            {
                                title: "Sales", name: "Sales",
                                labels: {
                                    template: "#= kendo.toString(value / 100, 'c') #"
                                }
                            },
                            { title: "Orders", name: "Orders" }
                        ],
                        //seriesDefaults: { type: "radarLine" },
                        series: [
                            { name: "Sales", field: "Total.NetSales", type: "area", axis: "Sales", aggregate: "sum" },
                            { name: "Order Count", field: "Total.OrderCount", type: "column", axis: "Orders", aggregate: "sum" },
                            { name: "Delivery", field: "Delivery.NetSales", type: "area", axis: "Sales", aggregate: "sum" },
                            { name: "Collection", field: "Collection.NetSales", type: "area", axis: "Sales", aggregate: "sum" },
                            { name: "Carry out", field: "CarryOut.NetSales", type: "area", axis: "Sales", aggregate: "sum" }
                        ]
                    };
                    if (baseUnit == "days") {
                        var category = options.categoryAxis[0];
                        category.labels = {
                            step: 3,
                            format: "d/M"
                        };
                    }
                    if (baseUnit == "months") {
                        var category = options.categoryAxis[0];
                        category.labels = {
                            dateFormats: {
                                days: "d/M"
                            }
                        };
                    }
                    return options;
                };
                return ChartOptions;
            })();
            ChainDashboard.ChartOptions = ChartOptions;
            var ValueFormater = (function () {
                function ValueFormater() {
                }
                ValueFormater.prototype.Currency = function (value) {
                    var x = value / 100;
                    return kendo.toString(x, "c");
                };
                ValueFormater.prototype.CurrencyDecimal = function (value) {
                    return kendo.toString(value, "c");
                };
                ValueFormater.prototype.DateFormat = function (value) {
                    return kendo.toString(value, "g");
                };
                return ValueFormater;
            })();
            ChainDashboard.ValueFormater = ValueFormater;
            app.service("dashboardQueryContext", DashboardQueryContext);
            app.service("groupedStoreResultsDataService", GroupedStoreResultsDataService);
            app.service("groupedStoreResultsService", GroupedStoreResultsService);
            app.service("valueFormater", ValueFormater);
            app.service("chartOptions", ChartOptions);
            app.service("groupedDataWarehouseStoreResultsService", GroupedDataWarehouseStoreResultsService);
        })(ChainDashboard = Reports.ChainDashboard || (Reports.ChainDashboard = {}));
    })(Reports = MyAndromeda.Reports || (MyAndromeda.Reports = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var Reports;
    (function (Reports) {
        var ChainDashboard;
        (function (ChainDashboard) {
            var app = angular.module("ChainDashboard.Today", ["ChainDashboard.Services"]);
            app.controller("chainTodaysSalesSummaryController", function ($scope, $stateParams, $timeout, dashboardQueryContext, chartOptions, groupedDataWarehouseStoreResultsService, valueFormater) {
                //only work with 5;
                //var observable = groupedDataWarehouseStoreResultsService
                //    .LoadChain(settings.chainId)
                //    .where(e => e !== null);
                var observable = groupedDataWarehouseStoreResultsService
                    .LoadChain(ChainDashboard.settings.chainId, null)
                    .where(function (e) { return e !== null; });
                var changeSubscription = dashboardQueryContext.Changed.throttle(700).subscribe(function (b) {
                    groupedDataWarehouseStoreResultsService
                        .LoadChain(ChainDashboard.settings.chainId, dashboardQueryContext.Query);
                });
                observable.subscribe(function (data) {
                    var allOrders = Rx.Observable.from(data.Stores).selectMany(function (store) { return Rx.Observable.from(store.Orders); });
                    var r = groupedDataWarehouseStoreResultsService.CreaateTotals(data.Name, allOrders);
                    $timeout(function () {
                        $scope.summary = r;
                    });
                });
                $scope.occasionChartOptions = chartOptions.DataWareHouseOccasionChart();
                $scope.acsChartOptions = chartOptions.DataWareHouseAcsApplicationChart();
                $scope.currency = valueFormater.CurrencyDecimal;
            });
            app.controller("chainTodaysSalesDetailController", function ($scope, $stateParams, $timeout, dashboardQueryContext, chartOptions, groupedDataWarehouseStoreResultsService, valueFormater) {
                var observable = groupedDataWarehouseStoreResultsService
                    .LoadChain(5, null)
                    .where(function (e) { return e !== null; });
                observable.subscribe(function (data) {
                    var summaries = [];
                    //var allOrders = Rx.Observable.from(data.Stores).selectMany((store) => Rx.Observable.from(store.Orders));
                    var stores = Rx.Observable.from(data.Stores);
                    stores.subscribe(function (store) {
                        var allOrders = Rx.Observable.from(store.Orders);
                        var r = groupedDataWarehouseStoreResultsService.CreaateTotals(store.Name, allOrders);
                        summaries.push(r);
                    });
                    $timeout(function () {
                        $scope.summaries = summaries;
                    });
                });
                $scope.currency = valueFormater.CurrencyDecimal;
            });
            app.controller("storeSalesDayDashboardSummaryController", function ($scope, $timeout, $stateParams, groupedStoreResultsService, groupedStoreResultsDataService, dashboardQueryContext, chartOptions, valueFormater) {
                MyAndromeda.Logger.Notify("Starting storeSalesDayDashboardSummaryController");
                $scope.currency = valueFormater.Currency;
                $scope.store = groupedStoreResultsService.StoreData;
                groupedStoreResultsService.StoreObservable.subscribe(function (store) {
                    MyAndromeda.Logger.Notify("set store");
                    $timeout(function () {
                        $scope.store = store;
                    });
                });
                if (groupedStoreResultsService.StoreData == null) {
                    MyAndromeda.Logger.Notify("load store data");
                    //ignore store 
                    groupedStoreResultsService.LoadStore($stateParams.storeId);
                }
            });
        })(ChainDashboard = Reports.ChainDashboard || (Reports.ChainDashboard = {}));
    })(Reports = MyAndromeda.Reports || (MyAndromeda.Reports = {}));
})(MyAndromeda || (MyAndromeda = {}));
/// <reference path="../../scripts/typings/angular-ui-router/angular-ui-router.d.ts" />
/// <reference path="myandromeda.reports.chaindashboard.models.d.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var Reports;
    (function (Reports) {
        var ChainDashboard;
        (function (ChainDashboard) {
            var app = angular.module("ChainDashboard", [
                "MyAndromeda.Resize",
                "MyAndromeda.Progress",
                "MyAndromeda.Data.Orders",
                "ChainDashboard.Services",
                "ChainDashboard.StoreMap",
                "ChainDashboard.Today",
                "ChainDashboard.Config",
                "kendo.directives",
                "oitozero.ngSweetAlert",
                "ngAnimate"
            ]);
            app.controller("chainSalesDashboardSummaryController", function ($scope, $timeout, groupedStoreResultsService, valueFormater, chartOptions) {
                $scope.hi = "hi ho";
                $scope.data = [];
                $scope.currency = valueFormater.Currency;
                var radarOptions = {
                    legend: {
                        position: "top"
                    },
                    categoryAxis: [
                        {
                            field: "WeekOfYear",
                            baseUnitStep: "auto",
                        }
                    ],
                    valueAxis: [
                        { title: "Sales", name: "Sales" },
                        { title: "Orders", name: "Orders" }
                    ],
                    //seriesDefaults: { type: "radarLine" },
                    series: [
                        { name: "Sales", field: "Total.NetSales", type: "area", axis: "Sales" },
                        { name: "Order Count", field: "Total.OrderCount", type: "column", axis: "Orders" }
                    ]
                };
                $scope.weekChartOptions = chartOptions.WeekChart();
                $scope.dayChartOptions = chartOptions.DayChart("days");
                $scope.weekChartOptions = chartOptions.DayChart("weeks");
                $scope.monthChartOptions = chartOptions.DayChart("months");
                var salesWeekData = new kendo.data.DataSource({});
                var salesMonthData = new kendo.data.DataSource({
                    group: [
                        { field: "CreatedTimeStamp" }
                    ],
                    schema: {
                        model: {
                            fields: {
                                CreatedTimeStamp: {
                                    type: "date"
                                }
                            }
                        }
                    }
                });
                $scope.radarOptions = radarOptions;
                $scope.salesDayData = salesMonthData;
                $scope.salesWeekData = salesWeekData;
                groupedStoreResultsService.ChainDataObservable.where(function (e) { return e !== null; }).subscribe(function (chainResult) {
                    $timeout(function () {
                        $scope.data = chainResult;
                        $scope.storeCount = chainResult.Data.length;
                        MyAndromeda.Logger.Notify("$scope.data :");
                        MyAndromeda.Logger.Notify(chainResult);
                        Rx.Observable.from(chainResult.WeekData).maxBy(function (e) { return e.Total.NetSales; }).subscribe(function (max) {
                            MyAndromeda.Logger.Notify("$scope.bestWeek");
                            MyAndromeda.Logger.Notify(max[0]);
                            $scope.bestWeek = max[0];
                        });
                        var dayData = [];
                        var dayDataObservable = Rx.Observable.from(chainResult.Data).flatMap(function (e) { return Rx.Observable.fromArray(e.DailyData); });
                        dayDataObservable.subscribe(function (o) {
                            dayData.push(o);
                        });
                        MyAndromeda.Logger.Notify("day data");
                        MyAndromeda.Logger.Notify(dayData);
                        salesWeekData.data(chainResult.WeekData);
                        salesMonthData.data(dayData);
                    });
                });
            });
            app.controller("chainSalesDashboardDetailController", function ($scope, $timeout, $state, groupedStoreResultsService, valueFormater) {
                $scope.data = [];
                $scope.currency = valueFormater.Currency;
                $scope.select = function (store) {
                    groupedStoreResultsService.StoreData = store;
                    $state.go("store-sales-dashboard", { storeId: store.StoreId });
                };
                groupedStoreResultsService.ChainDataObservable.subscribe(function (data) {
                    $scope.data = data;
                });
            });
            app.controller("storeSalesDashboardSummaryController", function ($scope, $timeout, $stateParams, groupedStoreResultsService, groupedStoreResultsDataService, dashboardQueryContext, valueFormater) {
                $scope.currency = valueFormater.Currency;
                $scope.store = groupedStoreResultsService.StoreData;
                groupedStoreResultsService.StoreObservable.subscribe(function (store) {
                    MyAndromeda.Logger.Notify("set store");
                    $timeout(function () {
                        $scope.store = store;
                    });
                });
                if (groupedStoreResultsService.StoreData == null) {
                    MyAndromeda.Logger.Notify("load store data");
                    groupedStoreResultsService.LoadStore($stateParams.storeId);
                }
            });
            app.controller("storeSalesDashboardDetailController", function ($scope, $timeout, $stateParams, groupedStoreResultsService, groupedStoreResultsDataService, dashboardQueryContext, chartOptions, valueFormater) {
                $scope.storeWeekOptions = chartOptions.WeekChart();
                $scope.storeDailyOptions = chartOptions.DayChart("days");
                $scope.storeWeeklyOptions = chartOptions.DayChart("weeks");
                $scope.storeMonthlyOptions = chartOptions.DayChart("months");
                var weekDataSource = new kendo.data.DataSource({});
                var dailyDataSource = new kendo.data.DataSource({
                    schema: {
                        model: {
                            fields: {
                                CreateTimeStamp: {
                                    type: "date"
                                }
                            }
                        }
                    }
                });
                $scope.storeWeekData = weekDataSource;
                $scope.dailyDataSource = dailyDataSource;
                var storeObservable = groupedStoreResultsService.StoreObservable.where(function (e) { return e !== null; }).subscribe(function (store) {
                    MyAndromeda.Logger.Notify("reload charts");
                    $timeout(function () {
                        $scope.store = store;
                        weekDataSource.data(store.WeekData);
                        dailyDataSource.data(store.DailyData);
                    });
                });
            });
            app.directive("dashboardAppFilter", function () {
                return {
                    controller: function ($scope, dashboardQueryContext) {
                        $scope.query = dashboardQueryContext.Query;
                        //dashboardQueryContext.Query.ToObj
                        $scope.$watch('query.ToObj', function () {
                            MyAndromeda.Logger.Notify("changes");
                            dashboardQueryContext.Changed.onNext(true);
                        });
                        $scope.$watch('query.FromObj', function () {
                            MyAndromeda.Logger.Notify("changes");
                            dashboardQueryContext.Changed.onNext(true);
                        });
                        $scope.today = function () {
                            dashboardQueryContext.Query.FromObj = new Date();
                        };
                        $scope.createPdf = function () {
                            MyAndromeda.Logger.Notify("hi");
                            var region = $(".pdfRegion");
                            kendo.drawing.drawDOM(region)
                                .then(function (group) {
                                // Render the result as a PDF file
                                return kendo.drawing.exportPDF(group, {
                                    creator: "Andromeda",
                                    keywords: "Chain,Report",
                                    date: new Date(),
                                    landscape: false,
                                    subject: "Report",
                                    title: "Chain",
                                    paperSize: "auto",
                                    margin: { left: "1cm", top: "1cm", right: "1cm", bottom: "1cm" }
                                });
                            })
                                .done(function (data) {
                                // Save the PDF file
                                kendo.saveAs({
                                    dataURI: data,
                                    fileName: "Dashboard.pdf",
                                });
                            });
                        };
                    },
                    restrict: "E",
                    templateUrl: "dashboard-app-filter.html"
                };
            });
            ChainDashboard.settings = {
                chainId: 0
            };
            function setupChainDashboard(id) {
                var element = document.getElementById(id);
                angular.bootstrap(element, ["ChainDashboard"]);
            }
            ChainDashboard.setupChainDashboard = setupChainDashboard;
            ;
        })(ChainDashboard = Reports.ChainDashboard || (Reports.ChainDashboard = {}));
    })(Reports = MyAndromeda.Reports || (MyAndromeda.Reports = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var Start;
    (function (Start) {
        var app = angular.module("MyAndromeda.Start.Config", [
            "MyAndromeda.Start.Controllers",
            "ui.router",
            "ngAnimate"
        ]);
        app.config(function ($stateProvider, $urlRouterProvider) {
            MyAndromeda.Logger.Notify("set start config");
            var app = {
                abstract: true,
                url: "/chain",
                template: '<div ui-view="main"></div>'
            };
            var appChainList = {
                url: "/list",
                views: {
                    "main": {
                        templateUrl: "chain-list.html",
                        controller: "chainListController"
                    },
                },
                cache: false
            };
            //var appChainsStoreList: ng.ui.IState = {
            //    url: "/:chainId",
            //    views: {
            //        "main": {
            //            templateUrl: "store-list.html",
            //            controller: "storeListController"
            //        }
            //    },
            //    cache: false
            //};
            $stateProvider.state("chain", app);
            $stateProvider.state("chain.list", appChainList);
            //$stateProvider.state("start-chain-store", appChainsStoreList);
            $urlRouterProvider.otherwise("/chain/list");
        });
        app.run(function ($templateCache) {
            MyAndromeda.Logger.Notify("Started config");
            angular
                .element('script[type="text/ng-template"]')
                .each(function (i, element) {
                $templateCache.put(element.id, element.innerHTML);
            });
        });
    })(Start = MyAndromeda.Start || (MyAndromeda.Start = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var Start;
    (function (Start) {
        var controllers = angular.module("MyAndromeda.Start.Controllers", ["MyAndromeda.Start.Services", "kendo.directives"]);
        controllers.controller("chainListController", function ($scope, userChainDataService) {
            var chainActionTemplate = $("#chain-actions-template").html();
            var storeTemplate = $("#chain-template").html();
            MyAndromeda.Logger.Notify("store template");
            MyAndromeda.Logger.Notify(storeTemplate);
            var chainListOptionsDataSource = new kendo.data.TreeListDataSource({
                //data: [
                //    { Id: 1, Name: "test", ParentId: null },
                //    { Id: 2, Name: "test 2", ParentId: 1 }
                //],
                schema: {
                    model: {
                        id: "Id",
                        parentId: "ParentId",
                        fields: {
                            Name: {
                                field: "Name", type: "string"
                            },
                            StoreCount: { field: "StoreCount", type: "number" },
                            ParentId: { field: "ParentId", nullable: true },
                        },
                        StoreCountLabel: function () {
                            var model = this;
                            if (model.ChildChainCount > 0) {
                                var chainTotal = model.ChildStoreCount + model.StoreCount;
                                return model.StoreCount + "/" + chainTotal;
                            }
                            return "" + model.StoreCount;
                        },
                        expanded: true
                    }
                }
            });
            var chainListOptions = {
                sortable: true,
                editable: false,
                filterable: {},
                //autoBind: false,
                resizable: true,
                dataSource: chainListOptionsDataSource,
                columns: [
                    //{ title: "Actions", width: 170, template: chainActionTemplate, filterable: false }
                    { field: "Name", title: "Name", template: storeTemplate },
                ]
            };
            var chainsPromise = userChainDataService.List();
            chainsPromise.then(function (callback) {
                MyAndromeda.Logger.Notify("chain call back data");
                var data = callback.data;
                chainListOptionsDataSource.data(data);
                var dataSourceData = chainListOptionsDataSource.data();
                MyAndromeda.Logger.Notify(dataSourceData);
            }).catch(function () {
                alert("Something went wrong");
            });
            $scope.chainListOptions = chainListOptions;
        });
        controllers.directive("chainGrid", function () {
            return {
                name: "chainGrid",
                controller: "storeListController",
                restrict: "E",
                scope: {
                    chain: "="
                },
                templateUrl: "store-list.html"
            };
        });
        controllers.controller("storeListController", function ($scope, $timeout, userStoreDataService) {
            var chain = $scope.chain;
            var status = {
                hasStores: false,
                hideStores: true
            };
            $scope.status = status;
            var storeListDataSource = new kendo.data.DataSource({
                transport: {
                    read: function (options) {
                        var promise = userStoreDataService.ListStoresByChainId(chain.Id);
                        promise.then(function (callback) {
                            options.success(callback.data);
                            if (callback.data.length > 0) {
                                $timeout(function () {
                                    status.hasStores = true;
                                });
                            }
                        });
                    }
                },
                sort: [
                    { field: "Name", dir: "asc" }
                ],
                serverSorting: false,
                serverFiltering: false,
                serverPaging: false
            });
            storeListDataSource.read();
            var storeTemplate = $("#store-list-template").html();
            var storeListOptions = {
                autoBind: true,
                sortable: true,
                dataSource: storeListDataSource,
                filterable: {
                    mode: "row"
                },
                columns: [
                    { title: "Name", field: "Name", width: 200 },
                    { title: "Actions", template: storeTemplate }
                ]
            };
            $scope.storeListOptions = storeListOptions;
        });
    })(Start = MyAndromeda.Start || (MyAndromeda.Start = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var Start;
    (function (Start) {
        var Services;
        (function (Services) {
            var services = angular.module("MyAndromeda.Start.Services", []);
            var UserChainDataService = (function () {
                function UserChainDataService($http) {
                    this.$http = $http;
                }
                UserChainDataService.prototype.List = function () {
                    var getChains = this.$http.get("/user/chains");
                    return getChains;
                };
                return UserChainDataService;
            })();
            Services.UserChainDataService = UserChainDataService;
            var UserStoreDataService = (function () {
                function UserStoreDataService($http) {
                    this.$http = $http;
                }
                UserStoreDataService.prototype.ListStores = function () {
                    var route = "/user/stores";
                    var getStores = this.$http.get(route);
                    return getStores;
                };
                UserStoreDataService.prototype.ListStoresByChainId = function (chainId) {
                    var route = kendo.format("/user/chains/{0}", chainId);
                    var getChains = this.$http.get(route);
                    return getChains;
                };
                return UserStoreDataService;
            })();
            Services.UserStoreDataService = UserStoreDataService;
            services.service("userChainDataService", UserChainDataService);
            services.service("userStoreDataService", UserStoreDataService);
        })(Services = Start.Services || (Start.Services = {}));
    })(Start = MyAndromeda.Start || (MyAndromeda.Start = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var Start;
    (function (Start) {
        var start = angular.module("MyAndromeda.Start", [
            "ngAnimate",
            "MyAndromeda.Start.Config",
            "MyAndromeda.Hr"
        ]);
        function setupStart(id) {
            var element = document.getElementById(id);
            angular.bootstrap(element, ["MyAndromeda.Start"]);
        }
        Start.setupStart = setupStart;
        ;
    })(Start = MyAndromeda.Start || (MyAndromeda.Start = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var Store;
    (function (Store) {
        var Services;
        (function (Services) {
            var storeService = (function () {
                function storeService(routes) {
                    this.routes = routes;
                }
                storeService.prototype.get = function (id, callback) {
                    var internal = this;
                    var route = {
                        type: "POST",
                        dataType: "json",
                        success: function (data) {
                            callback(data);
                        }
                    };
                    if (typeof (id) === "string") {
                        $.ajax($.extend({}, route, {
                            url: internal.routes.getByExternalId
                        }));
                    }
                    if (typeof (id) === "number") {
                        $.ajax($.extend({}, route, {
                            url: internal.routes.getById
                        }));
                    }
                };
                storeService.prototype.getStores = function (chainId, callback) {
                    var internal = this;
                };
                return storeService;
            })();
            Services.storeService = storeService;
        })(Services = Store.Services || (Store.Services = {}));
    })(Store = MyAndromeda.Store || (MyAndromeda.Store = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var Stores;
    (function (Stores) {
        var OpeningHours;
        (function (OpeningHours) {
            var app = angular.module("MyAndromeda.Store.OpeningHours.Config", [
                "ui.router",
                "kendo.directives", "oitozero.ngSweetAlert",
                "MyAndromeda.Store.OpeningHours.Controllers",
                "MyAndromeda.Store.OpeningHours.Services",
                "MyAndromeda.Store.OpeningHours.Directives"
            ]);
            app.config(function ($stateProvider, $urlRouterProvider) {
                var start = {
                    url: '/:andromedaSiteId',
                    controller: "OpeningHoursController",
                    templateUrl: "OpeningHours-template.html"
                };
                $stateProvider.state("opening-hours", start);
                $urlRouterProvider.otherwise("/" + OpeningHours.settings.andromedaSiteId);
            });
        })(OpeningHours = Stores.OpeningHours || (Stores.OpeningHours = {}));
    })(Stores = MyAndromeda.Stores || (MyAndromeda.Stores = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var Stores;
    (function (Stores) {
        var OpeningHours;
        (function (OpeningHours) {
            var app = angular.module("MyAndromeda.Store.OpeningHours.Controllers", []);
            app.controller("OpeningHoursController", function ($scope, storeOccasionSchedulerService) {
                var schedulerOptions = storeOccasionSchedulerService.CreateScheduler();
                $scope.schedulerOptions = schedulerOptions;
            });
        })(OpeningHours = Stores.OpeningHours || (Stores.OpeningHours = {}));
    })(Stores = MyAndromeda.Stores || (MyAndromeda.Stores = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var Stores;
    (function (Stores) {
        var OpeningHours;
        (function (OpeningHours) {
            var app = angular.module("MyAndromeda.Store.OpeningHours.Directives", []);
            app.directive("occasionTaskEditor", function () {
                return {
                    name: "occasionTaskEditor",
                    scope: {
                        task: "=task",
                    },
                    templateUrl: "occasionTaskEditor.html",
                    controller: function ($scope) {
                        var task = $scope.task;
                        MyAndromeda.Logger.Notify("Occasion task editor - started");
                        MyAndromeda.Logger.Notify("Occasion task");
                        MyAndromeda.Logger.Notify(task);
                        var occasionOptions = {
                            //dataSource: [
                            //    Models.occasionDefinitions.Delivery,
                            //    Models.occasionDefinitions.Collection,
                            //    Models.occasionDefinitions.DineIn
                            //],
                            dataSource: new kendo.data.DataSource({
                                data: [
                                    OpeningHours.Models.occasionDefinitions.Delivery,
                                    OpeningHours.Models.occasionDefinitions.Collection,
                                    OpeningHours.Models.occasionDefinitions.DineIn
                                ]
                            }),
                            valuePrimitive: true,
                            dataTextField: "Name",
                            dataValueField: "Name",
                            //ignoreCase: true,
                            autoBind: true
                        };
                        $scope.occasionOptions = occasionOptions;
                    }
                };
            });
            app.directive("occasionTask", function () {
                return {
                    name: "occasionTask",
                    scope: {
                        task: "=task",
                    },
                    templateUrl: "occasionTask.html",
                    controller: function ($scope, $element) {
                        var task = $scope.task;
                        var occasionTypeIsString = typeof (task.Occasions) === "string" ? true : false;
                        var occasionIsArray = typeof (task.Occasions) === "object" ? true : false;
                        var state = {
                            occasions: occasionTypeIsString ? task.Occasions.split(',') : task.Occasions,
                        };
                        var extra = {
                            hours: Math.abs(task.end.getTime() - task.start.getTime()) / 36e5,
                            startTime: kendo.toString(task.start, "HH:mm"),
                            endTime: kendo.toString(task.end, "HH:mm")
                        };
                        var definitions = OpeningHours.Models.occasionDefinitions;
                        var getColor = function (name) {
                            switch (name) {
                                case definitions.Delivery.Name: return OpeningHours.Models.occasionDefinitions.Delivery.Colour;
                                case definitions.Collection.Name: return OpeningHours.Models.occasionDefinitions.Collection.Colour;
                                case definitions.DineIn.Name: return OpeningHours.Models.occasionDefinitions.DineIn.Colour;
                            }
                        };
                        $scope.getColour = getColor;
                        $scope.state = state;
                        $scope.extra = extra;
                        var topElement = $($element).closest(".k-event");
                        MyAndromeda.Logger.Notify("occasions");
                        MyAndromeda.Logger.Notify(task.Occasions);
                        var twoColors = "repeating-linear-gradient(\n                        45deg,\n                        {0},\n                        {0} 10px,\n                        {1} 10px,\n                        {1} 20px\n                    )";
                        var threeColors = "repeating-linear-gradient(\n                        45deg,\n                        {0},\n                        {0} 10px,\n                        {1} 10px,\n                        {1} 20px,\n                        {2} 20px,\n                        {2} 30px\n                    )";
                        twoColors =
                            kendo.format(twoColors, definitions.Delivery.Colour, definitions.Collection.Colour);
                        threeColors =
                            kendo.format(threeColors, definitions.Delivery.Colour, definitions.Collection.Colour, definitions.DineIn.Colour);
                        if (task.Occasions.length == 2) {
                            topElement.css({
                                "background": twoColors,
                            });
                        }
                        else if (task.Occasions.length === 3) {
                            topElement.css({
                                "background": threeColors,
                            });
                        }
                        //var popover = topElement.popover({
                        //    title: "Task preview",
                        //    placement: "auto",
                        //    html: true,
                        //    content: "please wait",
                        //    trigger: "click"
                        //}).on("show.bs.popover", function () {
                        //    let html = topElement.html();
                        //    popover.attr('data-content', html);
                        //    var current = $(this);
                        //    setTimeout(() => { current.popover('hide'); }, 5000)
                        //});
                        //$scope.$on('$destroy', function () {
                        //    popover.hide();
                        //});
                    }
                };
            });
        })(OpeningHours = Stores.OpeningHours || (Stores.OpeningHours = {}));
    })(Stores = MyAndromeda.Stores || (MyAndromeda.Stores = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var Stores;
    (function (Stores) {
        var OpeningHours;
        (function (OpeningHours) {
            var Models;
            (function (Models) {
                Models.occasionDefinitions = {
                    Delivery: { Name: "Delivery", Colour: "#d9534f" },
                    Collection: { Name: "Collection", Colour: "#d9edf7" },
                    DineIn: {
                        Name: "Dine In", Colour: "#f2dede"
                    }
                };
                function getSchedulerDataSourceSchema() {
                    var model = {
                        id: "Id",
                        fields: {
                            Id: {
                                type: "string",
                                nullable: true
                            },
                            title: { from: "Title", validation: { required: true } },
                            start: { type: "date", from: "Start" },
                            end: { type: "date", from: "End" },
                            startTimezone: { from: "StartTimezone" },
                            endTimezone: { from: "EndTimezone" },
                            description: { from: "Description" },
                            recurrenceId: { from: "RecurrenceId" },
                            recurrenceRule: { from: "RecurrenceRule" },
                            recurrenceException: { from: "RecurrenceException" },
                            isAllDay: { type: "boolean", from: "IsAllDay" },
                            Occasions: {
                                defaultValue: ["Delivery", "Collection"]
                            }
                        }
                    };
                    return model;
                }
                Models.getSchedulerDataSourceSchema = getSchedulerDataSourceSchema;
                ;
            })(Models = OpeningHours.Models || (OpeningHours.Models = {}));
        })(OpeningHours = Stores.OpeningHours || (Stores.OpeningHours = {}));
    })(Stores = MyAndromeda.Stores || (MyAndromeda.Stores = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var Stores;
    (function (Stores) {
        var OpeningHours;
        (function (OpeningHours) {
            var Services;
            (function (Services) {
                var app = angular.module("MyAndromeda.Store.OpeningHours.Services", []);
                var StoreOccasionAvailabilityService = (function () {
                    function StoreOccasionAvailabilityService(scheduler) {
                        this.scheduler = scheduler;
                    }
                    StoreOccasionAvailabilityService.prototype.GetTasksInRange = function (start, end) {
                        var occurences = this.scheduler.occurrencesInRange(start, end);
                        return occurences;
                    };
                    StoreOccasionAvailabilityService.prototype.GetTasksByResource = function (start, end, task) {
                        var context = {
                            start: start,
                            end: end,
                            task: task
                        };
                        var currentTasks = this.GetTasksInRange(start, end)
                            .filter(function (e) { return e.Id !== task.Id && e.RecurrenceId !== task.Id; });
                        var startCheck = start.toLocaleTimeString();
                        var endCheck = end.toLocaleTimeString();
                        MyAndromeda.Logger.Notify("startCheck : " + startCheck + " | endCheck: " + endCheck);
                        MyAndromeda.Logger.Notify(context);
                        MyAndromeda.Logger.Notify("Tasks in range: " + currentTasks.length);
                        MyAndromeda.Logger.Notify(currentTasks);
                        var matchedOccurences = [];
                        var flagResource = [];
                        //let allResources = [
                        //    Models.occasionDefinitions.Delivery,
                        //    Models.occasionDefinitions.Collection,
                        //    Models.occasionDefinitions.DineIn
                        //];
                        var occasionTypeIsString = typeof (task.Occasions) === "string" ? true : false;
                        var taskResources = task.Occasions
                            ? occasionTypeIsString
                                ? task.Occasions.split(',')
                                : task.Occasions
                            : [];
                        MyAndromeda.Logger.Notify("check resources: ");
                        MyAndromeda.Logger.Notify(taskResources);
                        var map = currentTasks.map(function (e) {
                            return {
                                task: e,
                                occasion: e.Occasions
                            };
                        });
                        Rx.Observable.fromArray(map).where(function (e) {
                            for (var i = 0; i < e.occasion.length; i++) {
                                var compareOccasion = e.occasion[i];
                                for (var k = 0; k < taskResources.length; k++) {
                                    var occasion = taskResources[k];
                                    if (occasion.indexOf(compareOccasion) > -1) {
                                        MyAndromeda.Logger.Notify("task objection: " + e.task.title);
                                        MyAndromeda.Logger.Notify(e.task);
                                        return true;
                                    }
                                }
                            }
                            return false;
                        }).subscribe(function (overlaped) {
                            matchedOccurences.push(overlaped.task);
                        });
                        MyAndromeda.Logger.Notify("occurrences: " + matchedOccurences.length);
                        return matchedOccurences.length === 0;
                    };
                    StoreOccasionAvailabilityService.prototype.IsOccasionAvailable = function (start, end, task) {
                        return this.GetTasksByResource(start, end, task);
                    };
                    return StoreOccasionAvailabilityService;
                })();
                var StoreOccasionSchedulerService = (function () {
                    function StoreOccasionSchedulerService($http, uuidService, SweetAlert) {
                        this.$http = $http;
                        this.uuidService = uuidService;
                        this.SweetAlert = SweetAlert;
                    }
                    StoreOccasionSchedulerService.prototype.CreateDataSource = function () {
                        var _this = this;
                        var schema = {
                            data: "Data",
                            total: "Total",
                            model: OpeningHours.Models.getSchedulerDataSourceSchema()
                        };
                        var dataSource = new kendo.data.SchedulerDataSource({
                            batch: false,
                            transport: {
                                read: function (options) {
                                    MyAndromeda.Logger.Notify("Scheduler read");
                                    var route = "/api/chain/{0}/store/{1}/Occasions";
                                    route = kendo.format(route, OpeningHours.settings.chainId, OpeningHours.settings.andromedaSiteId);
                                    var promise = _this.$http.post(route, options.data);
                                    promise.then(function (callback) {
                                        options.success(callback.data);
                                    });
                                },
                                update: function (options) {
                                    MyAndromeda.Logger.Notify("Scheduler update");
                                    var route = "/api/chain/{0}/store/{1}/update-occasion";
                                    route = kendo.format(route, OpeningHours.settings.chainId, OpeningHours.settings.andromedaSiteId);
                                    var promise = _this.$http.post(route, options.data);
                                    promise.then(function (callback) {
                                        options.success();
                                    });
                                },
                                create: function (options) {
                                    MyAndromeda.Logger.Notify("Scheduler create");
                                    MyAndromeda.Logger.Notify(options.data);
                                    var route = "/api/chain/{0}/store/{1}/update-occasion";
                                    route = kendo.format(route, OpeningHours.settings.chainId, OpeningHours.settings.andromedaSiteId);
                                    var promise = _this.$http.post(route, options.data);
                                    promise.then(function (callback) {
                                        MyAndromeda.Logger.Notify("Create response:");
                                        MyAndromeda.Logger.Notify(callback.data);
                                        options.success(callback.data);
                                    });
                                },
                                destroy: function (options) {
                                    var route = "/api/chain/{0}/store/{1}/delete-occasion";
                                    route = kendo.format(route, OpeningHours.settings.chainId, OpeningHours.settings.andromedaSiteId);
                                    var promise = _this.$http.post(route, options.data);
                                    promise.then(function (callback) {
                                        options.success(callback.data);
                                    });
                                }
                            },
                            schema: schema
                        });
                        return dataSource;
                    };
                    StoreOccasionSchedulerService.prototype.CreateResources = function () {
                        var resources = [
                            {
                                title: "Occasion",
                                field: "Occasions",
                                multiple: true,
                                dataSource: [
                                    {
                                        text: OpeningHours.Models.occasionDefinitions.Delivery.Name,
                                        value: OpeningHours.Models.occasionDefinitions.Delivery.Name,
                                        color: OpeningHours.Models.occasionDefinitions.Delivery.Colour
                                    },
                                    {
                                        text: OpeningHours.Models.occasionDefinitions.Collection.Name,
                                        value: OpeningHours.Models.occasionDefinitions.Collection.Name,
                                        color: OpeningHours.Models.occasionDefinitions.Collection.Colour
                                    },
                                    {
                                        text: OpeningHours.Models.occasionDefinitions.DineIn.Name,
                                        value: OpeningHours.Models.occasionDefinitions.DineIn.Name,
                                        color: OpeningHours.Models.occasionDefinitions.DineIn.Colour
                                    }
                                ]
                            },
                        ];
                        return resources;
                    };
                    StoreOccasionSchedulerService.prototype.CreateScheduler = function () {
                        var _this = this;
                        var uuidService = this.uuidService;
                        var dataSource = this.CreateDataSource();
                        var schedulerOptions = {
                            date: new Date(),
                            majorTick: 60,
                            minorTickCount: 1,
                            workWeekStart: 0,
                            workWeekEnd: 6,
                            allDaySlot: true,
                            dataSource: dataSource,
                            timezone: "Europe/London",
                            currentTimeMarker: {
                                useLocalTimezone: false
                            },
                            editable: {
                                template: "<occasion-task-editor task='dataItem'></occasion-task-editor>"
                            },
                            pdf: {
                                fileName: "Opening hours",
                                title: "Schedule"
                            },
                            eventTemplate: "<occasion-task task='dataItem'></occasion-task>",
                            toolbar: ["pdf"],
                            showWorkHours: false,
                            resources: this.CreateResources(),
                            views: [
                                { type: "week", selected: true, showWorkHours: false },
                                { type: "month" }
                            ],
                            resize: function (e) {
                                var tester = new StoreOccasionAvailabilityService(e.sender);
                                if (!tester.IsOccasionAvailable(e.start, e.end, e.event)) {
                                    MyAndromeda.Logger.Notify("cancel resize");
                                    this.wrapper.find(".k-marquee-color").addClass("invalid-slot");
                                    e.preventDefault();
                                }
                            },
                            resizeEnd: function (e) {
                                MyAndromeda.Logger.Notify("resize-end");
                                var tester = new StoreOccasionAvailabilityService(e.sender);
                                if (!tester.IsOccasionAvailable(e.start, e.end, e.event)) {
                                    MyAndromeda.Logger.Notify("cancel resize");
                                    _this.SweetAlert.swal("Sorry", "A occasion already exists for this occasion in this range.", "error");
                                    e.preventDefault();
                                }
                            },
                            move: function (e) {
                                MyAndromeda.Logger.Notify("move-start");
                                MyAndromeda.Logger.Notify(e);
                                var tester = new StoreOccasionAvailabilityService(e.sender);
                                if (!tester.IsOccasionAvailable(e.start, e.end, e.event)) {
                                    this.wrapper.find(".k-event-drag-hint").addClass("invalid-slot");
                                }
                            },
                            moveEnd: function (e) {
                                MyAndromeda.Logger.Notify("move-end");
                                var tester = new StoreOccasionAvailabilityService(e.sender);
                                if (!tester.IsOccasionAvailable(e.start, e.end, e.event)) {
                                    MyAndromeda.Logger.Notify("cancel move");
                                    _this.SweetAlert.swal("Sorry", "A occasion already exists for this occasion in this range.", "error");
                                    e.preventDefault();
                                }
                            },
                            add: function (e) {
                                MyAndromeda.Logger.Notify("add");
                                MyAndromeda.Logger.Notify(e);
                                var tester = new StoreOccasionAvailabilityService(e.sender);
                                if (!tester.IsOccasionAvailable(e.event.start, e.event.end, e.event)) {
                                    MyAndromeda.Logger.Notify("cancel add");
                                    //SweetAlert.swal("Sorry!", name + " has been saved.", "success");
                                    _this.SweetAlert.swal("Sorry", "A occasion already exists for this occasion in this range.", "error");
                                    e.preventDefault();
                                }
                            },
                            save: function (e) {
                                MyAndromeda.Logger.Notify("save");
                                MyAndromeda.Logger.Notify(e);
                                var ev = e.event;
                                if (ev.Occasions) {
                                    var o = ev.Occasions.length;
                                    if (o.length === 0) {
                                        _this.SweetAlert.swal("occasions", "Please add at least one occasion", "information");
                                    }
                                }
                                var tester = new StoreOccasionAvailabilityService(e.sender);
                                if (!tester.IsOccasionAvailable(e.event.start, e.event.end, e.event)) {
                                    MyAndromeda.Logger.Notify("cancel save");
                                    _this.SweetAlert.swal("Sorry", "A task already exists for this occasion in this range.", "error");
                                    e.preventDefault();
                                }
                            }
                        };
                        return schedulerOptions;
                    };
                    return StoreOccasionSchedulerService;
                })();
                Services.StoreOccasionSchedulerService = StoreOccasionSchedulerService;
                app.service("storeOccasionSchedulerService", StoreOccasionSchedulerService);
            })(Services = OpeningHours.Services || (OpeningHours.Services = {}));
        })(OpeningHours = Stores.OpeningHours || (Stores.OpeningHours = {}));
    })(Stores = MyAndromeda.Stores || (MyAndromeda.Stores = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var Stores;
    (function (Stores) {
        var OpeningHours;
        (function (OpeningHours) {
            OpeningHours.settings = {
                chainId: 0,
                andromedaSiteId: 0
            };
            var app = angular.module("MyAndromeda.Stores.OpeningHours", [
                "MyAndromeda.Store.OpeningHours.Config",
                "MyAndromeda.Core",
                "MyAndromeda.Resize",
                "MyAndromeda.Progress",
                "ngAnimate",
                "ui.bootstrap"
            ]);
            app.run(function ($templateCache) {
                MyAndromeda.Logger.Notify("Started Opening Hours");
                angular
                    .element('script[type="text/ng-template"]')
                    .each(function (i, element) {
                    $templateCache.put(element.id, element.innerHTML);
                });
            });
            function boostrap(element, chainId, andromedaSiteId) {
                OpeningHours.settings.chainId = chainId;
                OpeningHours.settings.andromedaSiteId = andromedaSiteId;
                var e = $(element);
                angular.bootstrap(e, ["MyAndromeda.Stores.OpeningHours"]);
            }
            OpeningHours.boostrap = boostrap;
        })(OpeningHours = Stores.OpeningHours || (Stores.OpeningHours = {}));
    })(Stores = MyAndromeda.Stores || (MyAndromeda.Stores = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var Users;
    (function (Users) {
        var Models;
        (function (Models) {
            var User = (function () {
                function User(id, userName, firstName, surName, roles) {
                    this.Id = id;
                    this.Username = userName;
                    this.FirstName = firstName;
                    this.SurName = surName;
                    if (roles) {
                        this.Roles = roles;
                    }
                }
                return User;
            })();
            Models.User = User;
        })(Models = Users.Models || (Users.Models = {}));
    })(Users = MyAndromeda.Users || (MyAndromeda.Users = {}));
})(MyAndromeda || (MyAndromeda = {}));
//module MyAndromeda
//{
//    export module Services
//    {
//        export class UserService implements IUserService {
//            private serviceRoutes: IUserServiceRoutes;
//            public dataSource: kendo.data.DataSource;
//            constructor(serviceRoutes: IUserServiceRoutes)
//            {
//                this.serviceRoutes = serviceRoutes;
//            }
//            public findById(id: number): IUser {
//                var internal = this;
//                var user = <IUser>internal.dataSource.data().find(function (item, index, source) {
//                    return item.Id === id;
//                });
//                return user;
//            }
//            public findByUserName(userName: string): IUser {
//                var internal = this;
//                var user = <IUser>internal.dataSource.data().find(function (item, index, source) {
//                    return item.UserName === userName;
//                });
//                return user;
//            }
//            private initDataSource() : void {
//                var internal = this;
//                var data = {
//                    __RequestVerificationToken: internal.serviceRoutes.antifrorgeryToken
//                };
//                this.dataSource = new kendo.data.DataSource({
//                    transport: {
//                        read: internal.serviceRoutes.read,
//                        data: data
//                    }
//                });
//            } 
//            public init(): void {
//                this.initDataSource();
//            }
//        }
//    }
//}
//interface IUserServiceRoutes
//{
//    read: string;
//    antifrorgeryToken: string;
//    //readByChain: string;
//    //readByStore: string;
//} 
var MyAndromeda;
(function (MyAndromeda) {
    var Validation;
    (function (Validation) {
        var PasswordValidator = (function () {
            function PasswordValidator(options) {
                this.options = options;
                this.BuildProgress();
                this.SeekEvents();
            }
            PasswordValidator.prototype.BuildProgress = function () {
                var internal = this;
                this.progressBar = $(this.options.ProgressElement).kendoProgressBar({
                    change: internal.OnProgressChange,
                    max: 9,
                    min: -1,
                    value: 0,
                    animation: true,
                    type: "value"
                }).data("kendoProgressBar");
                this.progressBar.progressStatus.text("Empty");
            };
            PasswordValidator.prototype.SeekEvents = function () {
                var internal = this;
                $(this.options.InputElement).on("change keyup", function (e) {
                    //validation logic?
                    var $object = $(this);
                    var length = $object.length;
                    var password = $object.val();
                    var pattern = "^.*(?=.{6,})(?=.*[a-z])(?=.*[A-Z])(?=.*[\d\W]).*$", patterns = PasswordValidator.Patterns, banned = PasswordValidator.BannedList;
                    var matches = new Array();
                    patterns.forEach(function (item) {
                        matches.push(item.r.test(password) ? item.score : 0);
                    });
                    var matchCount = 0;
                    matches.forEach(function (value) {
                        matchCount += value;
                    });
                    if (banned.indexOf(password.toLowerCase()) > 0) {
                        internal.progressBar.value(-1);
                        //internal.progressBar.progressStatus.text(PasswordValidator.BannedPasswordMessage);
                        return;
                    }
                    internal.progressBar.value(matchCount);
                });
            };
            PasswordValidator.prototype.OnProgressChange = function (e) {
                var progressStatus = e.sender.progressStatus, progressWrapper = e.sender.progressWrapper;
                progressStatus.css({
                    "background-image": "none",
                    "border-image": "none"
                });
                if (e.value < 0) {
                    progressStatus.text(PasswordValidator.BannedPasswordMessage);
                }
                else if (e.value < 1) {
                    progressStatus.text(PasswordValidator.EmptyMessage);
                }
                else if (e.value <= 3) {
                    progressStatus.text(PasswordValidator.WeakMessage);
                    progressWrapper.css({
                        "background-color": "#EE9F05",
                        "border-color": "#EE9F05"
                    });
                }
                else if (e.value <= 6) {
                    progressStatus.text(PasswordValidator.GoodMessage);
                    progressWrapper.css({
                        "background-color": "#428bca",
                        "border-color": "#428bca"
                    });
                }
                else {
                    progressStatus.text(PasswordValidator.StrongMessage);
                    progressWrapper.css({
                        "background-color": "#8EBC00",
                        "border-color": "#8EBC00"
                    });
                }
            };
            PasswordValidator.BannedPasswordMessage = "Banned or invalid password";
            PasswordValidator.EmptyMessage = "Empty";
            PasswordValidator.WeakMessage = "Weak";
            PasswordValidator.GoodMessage = "Good";
            PasswordValidator.StrongMessage = "Strong";
            PasswordValidator.Patterns = [
                {
                    name: "length1",
                    r: new RegExp("(.{8,})"),
                    score: 2
                },
                {
                    name: "length2",
                    r: new RegExp("(.{12,})"),
                    score: 2
                },
                {
                    name: "length3",
                    r: new RegExp("(.{16,})"),
                    score: 3
                },
                {
                    name: "lowercasePattern",
                    r: new RegExp("[a-z]"),
                    score: 1
                },
                {
                    name: "uppercasePattern",
                    r: new RegExp("[A-Z]"),
                    score: 1
                },
                {
                    name: "digitPattern",
                    r: new RegExp("[0-9]"),
                    score: 1
                },
                {
                    name: "specialCharacter",
                    r: new RegExp(".[!@#$%^&*?_~]"),
                    score: 2
                }
            ];
            PasswordValidator.BannedList = [
                '1234',
                '12345',
                '123456',
                '1234567',
                '12345678',
                '123456789',
                'password',
                'pass',
                'pass1',
                'pass12',
                'pass123',
                'pass1234',
                'iloveyou',
                'princess',
                'rockyou',
                '12345678',
                'abc123',
                'babygirl',
                'monkey',
                'lovely',
                '654321',
                'qwerty',
                'qwerty1234',
                'password1',
                'welcome',
                'welcome1',
                'password2',
                'password01',
                'password3',
                'p@ssw0rd',
                'passw0rd',
                'password4',
                'password123',
                'summer09',
                'password6',
                'password7',
                'password9',
                'password8',
                'welcome2',
                'welcome01',
                'winter12',
                'spring2012',
                'summer',
                'winter',
                'password123456789',
                'password12345678910',
                'password1234567890'
            ];
            return PasswordValidator;
        })();
        Validation.PasswordValidator = PasswordValidator;
    })(Validation = MyAndromeda.Validation || (MyAndromeda.Validation = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var Validation;
    (function (Validation) {
        //http://docs.telerik.com/kendo-ui/getting-started/framework/validator/overview#pattern--constrains-the-value-to-match-a-specific-regular-expression
        var MyAndromedaValidation = (function () {
            function MyAndromedaValidation() {
            }
            MyAndromedaValidation.CreateValidator = function (element, options) {
                //var optionRules = $.extend({}, MyAndromedaValidation.Rules, options.rules)
                if (options.rules) {
                    options.rules = $.extend({}, MyAndromedaValidation.Options.rules, options.rules);
                }
                var $e = $(element);
                return $e.kendoValidator(options).data("kendovalidator");
            };
            MyAndromedaValidation.Options = {
                rules: {}
            };
            MyAndromedaValidation.ActiveValidators = new Array();
            return MyAndromedaValidation;
        })();
        Validation.MyAndromedaValidation = MyAndromedaValidation;
        /* automatically hook up all forms with a validator */
        $(function () {
            console.log("setting up validators");
            var forms = document.getElementsByTagName("form");
            for (var i = 0; i < forms.length; i++) {
                var $f = $(forms[i]);
                if ($f.data("ignore")) {
                    return;
                }
                var validator = MyAndromedaValidation.CreateValidator($f, {});
            }
            console.log("setup " + forms.length + " validators");
        });
    })(Validation = MyAndromeda.Validation || (MyAndromeda.Validation = {}));
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    var Vouchers;
    (function (Vouchers) {
        var Services;
        (function (Services) {
            var VoucherListService = (function () {
                function VoucherListService() {
                    var internal = this;
                    this.vm = kendo.observable({
                        activeOptions: [
                            { name: VoucherListService.ActiveLabel, value: "true" },
                            { name: VoucherListService.InActiveLabel, value: "false" }
                        ],
                        selectedActiveOption: "true",
                        searchText: "",
                        filters: {
                            byNameDescription: { value: "", active: false },
                            byStatus: { value: "", active: false }
                        },
                        resetFilters: function (e) {
                            //e.preventDefault();
                            internal.ResetFilters();
                        }
                    });
                }
                VoucherListService.prototype.Init = function () {
                    var internal = this;
                    this.vm.bind("change", function (field) {
                        internal.VmChanged();
                    });
                    kendo.bind("#filters", this.vm);
                    this.VmChanged();
                };
                VoucherListService.prototype.ResetFilters = function () {
                    var datasource = this.GetGridDatasource(), vm = this.vm;
                    vm.set("searchText", "");
                    vm.set("selectedActiveOption", "true");
                    datasource.filter([]);
                    var a;
                };
                VoucherListService.prototype.VmChanged = function () {
                    var vm = this.vm;
                    var a = {
                        text: vm.get("searchText"),
                        option: vm.get("selectedActiveOption")
                    };
                    var gridDatasource = this.GetGridDatasource();
                    //[{ field="VoucherCode", operator="eq", value="monday"}, Object { field="IsActive", operator="eq", value=true}]
                    //your not working .. why?
                    var f = {
                        logic: "and",
                        filters: [
                            this.GetDatasourceFilterItemForSearch(a.text),
                            this.GetDatasourceFilterItemForStatus(a.option)
                        ]
                    };
                    console.log(f);
                    gridDatasource.filter(f);
                };
                VoucherListService.prototype.GetDatasourceFilterItemForSearch = function (searchText) {
                    //http://docs.telerik.com/kendo-ui/api/framework/datasource
                    //search code or description
                    var f = {
                        logic: "or",
                        filters: [
                            {
                                field: "VoucherCode",
                                operator: "contains",
                                value: searchText
                            },
                            {
                                field: "Description",
                                operator: "contains",
                                value: searchText
                            }
                        ]
                    };
                    return f;
                };
                VoucherListService.prototype.GetDatasourceFilterItemForStatus = function (value) {
                    var vm = this.vm;
                    var filterValue = value === "true" ? true : false;
                    return {
                        field: "IsActive",
                        operator: "eq",
                        value: filterValue
                    };
                };
                VoucherListService.prototype.GetGridDatasource = function () {
                    var grid, datasource;
                    grid = $("#VouchersList").data("kendoGrid");
                    datasource = grid.dataSource;
                    return datasource;
                };
                VoucherListService.ActiveLabel = "Active";
                VoucherListService.InActiveLabel = "Inactive";
                return VoucherListService;
            })();
            Services.VoucherListService = VoucherListService;
        })(Services = Vouchers.Services || (Vouchers.Services = {}));
    })(Vouchers = MyAndromeda.Vouchers || (MyAndromeda.Vouchers = {}));
})(MyAndromeda || (MyAndromeda = {}));
/// <reference path="../general/resizemodule.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var WebHooks;
    (function (WebHooks) {
        var Tests;
        (function (Tests) {
            var moduleName = "MyAndromeda.WebHook.Tests";
            var m = angular.module(moduleName, [
                "MyAndromeda.Resize",
                "MyAndromeda.Progress",
                "ngRoute",
                "ngAnimate",
                "kendo.directives"
            ]);
            m.run(function ($templateCache) {
                MyAndromeda.Logger.Notify("WebHook tests Started");
                angular.element('script[type="text/template"]').each(function (i, element) {
                    $templateCache.put(element.id, element.innerHTML);
                });
            });
            m.config(function ($routeProvider) {
                $routeProvider.when('/', {
                    templateUrl: "start.html",
                    controller: "StartController"
                });
                $routeProvider.when('/store-status/:andromedaSiteId', {
                    templateUrl: "store-status.html",
                    controller: "StoreStatusController"
                });
                $routeProvider.when('/order-status/:andromedaSiteId', {
                    templateUrl: "order-status.html",
                    controller: "OrderStatusController"
                });
                $routeProvider.when('/menu-version/:andromedaSiteId', {
                    templateUrl: "menu-version.html",
                    controller: "MenuVersionController"
                });
                $routeProvider.when('/delivery-time/:andromedaSiteId', {
                    templateUrl: "delivery-time.html",
                    controller: "DeliveryTimeController"
                });
            });
            m.controller("DeliveryTimeController", function ($scope, $routeParams, webHookTestService, progressService) {
                var settings = {
                    Edt: null
                };
                $scope.settings = settings;
                $scope.send = function (value) {
                    progressService.Show();
                    var request = webHookTestService.DeliveryTime({
                        AndromedaSiteId: $routeParams.andromedaSiteId,
                        Source: "test",
                        Edt: settings.Edt
                    });
                    Rx.Observable.fromPromise(request).subscribe(function () {
                        $scope.settings.messages = "Sent! No problems.";
                        progressService.Hide();
                    }, function (ex) {
                        $scope.settings.messages = "Error";
                        progressService.Hide();
                    }, function () {
                        progressService.Hide();
                    });
                };
            });
            m.controller("MenuVersionController", function ($scope, $routeParams, webHookTestService, progressService) {
                var settings = {
                    Version: null
                };
                $scope.settings = settings;
                $scope.send = function (value) {
                    progressService.Show();
                    var request = webHookTestService.MenuVersion({
                        AndromedaSiteId: $routeParams.andromedaSiteId,
                        Source: "test",
                        Version: settings.Version
                    });
                    Rx.Observable.fromPromise(request).subscribe(function () {
                        $scope.settings.messages = "Sent! No problems.";
                        progressService.Hide();
                    }, function (ex) {
                        $scope.settings.messages = "Error";
                        progressService.Hide();
                    }, function () {
                        progressService.Hide();
                    });
                };
            });
            m.controller("StartController", function ($scope, $http, $timeout, resizeService, progressService) {
                MyAndromeda.Logger.Notify("start");
                var resizeSubscription = resizeService.ResizeObservable.subscribe(function (e) {
                    //do i have anything to resize
                });
                var dataSource = new kendo.data.DataSource({
                    "transport": {
                        "read": { "url": "/api/Store" },
                    },
                    "schema": { "errors": "Errors" }
                });
                $scope.storeDataSource = dataSource;
                $scope.settings = {
                    AndromedaSiteId: null
                };
            });
            m.controller("StoreStatusController", function ($scope, $routeParams, webHookTestService, progressService) {
                var element = document.getElementById("WebhookTest");
                progressService.Create(element);
                $scope.settings = {};
                $scope.send = function (value) {
                    progressService.Show();
                    var request = webHookTestService.OnlineStateTest({
                        AndromedaSiteId: $routeParams.andromedaSiteId,
                        Online: value,
                        Source: "test"
                    });
                    Rx.Observable.fromPromise(request).subscribe(function () {
                        $scope.settings.messages = "Sent! No problems.";
                        progressService.Hide();
                    }, function (ex) {
                        $scope.settings.messages = "Error";
                        progressService.Hide();
                    }, function () {
                        progressService.Hide();
                    });
                };
            });
            m.controller("OrderStatusController", function ($scope, $routeParams, progressService, webHookTestService) {
                var orderDataSource = new kendo.data.DataSource({
                    "transport": {
                        "read": {
                            "url": "/api/GprsOrders",
                            "data": {
                                andromedaSiteId: $routeParams.andromedaSiteId
                            }
                        }
                    }
                });
                var acsDataSource = new kendo.data.DataSource({
                    transport: {
                        read: {
                            url: function () { return "/api/acs/list/" + $routeParams.andromedaSiteId; },
                            "type": "GET"
                        }
                    }
                });
                var element = document.getElementById("WebhookTest");
                progressService.Create(element);
                var settings = {
                    Status: null,
                    StatusDescription: null
                };
                $scope.orderDataSource = orderDataSource;
                $scope.acsDataSource = acsDataSource;
                $scope.settings = settings;
                //jQuery(function () { jQuery("#Order_id").kendoComboBox({ 
                //"dataSource": { "transport": { "read": { "url": "/api/GprsOrders", "data": filterOrders }, "prefix": "" }, "serverFiltering": true, "filter": [], "schema": { "errors": "Errors" } }, "dataTextField": "RamesesOrderNum", "dataValueField": "RamesesOrderNum", "cascadeFrom": "andromedaSiteId" }); });
                $scope.send = function (value) {
                    progressService.Show();
                    var request = webHookTestService.OrderStatusTest({
                        AndromedaSiteId: $routeParams.andromedaSiteId,
                        ExternalOrderId: $scope.settings.Order.ExternalOrderRef,
                        //ExternalAcsApplicationId: $scope.settings.AcsApplication.ExternalApplicationId,
                        AcsApplicationId: $scope.settings.AcsApplication ? $scope.settings.AcsApplication.Id : null,
                        Source: "test",
                        Status: settings.Status,
                        StatusDescription: settings.StatusDescription
                    });
                    Rx.Observable.fromPromise(request).subscribe(function () {
                        $scope.settings.messages = "Sent! No problems.";
                        progressService.Hide();
                    }, function (ex) {
                        $scope.settings.messages = "Error";
                        progressService.Hide();
                    }, function () {
                        progressService.Hide();
                    });
                };
            });
            var WebHookTestService = (function () {
                function WebHookTestService($http) {
                    this.orderStatusUrl = "/web-hooks/store/orders/update-order-status";
                    this.storeStatusUrl = "/web-hooks/store/update-status";
                    this.updateDeliveryTime = "/web-hooks/store/update-estimated-delivery-time";
                    this.updateMenuChange = "/web-hooks/store/update-menu";
                    this.$http = $http;
                    MyAndromeda.Logger.Notify("Where am i");
                }
                WebHookTestService.prototype.OnlineStateTest = function (model) {
                    var promise = this.$http.post(this.storeStatusUrl, model);
                    return promise;
                };
                WebHookTestService.prototype.OrderStatusTest = function (model) {
                    var promise = this.$http.post(this.orderStatusUrl, model);
                    return promise;
                };
                WebHookTestService.prototype.MenuVersion = function (model) {
                    var promise = this.$http.post(this.updateMenuChange, model);
                    return promise;
                };
                WebHookTestService.prototype.DeliveryTime = function (model) {
                    var promise = this.$http.post(this.updateDeliveryTime, model);
                    return promise;
                };
                return WebHookTestService;
            })();
            Tests.WebHookTestService = WebHookTestService;
            m.service("webHookTestService", WebHookTestService);
            //"Store Online Status", "EDT", "Menu Version", "Order Status"
            function Setup(id) {
                var element = document.getElementById(id);
                angular.bootstrap(element, [moduleName]);
            }
            Tests.Setup = Setup;
        })(Tests = WebHooks.Tests || (WebHooks.Tests = {}));
    })(WebHooks = MyAndromeda.WebHooks || (MyAndromeda.WebHooks = {}));
})(MyAndromeda || (MyAndromeda = {}));
/// <reference path="../general/resizemodule.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    var WebHooks;
    (function (WebHooks) {
        var moduleName = "MyAndromeda.WebHooks";
        var m = angular.module(moduleName, [
            "MyAndromeda.Resize",
            "MyAndromeda.Progress",
            "ngRoute",
            "ngAnimate",
            "kendo.directives"
        ]);
        m.run(function ($templateCache) {
            MyAndromeda.Logger.Notify("WebHooks Started");
            angular.element('script[type="text/template"]').each(function (i, element) {
                $templateCache.put(element.id, element.innerHTML);
            });
        });
        m.config(function ($routeProvider) {
            $routeProvider.when('/', {
                templateUrl: "start.html",
                controller: "StartControler"
            });
        });
        m.controller("StartControler", function ($scope, $timeout, resizeService, progressService, webHookService, webHookTypes) {
            MyAndromeda.Logger.Notify("start");
            var resizeSubscription = resizeService.ResizeObservable.subscribe(function (e) {
                //do i have anything to resize
            });
            var element = document.getElementById("WebHooks");
            progressService.Create(element).Show();
            var globalSettings = null;
            var settingsPromise = webHookService.Read();
            var settingsObservable = Rx.Observable
                .fromPromise(settingsPromise);
            var refresh = function (settings) {
                globalSettings = settings;
                MyAndromeda.Logger.Notify("settings:");
                MyAndromeda.Logger.Notify(globalSettings);
                $scope.webHookTypes = webHookTypes;
                var t = Rx.Observable.fromArray(webHookTypes);
                t.subscribe(function (setting) {
                    MyAndromeda.Logger.Notify(setting);
                    //prepare the settings
                    if (!globalSettings[setting.Key]) {
                        globalSettings[setting.Key] = [];
                    }
                }, function (ex) { }, function () {
                    $timeout(function () {
                        MyAndromeda.Logger.Notify("new settings");
                        MyAndromeda.Logger.Notify(globalSettings);
                        $scope.settings = globalSettings;
                        progressService.Hide();
                    });
                });
            };
            $scope.getGroupNameFromKey = function (key) {
                var find = webHookTypes.filter(function (e) { return e.Key === key; });
                if (find.length === 0) {
                    return key + " not found";
                }
                return find[0].Name;
            };
            $scope.add = function (key) {
                MyAndromeda.Logger.Notify("add to: " + key);
                var group = $scope.settings[key];
                var model = {
                    Name: "Default",
                    CallBackUrl: "",
                    RequestHeaders: {},
                    Enabled: true
                };
                group.push(model);
            };
            $scope.update = function () {
                progressService.Show();
                var promise = webHookService.Update(globalSettings);
                Rx.Observable.fromPromise(promise).subscribe(function () { }, function (ex) {
                    progressService.Hide();
                }, function () {
                    progressService.Hide();
                });
            };
            $scope.remove = function (key, subscription) {
                var group = globalSettings[key];
                globalSettings[key] = group.filter(function (e) { return e !== subscription; });
            };
            settingsObservable
                .subscribe(function (response) {
                MyAndromeda.Logger.Notify(response.data);
                //settings = response.data;
                refresh(response.data);
            }, function (ex) {
                MyAndromeda.Logger.Error(ex);
            }, function () {
            });
        });
        var WebHookService = (function () {
            function WebHookService($http) {
                this.$http = $http;
                MyAndromeda.Logger.Notify("Where am i");
            }
            WebHookService.prototype.Read = function () {
                var route = WebHookService.readRoute + WebHookService.acsApplicationId;
                var promise = this.$http.get(route);
                return promise;
            };
            WebHookService.prototype.Update = function (data) {
                var route = WebHookService.updateRoute + WebHookService.acsApplicationId;
                var promise = this.$http.post(route, data);
                return promise;
            };
            WebHookService.readRoute = "";
            WebHookService.updateRoute = "";
            WebHookService.acsApplicationId = "";
            return WebHookService;
        })();
        WebHooks.WebHookService = WebHookService;
        m.service("webHookService", WebHookService);
        //"Store Online Status", "EDT", "Menu Version", "Order Status"
        var storeStatus = { Key: "StoreOnlineStatus", Name: "Store Online Status" };
        var estimatedDeliveryTime = { Key: "Edt", Name: "EDT" };
        var menuVersion = { Key: "MenuVersion", Name: "Menu Version" };
        var menuItems = { Key: "MenuItems", Name: "Menu Items" };
        var orderStatus = { Key: "OrderStatus", Name: "Order Status" };
        var bringg = { Key: "BringUpdates", Name: "Bringg" };
        var bringgEta = { Key: "BringEtaUpdates", Name: "Bringg ETA" };
        var webHookTypes = [
            storeStatus,
            estimatedDeliveryTime,
            menuVersion,
            menuItems,
            orderStatus,
            bringg,
            bringgEta
        ];
        m.constant("webHookTypes", webHookTypes);
        function Setup(id) {
            var element = document.getElementById(id);
            angular.bootstrap(element, [moduleName]);
        }
        WebHooks.Setup = Setup;
    })(WebHooks = MyAndromeda.WebHooks || (MyAndromeda.WebHooks = {}));
})(MyAndromeda || (MyAndromeda = {}));
//# sourceMappingURL=MyAndromeda.App.All.js.map