declare module MyAndromeda.Users.Models
{
    export interface IUser {
        Id: number;
        Username: string;
        FirstName: string;
        SurName: string;

        Roles: IRole[];
    }

    export interface IObservableUser extends IUser {
        get: Function;
        set; Function;
        dirty: boolean;
    }

    export interface IUserService
    {
        findById(id: number): IUser;
        findByUserName(userName: string): IUser;

        dataSource: kendo.data.DataSource;

        init(): void;
    }

    export interface IRole
    {
        Id: number;
        Name: string;

        Permissions: IPermission[];
    }

    export interface IPermission
    {
        Id: number;
        Name: string;
        Description: string;   
    }

    export interface IAuthorizationService
    {
        addRole(IUser, IRole): void;
        removeRole(IUser, IRole): void;
    }

    export interface IUserAuthorizationService
    {
    
    }
}

