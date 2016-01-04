declare module AndroWeb.Models {
    export interface IMenuItemWrapper {
        src: string;
        templateName: string;
        name: string;
        description: string;
        price: string;
        className: string;
        id: number;

        menuItem: IMenuItem;
    }
    export interface IMenuItem {
        name: string;
        Desc: string;
        ColPric: number;
        DelPrice: number;
    }

    export interface IThumbnail {
        Src: string;
    }

    export interface IMyAndromedaThumbnailObject {
        Server: {
            Endpoint: string
        }

    }

    export interface ISite {
        siteId?: string;
        name?: string;
        isOpen?: boolean;
        estDelivTime?: number;
    }

    export interface ISiteDetails extends ISite {
        siteLoyalties?: ILoyaltyProvider[];

    }
}