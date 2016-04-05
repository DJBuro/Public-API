module MyAndromeda.Hr.Models {
    export interface IStore {
        ChainId: number;
        AndromedaSiteId: number;
        Name: string;
    }

    export interface IEmployee
    {
        //Store?: string;

        Id?: string;
        Deleted?: boolean;

        Code?: string;
        ShortName?: string;
        Name?: string;
        Email?: string;
        Phone?: string;

        Gender?: string;
        DateOfBirth?: string;
        Department?: string;
        PrimaryRole?: string;
        Roles: string[];
        ProfilePic?: string; //end url.
        
        Skills?: string[];
        DrivingLicense?: string;
        PayrollNumber?: string;
        NationalInsurance?: string; 

        Documents: Models.IEmployeeDocument[];

        ShiftStatus?: IEmployeeShiftStatus;
    }

    export interface ITaskType {
        text: string;
        value: string; 
        color: string;         
    }

    export var taskTypes: Array<ITaskType> = [
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

    export var departments = [
        { text: 'Front of house', majorColour: "#AA6C39", minorColour: "#FFD0AA" },
        { text: 'Kitchen', majorColour: "#2D882D", minorColour: "#87CC87" },
        { text: 'Management', majorColour: "#AA3939", minorColour: "#FFAAAA" },
        { text: 'Delivery', majorColour: "#483d8b", minorColour: "#938CC5" }
    ];

    export var getSchedulerDataSourceSchema = (andromedaSiteId: number, employeeService: Services.EmployeeService, employeeId?: string) => {
        let employeePart = () => {
            var employee = <kendo.data.DataSourceSchemaModelField>{
                type: "string",
                //defaultValue: employeeId,
                nullable: false,
                validation: {
                    required: true
                }
            }

            if (employeeId)
            {
                employee.defaultValue = employeeId;
            }

            return employee;
        };

        let model = <kendo.data.DataSourceSchemaModel>{
                id: "Id",
                fields: {
                    Id: <kendo.data.DataSourceSchemaModelField>{
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
                    AndromedaSiteId: <kendo.data.DataSourceSchemaModelField>{
                        type: "number",
                        defaultValue: andromedaSiteId,
                        nullable: false,
                        validation: {
                            required: true
                        }
                    },
                    TaskType: <kendo.data.DataSourceSchemaModelField>{
                        type: "string",
                        defaultValue: "Shift",
                        nullable: false,
                        validation: {
                            required: true
                        }
                    },
                    Department: <kendo.data.DataSourceSchemaModelField>{
                        type: "string",
                        nullable: true
                    }
                    //Employee: function () {
                    //    let id = this.EmployeeId;
                    //    if (!id) {
                    //        return;
                    //    }

                    //    let employee = employeeService.StoreEmployeeDataSource.get(id);
                    //    return employee;
                    //},
                    //Department: function () {
                    //    var employee: Models.IEmployee = this.Employee;
                    //    if (employee) {
                    //        return employee.Department;
                    //    }

                    //    return "";
                    //}
                }
        }

        return model;
    };

    
    export var employeeDataSourceSchema: kendo.data.DataSourceSchemaModelWithFieldsObject = {
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

    export interface IEmployeeTask
    {
        id: string;
        Id: string;
        Title: string;
        start: Date;
        end: Date;
        Start: string;
        End: string;
        StartTimezone: string;
        EndTimezone: string;
        Description: string;
        RecurrenceId: string;
        RecurrenceRule: string;
        RecurrenceException: string;
        isAllDay: boolean;
        IsAllDay: boolean;
        EmployeeId: string;
        AndromedaSiteId: number;
        TaskType: string;
        Department: string;
    }

    export interface IEmployeeDocument
    {
        Id: string;
        Name?: string;
        //DocumentUrl?: string;
        Files: IFile[]
    }

    export interface IFile
    {
        FileName?: string
    }

    export interface IEmployeeShiftStatus
    {
        OnShift?: boolean;
        OnCall?: boolean;
        Available?: boolean;
    }

    export interface IEmployeeGlobalState extends ng.ui.IState  {
        chainId: number;
    }

    export interface IEmployeeStoreListState extends IEmployeeGlobalState {
        andromedaSiteId: number;
    }
} 