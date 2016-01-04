module MyAndromeda.Reporting.Models 
{
    export class Result { 
        public key: string;
        public data: kendo.data.DataSource;
            
        constructor(key: string, data: any) {
            this.key = key;
            if (data instanceof kendo.data.DataSource) {
                this.data = data;
            } else {
                this.CreateDataSource(data);
            }
        }

        private CreateDataSource(data: any) {
            var model = { data: []};
            if (!data) { throw new Error("Data is missing") }
            if (data.length) {
                model.data = data;
            } else if (data.data) {
                //suggests that data is a data source scaffold instead. 
                model = data;
            }

            this.data = new kendo.data.DataSource(model);
        }

        public Bind(eventName: string, handler: Function) {
            this.data.bind(eventName, handler);
        }
    }

    export interface ISummaryOrder {
        ACSOrderId: string;
        TimeStamp: Date;
        OrderPlacedTime: Date;
        WeekNumber: number;
        DeliveryCharge: number;
        FinalPrice: number;
        PayType: string;
        TotalTax: number;
        FirstName: string;
        LastName: string;
        OrderType: string;
    }

    export interface IDailyServiceMetrics {
        TotalOrders: number;
        OrdersLessThan30Mins: number;
        PercentageLessThan30Mins: number;
        OrdersLessThan45Mins: number;
        PercentageLessThan40Mins: number;
        AvgMake: number;
        AvgOutTheDoor: number;
        AvgDoorTime: number;
        RackTime: number;
    }

    export interface IChainDailySummaryOptions extends IDailySummaryOptions {
        gridElement: string;
        hourlyUrl: string;
        tabStripItemName: string;
        tabStripElement: string;
    }
} 