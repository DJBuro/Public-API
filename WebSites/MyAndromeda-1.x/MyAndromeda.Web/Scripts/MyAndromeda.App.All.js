var MyAndromeda;
(function (MyAndromeda) {
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
    })(MyAndromeda.Menu || (MyAndromeda.Menu = {}));
    var Menu = MyAndromeda.Menu;
})(MyAndromeda || (MyAndromeda = {}));
/// <reference path="../Menu/MyAndromeda.Menu.Logger.ts" />
/// <reference path="../../Scripts/typings/angularjs/angular-route.d.ts" />
/// <reference path="../../Scripts/typings/angularjs/angular.d.ts" />
var MyAndromeda;
(function (MyAndromeda) {
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
                    "kendo.directives",
                    "ngAnimate"
                ]);

                WebOrdering.Logger.Notify("Assign " + Angular.ServicesInitilizations.length + " services");
                Angular.ServicesInitilizations.forEach(function (value) {
                    value(app);
                });

                WebOrdering.Logger.Notify("Assign " + Angular.ControllersInitilizations.length + " controllers");
                Angular.ControllersInitilizations.forEach(function (value) {
                    value(app);
                });

                //app.config([
                //    Services.ContextService.Name,
                //    function(contextService){
                //        //stuff
                //    }
                //]);
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
    })(MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
    var WebOrdering = MyAndromeda.WebOrdering;
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    (function (WebOrdering) {
        /// <reference path="MyAndromeda.WebOrdering.App.ts" />
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
                    var settingsSubscription = contextService.ModelSubject.where(function (e) {
                        return e !== null;
                    }).subscribe(function (websiteSettings) {
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
        })(WebOrdering.Controllers || (WebOrdering.Controllers = {}));
        var Controllers = WebOrdering.Controllers;
    })(MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
    var WebOrdering = MyAndromeda.WebOrdering;
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    (function (WebOrdering) {
        /// <reference path="MyAndromeda.WebOrdering.App.ts" />
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
                            } else {
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
                    var settingsSubscription = contextService.ModelSubject.where(function (e) {
                        return e !== null;
                    }).subscribe(function (websiteSettings) {
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
        })(WebOrdering.Controllers || (WebOrdering.Controllers = {}));
        var Controllers = WebOrdering.Controllers;
    })(MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
    var WebOrdering = MyAndromeda.WebOrdering;
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    (function (WebOrdering) {
        /// <reference path="MyAndromeda.WebOrdering.App.ts" />
        (function (Controllers) {
            WebOrdering.Angular.ControllersInitilizations.push(function (app) {
                app.controller(CustomEmailTemplateController.Name, [
                    '$scope', '$timeout',
                    WebOrdering.Services.ContextService.Name,
                    WebOrdering.Services.WebOrderingWebApiService.Name,
                    function ($scope, $timeout, contextService, webOrderingWebApiService) {
                        CustomEmailTemplateController.OnLoad($scope, $timeout, contextService, webOrderingWebApiService);

                        /* going to leave kendo to manage the observable object */
                        CustomEmailTemplateController.SetupKendoMvvm($scope, $timeout, contextService);
                    }
                ]);
            });

            var CustomEmailTemplateController = (function () {
                function CustomEmailTemplateController() {
                }
                CustomEmailTemplateController.OnLoad = function ($scope, $timout, contextService, webOrderingWebApiService) {
                    $scope.ResetColors = function () {
                        console.log("Resetting email template colors..");
                        contextService.Model.CustomEmailTemplate.HeaderColour = contextService.Model.CustomEmailTemplate.LiveHeaderColour;
                        contextService.Model.CustomEmailTemplate.FooterColour = contextService.Model.CustomEmailTemplate.LiveFooterColour;
                        var viewElement = $("#CustomEmailTemplateController");
                        kendo.bind(viewElement, contextService.Model.CustomEmailTemplate);
                    };

                    $scope.SaveChanges = function () {
                        console.log("save");
                        $scope.SetLiveColors();
                        webOrderingWebApiService.Update();
                        var viewElement = $("#CustomEmailTemplateController");
                        kendo.bind(viewElement, contextService.Model.CustomEmailTemplate);
                    };

                    $scope.SetLiveColors = function () {
                        contextService.Model.CustomEmailTemplate.LiveHeaderColour = contextService.Model.CustomEmailTemplate.HeaderColour;
                        contextService.Model.CustomEmailTemplate.LiveFooterColour = contextService.Model.CustomEmailTemplate.FooterColour;
                    };
                };

                CustomEmailTemplateController.SetupKendoMvvm = function ($scope, $timout, contextService) {
                    var settingsSubscription = contextService.ModelSubject.where(function (e) {
                        return e !== null;
                    }).subscribe(function (websiteSettings) {
                        var customEmailTemplate = websiteSettings.CustomEmailTemplate;
                        var correct = function (key, value) {
                            if (!customEmailTemplate.get(key)) {
                                customEmailTemplate.set(key, "#EEEEEE");
                            }
                        };
                        correct("HeaderColour", "HeaderColour");
                        correct("FooterColour", "FooterColour");
                        $scope.SetLiveColors();
                        var viewElement = $("#CustomEmailTemplateController");
                        kendo.bind(viewElement, websiteSettings.CustomEmailTemplate);
                        $timout(function () {
                        });
                    });

                    $scope.$on('$destroy', function () {
                        settingsSubscription.dispose();
                    });
                };
                CustomEmailTemplateController.Name = "CustomEmailTemplateController";
                return CustomEmailTemplateController;
            })();
            Controllers.CustomEmailTemplateController = CustomEmailTemplateController;
        })(WebOrdering.Controllers || (WebOrdering.Controllers = {}));
        var Controllers = WebOrdering.Controllers;
    })(MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
    var WebOrdering = MyAndromeda.WebOrdering;
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    (function (WebOrdering) {
        /// <reference path="MyAndromeda.WebOrdering.App.ts" />
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
                    var settingsSubscription = contextService.ModelSubject.where(function (e) {
                        return e !== null;
                    }).subscribe(function (websiteSettings) {
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
        })(WebOrdering.Controllers || (WebOrdering.Controllers = {}));
        var Controllers = WebOrdering.Controllers;
    })(MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
    var WebOrdering = MyAndromeda.WebOrdering;
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    (function (WebOrdering) {
        /// <reference path="MyAndromeda.WebOrdering.App.ts" />
        (function (Controllers) {
            WebOrdering.Angular.ControllersInitilizations.push(function (app) {
                app.controller(CustomThemeSettingsController.Name, [
                    '$scope', '$timeout',
                    WebOrdering.Services.ContextService.Name,
                    WebOrdering.Services.WebOrderingWebApiService.Name,
                    function ($scope, $timeout, contextService, webOrderingWebApiService) {
                        CustomThemeSettingsController.OnLoad($scope, $timeout, contextService, webOrderingWebApiService);

                        /* going to leave kendo to manage the observable object */
                        CustomThemeSettingsController.SetupKendoMvvm($scope, $timeout, contextService);
                    }
                ]);
            });

            var CustomThemeSettingsController = (function () {
                function CustomThemeSettingsController() {
                }
                CustomThemeSettingsController.OnLoad = function ($scope, $timout, contextService, webOrderingWebApiService) {
                    $scope.ResetColors = function () {
                        console.log("Resetting colors..");

                        contextService.Model.CustomThemeSettings.ColourRange1 = contextService.Model.CustomThemeSettings.LiveColourRange1;
                        contextService.Model.CustomThemeSettings.ColourRange2 = contextService.Model.CustomThemeSettings.LiveColourRange2;
                        contextService.Model.CustomThemeSettings.ColourRange3 = contextService.Model.CustomThemeSettings.LiveColourRange3;
                        contextService.Model.CustomThemeSettings.ColourRange4 = contextService.Model.CustomThemeSettings.LiveColourRange4;
                        contextService.Model.CustomThemeSettings.ColourRange5 = contextService.Model.CustomThemeSettings.LiveColourRange5;
                        contextService.Model.CustomThemeSettings.ColourRange6 = contextService.Model.CustomThemeSettings.LiveColourRange6;

                        var viewElement = $("#CustomThemeSettingsController");

                        kendo.bind(viewElement, contextService.Model.CustomThemeSettings);
                    };

                    $scope.SaveChanges = function () {
                        console.log("save");
                        $scope.SetLiveColors();
                        webOrderingWebApiService.Update();
                        var viewElement = $("#CustomThemeSettingsController");
                        kendo.bind(viewElement, contextService.Model.CustomThemeSettings);
                    };

                    $scope.SetLiveColors = function () {
                        contextService.Model.CustomThemeSettings.LiveColourRange1 = contextService.Model.CustomThemeSettings.ColourRange1;
                        contextService.Model.CustomThemeSettings.LiveColourRange2 = contextService.Model.CustomThemeSettings.ColourRange2;
                        contextService.Model.CustomThemeSettings.LiveColourRange3 = contextService.Model.CustomThemeSettings.ColourRange3;
                        contextService.Model.CustomThemeSettings.LiveColourRange4 = contextService.Model.CustomThemeSettings.ColourRange4;
                        contextService.Model.CustomThemeSettings.LiveColourRange5 = contextService.Model.CustomThemeSettings.ColourRange5;
                        contextService.Model.CustomThemeSettings.LiveColourRange6 = contextService.Model.CustomThemeSettings.ColourRange6;
                    };
                };

                CustomThemeSettingsController.SetupKendoMvvm = function ($scope, $timout, contextService) {
                    var settingsSubscription = contextService.ModelSubject.where(function (e) {
                        return e !== null;
                    }).subscribe(function (websiteSettings) {
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

                        $scope.SetLiveColors();
                        var viewElement = $("#CustomThemeSettingsController");
                        kendo.bind(viewElement, websiteSettings.CustomThemeSettings);

                        $timout(function () {
                        });
                    });

                    $scope.$on('$destroy', function () {
                        settingsSubscription.dispose();
                    });
                };
                CustomThemeSettingsController.Name = "CustomThemeSettingsController";
                return CustomThemeSettingsController;
            })();
            Controllers.CustomThemeSettingsController = CustomThemeSettingsController;
        })(WebOrdering.Controllers || (WebOrdering.Controllers = {}));
        var Controllers = WebOrdering.Controllers;
    })(MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
    var WebOrdering = MyAndromeda.WebOrdering;
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    (function (WebOrdering) {
        /// <reference path="MyAndromeda.WebOrdering.App.ts" />
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
                        } else {
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
                    var settingsSubscription = contextService.ModelSubject.where(function (e) {
                        return e !== null;
                    }).subscribe(function (webSiteSettings) {
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
        })(WebOrdering.Controllers || (WebOrdering.Controllers = {}));
        var Controllers = WebOrdering.Controllers;
    })(MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
    var WebOrdering = MyAndromeda.WebOrdering;
})(MyAndromeda || (MyAndromeda = {}));
/// <reference path="MyAndromeda.WebOrdering.App.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    (function (WebOrdering) {
        (function (Controllers) {
            WebOrdering.Angular.ControllersInitilizations.push(function (app) {
                app.controller(GeneralSettingsController.Name, [
                    '$scope', '$timeout',
                    WebOrdering.Services.ContextService.Name,
                    WebOrdering.Services.WebOrderingWebApiService.Name,
                    function ($scope, $timeout, contextService, webOrderingWebApiService) {
                        GeneralSettingsController.OnLoad($scope, $timeout, contextService, webOrderingWebApiService);

                        /* going to leave kendo to manage the observable object */
                        GeneralSettingsController.SetupKendoMvvm($scope, $timeout, contextService);
                    }
                ]);
            });

            var GeneralSettingsController = (function () {
                function GeneralSettingsController() {
                }
                GeneralSettingsController.OnLoad = function ($scope, $timeout, contextService, webOrderingWebApiService) {
                    var _this = this;
                    $scope.$watch("MinimumDeliveryAmount", function (newValue, oldValue) {
                        newValue = isNaN(newValue) ? null : newValue * 100;

                        if (!contextService.Model) {
                            return;
                        }

                        contextService.Model.GeneralSettings.set("MinimumDeliveryAmount", newValue);
                    });

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

                            _this.ShowFacebookIdInput($scope, $timeout, false);
                        }
                    };
                };

                GeneralSettingsController.SetupKendoMvvm = function ($scope, $timeout, contextService) {
                    var settingsSubscription = contextService.ModelSubject.where(function (e) {
                        return e !== null;
                    }).subscribe(function (websiteSettings) {
                        var viewElement = $("#GeneralSettingsController");
                        var generalSettings = websiteSettings.GeneralSettings;
                        var customerAccountSettings = websiteSettings.CustomerAccountSettings;

                        kendo.bind(viewElement, websiteSettings);

                        var minDeliveryValue = websiteSettings.GeneralSettings.get("MinimumDeliveryAmount");
                        $scope.MinimumDeliveryAmount = minDeliveryValue ? minDeliveryValue / 100 : 0;

                        var visible = customerAccountSettings.get("EnableFacebookLogin");
                        GeneralSettingsController.ShowFacebookIdInput($scope, $timeout, visible);

                        GeneralSettingsController.WatchForValidLoginSettings($scope, $timeout, customerAccountSettings);
                    });

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
                    IsEnableAndromedaLogin: true
                };
                return GeneralSettingsController;
            })();
            Controllers.GeneralSettingsController = GeneralSettingsController;
        })(WebOrdering.Controllers || (WebOrdering.Controllers = {}));
        var Controllers = WebOrdering.Controllers;
    })(MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
    var WebOrdering = MyAndromeda.WebOrdering;
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    (function (WebOrdering) {
        /// <reference path="MyAndromeda.WebOrdering.App.ts" />
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
                    var settingsSubscription = contextService.ModelSubject.where(function (e) {
                        return e !== null;
                    }).subscribe(function (websiteSettings) {
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
        })(WebOrdering.Controllers || (WebOrdering.Controllers = {}));
        var Controllers = WebOrdering.Controllers;
    })(MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
    var WebOrdering = MyAndromeda.WebOrdering;
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    (function (WebOrdering) {
        /// <reference path="MyAndromeda.WebOrdering.App.ts" />
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
                    var settingsSubscription = contextService.ModelSubject.where(function (e) {
                        return e !== null;
                    }).subscribe(function (websiteSettings) {
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
        })(WebOrdering.Controllers || (WebOrdering.Controllers = {}));
        var Controllers = WebOrdering.Controllers;
    })(MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
    var WebOrdering = MyAndromeda.WebOrdering;
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    (function (WebOrdering) {
        /// <reference path="MyAndromeda.WebOrdering.App.ts" />
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
                    var settingsSubscription = contextService.ModelSubject.where(function (e) {
                        return e !== null;
                    }).subscribe(function (webSiteSettings) {
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
        })(WebOrdering.Controllers || (WebOrdering.Controllers = {}));
        var Controllers = WebOrdering.Controllers;
    })(MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
    var WebOrdering = MyAndromeda.WebOrdering;
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    (function (WebOrdering) {
        /// <reference path="MyAndromeda.WebOrdering.App.ts" />
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
                            TwitterRequired: function (intput) {
                            }
                        }
                    };
                };

                SocialNetworkSettingsController.SetupKendoMvvm = function ($scope, $timout, contextService) {
                    var settingsSubscription = contextService.ModelSubject.where(function (e) {
                        return e !== null;
                    }).subscribe(function (websiteSettings) {
                        var viewElement = $("#SocialNetworkSettingsController");
                        kendo.bind(viewElement, websiteSettings.SocialNetworkSettings);
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
        })(WebOrdering.Controllers || (WebOrdering.Controllers = {}));
        var Controllers = WebOrdering.Controllers;
    })(MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
    var WebOrdering = MyAndromeda.WebOrdering;
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    (function (WebOrdering) {
        /// <reference path="MyAndromeda.WebOrdering.App.ts" />
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
                    var settingsSubscription = contextService.ModelSubject.where(function (e) {
                        return e !== null;
                    }).subscribe(function (websiteSettings) {
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
        })(WebOrdering.Controllers || (WebOrdering.Controllers = {}));
        var Controllers = WebOrdering.Controllers;
    })(MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
    var WebOrdering = MyAndromeda.WebOrdering;
})(MyAndromeda || (MyAndromeda = {}));
/// <reference path="../Menu/MyAndromeda.Menu.Logger.ts" />
/// <reference path="../../Scripts/typings/angularjs/angular-route.d.ts" />
/// <reference path="../../Scripts/typings/angularjs/angular.d.ts" />
var MyAndromeda;
(function (MyAndromeda) {
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
    })(MyAndromeda.DeliveryZonesByRadius || (MyAndromeda.DeliveryZonesByRadius = {}));
    var DeliveryZonesByRadius = MyAndromeda.DeliveryZonesByRadius;
})(MyAndromeda || (MyAndromeda = {}));
/// <reference path="MyAndromeda.DeliveryZonesByRadius.App.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    (function (DeliveryZonesByRadius) {
        (function (Controllers) {
            DeliveryZonesByRadius.Angular.ControllersInitilizations.push(function (app) {
                app.controller(DeliveryZoneNamesController.Name, [
                    '$scope', '$timeout',
                    DeliveryZonesByRadius.Services.ContextService.Name,
                    DeliveryZonesByRadius.Services.DeliveryZonesByRadiusWebApiService.Name,
                    function ($scope, $timeout, contextService, deliveryZonesByRadiusApiService) {
                        DeliveryZoneNamesController.OnLoad($scope, $timeout, contextService, deliveryZonesByRadiusApiService);

                        DeliveryZoneNamesController.SetupSubscriptions($scope, $timeout, contextService);
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
                                var unselectedItems = contextService.Model.PostCodeSectors.filter(function (e) {
                                    return !e.IsSelected;
                                });
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
                        var unselectedItems = data.filter(function (e) {
                            return !e.IsSelected;
                        });
                        if (unselectedItems.length === 0) {
                            $scope.SelectAll = true;
                        } else if (unselectedItems.length < data.length) {
                            $scope.SelectAll = false;
                        } else {
                            $scope.SelectAll = false;
                        }
                    };
                };

                DeliveryZoneNamesController.SetupSubscriptions = function ($scope, $timeout, contextService) {
                    var settingsSubscription = contextService.ModelSubject.subscribe(function (deliveryZoneByRadius) {
                        $timeout(function () {
                            $scope.ViewModel = deliveryZoneByRadius;
                            $scope.PostCodeOptionsListView.dataSource.data($scope.ViewModel.PostCodeSectors);
                            $scope.ViewModel.HasPostCodes = deliveryZoneByRadius.PostCodeSectors.length === 0 ? false : true;
                            var unselectedItems = contextService.Model.PostCodeSectors.filter(function (e) {
                                return !e.IsSelected;
                            });
                            $scope.SelectAll = unselectedItems.length === 0 ? true : false;
                        });
                    });

                    var settingsPostcodeSubscription = contextService.PostcodeModels.subscribe(function (newDeliveryOptions) {
                        $scope.ViewModel.HasPostCodes = newDeliveryOptions.length === 0 ? false : true;
                        $timeout(function () {
                            var vm = $scope.ViewModel;
                            var postCodeOptions = vm.PostCodeSectors;
                            postCodeOptions = new kendo.data.ObservableArray(newDeliveryOptions);
                            vm.PostCodeSectors = postCodeOptions;
                            $scope.PostCodeOptionsListView.dataSource.data(postCodeOptions);
                            var unselectedItems = contextService.Model.PostCodeSectors.filter(function (e) {
                                return !e.IsSelected;
                            });
                            $scope.SelectAll = unselectedItems.length === 0 ? true : false;
                            $scope.ViewModel.HasPostCodes = (contextService.Model.PostCodeSectors == null || contextService.Model.PostCodeSectors.length === 0) ? false : true;
                        });
                    });

                    $scope.$on('$destroy', function () {
                        settingsSubscription.dispose();
                    });
                };
                DeliveryZoneNamesController.Name = "DeliveryZoneNamesController";
                return DeliveryZoneNamesController;
            })();
            Controllers.DeliveryZoneNamesController = DeliveryZoneNamesController;
        })(DeliveryZonesByRadius.Controllers || (DeliveryZonesByRadius.Controllers = {}));
        var Controllers = DeliveryZonesByRadius.Controllers;
    })(MyAndromeda.DeliveryZonesByRadius || (MyAndromeda.DeliveryZonesByRadius = {}));
    var DeliveryZonesByRadius = MyAndromeda.DeliveryZonesByRadius;
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
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
    })(MyAndromeda.DeliveryZonesByRadius || (MyAndromeda.DeliveryZonesByRadius = {}));
    var DeliveryZonesByRadius = MyAndromeda.DeliveryZonesByRadius;
})(MyAndromeda || (MyAndromeda = {}));
/// <reference path="MyAndromeda.DeliveryZonesByRadius.App.ts" />
/// <reference path="MyAndromeda.DeliveryZonesByRadius.App.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    (function (DeliveryZonesByRadius) {
        /// <reference path="MyAndromeda.DeliveryZonesByRadius.App.ts" />
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
        })(DeliveryZonesByRadius.Services || (DeliveryZonesByRadius.Services = {}));
        var Services = DeliveryZonesByRadius.Services;
    })(MyAndromeda.DeliveryZonesByRadius || (MyAndromeda.DeliveryZonesByRadius = {}));
    var DeliveryZonesByRadius = MyAndromeda.DeliveryZonesByRadius;
})(MyAndromeda || (MyAndromeda = {}));
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
var MyAndromeda;
(function (MyAndromeda) {
    /// <reference path="MyAndromeda.DeliveryZonesByRadius.App.ts" />
    (function (DeliveryZonesByRadius) {
        var Settings = (function () {
            function Settings() {
            }
            Settings.AndromedaSiteId = 0;

            Settings.ReadRoute = "/api/{0}/DeliveryZonesByRadius/Read";

            Settings.ReadPostCodesRoute = "/api/{0}/DeliveryZonesByRadius/GeneratePostCodeSectors";

            Settings.UpdateRoute = "/api/{0}/DeliveryZonesByRadius/Update";
            return Settings;
        })();
        DeliveryZonesByRadius.Settings = Settings;
        ;
    })(MyAndromeda.DeliveryZonesByRadius || (MyAndromeda.DeliveryZonesByRadius = {}));
    var DeliveryZonesByRadius = MyAndromeda.DeliveryZonesByRadius;
})(MyAndromeda || (MyAndromeda = {}));
/// <reference path="../../Scripts/typings/angularjs/angular-route.d.ts" />
/// <reference path="../../Scripts/typings/angularjs/angular.d.ts" />
var MyAndromeda;
(function (MyAndromeda) {
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

                app.config([
                    '$routeProvider', function ($routeProvider) {
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
    })(MyAndromeda.Menu || (MyAndromeda.Menu = {}));
    var Menu = MyAndromeda.Menu;
})(MyAndromeda || (MyAndromeda = {}));
/// <reference path="../../Scripts/typings/linqjs/linq.d.ts" />
/// <reference path="../../Scripts/typings/jquery/jquery.d.ts" />
var MyAndromeda;
(function (MyAndromeda) {
    (function (Chain) {
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

                    var viableStores = Enumerable.from(stores).where(function (e) {
                        return e.latitude !== null && e.longitude !== null && e.longitude.length > 0 && e.latitude.length > 0;
                    });

                    //zoom out
                    if (viableStores.count() === 0) {
                        map.zoom(1);
                        map.center([0, 0]);

                        return;
                    }

                    var centeredPosition = function () {
                        //console.log(viableStores.toArray());
                        var avgLat = viableStores.select(function (e) {
                            return parseFloat(e.latitude);
                        }).average();
                        var avgLong = viableStores.select(function (e) {
                            return parseFloat(e.longitude);
                        }).average();
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
        })(Chain.Services || (Chain.Services = {}));
        var Services = Chain.Services;
    })(MyAndromeda.Chain || (MyAndromeda.Chain = {}));
    var Chain = MyAndromeda.Chain;
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    (function (Menu) {
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

                    dataSource.bind("requestStart", function () {
                        start.onNext(true);
                    });
                    dataSource.bind("requestEnd", function () {
                        end.onNext(true);
                    });

                    $scope.$on("$destroy", function () {
                        startLoading.dispose();
                        endLoading.dispose();
                    });
                };
                ToppingsController.Name = "ToppingsController";
                ToppingsController.Route = "/";
                ToppingsController.Template = function () {
                    return $("#MenuTemplate").html();
                };
                return ToppingsController;
            })();
            Controllers.ToppingsController = ToppingsController;
        })(Menu.Controllers || (Menu.Controllers = {}));
        var Controllers = Menu.Controllers;
    })(MyAndromeda.Menu || (MyAndromeda.Menu = {}));
    var Menu = MyAndromeda.Menu;
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    (function (Menu) {
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
        })(Menu.Services || (Menu.Services = {}));
        var Services = Menu.Services;
    })(MyAndromeda.Menu || (MyAndromeda.Menu = {}));
    var Menu = MyAndromeda.Menu;
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    (function (Menu) {
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
        })(Menu.Services || (Menu.Services = {}));
        var Services = Menu.Services;
    })(MyAndromeda.Menu || (MyAndromeda.Menu = {}));
    var Menu = MyAndromeda.Menu;
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    (function (Menu) {
        (function (Models) {
            var thumbItem = (function () {
                function thumbItem() {
                }
                return thumbItem;
            })();
            Models.thumbItem = thumbItem;

            
        })(Menu.Models || (Menu.Models = {}));
        var Models = Menu.Models;
    })(MyAndromeda.Menu || (MyAndromeda.Menu = {}));
    var Menu = MyAndromeda.Menu;
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    (function (Menu) {
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
                        dataSource: this.menuService.menuItemService.dataSource,
                        template: kendo.template(this.options.listview.template),
                        editTemplate: kendo.template(this.options.listview.editTemplate)
                    }).data("kendoListView");
                };

                MenuOrderingControllerService.prototype.highlighOriginalItem = function (item) {
                    $(item).animate({ "margin-left": "10px", "opacity": 0.4 }, 100).animate({ "margin-left": "0px" }, 100).css({ "border-color": "#E7001B", "border-width": "4px" });
                };
                MenuOrderingControllerService.prototype.UnhighlightOriginalItem = function (item) {
                    $(item).css({ "border-width": "0px" });
                };

                MenuOrderingControllerService.prototype.initListViewEvents = function () {
                    var internal = this;
                    var ds = internal.menuService.menuItemService.dataSource;

                    $(internal.options.ids.listViewId).kendoDraggable({
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
                    });

                    $(internal.options.ids.listViewId).kendoDropTargetArea({
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

                            //if ($e.is(".moving")) { return; }
                            $($e).animate({
                                "margin-left": "0px",
                                "opacity": 1,
                                "border-width": 0
                            }, 200);
                        },
                        drop: function (e) {
                            var draggableThing = e.draggable;
                            var hint = draggableThing.hint;
                            var draggableDataItem = ds.getByUid(hint.data("uid")), dropTargetDataItem = ds.getByUid(e.dropTarget.data("uid"));

                            internal.movePlaces(dropTargetDataItem, draggableDataItem, Positioning.BEFORE);
                            internal.normalize();
                            ds.sort({ field: "WebSequence", dir: "asc" });
                            internal.menuService.menuItemService.dataSource.sync();
                        }
                    });
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
                    } else if (droppedOnItem.position === 0) {
                        //move item to position 1
                        var moveItemPosition = droppedOnItem.webSesquence / 2;
                        moveItem.set("WebSequence", moveItemPosition);
                    } else if (droppedOnItem.position === max) {
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
                    } else {
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
                    view.forEach(function (item, index, source) {
                        item.set("WebSequence", start);
                        start += 100;
                    });

                    internal.positionWindowViewModel.trigger("change");
                    ds.trigger("change");
                };

                MenuOrderingControllerService.prototype.closeWindow = function (saveChanges) {
                    var kendoWindow = $("#positionWindow").data("kendoWindow");
                    var ds = this.menuService.menuItemService.dataSource;
                    if (saveChanges) {
                        ds.sync();
                    } else {
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

                    return hint;
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
        })(Menu.Services || (Menu.Services = {}));
        var Services = Menu.Services;
    })(MyAndromeda.Menu || (MyAndromeda.Menu = {}));
    var Menu = MyAndromeda.Menu;
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    (function (Menu) {
        /// <reference path="../../Scripts/typings/kendo/kendo.all.d.ts" />
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
                    var internal = this, dataSource = this.menuService.menuItemService.dataSource;
                    this.listView = $(this.options.ids.listViewId).kendoListView({
                        dataSource: dataSource,
                        template: kendo.template(this.options.listview.template),
                        editTemplate: kendo.template(this.options.listview.editTemplate),
                        save: function (e) {
                            var listView = internal.listView, dataSource = internal.menuService.menuItemService.dataSource, item = e.item, model = e.model;

                            //prevent the sync being called on the data source
                            e.preventDefault();

                            //close the list view editor
                            internal.closeCurrentEditedRow();
                            dataSource.trigger("change");
                            //d.anyDirtyItems();
                        },
                        edit: function (e) {
                            var editors = $(".k-editor");
                            internal.manageHtmlEditors(editors);
                        }
                    }).data("kendoListView");
                };

                MenuControllerService.prototype.cleanHtmlEvent = function (e) {
                    e.html = $(e.html).text();
                };

                MenuControllerService.prototype.manageHtmlEditors = function (query) {
                    var internal = this, editors = $.map(query, function (element) {
                        return $(element).data("kendoEditor");
                    });

                    editors.forEach(function (editor) {
                        editor.unbind("paste", internal.cleanHtmlEvent);
                        editor.bind("paste", internal.cleanHtmlEvent);
                    });
                };

                MenuControllerService.prototype.closeCurrentEditedRow = function () {
                    var listView = this.listView;
                    listView._closeEditable(true);
                };

                MenuControllerService.prototype.initListViewEvents = function () {
                    var internal = this;

                    this.listView.bind("edit", function (e) {
                        //physical html element
                        var menuItemElement = $(e.item), id = menuItemElement.data("id"), uploadId = "#files" + id, advancedUploadId = "#cropFiles" + id;

                        //need to find the observable model
                        var menuItem = internal.menuService.menuItemService.findById(id);

                        internal.initEditListViewItem(uploadId, advancedUploadId, menuItem);
                    });
                };

                //list view item events:
                MenuControllerService.prototype.initListViewItemEvents = function () {
                    var listViewId = this.options.ids.listViewId, internal = this;

                    //watch for the edit button clicked.
                    $(listViewId).on("click", ".k-button-edit", function (e) {
                        e.preventDefault();
                        var row = $(this).closest(".menu-item");

                        //check if any editor rows are active
                        var editorRow = internal.listView.element.find(".k-edit-item");

                        if (editorRow.length == 0) {
                            //internal.listView.cancel();
                            internal.listView.edit(row);
                            return;
                        }

                        internal.closeCurrentEditedRow();
                        internal.listView.edit(row);
                    });

                    //watch for any save event
                    $(listViewId).on("click", ".k-button-save", function (e) {
                        e.preventDefault();

                        //save changes
                        internal.listView.save();
                    });

                    //watch for any cancel event
                    $(listViewId).on("click", ".k-button-cancel", function (e) {
                        e.preventDefault();
                        internal.listView.cancel();
                    });
                };

                MenuControllerService.prototype.initEditListViewItem = function (elementId, advancedElementId, menuItem) {
                    var internal = this;
                    var folder = "?folderPath=" + menuItem.Id;
                    var removeFile = Menu.Settings.Routes.MenuItems.RemoveImageUrl + folder;

                    var uploadBox = $(elementId).kendoUpload({
                        async: {
                            saveUrl: Menu.Settings.Routes.MenuItems.SaveImageUrl + folder,
                            removeUrl: removeFile,
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

                        for (var i = 0; i < e.response.length; i++) {
                            var thumb = e.response[i];
                            thumbData.push(thumb);
                        }

                        //menu item is an observable, ergo updating the array should make it happy.
                        menuItem.set("Thumbs", thumbData);

                        var relatedItems = internal.menuService.menuItemService.getRelatedItems([menuItem]);
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

                    var advancedUploadBox;
                    var onSelect = function (selectArgs) {
                        var fileList = selectArgs.files;
                        var fileList2 = advancedUploadBox.element.find("input");

                        fileList.forEach(function (value, index) {
                            var fileReader = new FileReader();

                            //console.log("foreach");
                            fileReader.onload = function (onLoadArgs) {
                                //console.log("onload");
                                //console.log(fileReader.result);
                                var image = $('<img />').attr("src", fileReader.result);
                                var imageHolder = advancedUploadBox.element.closest(".k-widget").find(".imageHolder");
                                imageHolder.append(image);
                            };
                            fileReader.readAsDataURL(value.rawFile);
                        });
                    };

                    advancedUploadBox = $(advancedElementId).kendoUpload({
                        async: {
                            saveUrl: Menu.Settings.Routes.MenuItems.SaveImageUrl + folder,
                            removeUrl: removeFile,
                            autoUpload: false,
                            batch: false
                        },
                        template: $("#thumbnails-crop-template").html(),
                        select: onSelect
                    }).data("kendoUpload");

                    //remove thumbnail
                    $(this.options.ids.listViewId).on("click", ".k-button-remove-thumb", function () {
                        var itemUrl = $(this).closest(".thumb").data("fileName");

                        var call = $.ajax({
                            url: removeFile,
                            type: "POST",
                            dataType: "json",
                            data: { fileName: itemUrl }
                        });

                        call.done(function (e) {
                            internal.menuService.menuItemService.removeThumb(menuItem, itemUrl);
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
                        var relatedItems = internal.menuService.menuItemService.getRelatedItems([menuItem]);

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
                        //menuItem.Thumbs.forEach((thumb: Models.IMenuItemThumb) => {
                        //    if (thumb.Url) {
                        //        var call = $.ajax({
                        //            url: removeFile,
                        //            type: "POST",
                        //            dataType: "json",
                        //            data: { fileName: thumb.FileName }
                        //        });
                        //        call.done(function (e) {
                        //            internal.menuService.menuItemService.removeThumb(menuItem, thumb.FileName);
                        //        });
                        //        call.fail(function (e) {
                        //            alert("There was an error processing the request. Please try again");
                        //        });
                        //    }
                        //})
                    });
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
                    this.initListViewEvents();
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
        })(Menu.Services || (Menu.Services = {}));
        var Services = Menu.Services;
    })(MyAndromeda.Menu || (MyAndromeda.Menu = {}));
    var Menu = MyAndromeda.Menu;
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    (function (Menu) {
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

                MenuService.prototype.CreateCategoryService = function (category1, category2, displayCategories) {
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
                        menuItem.isNew = function () {
                            return false;
                        };
                        menuItem.Category1Name = category1 ? category1.Name : "";
                        menuItem.Category2Name = category2 ? category2.Name : "";
                        menuItem.DisplayName = function () {
                            var self = this, webName = self.get("WebName"), name = self.get("Name");
                            if (webName && webName.length > 0) {
                                return webName;
                            } else {
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
                                    internal.CreateCategoryService(c1, c2, display);

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
                                var data = options.data, menuItem = null, similarItems = null;

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
                                }
                            }
                        },
                        serverPaging: false,
                        serverFiltering: false,
                        serverSorting: false,
                        pageSize: 10
                    });

                    this.menuItemService = new Services.menuItemService(dataSource);
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
        })(Menu.Services || (Menu.Services = {}));
        var Services = Menu.Services;
    })(MyAndromeda.Menu || (MyAndromeda.Menu = {}));
    var Menu = MyAndromeda.Menu;
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    (function (Menu) {
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
        })(Menu.Services || (Menu.Services = {}));
        var Services = Menu.Services;
    })(MyAndromeda.Menu || (MyAndromeda.Menu = {}));
    var Menu = MyAndromeda.Menu;
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    (function (Menu) {
        (function (Services) {
            Menu.Angular.ServicesInitilizations.push(function (app) {
            });

            var menuItemService = (function () {
                function menuItemService(dataSource) {
                    this.dataSource = dataSource;
                    this.menuStatus = new Services.MenuStatus(dataSource);
                }
                //find elements based on the id
                menuItemService.prototype.findById = function (id) {
                    return this.dataSource.get(id);
                };

                //find elements based on the name
                menuItemService.prototype.getRelatedItems = function (menuItems) {
                    var internal = this;
                    var series = Enumerable.from(menuItems).selectMany(function (x) {
                        return internal.dataSource.data().filter(function (value, index, array) {
                            return value.Name == x.Name && value.Id !== x.Id;
                        });
                    }).toArray();

                    return series;
                };

                menuItemService.prototype.removeThumb = function (menuItem, filename) {
                    var thumbs = menuItem.get("Thumbs");
                    var thumb = thumbs.find(function (item, index, array) {
                        return item.FileName === filename;
                    });
                    thumbs.remove(thumb);
                };

                menuItemService.prototype.updateItems = function (menuItems, updateDataSource) {
                    var internal = this;
                    for (var i = 0; i < menuItems.length; i++) {
                        var item = menuItems[i];
                        var dataSourceItem = internal.findById(item.Id), dataItem = dataSourceItem;
                        if (!dataSourceItem) {
                            continue;
                        }

                        //dataSourceItem.dirty = false;
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

                menuItemService.prototype.anyDirtyItems = function () {
                    var internal = this;

                    return this.dataSource.hasChanges();
                };
                return menuItemService;
            })();
            Services.menuItemService = menuItemService;
        })(Menu.Services || (Menu.Services = {}));
        var Services = Menu.Services;
    })(MyAndromeda.Menu || (MyAndromeda.Menu = {}));
    var Menu = MyAndromeda.Menu;
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    (function (Menu) {
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
        })(Menu.Services || (Menu.Services = {}));
        var Services = Menu.Services;
    })(MyAndromeda.Menu || (MyAndromeda.Menu = {}));
    var Menu = MyAndromeda.Menu;
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    (function (Menu) {
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
        })(Menu.Services || (Menu.Services = {}));
        var Services = Menu.Services;
    })(MyAndromeda.Menu || (MyAndromeda.Menu = {}));
    var Menu = MyAndromeda.Menu;
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    (function (Reporting) {
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
                                setTimeout(function () {
                                    act();
                                }, internal.loadDelay);
                            });
                        } else {
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
        })(Reporting.Services || (Reporting.Services = {}));
        var Services = Reporting.Services;
    })(MyAndromeda.Reporting || (MyAndromeda.Reporting = {}));
    var Reporting = MyAndromeda.Reporting;
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    (function (Reporting) {
        (function (Models) {
            var Result = (function () {
                function Result(key, data) {
                    this.key = key;
                    if (data instanceof kendo.data.DataSource) {
                        this.data = data;
                    } else {
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
                    } else if (data.data) {
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
        })(Reporting.Models || (Reporting.Models = {}));
        var Models = Reporting.Models;
    })(MyAndromeda.Reporting || (MyAndromeda.Reporting = {}));
    var Reporting = MyAndromeda.Reporting;
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    (function (Menu) {
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
                    $scope.$on('$destroy', function () {
                    });
                };

                MenuItemsController.SetupScope = function ($scope) {
                    $scope.$on('$destroy', function () {
                    });
                };
                MenuItemsController.Template = "Templates/MenuItems";
                MenuItemsController.Name = "MenuItemsController";
                MenuItemsController.Route = "/MenuItems";
                return MenuItemsController;
            })();
            Controllers.MenuItemsController = MenuItemsController;
        })(Menu.Controllers || (Menu.Controllers = {}));
        var Controllers = Menu.Controllers;
    })(MyAndromeda.Menu || (MyAndromeda.Menu = {}));
    var Menu = MyAndromeda.Menu;
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    (function (Menu) {
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
                    RemoveImageUrl: ""
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
        })(Menu.Settings || (Menu.Settings = {}));
        var Settings = Menu.Settings;
    })(MyAndromeda.Menu || (MyAndromeda.Menu = {}));
    var Menu = MyAndromeda.Menu;
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    (function (Menu) {
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
                    $scope.Publish = function () {
                        publishingService.openWindow();
                    };
                    $scope.ToolbarOptions = {
                        items: [
                            //{ type: "button", text: "Menu Items" },
                            //{ type: "button", text: "Menu Sequencing" },
                            //{ type: "button", text: "Toppings" },
                            //{ type: "separator" },
                            { type: "button", text: "Publish", click: function () {
                                    publishingService.openWindow();
                                } }
                        ]
                    };
                };

                MenuNavigationController.OnLoad = function ($scope, menuNavigationService) {
                    $scope.$on("$destroy", function () {
                    });
                };
                MenuNavigationController.Name = "MenuNavigationController";
                return MenuNavigationController;
            })();
            Controllers.MenuNavigationController = MenuNavigationController;
        })(Menu.Controllers || (Menu.Controllers = {}));
        var Controllers = Menu.Controllers;
    })(MyAndromeda.Menu || (MyAndromeda.Menu = {}));
    var Menu = MyAndromeda.Menu;
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    (function (Menu) {
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
        })(Menu.Controllers || (Menu.Controllers = {}));
        var Controllers = Menu.Controllers;
    })(MyAndromeda.Menu || (MyAndromeda.Menu = {}));
    var Menu = MyAndromeda.Menu;
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    (function (Menu) {
        /// <reference path="../../Scripts/typings/rx.js/rx.d.ts" />
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
                        ResetFilters: function () {
                            _this.ResetFilters();
                        }
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
        })(Menu.Services || (Menu.Services = {}));
        var Services = Menu.Services;
    })(MyAndromeda.Menu || (MyAndromeda.Menu = {}));
    var Menu = MyAndromeda.Menu;
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    (function (Reporting) {
        ///// <reference path="../../Scripts/typings/kendo/kendo.all.d.ts" />
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
                    } else {
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
                    } else {
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
        })(Reporting.Services || (Reporting.Services = {}));
        var Services = Reporting.Services;
    })(MyAndromeda.Reporting || (MyAndromeda.Reporting = {}));
    var Reporting = MyAndromeda.Reporting;
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    (function (GridExport) {
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
        })(GridExport.Services || (GridExport.Services = {}));
        var Services = GridExport.Services;
    })(MyAndromeda.GridExport || (MyAndromeda.GridExport = {}));
    var GridExport = MyAndromeda.GridExport;
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
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
                    try  {
                        console.log(data);
                    } catch (e) {
                    }
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
                    } else {
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
    })(MyAndromeda.Hubs || (MyAndromeda.Hubs = {}));
    var Hubs = MyAndromeda.Hubs;
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    (function (Hubs) {
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
        })(Hubs.Services || (Hubs.Services = {}));
        var Services = Hubs.Services;
    })(MyAndromeda.Hubs || (MyAndromeda.Hubs = {}));
    var Hubs = MyAndromeda.Hubs;
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
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
                if (this.connected) {
                    return;
                }
                if (this.connecting) {
                    return;
                }
                if (this.setup) {
                    return;
                }

                //$.connection.hub.logging = true;
                var internal = this, hubConnection = $.connection.hub;

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

                if (document.URL.indexOf("localhost") >= 0) {
                    transportType = "webSockets";
                }

                hubConnection.start({
                    transport: transportType
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
                    try  {
                        console.log(data);
                    } catch (e) {
                    }
                }
            };
            return MyAndromedaHubConnection;
        })();
        Hubs.MyAndromedaHubConnection = MyAndromedaHubConnection;
    })(MyAndromeda.Hubs || (MyAndromeda.Hubs = {}));
    var Hubs = MyAndromeda.Hubs;
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    (function (Menu) {
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
        })(Menu.Services || (Menu.Services = {}));
        var Services = Menu.Services;
    })(MyAndromeda.Menu || (MyAndromeda.Menu = {}));
    var Menu = MyAndromeda.Menu;
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    (function (WebOrdering) {
        /// <reference path="MyAndromeda.WebOrdering.App.ts" />
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
                    var isDataBusySubscription = webOrderingWebApiService.IsWebOrderingBusy.subscribe(function (value) {
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
                    $scope.$on('$destroy', function () {
                    });
                };
                PickThemeController.Name = "PickThemeController";
                PickThemeController.Route = "/";
                return PickThemeController;
            })();
            Controllers.PickThemeController = PickThemeController;
        })(WebOrdering.Controllers || (WebOrdering.Controllers = {}));
        var Controllers = WebOrdering.Controllers;
    })(MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
    var WebOrdering = MyAndromeda.WebOrdering;
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    (function (WebOrdering) {
        /// <reference path="../../Scripts/typings/rx.js/rx.binding-lite.d.ts" />
        /// <reference path="MyAndromeda.WebOrdering.App.ts" />
        (function (Services) {
            WebOrdering.Angular.ServicesInitilizations.push(function (app) {
                app.factory(WebOrderingWebApiService.Name, [
                    Services.ContextService.Name,
                    function (contextService) {
                        var instnance = new WebOrderingWebApiService(contextService);
                        return instnance;
                    }
                ]);
            });

            var WebOrderingWebApiService = (function () {
                function WebOrderingWebApiService(context) {
                    var _this = this;
                    this.Context = context;

                    this.IsPreviewReady = new Rx.Subject();
                    this.IsWebOrderingBusy = new Rx.BehaviorSubject(false);
                    this.Search = new Rx.Subject();

                    if (!WebOrdering.Settings.AndromedaSiteId) {
                        throw "Settings.AndromedaSiteId is undefined";
                    }

                    var read = kendo.format(WebOrdering.Settings.ReadRoute, WebOrdering.Settings.AndromedaSiteId, WebOrdering.Settings.WebSiteId);

                    this.IsWebOrderingBusy.onNext(true);
                    var promise = $.ajax({
                        url: read,
                        type: "GET",
                        dataType: "json"
                    });

                    this.watcher = new Rx.Subject();
                    this.watcher.distinctUntilChanged(function (x) {
                        return x.WebSiteId;
                    }).subscribe(function (settings) {
                        _this.Context.ModelSubject.onNext(settings);
                    });

                    this.Context.ModelSubject.where(function (e) {
                        return e !== null;
                    }).subscribe(function (v) {
                        console.log(v.WebSiteName);
                    });

                    promise.done(function (result) {
                        var observable = kendo.observable(result[0]);
                        _this.Context.Model = observable;

                        //this.Context.Model.SocialNetworkSettings.bind("change", (field:kendo.ui.observ) => {
                        //});
                        //this.Context.Model.SocialNetworkSettings.TwitterSettings.set("IsFollow", this.Context.Model.SocialNetworkSettings.TwitterSettings.IsEnable);
                        //$scope.ShowFacebookSettings = websiteSettings.SocialNetworkSettings.FacebookSettings.get("IsEnable");
                        //    $scope.ShowTwitterSettings = websiteSettings.SocialNetworkSettings.TwitterSettings.get("IsEnable");
                        //    $scope.ShowInstagramSettings = websiteSettings.SocialNetworkSettings.InstagramSettings.get("IsEnable");
                        _this.Context.ModelSubject.onNext(observable);

                        _this.IsWebOrderingBusy.onNext(false);
                    });

                    promise.fail(function () {
                        _this.IsWebOrderingBusy.onNext(false);
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

                    var raw = JSON.stringify(this.Context.Model);
                    var promise = $.ajax({
                        url: publish,
                        type: "POST",
                        contentType: 'application/json',
                        dataType: "json",
                        data: raw
                    });

                    this.IsWebOrderingBusy.onNext(true);

                    promise.done(function () {
                        _this.IsWebOrderingBusy.onNext(false);
                    });
                    promise.fail(function () {
                        _this.IsWebOrderingBusy.onNext(false);
                    });
                };

                WebOrderingWebApiService.prototype.Preview = function () {
                    var _this = this;
                    var preview = kendo.format(WebOrdering.Settings.PreviewRoute, WebOrdering.Settings.AndromedaSiteId, WebOrdering.Settings.WebSiteId);

                    var raw = JSON.stringify(this.Context.Model);
                    var promise = $.ajax({
                        url: preview,
                        type: "POST",
                        contentType: 'application/json',
                        dataType: "json",
                        data: raw
                    });

                    this.IsWebOrderingBusy.onNext(true);

                    promise.done(function () {
                        _this.IsWebOrderingBusy.onNext(false);
                        _this.IsPreviewReady.onNext(true);
                    });
                    promise.fail(function () {
                        _this.IsWebOrderingBusy.onNext(false);
                    });
                };

                WebOrderingWebApiService.prototype.Update = function () {
                    var _this = this;
                    var update = kendo.format(WebOrdering.Settings.UpdateRoute, WebOrdering.Settings.AndromedaSiteId, WebOrdering.Settings.WebSiteId);
                    console.log("sync");

                    var raw = JSON.stringify(this.Context.Model);
                    console.log(this.Context.Model);
                    var promise = $.ajax({
                        url: update,
                        type: "POST",
                        contentType: 'application/json',
                        dataType: "json",
                        data: raw
                    });

                    this.IsWebOrderingBusy.onNext(true);

                    promise.done(function () {
                        _this.IsWebOrderingBusy.onNext(false);
                    });
                    promise.fail(function () {
                        _this.IsWebOrderingBusy.onNext(false);
                    });
                };
                WebOrderingWebApiService.Name = "WebOrderingWebApiService";
                return WebOrderingWebApiService;
            })();
            Services.WebOrderingWebApiService = WebOrderingWebApiService;
        })(WebOrdering.Services || (WebOrdering.Services = {}));
        var Services = WebOrdering.Services;
    })(MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
    var WebOrdering = MyAndromeda.WebOrdering;
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    /// <reference path="MyAndromeda.WebOrdering.App.ts" />
    (function (WebOrdering) {
        var Settings = (function () {
            function Settings() {
            }
            Settings.AndromedaSiteId = 0;
            Settings.WebSiteId = 0;

            Settings.ReadRoute = "/api/{0}/AndroWebOrdering/{1}/Read";

            Settings.UpdateRoute = "/api/{0}/AndroWebOrdering/{1}/Update";

            Settings.PublishRoute = "/api/{0}/AndroWebOrdering/{1}/Publish";

            Settings.PreviewRoute = "/api/{0}/AndroWebOrdering/{1}/Preview";
            return Settings;
        })();
        WebOrdering.Settings = Settings;
        ;
    })(MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
    var WebOrdering = MyAndromeda.WebOrdering;
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    (function (WebOrdering) {
        /// <reference path="../../Scripts/typings/rx.js/rx.time-lite.d.ts" />
        /// <reference path="../../Scripts/typings/rx.js/rx.binding-lite.d.ts" />
        /// <reference path="MyAndromeda.WebOrdering.App.ts" />
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
                    this.Search.throttle(1000).subscribe(function (value) {
                        return _this.SearcInternal(value);
                    });
                }
                WebOrderingThemeWebApiService.prototype.GetThemeDataSource = function () {
                    var _this = this;
                    var route = kendo.format('/api/AndroWebOrderingTheme/{0}/List', WebOrdering.Settings.AndromedaSiteId);
                    this.dataSource = new kendo.data.DataSource({
                        transport: {
                            read: route
                        }
                    });

                    this.dataSource.bind("requestStart", function () {
                        _this.IsBusy.onNext(true);
                    });
                    this.dataSource.bind("requestEnd", function () {
                        _this.IsBusy.onNext(false);
                    });

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
        })(WebOrdering.Services || (WebOrdering.Services = {}));
        var Services = WebOrdering.Services;
    })(MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
    var WebOrdering = MyAndromeda.WebOrdering;
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
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
    })(MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
    var WebOrdering = MyAndromeda.WebOrdering;
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    (function (WebOrdering) {
        /// <reference path="MyAndromeda.WebOrdering.App.ts" />
        (function (Services) {
            WebOrdering.Angular.ServicesInitilizations.push(function (app) {
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
                    this.ModelSubject = new Rx.BehaviorSubject(null);
                }
                ContextService.Name = "ContextService";
                return ContextService;
            })();
            Services.ContextService = ContextService;
        })(WebOrdering.Services || (WebOrdering.Services = {}));
        var Services = WebOrdering.Services;
    })(MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
    var WebOrdering = MyAndromeda.WebOrdering;
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    (function (WebOrdering) {
        /// <reference path="MyAndromeda.WebOrdering.App.ts" />
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
                    var webSiteImageUploadRoute = kendo.format("/api/{0}/AndroWebOrdering/{1}/UploadLogo/{2}", WebOrdering.Settings.AndromedaSiteId, WebOrdering.Settings.WebSiteId, "website");
                    var mobileImageUploadRoute = kendo.format("/api/{0}/AndroWebOrdering/{1}/UploadLogo/{2}", WebOrdering.Settings.AndromedaSiteId, WebOrdering.Settings.WebSiteId, "mobile");
                    var FaviconUploadRoute = kendo.format("/api/{0}/AndroWebOrdering/{1}/UploadLogo/{2}", WebOrdering.Settings.AndromedaSiteId, WebOrdering.Settings.WebSiteId, "favicon");

                    if (!$scope.MainImageUpload) {
                        alert("MainImageUpload hasnt been created. Pester Matt");
                    }

                    $scope.MainImageUpload.setOptions({ async: { saveUrl: webSiteImageUploadRoute, autoUpload: true }, showFileList: false, multiple: false });
                    $scope.MainImageUpload.bind("success", function (result) {
                        console.log(result);
                        var observableObject = contextService.Model.SiteDetails;
                        observableObject.set("WebsiteLogoPath", result.response.Url);

                        var r = Math.floor(Math.random() * 99999) + 1;
                        $timeout(function () {
                            $scope.TempWebsiteLogoPath = result.response.Url + "?k=" + r;
                            $scope.HasWebsiteLogo = true;
                        });
                    });

                    if (!$scope.MobileImageUpload) {
                        alert("MobileImageUpload hasn't been created. Pester Matt");
                    }

                    $scope.MobileImageUpload.setOptions({
                        async: {
                            saveUrl: mobileImageUploadRoute,
                            autoUpload: true
                        },
                        showFileList: false,
                        multiple: false
                    });
                    $scope.MobileImageUpload.bind("success", function (result) {
                        console.log(result);

                        var observableObject = contextService.Model.SiteDetails;
                        observableObject.set("MobileLogoPath", result.response.Url);

                        var r = Math.floor(Math.random() * 99999) + 1;
                        $timeout(function () {
                            $scope.TempMobileLogoPath = result.response.Url + "?k=" + r;
                            $scope.HasMobileLogo = true;
                        });
                    });

                    $scope.FaviconImageUpload.setOptions({ async: { saveUrl: FaviconUploadRoute, autoUpload: true }, showFileList: false, multiple: false });
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
                    var settingsSubscription = contextService.ModelSubject.where(function (e) {
                        return e !== null;
                    }).subscribe(function (webSiteSettings) {
                        var viewElement = $("#SiteDetailsController");
                        kendo.bind(viewElement, webSiteSettings.SiteDetails);

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
        })(WebOrdering.Controllers || (WebOrdering.Controllers = {}));
        var Controllers = WebOrdering.Controllers;
    })(MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
    var WebOrdering = MyAndromeda.WebOrdering;
})(MyAndromeda || (MyAndromeda = {}));
/// <reference path="MyAndromeda.WebOrdering.App.ts" />
/// <reference path="MyAndromeda.Menu.App.ts" />
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
var MyAndromeda;
(function (MyAndromeda) {
    (function (WebOrdering) {
        /// <reference path="MyAndromeda.WebOrdering.App.ts" />
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

                    webOrderingWebApiService.IsPreviewReady.subscribe(function (busy) {
                        $scope.PreviewWindow.refresh({});
                        $scope.PreviewWindow.open();
                        $scope.PreviewWindow.center();
                    });

                    webOrderingWebApiService.IsWebOrderingBusy.subscribe(function (busy) {
                        $timout(function () {
                            if (busy) {
                                if (!$scope.Modal) {
                                    return;
                                }
                                var m = $scope.Modal;
                                m.open();
                            } else {
                                if (!$scope.Modal) {
                                    return;
                                }
                                var m = $scope.Modal;
                                m.close();
                            }
                            $scope.IsBusy = busy;
                        });
                    });
                };
                StatusController.Name = "StatusController";
                return StatusController;
            })();
            Controllers.StatusController = StatusController;
        })(WebOrdering.Controllers || (WebOrdering.Controllers = {}));
        var Controllers = WebOrdering.Controllers;
    })(MyAndromeda.WebOrdering || (MyAndromeda.WebOrdering = {}));
    var WebOrdering = MyAndromeda.WebOrdering;
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    (function (Menu) {
        (function (Controllers) {
            var FtpController = (function () {
                function FtpController() {
                }
                FtpController.SetupScope = function ($scope, $timeout, ftpService) {
                    $scope.DeleteLocalFile = function () {
                        if (confirm("Are you sure you want to delete the current version? Thumbnails will not be effected.")) {
                            ftpService.DeleteLocalFile();
                        }
                    };
                    $scope.DownloadFromFtp = function () {
                        return ftpService.StartFtpDownload();
                    };
                    $scope.UploadToFtp = function () {
                        return ftpService.StartFtpUpload();
                    };
                    $scope.CheckDbVersion = function () {
                        return ftpService.GetFtpVersion();
                    };

                    //ftp download status
                    ftpService.FtpDownloadBusy.subscribe(function (busy) {
                        $timeout(function () {
                            $scope.DownloadBusy = busy;
                        });
                    });
                    ftpService.FtpDownloadErrors.subscribe(function (error) {
                        $timeout(function () {
                            $scope.Errors = error;
                        });
                    });

                    //ftp upload status
                    ftpService.FtpUploadBusy.subscribe(function (busy) {
                        $timeout(function () {
                            $scope.UploadBusy = busy;
                        });
                    });
                    ftpService.FtpUploadErrors.subscribe(function (error) {
                        $timeout(function () {
                            $scope.Errors = error;
                        });
                    });

                    //ftp access status
                    ftpService.LocalAccessVersion.subscribe(function (version) {
                        $timeout(function () {
                            $scope.Version = version;
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
        })(Menu.Controllers || (Menu.Controllers = {}));
        var Controllers = Menu.Controllers;
    })(MyAndromeda.Menu || (MyAndromeda.Menu = {}));
    var Menu = MyAndromeda.Menu;
})(MyAndromeda || (MyAndromeda = {}));

var MyAndromeda;
(function (MyAndromeda) {
    (function (Menu) {
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

                FtpService.prototype.GetFtpVersion = function () {
                    var _this = this;
                    var route = Menu.Settings.Routes.Ftp.Version;

                    this.ValidateRoute(route);

                    var promise = this.$http.post(route, {});

                    this.LocalAccessVersionBusy.onNext(true);

                    promise.success(function (data, status, headers, config) {
                        _this.LocalAccessVersion.onNext(data);
                    });
                    promise.finally(function () {
                        _this.LocalAccessVersionBusy.onNext(false);
                    });
                };

                FtpService.prototype.StartFtpDownload = function () {
                    var _this = this;
                    var route = Menu.Settings.Routes.Ftp.DownloadMenu;

                    this.ValidateRoute(route);
                    this.FtpDownloadBusy.onNext(true);

                    var promise = this.$http.post(route, {});

                    promise.finally(function () {
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
        })(Menu.Services || (Menu.Services = {}));
        var Services = Menu.Services;
    })(MyAndromeda.Menu || (MyAndromeda.Menu = {}));
    var Menu = MyAndromeda.Menu;
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    (function (Menu) {
        /// <reference path="MyAndromeda.Menu.Controllers.FtpController.ts" />
        (function (Modules) {
            Modules.MenuFtpModuleName = "MenuFtpModule";

            var FtpModule = angular.module(Modules.MenuFtpModuleName, []);

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
        })(Menu.Modules || (Menu.Modules = {}));
        var Modules = Menu.Modules;
    })(MyAndromeda.Menu || (MyAndromeda.Menu = {}));
    var Menu = MyAndromeda.Menu;
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    (function (Reporting) {
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
                    } else {
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
        })(Reporting.Services || (Reporting.Services = {}));
        var Services = Reporting.Services;
    })(MyAndromeda.Reporting || (MyAndromeda.Reporting = {}));
    var Reporting = MyAndromeda.Reporting;
})(MyAndromeda || (MyAndromeda = {}));
/// <reference path="../../Scripts/typings/linqjs/linq.jquery.d.ts" />
/// <reference path="../../Scripts/typings/signalr/signalr.d.ts" />
var MyAndromeda;
(function (MyAndromeda) {
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
                } else if (e.value < 1) {
                    progressStatus.text(PasswordValidator.EmptyMessage);
                } else if (e.value <= 3) {
                    progressStatus.text(PasswordValidator.WeakMessage);

                    progressWrapper.css({
                        "background-color": "#EE9F05",
                        "border-color": "#EE9F05"
                    });
                } else if (e.value <= 6) {
                    progressStatus.text(PasswordValidator.GoodMessage);

                    progressWrapper.css({
                        "background-color": "#428bca",
                        "border-color": "#428bca"
                    });
                } else {
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
    })(MyAndromeda.Validation || (MyAndromeda.Validation = {}));
    var Validation = MyAndromeda.Validation;
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
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
    })(MyAndromeda.Validation || (MyAndromeda.Validation = {}));
    var Validation = MyAndromeda.Validation;
})(MyAndromeda || (MyAndromeda = {}));
/* todo add add custom rules */
/*
MyAndromeda.Validation.MyAndromedaValidation
{
}
*/
var MyAndromeda;
(function (MyAndromeda) {
    (function (Vouchers) {
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
        })(Vouchers.Services || (Vouchers.Services = {}));
        var Services = Vouchers.Services;
    })(MyAndromeda.Vouchers || (MyAndromeda.Vouchers = {}));
    var Vouchers = MyAndromeda.Vouchers;
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    (function (Chain) {
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
        })(Chain.Services || (Chain.Services = {}));
        var Services = Chain.Services;
    })(MyAndromeda.Chain || (MyAndromeda.Chain = {}));
    var Chain = MyAndromeda.Chain;
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    (function (Store) {
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
        })(Store.Services || (Store.Services = {}));
        var Services = Store.Services;
    })(MyAndromeda.Store || (MyAndromeda.Store = {}));
    var Store = MyAndromeda.Store;
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    (function (Reporting) {
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
        })(Reporting.Services || (Reporting.Services = {}));
        var Services = Reporting.Services;
    })(MyAndromeda.Reporting || (MyAndromeda.Reporting = {}));
    var Reporting = MyAndromeda.Reporting;
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
    (function (Users) {
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
        })(Users.Models || (Users.Models = {}));
        var Models = Users.Models;
    })(MyAndromeda.Users || (MyAndromeda.Users = {}));
    var Users = MyAndromeda.Users;
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
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
                    try  {
                        console.log(data);
                    } catch (e) {
                    }
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
                connection.start().done(function () {
                    SynchronizationHub.log("Connected");
                }).fail(function () {
                    SynchronizationHub.log("Could not connect");
                });

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
    })(MyAndromeda.Hubs || (MyAndromeda.Hubs = {}));
    var Hubs = MyAndromeda.Hubs;
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
    (function (Reporting) {
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
                            total: function (result) {
                                return result.Data.length;
                            }
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
        })(Reporting.Services || (Reporting.Services = {}));
        var Services = Reporting.Services;
    })(MyAndromeda.Reporting || (MyAndromeda.Reporting = {}));
    var Reporting = MyAndromeda.Reporting;
})(MyAndromeda || (MyAndromeda = {}));

var MyAndromeda;
(function (MyAndromeda) {
    (function (Reporting) {
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

                    try  {
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
                    } catch (e) {
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
        })(Reporting.Services || (Reporting.Services = {}));
        var Services = Reporting.Services;
    })(MyAndromeda.Reporting || (MyAndromeda.Reporting = {}));
    var Reporting = MyAndromeda.Reporting;
})(MyAndromeda || (MyAndromeda = {}));
var MyAndromeda;
(function (MyAndromeda) {
    (function (KendoExtensions) {
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

                    this.getEditor().paste(template);
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
        })(KendoExtensions.EditorTools || (KendoExtensions.EditorTools = {}));
        var EditorTools = KendoExtensions.EditorTools;
    })(MyAndromeda.KendoExtensions || (MyAndromeda.KendoExtensions = {}));
    var KendoExtensions = MyAndromeda.KendoExtensions;
})(MyAndromeda || (MyAndromeda = {}));
//# sourceMappingURL=MyAndromeda.App.All.js.map
