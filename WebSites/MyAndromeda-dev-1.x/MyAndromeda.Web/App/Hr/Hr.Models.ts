module MyAndromeda.Hr.Models {
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