module MyAndromeda.Stores.OpeningHours {

    var app = angular.module("MyAndromeda.Store.OpeningHours.Directives", []);

    app.directive("occasionTaskEditor", () => {
        return {
            name: "occasionTaskEditor",
            scope: {
                task: "=task",
            },
            templateUrl: "occasionTaskEditor.html",
            controller: ($scope ) => {
                var task: Models.IOccasionTask = $scope.task;

                Logger.Notify("Occasion task editor - started");

                Logger.Notify("Occasion task");
                Logger.Notify(task);

                let occasionOptions: kendo.ui.MultiSelectOptions = {
                    //dataSource: [
                    //    Models.occasionDefinitions.Delivery,
                    //    Models.occasionDefinitions.Collection,
                    //    Models.occasionDefinitions.DineIn
                    //],
                    dataSource: new kendo.data.DataSource({
                        data: [
                            Models.occasionDefinitions.Delivery,
                            Models.occasionDefinitions.Collection,
                            Models.occasionDefinitions.DineIn
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

    app.directive("repeatWeeklyEditor", () => {
        return {
            name: "repeatWeeklyEditor",
            templateUrl: "repeatWeeklyEditor.html",
            scoppe: {
                task: "=task"
            },
            controller: ($scope) => {
                var task: Models.IOccasionTask = $scope.task;

                Logger.Notify("repeatWeeklyEditor started: " + task); 

                const partialRecurrenceRule = "FREQ=WEEKLY;BYDAY=";
                let recurrenceRule = task.recurrenceRule;
                let rule = {
                    Monday: false,
                    Tuesday: false,
                    Wednesday: false,
                    Thursday: false,
                    Friday: false,
                    Saturday: false,
                    Sunday: false
                };
                let parseRuleToRecurrence = () => {
                    let recurrence = partialRecurrenceRule;
                    //MO,TU,WE,TH,FR,SA,SU
                    let hasDays = false;
                    let add = (day) => {
                        recurrence += (hasDays ? "," + day : day);
                        hasDays = true;
                        
                    };
                    if (rule.Monday) {
                        add("MO");
                    }
                    if (rule.Tuesday) {
                        add("TU");
                    }
                    if (rule.Wednesday) {
                        add("WE");
                    }
                    if (rule.Thursday) {
                        add("TH");
                    }
                    if (rule.Friday) {
                        add("FR");
                    }
                    if (rule.Saturday) {
                        add("SA");
                    }
                    if (rule.Sunday) {
                        add("SU");
                    }
                    return recurrence;
                };
                let parseRecurrenceToRule = () => {
                    var a = recurrenceRule.split(partialRecurrenceRule);
                    Logger.Notify("Split");
                    Logger.Notify(a);
                    var b = a[1];
                    var c = b.split(",");

                    c.forEach((day) => {
                        switch (day) {
                            case "MO": rule.Monday = true; break;
                            case "TU": rule.Tuesday = true; break;
                            case "WE": rule.Wednesday = true; break;
                            case "TH": rule.Thursday = true; break;
                            case "FR": rule.Friday = true; break;
                            case "SA": rule.Saturday = true; break;
                            case "SU": rule.Sunday = true; break;
                        }
                    });
                };

                parseRecurrenceToRule();

                $scope.rule = rule;
                $scope.ruleChanged = () => {
                    Logger.Notify("rule changed");
                    let newRule = parseRuleToRecurrence();
                    Logger.Notify(newRule);

                    task.recurrenceRule = newRule;
                };
            }

        };
    });

    app.directive("occasionTask", () => {

        return {
            name: "occasionTask",
            scope: {
                task: "=task",
            },
            templateUrl: "occasionTask.html",
            controller: ($scope, $element) => {

                var task: Models.IOccasionTask = $scope.task;

                var occasionTypeIsString = typeof (task.Occasions) === "string" ? true : false;
                var occasionIsArray = typeof (task.Occasions) === "object" ? true : false;

                var state = {
                    occasions:  task.Occasions,
                };

                var extra = {
                    hours: Math.abs(task.end.getTime() - task.start.getTime()) / 36e5,
                    startTime: kendo.toString(task.start, "HH:mm"),
                    endTime: kendo.toString(task.end, "HH:mm")
                };

                var definitions = Models.occasionDefinitions;
                let getColor = (name: string) => {
                    switch (name) {
                        case definitions.Delivery.Name: return Models.occasionDefinitions.Delivery.Colour;
                        case definitions.Collection.Name: return Models.occasionDefinitions.Collection.Colour;
                        case definitions.DineIn.Name: return Models.occasionDefinitions.DineIn.Colour;
                    }
                };

                $scope.getColour = getColor; 
                $scope.state = state;
                $scope.extra = extra; 

                let topElement = $($element).closest(".k-event");

                Logger.Notify("occasions");
                Logger.Notify(task.Occasions); 
                let twoColors = `repeating-linear-gradient(
                        45deg,
                        {0},
                        {0} 10px,
                        {1} 10px,
                        {1} 20px
                    )`;
                let threeColors = `repeating-linear-gradient(
                        45deg,
                        {0},
                        {0} 10px,
                        {1} 10px,
                        {1} 20px,
                        {2} 20px,
                        {2} 30px
                    )`;

                twoColors =
                    kendo.format(twoColors, definitions.Delivery.Colour, definitions.Collection.Colour);
                threeColors =
                    kendo.format(threeColors, definitions.Delivery.Colour, definitions.Collection.Colour, definitions.DineIn.Colour);

                if (task.Occasions.length == 2) {
                    topElement.css({
                        "background": twoColors,
                        //"text-shadow": "0px 0px 4px #ffffff"
                    });
                } else if (task.Occasions.length === 3) {
                    topElement.css({
                        "background": threeColors,
                        //"text-shadow": "0px 0px 4px #ffffff"
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

} 