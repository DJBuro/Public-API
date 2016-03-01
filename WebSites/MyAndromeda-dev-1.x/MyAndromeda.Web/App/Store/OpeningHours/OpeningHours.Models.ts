module MyAndromeda.Stores.OpeningHours.Models {

    export var occasionDefinitions = {
        Delivery: { Name: "Delivery", Colour: "#d9534f" },
        Collection: { Name: "Collection", Colour: "#d9edf7" },
        DineIn: {
            Name: "Dine In", Colour: "#f2dede"
        }
    };

    export interface IOccasionTask {
        Id: string;
        title: string;
        Title: string;
        start: Date;
        end: Date;
        Start: string;
        End: string;
        StartTimezone: string;
        EndTimezone: string;
        Description: string;
        RecurrenceId: string;
        RecurrenceRule: string;
        RecurrenceException: string;
        IsAllDay: boolean;
        Occasions: string;
    }


    export function getSchedulerDataSourceSchema() {
        let model = <kendo.data.DataSourceSchemaModel>{
            id: "Id",
            fields: {
                Id: <kendo.data.DataSourceSchemaModelField>{
                    type: "string",
                    nullable: true
                },
                title: { from: "Title", validation: { required: true } },
                start: { type: "date", from: "Start" },
                end: { type: "date", from: "End" },
                startTimezone: { from: "StartTimezone" },
                endTimezone: { from: "EndTimezone" },
                description: { from: "Description" },
                recurrenceId: { from: "RecurrenceId" },
                recurrenceRule: { from: "RecurrenceRule" },
                recurrenceException: { from: "RecurrenceException" },
                isAllDay: { type: "boolean", from: "IsAllDay" },
                Occasions: {
                    defaultValue : ["Delivery", "Collection"]
                }
                //Occasions: <kendo.data.DataSourceSchemaModelField>{
                //    type: "string",
                //    defaultValue: "",
                //    nullable: false,
                //    validation: {
                //        required: true
                //    }
                //}
            }
        }

        return model;
    };
    
} 