/// <reference path="MyAndromeda.WebOrdering.App.ts" />

module MyAndromeda.WebOrdering.Controllers {
    Angular.ControllersInitilizations.push((app) => {
        app.controller(CarouselController.Name,
            [
                '$scope', '$timeout',

                Services.ContextService.Name,
                Services.WebOrderingWebApiService.Name,

                ($scope, $timeout, contextService, webOrderingWebApiService) => {

                    CarouselController.OnLoad($scope, $timeout, contextService, webOrderingWebApiService);

                    /* going to leave kendo to manage the observable object */
                    CarouselController.SetupCarousels($scope, $timeout, contextService);
                }
            ]);
    });

    export class CarouselController {
        public static Name: string = "CarouselController";

        public static OnLoad(
            $scope: Scopes.ICarouselControllerScope,
            $timout: ng.ITimeoutService,
            contextService: Services.ContextService,
            webOrderingWebApiService: Services.WebOrderingWebApiService) {
            /* Add image auto upload */
            $scope.ShowCarousel = true;
            $scope.ShowHtmlEditor = false;
            $scope.ShowImageEditor = false;

            $scope.SaveChanges = () => {
                var data = $scope.CarouselBlocks;
                data.forEach((block) => {
                    console.log("sync block " + block.Carousel.Name);

                    /* this important otherwise the local data source does not update the main object */
                    //block.DataSource.sync();
                    //block.Carousel.h
                });

                webOrderingWebApiService.Update();
            };

            /* These are setup depending on the task ... create / edit */
            $scope.CancelItem = () => { };
            $scope.SaveItem = () => { };

        }

        public static CreateGuid() {
            var S4 = () => {
                return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
            };

            // then to call it, plus stitch in '4' in the third group
            var guid = (S4() + S4() + "-" + S4() + "-4" + S4().substr(0, 3) + "-" + S4() + "-" + S4() + S4() + S4()).toLowerCase();
            return guid;
        }

        public static CreateItem(type: string): kendo.data.Model {
            return new kendo.data.Model({
                Id: CarouselController.CreateGuid(),
                Type: type,
                ImageUrl: "",
                Description: "",
                HTML: ""
            });
        }


        public static LoadImageEditor(
            $scope: Scopes.ICarouselControllerScope,
            $timout: ng.ITimeoutService,
            carousel: Models.ICarousel, item) {
            $scope.ShowImageEditor = true;
            $scope.ShowCarousel = false;
            $scope.ShowHtmlEditor = false;

            $scope.HasImageSlideUrl = item.ImageUrl
            kendo.unbind("#ImageTemplate");
            kendo.bind("#ImageTemplate", item);

            //part of the scope again.
            var upload = $scope.CarouselImageUpload;

            var carouselImageUploadUrl = kendo.format("/api/{0}/AndroWebOrdering/{1}/UploadCarouselImage/{2}/{3}",
                Settings.AndromedaSiteId,
                Settings.WebSiteId,
                carousel.Name,
                item.Id);

            Logger.Notify("carouselImageUploadUrl" + carouselImageUploadUrl);
            upload.setOptions({

                async: {
                    saveUrl: carouselImageUploadUrl,
                    autoUpload: true
                },
                showFileList: false, 
                multiple: false 
            });

            upload.unbind("success");
            upload.bind("success", (result) => {

                console.log(result.response);
                console.log(result.response.Url);
                var r = Math.floor(Math.random() * 99999) + 1;
                item.set("ImageUrl", result.response.Url+"?k="+r);

                $timout(() => {
                    $scope.HasImageSlideUrl = true;
                    $scope.BindCarouselsList();
                });
            });
        }

        public static LoadHtmlEditor($scope: Scopes.ICarouselControllerScope, item) {
            $scope.ShowCarousel = false;
            $scope.ShowHtmlEditor = true;
            $scope.ShowImageEditor = false;

            kendo.unbind("#HtmlTemplate");
            kendo.bind("#HtmlTemplate", item);
        }

        public static ResetForm($scope: Scopes.ICarouselControllerScope,
            $timeout: ng.ITimeoutService, contextService): void {
            $timeout(() => {
                $scope.ShowCarousel = true;
                $scope.ShowHtmlEditor = false;
                $scope.ShowImageEditor = false;
            });
            $scope.BindCarouselsList();
        }

        public static SetupCarousels(
            $scope: Scopes.ICarouselControllerScope,
            $timeout: ng.ITimeoutService,
            contextService: Services.ContextService): void {
            var actionsTemplate = $("#EditButtonTemplate").html();

            $scope.EditCarouselItem = (carouselName: string, carouselItemId: string) => {
                console.log("clicked on a item");
                var s = kendo.format("index: {0}; item: {1}", carouselName, carouselItemId);

                var carousels = $scope.CarouselBlocks.filter((e) => { return e.Carousel.Name == carouselName });
                var c = carousels[0];
                //find the carousel item 
                var m: Models.ICarouselItem = c.DataSource.view().find((item: Models.ICarouselItem) => { return item.Id == carouselItemId; });
                $scope.HtmlBeforeEdit = m.HTML;

                switch (m.Type.toLowerCase()) {
                    case "image": this.LoadImageEditor($scope, $timeout, c.Carousel, m);
                        break;
                    case "html": this.LoadHtmlEditor($scope, m);
                        break;
                    default: throw kendo.format("{0} type is not supported", m.Type)
                }

                $scope.SaveItem = () => {
                    
                    //(c.DataSource.view().find((item: Models.ICarouselItem) => { return item.Id == carouselItemId; })).ImageUrl += "?k=" + r;
                    CarouselController.ResetForm($scope, $timeout, contextService);
                    $scope.SaveChanges();
                };
                $scope.CancelItem = (Id) => {
                    (c.DataSource.view().find((item: Models.ICarouselItem) => { return item.Id == carouselItemId; })).HTML = $scope.HtmlBeforeEdit;                                       
                    CarouselController.ResetForm($scope, $timeout, contextService);
                    $scope.HtmlBeforeEdit = "";
                };
            };

            $scope.CreateImageCarouselItem = (carouselName: string) => {
                var carousels = $scope.CarouselBlocks.filter((e) => { return e.Carousel.Name == carouselName });
                var c = carousels[0];

                var newCarouselItem = CarouselController.CreateItem("image");

                //c.DataSource.add(newCarouselItem);
                $scope.SaveItem = () => {
                    c.DataSource.add(newCarouselItem);
                    CarouselController.ResetForm($scope, $timeout, contextService);
                    $scope.SaveChanges();
                };

                $scope.CancelItem = () => {
                    CarouselController.ResetForm($scope, $timeout, contextService);
                };

                CarouselController.LoadImageEditor($scope, $timeout, c.Carousel, newCarouselItem);
            };

            $scope.CreateHtmlCarouselItem = (carouselName: string) => {
                var carousels = $scope.CarouselBlocks.filter((e) => { return e.Carousel.Name == carouselName });
                var c = carousels[0];
                var newCarouselItem = CarouselController.CreateItem("Html");

                $scope.SaveItem = () => {
                    c.DataSource.add(newCarouselItem);
                    CarouselController.ResetForm($scope, $timeout, contextService);
                    $scope.SaveChanges();
                };

                $scope.CancelItem = () => {
                    CarouselController.ResetForm($scope, $timeout, contextService);
                };

                CarouselController.LoadHtmlEditor($scope, newCarouselItem);
            };


            $scope.RemoveCarouselItem = (carouselName: string, carouselItemId: string) => {
                var carousels = $scope.CarouselBlocks.filter((e) => { return e.Carousel.Name == carouselName });
                var c = carousels[0];
                var m = c.DataSource.view().find((item: Models.ICarouselItem) => { return item.Id == carouselItemId; });

                //remove from data source
                c.DataSource.remove(m);
                $scope.SaveChanges();
            };

            $scope.BindCarouselsList = () => {
                var toolbar = $("#CarouselHeaderTemplate").html();
                var editTemplate = $("#CarouselItemEditTemplate").html();
                var descriptionTemplate = $("#DescriptionTemplate").html();

                var settingsSubscription = contextService.ModelSubject
                    .where((e) => { return e !== null; })
                    .subscribe((websiteSettings) => {
                        $timeout(() => {
                            var carousels = websiteSettings.Home.Carousels;
                            var carouselEditors: Models.ICarouselEditorModel[] = carousels.map((carousel, index) => {

                                var dataSource = new kendo.data.DataSource({
                                    data: carousel.Items,
                                    schema: {
                                        model: {
                                            id: "Id"
                                        }
                                        //id: "Id"
                                    }

                                });

                                var model: Models.ICarouselEditorModel = {
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

                $scope.$on('$destroy', () => {
                    settingsSubscription.dispose();
                });

            }
           $scope.BindCarouselsList();
        }

    }


} 