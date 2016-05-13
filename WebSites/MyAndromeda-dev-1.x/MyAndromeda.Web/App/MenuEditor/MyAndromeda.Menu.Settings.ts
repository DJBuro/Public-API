module MyAndromeda.Menu.Settings {
    export class Routes {
        public static Toppings: IToppingRoutes = {
            List: "",
            Update: ""
        } 

        public static MenuItems: IMenuIemRoutes = {
            ListMenuItems: "",
            SaveMenuItems: "",
            SaveImageUrl: "",
            RemoveImageUrl: "",
        }

        public static Ftp : IFtpRoutes = {
            DownloadMenu: "",
            UploadMenu: "",
            Version: "",
            Delete:""
        }

        public static Publish: string = "";
    }

    export interface IToppingRoutes {
        List: string;
        Update: string;
    };

    export interface IPublishRoutes {

    };

    export interface IMenuIemRoutes {
        ListMenuItems: string;
        SaveMenuItems: string;
        SaveImageUrl: string;
        RemoveImageUrl: string;
    };

    export interface IFtpRoutes {
        Version: string;
        DownloadMenu: string;
        UploadMenu: string;
        Delete: string;
    }
}