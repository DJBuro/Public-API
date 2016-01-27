module MyAndromeda.Hr.Models {
    export interface IEmployee
    {
        Store?: string;

        Id?: string;
        Code?: string;
        Name?: string;
        PrimaryRole?: string;
        Roles: string[];
        ProfilePic?: string; //end url.
        Email?: string;
        Phone?: string;

        ShiftStatus: IEmployeeShiftStatus;
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