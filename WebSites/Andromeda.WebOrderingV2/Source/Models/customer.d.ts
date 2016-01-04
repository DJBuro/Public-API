declare module AndroWeb.Models
{
    export interface ICustomer
    {
        accountNumber: string;
        address: any;
        contacts: IContact[];
        facebookId: string;
        facebookUsername: string;
        firstName: string;
        surname: string;
        title: string;

        loyalties: ICustomerLoyalty[]
    }

    export interface IContact
    {

    }

    export interface ICustomerObservable 
    {
        firstName: KnockoutObservable<string>;

        loyalties: KnockoutObservableArray<ICustomerLoyalty>;
    }

    export interface ICustomerLoyalty
    {
        Id: string;
        CustomerId: string;
        ProviderName: string;
        Points?: number;
        PointsGained?: number;
        PointsUsed?: number;
    }
    
    
} 