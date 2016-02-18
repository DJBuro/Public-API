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

    

    export var getSchedulerDataSourceSchema = (andromedaSiteId: number, employeeId?: string) => {
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
                    }
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
        IsAllDay: boolean;
        EmployeeId: string;
        AndromedaSiteId: number;
        TaskType: string;
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