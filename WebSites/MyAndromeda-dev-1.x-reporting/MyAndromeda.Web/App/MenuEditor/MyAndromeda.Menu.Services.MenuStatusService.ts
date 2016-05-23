 module MyAndromeda.Menu.Services 
 {
    

    export class MenuStatus {
        public static Name = "MenuStatusService";

        public static LeaveMesssage = "There are unsaved changes to the menu. Are you sure you want to leave?";
        public static EditedMessage = "There are unsaved changes to the menu. Don't forget to save changes as you go along."; 

        public static HasChangesField = "HasChanges";
        public static CanPublishField = "CanPublish";
        public static MessageField = "Message";

        observable: kendo.data.ObservableObject
        dataSource: kendo.data.DataSource;

        constructor(dataSource: kendo.data.DataSource)
        {
            this.dataSource = dataSource;
            this.observable = kendo.observable({
                Message: MenuStatus.EditedMessage,
                HasChanges: false,
                CanPublish: false,
                SaveChanges: () => {
                    dataSource.sync();
                },
                CancelChanges: () => {
                    if (confirm("All changes will be lost, continue?")) {
                        dataSource.cancelChanges();
                    }
                }
            });

            this.intitDataSource();
            this.initPageEvents();
        }

        private intitDataSource(): void
        {
            var internal = this;
            this.dataSource.bind("change", () => {
                internal.observable.set(MenuStatus.HasChangesField, internal.dataSource.hasChanges());
                internal.observable.set(MenuStatus.CanPublishField, !internal.dataSource.hasChanges());
            });
        }

        private initPageEvents(): void
        {
            $(window).on("beforeunload", (e) => {
                if (this.dataSource.hasChanges()) { 
                    //return MenuStatus.LeaveMesssage;
                    e.preventDefault();
                }
            });
        }
    }

 }