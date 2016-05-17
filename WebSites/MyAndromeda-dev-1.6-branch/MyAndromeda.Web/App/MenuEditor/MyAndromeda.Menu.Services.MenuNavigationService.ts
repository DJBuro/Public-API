module MyAndromeda.Menu.Services {
    Angular.ServicesInitilizations.push((app) => {
        app.factory(MenuNavigationService.Name, [
            () => {
                var instance = new MenuNavigationService(); 

                return instance;
            }
        ]);
    });


    export class MenuNavigationService {
        public static Name : string = "MenuNavigationService";
        
        public constructor () {
            
        }
    }
}