module MyAndromeda.Stores.OpeningHours.Models {

    export function getSchedulerDataSourceSchema() {
        let model = <kendo.data.DataSourceSchemaModel>{
            id: "Id",
            fields: {
                Id: <kendo.data.DataSourceSchemaModelField>{
                    type: "string",
                    nullable: true
                },
                title: { from: "Title", defaultValue: "No title", validation: { required: true } },
                start: { type: "date", from: "Start" },
                end: { type: "date", from: "End" },
                startTimezone: { from: "StartTimezone" },
                endTimezone: { from: "EndTimezone" },
                description: { from: "Description" },
                recurrenceId: { from: "RecurrenceId" },
                recurrenceRule: { from: "RecurrenceRule" },
                recurrenceException: { from: "RecurrenceException" },
                isAllDay: { type: "boolean", from: "IsAllDay" },
                Occasions: <kendo.data.DataSourceSchemaModelField>{
                    type: "string",
                    defaultValue: "All",
                    nullable: false,
                    validation: {
                        required: true
                    }
                }
            }
        }

        return model;
    };
    
} 