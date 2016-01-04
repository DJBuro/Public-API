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
