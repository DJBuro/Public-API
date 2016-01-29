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

        ShiftStatus?: IEmployeeShiftStatus;
    }

    export interface IEmployeeDocument
    {
        Name?: string;
        DocumentUrl?: string;
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