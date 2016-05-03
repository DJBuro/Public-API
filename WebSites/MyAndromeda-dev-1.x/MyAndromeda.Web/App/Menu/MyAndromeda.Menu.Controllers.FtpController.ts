module MyAndromeda.Menu.Controllers {

    export class FtpController 
    {
        public static Name: string = "FtpController";

        public static SetupScope(
            $scope: IFtpControllerScope,
            $timeout: ng.ITimeoutService,
            ftpService: MyAndromeda.Menu.Services.FtpService)
        {
            $scope.DeleteLocalFile = () => { 
                var c = confirm("Are you sure you want to delete the current version? Thumbnails will not be effected.");
                if(c){
                    ftpService.DeleteLocalFile(); 
                }
            };

            $scope.DownloadFromFtp = () => ftpService.StartFtpDownload();
            $scope.UploadToFtp = () => ftpService.StartFtpUpload();
            $scope.CheckDbVersion = () => ftpService.GetVersion();

            //FTP download status 
            ftpService.FtpDownloadBusy.subscribe((busy) => {
                $timeout(() => { $scope.DownloadBusy = busy; });
            });
            ftpService.FtpDownloadErrors.subscribe((error) => {
                $timeout(() => { $scope.Errors = error; });
            });
            //FTP upload status
            ftpService.FtpUploadBusy.subscribe((busy) => {
                $timeout(() => { $scope.UploadBusy = busy; });
            });
            ftpService.FtpUploadErrors.subscribe((error) => {
                $timeout(() => { $scope.Errors = error; });
            });
            //FTP access status
            ftpService.LocalAccessVersion.subscribe((versionModel) => {
                $timeout(() => {
                    $scope.Version = versionModel.Version;
                    $scope.UpdatedOn = versionModel.UpdatedOn;
                    $scope.LastDownloaded = versionModel.LastDownloaded;
                });
            });
            

            var a = ftpService.FtpUploadBusy, 
                b = ftpService.FtpDownloadBusy, 
                c = ftpService.LocalAccessVersionBusy,
                d = ftpService.DeleteBusy;

            var anyBusy = Rx.Observable.combineLatest(a,b,c,d, (o1,o2,o3,o4) => {
                return o1 || o2 || o3 || o4;
            });

            anyBusy.subscribe(busy => {
                $timeout(()=> {
                    $scope.BlockAccess = busy;
                });
            });

            $scope.CheckDbVersion();   
        }
    }

    export interface IFtpControllerScope extends ng.IScope {
        DeleteLocalFile : () => void;
        DownloadFromFtp : () => void;
        UploadToFtp: () => void;
        CheckDbVersion: () => void;

        BlockAccess: boolean;
        DownloadBusy: boolean;
        UploadBusy: boolean;

        Version: string;
        UpdatedOn: string;
        LastDownloaded: string;

        HasErros: boolean;
        Errors: string;
    }
}

module MyAndromeda.Menu.Services {
    export class FtpService 
    {
        public static Name: string = "FtpControllerService";
        
        public $http: ng.IHttpService;

        public LocalAccessVersion: Rx.Subject<IMenuVersion>;
        public LocalAccessVersionBusy: Rx.BehaviorSubject<boolean>;

        public FtpDownloadBusy: Rx.BehaviorSubject<boolean>;
        public FtpDownloadErrors: Rx.ISubject<string>; 
        public FtpUploadBusy : Rx.BehaviorSubject<boolean>;
        public FtpUploadErrors : Rx.ISubject<string>;
        
        public DeleteBusy: Rx.BehaviorSubject<boolean>;

        constructor($http: ng.IHttpService)
        {
            this.$http = $http;

            this.FtpDownloadBusy = new Rx.BehaviorSubject<boolean>(false);
            this.FtpDownloadErrors = new Rx.Subject<string>();
            this.FtpUploadBusy = new Rx.BehaviorSubject<boolean>(false);
            this.FtpUploadErrors = new Rx.Subject<string>();
            this.LocalAccessVersion = new Rx.Subject<IMenuVersion>();
            this.LocalAccessVersionBusy = new Rx.BehaviorSubject<boolean>(false);
            this.DeleteBusy = new Rx.BehaviorSubject<boolean>(false);
        }

        private ValidateRoute(route: string) : void {
            route || (route = "");

            if(route.length === 0){
                throw "The route locations have not been set.";
            }
        }

        public GetVersion(): ng.IHttpPromise<IMenuVersion> {
            var route = Settings.Routes.Ftp.Version;

            this.ValidateRoute(route);
            
            var promise = this.$http.post(route, {});

            this.LocalAccessVersionBusy.onNext(true);

            promise.success((data: IMenuVersion, status, headers, config) => {
                this.LocalAccessVersion.onNext(data);
            }); 

            promise.finally(() => { this.LocalAccessVersionBusy.onNext(false); });

            return promise;
        }

        public StartFtpDownload(): void {
            var route = Settings.Routes.Ftp.DownloadMenu;
            
            this.ValidateRoute(route);
            this.FtpDownloadBusy.onNext(true);
            
            var promise = this.$http.post(route, {});
            
            promise.then((result) => {
                var versionPromise = this.GetVersion();

                return versionPromise;
            }).finally(() => {
                this.FtpDownloadBusy.onNext(false);
            });
        }

        public StartFtpUpload(): void {
            var route = Settings.Routes.Ftp.UploadMenu;
            
            this.ValidateRoute(route);
            this.FtpUploadBusy.onNext(true);
            
            var promise = this.$http.post(route, {});

            promise.finally(() => {
                this.FtpUploadBusy.onNext(false);
            });
        }

        public DeleteLocalFile(): void {
            var route = Settings.Routes.Ftp.Delete;

            this.ValidateRoute(route);

            this.DeleteBusy.onNext(true);

            var promise = this.$http.post(route, {});

            promise.finally(() => {
                this.DeleteBusy.onNext(false);
            });
        }
        
    }

    export interface IMenuVersion
    {
        Version: string;
        UpdatedOn: string;
        LastDownloaded: string;
    }
}
