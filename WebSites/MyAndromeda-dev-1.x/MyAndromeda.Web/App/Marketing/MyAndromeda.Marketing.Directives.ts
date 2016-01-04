/// <reference path="myandromeda.marketing.ts" />
module MyAndromeda.Marketing {
    m.controller('UibTabsetAdvancedController', function($scope, SweetAlert) {
        var ctrl = this,
            tabs = ctrl.tabs = $scope.tabs = [];

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
            } else if (tab.active) {
                ctrl.select(tab);
            } else {
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

    m.directive("uibTabsetExtension", () => {
        return {
            restrict: 'EA',
            transclude: true,
            replace: true,
            scope: {
                type: '@'
            },
            controller: 'UibTabsetAdvancedController',
            templateUrl: 'template/tabs/tabset.html',
            link: function (scope: any, element, attrs) {
                scope.vertical = angular.isDefined(attrs.vertical) ? scope.$parent.$eval(attrs.vertical) : false;
                scope.justified = angular.isDefined(attrs.justified) ? scope.$parent.$eval(attrs.justified) : false;
            }
        };
    });

    m.directive("recipientTemplate", () => {
        return {
            name: "recipientTemplate",
            restrict: "E", 
            transclude: true,
            link: ($scope: any, element, attrs, controller, transclude) => {
                transclude($scope, (clone, scope) => {
                    element.append(clone);
                });
            },
            controller: ($scope, recipientService: RecipientService) => {
                var andromedaSiteId = $scope.andromedaSiteId;
                var dataSource = new kendo.data.DataSource({
                    transport: {
                        read: (options) => {
                            
                            var model = $scope.model
                            var promise = recipientService.LoadRecipients(andromedaSiteId, model);

                            promise.then(success => {
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

                var gridOptions: kendo.ui.GridOptions;
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

    m.directive("updateBodyContent", () => {
        return {
            name: "updateBodyContent",
            restrict: "A",
            link: ($scope, element, attrs) => {
                var $body = $(element).contents().find('body');

                $(element).load(() => {
                    $body = $(element).contents().find('body');
                });

                attrs.$observe("dynamicContent", (val) => {
                    $body.html(val);
                });
            }
        };
    });

    m.directive("contactTemplate", () => {
        return {
            name: "contactTemplate",
            restrict: "E",
            "require": [
                "^andromedaSiteId"
            ],
            scope: {
                andromedaSiteId: "@"
            },
            controller: ($scope, $element,
                SweetAlert,
                marketingEventService: MarketingEventService,
                progressService: Services.ProgressService) =>
            {
                var andromedaSiteId = $scope.andromedaSiteId;
                var request = marketingEventService.LoadContactDetails(andromedaSiteId);
                var save = () => {
                    var validator: kendo.ui.Validator = $scope.validator;

                    if (!validator.validate()) { return; }

                    progressService.ShowProgress($element);

                    var promise = marketingEventService.SaveContact(andromedaSiteId, $scope.contact);

                    Rx.Observable
                        .fromPromise(promise)
                        .subscribe((result) => {
                            Logger.Notify("saved");
                            progressService.HideProgress($element);

                            SweetAlert.swal("Saved!","", "success");
                        }, (ex) => {
                            SweetAlert.swal("A error happened!", "Try again?", "error");
                        }, () => { });

                };

                var mainToolbarOptions: kendo.ui.ToolBarOptions = {
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
                    .subscribe((result) => {
                        var data = result.data;
                        $scope.contact = data;

                    }, (ex) => { }, () => { });

                $scope.mainToolbarOptions = mainToolbarOptions;
                
            },
            templateUrl: "contact-template.html"
        };
    });

    m.directive("eventTemplate", () => {
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
            controller: ($scope, $element,
                SweetAlert,
                marketingEventService: MarketingEventService,
                progressService: Services.ProgressService) =>
            {
                var andromedaSiteId = $scope.andromedaSiteId;
                var container = $scope.container;
                var externalSiteId = $scope.externalSiteId;
                var showSendingButton = $scope.showSendingButton;



                var load = () => {
                    var promise: ng.IHttpPromise<Models.IMarketingType>;

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
                            break
                        }
                        default: {
                            alert("marketing type is not setup for this section: " + $scope.marketingType);
                            return;
                        }
                    }

                    return promise;
                };

                var save = () => {

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

                    Rx.Observable.fromPromise(promise).subscribe((result) => {
                        progressService.HideProgress($element);
                        SweetAlert.swal("Saved!", "", "success");
                    }, (ex) => {
                        SweetAlert.swal("Error!", "Try again?", "error");
                    }, () => { });

                };

                var sendNow = () => {
                    progressService.ShowProgress($element);

                    var promise = marketingEventService.SendNow(andromedaSiteId, $scope.model);

                    Rx.Observable.fromPromise(promise).subscribe((result) => {
                        progressService.HideProgress($element);
                    });
                };

                var openPreivew = () => {
                    var popup: kendo.ui.Popup = $scope.createPreviewWindow;
                    var p: any = popup;
                    p.center();
                    popup.open();
                };

                var sendPreview = () => {
                    var validator: kendo.ui.Validator = $scope.previewValidator;
                    if (!validator.validate()) {
                        return;
                    }

                    var popup: kendo.ui.Popup = $scope.createPreviewWindow;
                    popup.close();
                    var promise = marketingEventService.PreviewEmail(andromedaSiteId, {
                        Model: $scope.model,
                        Preview: $scope.previewSettings
                    });

                    Rx.Observable.fromPromise(promise).subscribe((result) => {
                        $scope.model = result.data;

                        var popup: kendo.ui.Popup = $scope.showPreviewWindow;
                        var p: any = popup;
                        //p.content(result.data.Preview);
                        p.center();
                        popup.open();

                    }, (ex) => { });
                };

                var preview = () => {
                    var popup: kendo.ui.Popup = $scope.createPreviewWindow;
                    var p: any = popup;
                    p.center();
                    popup.open();
                };


                var toolbarOptions: kendo.ui.ToolBarOptions = {
                    resizable: true,
                    items: [
                        { template: "<label>SMS:{{smsCount}}</label>" }
                    ]
                };

                var mainToolbarOptions: kendo.ui.ToolBarOptions = {
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
                            click: () => {
                                var window = $scope.showRecipientWindow;
                                window.center();
                                window.open();

                                var grid: kendo.ui.Grid = $scope.recipientList;
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
                        //template: "<a class='k-button' ng-click='sendNow()'>Launch</a>"
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


                var fileLocation = (filePath: string) => {
                    Logger.Notify("filePath:");
                    Logger.Notify(filePath);
                    var url = "http://cdn.myandromedaweb.co.uk/{0}/stores/{1}/{2}";
                    url = kendo.format(url, container, externalSiteId, filePath).toLowerCase();
                    return url;
                };

                var thumbPath = (folder: string, file: string) => {
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

                Rx.Observable.fromPromise(promise).subscribe((result) => {
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
        }
    });


    m.directive("tokenTemplate", () => {
        return {
            name: "tokenTemplate",
            restrict: "E",
            controller: ($scope: any, tokenDataService: TokenDataService) => {
                var selected: Models.IToken = null;
                var getTools = () => {
                    var tokenPicker : kendo.ui.DropDownList = $scope.tokenPicker;

                    return tokenPicker;
                };
                var select = () => {
                    var editor: kendo.ui.Editor = $scope.EmailTemplateEditor;
                    
                    var tokenPicker = getTools();
                    var selected = tokenPicker.dataItem();

                    editor.paste(selected.Value, {});
                };

                $scope.tokenDataSource = tokenDataService.dataSource;
                $scope.selectToken = select;

            },
            link: ($scope: any, element, attrs, controller, transclude) => {

                transclude($scope, (clone, scope) => {
                    element.append(clone);
                });

            },
            templateUrl: "emailTokenTemplate.html",
            transclude: true
        }
    });


} 