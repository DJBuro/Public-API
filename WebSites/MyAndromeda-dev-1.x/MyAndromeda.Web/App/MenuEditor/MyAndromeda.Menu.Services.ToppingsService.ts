module MyAndromeda.Menu.Services {
    Angular.ServicesInitilizations.push((app) => {
        app.factory(MenuToppingsService.Name, [
            () => {
                var instnance = new MenuToppingsService();
                return instnance;
            }
        ]);
    });

    export class MenuToppingsService {
        public static Name: string = "ToppingsService";

        private dataSource: kendo.data.DataSource;

        constructor()
        {
            
        }

        public GetDataSource(): kendo.data.DataSource 
        {
            var internal = this;
            if(this.dataSource){ return this.dataSource; }
            
            var routes = Settings.Routes.Toppings;

            var dataSourceGroup : kendo.data.DataSourceGroupItem[] = [
                { field: "Name", dir: "" }
            ];
            this.dataSource = new kendo.data.DataSource({
                type: "aspnetmvc-ajax",
                batch: true,
                pageSize: 10,
                //group: dataSourceGroup,
                transport: { 
                    read: { url : routes.List },
                    update: { url : routes.Update }
                },
                schema: {
                    data:"Data",
                    total:"Total",
                    errors:"Errors",
                    model: {
                        id: "Id", 
                        UpdateAllDelivery : function(confirm) {
                            var item : Models.IToppingGroup = this;

                            item.ToppingVarients.forEach((varient) => {
                                var c = item.get("CollectionPrice"), d = item.get("DeliveryPrice"), s =  item.get("DineInPrice");
                                varient.set("DeliveryPrice", d);
                                varient.trigger("change");
                            });

                            item.trigger("change");
                        },
                        UpdateAllCollection : function(confirm) {
                            var item : Models.IToppingGroup = this;

                            item.ToppingVarients.forEach((varient) => {
                                var c = item.get("CollectionPrice");
                                
                                varient.set("CollectionPrice", c);

                                varient.trigger("change");
                            });
                        },
                        UpdateAllDineIn : function(confirm) {  
                            var item : Models.IToppingGroup = this;

                            item.ToppingVarients.forEach((varient) => {
                                var s = item.get("DineInPrice");
                                
                                varient.set("DineInPrice", s);

                                varient.trigger("change");
                            });
                        },
                        UpdateAllToppingPrices : function(confirm) {
                            var item : Models.IToppingGroup = this;
                            console.log(item);

                            item.ToppingVarients.forEach((varient) => {
                                var c = item.get("CollectionPrice"), d = item.get("DeliveryPrice"), s =  item.get("DineInPrice");
                                
                                varient.set("CollectionPrice", c);
                                varient.set("DeliveryPrice", d);
                                varient.set("DineInPrice", s);

                                varient.trigger("change");
                            });

                            item.trigger("change");

                        },
                        ColorStatus: function(){
                            var item : Models.IToppingGroup = this;
                            if (this.dirty) { return "#F29A00"; }
                            return "#A4E400";
                        }
                    }
                }
            }); 
            
            return this.dataSource;   
        }

        public FindTopppings(predicate : (topping : Models.ITopping) => boolean) : Array<Models.ITopping>
        {
            var data = this.dataSource.data();
            var filtered = data.filter(predicate);
            return filtered;   
        }

    }
}