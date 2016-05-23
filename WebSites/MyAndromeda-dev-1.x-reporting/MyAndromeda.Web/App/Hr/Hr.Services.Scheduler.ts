/// <reference path="hr.services.ts" />

module MyAndromeda.Hr.Services {
    var app = angular.module("MyAndromeda.Hr.Services.Scheduler", [
        "MyAndromeda.Hr.Services"
    ]);

    export class EmployeeSchedulerService {
        public saving: Rx.Subject<boolean> = new Rx.Subject<boolean>();

        constructor(private employeeServiceState: EmployeeServiceState, private employeeService: Services.EmployeeService, private SweetAlert) {

        }

        private GetResources(stores: Models.IStore[], employee?: Models.IEmployee): Array<{}> {
            let employeePart = () => {
                var part = {
                    field: "EmployeeId",
                    dataTextField: "ShortName",
                    dataValueField: "Id",
                    title: "Employee",
                    name: "Employee",
                    dataSource: <any>[]
                }


                if (employee) {
                    part.dataSource = [employee];
                    return part;
                }

                let employees = this.employeeService.StoreEmployeeDataSource;
                part.dataSource = employees;

                return part;
            };

            let departmentAvailable = false;
            if (employee) {
                if (employee.Department) {
                    Logger.Notify("department: " + employee.Department);
                    departmentAvailable = true;
                }
            }
            
            //= employee && employee.Department;
            let resources = [
                {
                    title: "Task",
                    field: "TaskType",
                    dataSource: Models.taskTypes
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
        }

        public GetStoreEmployeeScheduler(stores: Array<Models.IStore>) {
            let start = new Date();
            let end = new Date();

            start.setHours(0);
            end.setHours(24);

            let chainId = this.employeeServiceState.CurrentChainId,
                andromedaSiteId = this.employeeServiceState.CurrentAndromedaSiteId,
                currentStore = stores[0],
                dataSource = this.employeeService.GetDataSourceForStoreScheduler(chainId, andromedaSiteId);

            let employeeGroupTemplate = `
                <div>
                    #=text#
                </div>
                <div>
                    <span class="label" style="background-color:#=majorColor#">#=employee.Department #</span>
                </div>
                <div>
                    <span class="label" style="background-color:#=minorColor#">#=employee.PrimaryRole #</span>
                </div>
            `;

            let schedulerOptions: kendo.ui.SchedulerOptions = {
                date: new Date(),
                workDayStart: start,
                workDayEnd: end,
                majorTick: 60,
                minorTickCount: 1,
                workWeekStart: 0,
                workWeekEnd: 6,
                allDaySlot: false,
                dataSource: dataSource,
                timezone: "Etc/UTC",
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
                    <any>{
                        title: "Week Overview",
                        selected: true,
                        eventHeight: 20,
                        type: "kendo.ui.MonthTimeWeekView",
                        eventTemplate: "<working-task task='dataItem'></working-task>",
                        //dayTemplate: "",
                        dayTemplate: '#:kendo.toString(date, "dd")#',

                        group: <any>{
                            orientation: "vertical",
                            resources: ["Employee"]
                        }
                    }
                ],

                resize: function (e: any) {
                    Logger.Notify("resize"); Logger.Notify(e);
                    let checker = new EmployeeAvailabilityTestService(e.sender);

                    let valid = checker.IsWorkAvailable(e.start, e.end, e.event);
                    if (!valid) {
                        this.wrapper.find(".k-marquee-color").addClass("invalid-slot");
                        e.preventDefault();
                    }
                },
                resizeEnd: (e: any) => {
                    Logger.Notify("resize-end"); Logger.Notify(e);

                    var tester = new EmployeeAvailabilityTestService(e.sender);

                    if (tester.IsWorkAvailable(e.start, e.end, e.event)) {
                        e.event.set("dirty", true);

                        return;
                    }

                    Logger.Notify("cancel resize");

                    this.SweetAlert.swal("Sorry", "The employee already has a job in this range", "error");

                    e.preventDefault();
                },
                move: function (e: any) {
                    Logger.Notify("move"); Logger.Notify(e);

                    var tester = new EmployeeAvailabilityTestService(e.sender);

                    if (tester.IsWorkAvailable(e.start, e.end, e.event)) {
                        return;
                    }

                    this.wrapper.find(".k-event-drag-hint").addClass("invalid-slot");
                },
                moveEnd: (e) => {
                    Logger.Notify("move-end"); Logger.Notify(e);

                    var tester = new EmployeeAvailabilityTestService(e.sender);
                    if (tester.IsWorkAvailable(e.start, e.end, e.event)) {

                        e.event.set("dirty", true);
                        return;
                    }

                    Logger.Notify("cancel move");

                    this.SweetAlert.swal("Sorry", "The employee already has a job in this range", "error");

                    e.preventDefault();
                },
                add: (e) => {
                    Logger.Notify("team scheduler - add - run");
                    Logger.Notify(e);

                    var tester = new EmployeeAvailabilityTestService(e.sender);

                    Logger.Notify("test all day validation");
                    let validSelection = tester.IsAllDayValid(<any>e.event, (invalidReason) => {
                        this.SweetAlert.swal("Sorry", invalidReason, "error");
                    });

                    if (!validSelection) {
                        Logger.Notify("cancel add");
                        e.preventDefault();
                        return;
                    }

                    var tester = new EmployeeAvailabilityTestService(e.sender);
                    if (!tester.IsWorkAvailable(e.event.start, e.event.end, e.event)) {
                        Logger.Notify("cancel add");
                        this.SweetAlert.swal("Sorry", "The employee already has a job in this range", "error");

                        e.preventDefault();
                    }
                },
                save: (e) => {
                    Logger.Notify("save"); Logger.Notify(e);

                    var tester = new EmployeeAvailabilityTestService(e.sender);

                    Logger.Notify("test all day validation");
                    let validSelection = tester.IsAllDayValid(<any>e.event, (invalidReason) => {
                        this.SweetAlert.swal("Sorry", invalidReason, "error");
                    });

                    if (!validSelection) {
                        Logger.Notify("cancel add");
                        e.preventDefault();
                        return;
                    }

                    if (!tester.IsWorkAvailable(e.event.start, e.event.end, e.event)) {
                        Logger.Notify("cancel save");
                        this.SweetAlert.swal("Sorry", "The employee already has a job in this range", "error");

                        e.preventDefault();
                    }

                    e.event.set("dirty", true);
                }


            }

            return schedulerOptions;
        }

        public GetSingleEmployeeScheduler(stores: Array<Models.IStore>, employee: Models.IEmployee) {
            let start = new Date();
            let end = new Date();

            start.setHours(0);
            end.setHours(24);

            let chainId = this.employeeServiceState.CurrentChainId,
                andromedaSiteId = this.employeeServiceState.CurrentAndromedaSiteId,
                dataSource = this.employeeService.GetDataSourceForEmployeeScheduler(chainId, andromedaSiteId, employee.Id);
            
            let schedulerOptions: kendo.ui.SchedulerOptions = {
                date: new Date(),
                workDayStart: start,
                workDayEnd: end,
                majorTick: 60,
                minorTickCount: 1,
                workWeekStart: 0,
                workWeekEnd: 6,
                allDaySlot: false,

                dataSource: dataSource,
                //timezone: "Etc/UTC",
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
                resize: function (e: any) {
                    Logger.Notify("resize"); Logger.Notify(e);
                    let checker = new EmployeeAvailabilityTestService(e.sender);

                    let valid = checker.IsWorkAvailable(e.start, e.end, e.event);
                    if (!valid) {
                        this.wrapper.find(".k-marquee-color").addClass("invalid-slot");
                        e.preventDefault();
                    }
                },
                resizeEnd: (e: any) => {
                    Logger.Notify("resize-end"); Logger.Notify(e);

                    var tester = new EmployeeAvailabilityTestService(e.sender);

                    if (!tester.IsWorkAvailable(e.start, e.end, e.event)) {
                        Logger.Notify("cancel resize");
                        this.SweetAlert.swal("Sorry", "The employee already has a job in this range", "error");

                        e.preventDefault();
                    }
                },
                move: function (e: any) {
                    Logger.Notify("move"); Logger.Notify(e);

                    var tester = new EmployeeAvailabilityTestService(e.sender);

                    if (!tester.IsWorkAvailable(e.start, e.end, e.event)) {
                        this.wrapper.find(".k-event-drag-hint").addClass("invalid-slot");
                    }
                },
                moveEnd: (e) => {
                    Logger.Notify("move-end"); Logger.Notify(e);

                    var tester = new EmployeeAvailabilityTestService(e.sender);
                    if (!tester.IsWorkAvailable(e.start, e.end, e.event)) {
                        Logger.Notify("cancel move");

                        this.SweetAlert.swal("Sorry", "The employee already has a job in this range", "error");

                        e.preventDefault();
                    }
                },
                add: (e) => {
                    Logger.Notify("team scheduler - add - run");
                    Logger.Notify(e);

                    var tester = new EmployeeAvailabilityTestService(e.sender);

                    Logger.Notify("test all day validation");
                    let validSelection = tester.IsAllDayValid(<any>e.event, (invalidReason) => {
                        this.SweetAlert.swal("Sorry", invalidReason, "error");
                    });

                    if (!validSelection) {
                        Logger.Notify("cancel add");
                        e.preventDefault();
                        return;
                    }

                    if (!tester.IsWorkAvailable(e.event.start, e.event.end, e.event)) {
                        Logger.Notify("cancel add");
                        //SweetAlert.swal("Sorry!", name + " has been saved.", "success");
                        this.SweetAlert.swal("Sorry", "The employee already has a job in this range", "error");

                        e.preventDefault();
                    }
                },
                save: (e) => {
                    Logger.Notify("team scheduler - save - run");
                    Logger.Notify(e);

                    var tester = new EmployeeAvailabilityTestService(e.sender);

                    let validSelection = tester.IsAllDayValid(<any>e.event, (invalidReason) => {
                        this.SweetAlert.swal("Sorry", invalidReason, "error");
                    });
                    
                    if (!tester.PreverTasksOverlapping(<any>e.event)) {
                        let message = 'At this time employee has another event! You cannot overlap events!';
                        this.SweetAlert.swal("Sorry", message, "error");

                        e.preventDefault();
                    }

                    if (!validSelection) {
                        e.preventDefault();
                    }

                    if (!tester.IsWorkAvailable(e.event.start, e.event.end, e.event)) {
                        Logger.Notify("cancel save");
                        this.SweetAlert.swal("Sorry", "The employee already has a job in this range", "error");

                        e.preventDefault();
                    }
                }
            }

            return schedulerOptions;
        }

        public IsEmployeeFree() {
            //var occurrences = occurrencesInRangeByResource(start, end, "attendee", event, resources);
        }

        public OccurrencesInRangeByResource() {

        }
    }

    app.service("employeeSchedulerService", EmployeeSchedulerService);

}