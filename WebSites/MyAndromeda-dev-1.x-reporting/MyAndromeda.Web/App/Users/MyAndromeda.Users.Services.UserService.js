//module MyAndromeda
//{
//    export module Services
//    {
//        export class UserService implements IUserService {
//            private serviceRoutes: IUserServiceRoutes;
//            public dataSource: kendo.data.DataSource;
//            constructor(serviceRoutes: IUserServiceRoutes)
//            {
//                this.serviceRoutes = serviceRoutes;
//            }
//            public findById(id: number): IUser {
//                var internal = this;
//                var user = <IUser>internal.dataSource.data().find(function (item, index, source) {
//                    return item.Id === id;
//                });
//                return user;
//            }
//            public findByUserName(userName: string): IUser {
//                var internal = this;
//                var user = <IUser>internal.dataSource.data().find(function (item, index, source) {
//                    return item.UserName === userName;
//                });
//                return user;
//            }
//            private initDataSource() : void {
//                var internal = this;
//                var data = {
//                    __RequestVerificationToken: internal.serviceRoutes.antifrorgeryToken
//                };
//                this.dataSource = new kendo.data.DataSource({
//                    transport: {
//                        read: internal.serviceRoutes.read,
//                        data: data
//                    }
//                });
//            } 
//            public init(): void {
//                this.initDataSource();
//            }
//        }
//    }
//}
//interface IUserServiceRoutes
//{
//    read: string;
//    antifrorgeryToken: string;
//    //readByChain: string;
//    //readByStore: string;
//} 
