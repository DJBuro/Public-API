using System;
using System.Data.Entity;
using MyAndromeda.Data.DataWarehouse.Models;
using MyAndromeda.Web.Controllers.Api.Hr;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MyAndromeda.Web.Controllers.Api.Hr.Models
{
    public static class EmployeeRecordModelExtensions
    {
        public static EmployeeRecordModel ToViewModel(this EmployeeRecord dbItem)
        {
            EmployeeRecordModel result = null;
            if (string.IsNullOrWhiteSpace(dbItem.Data))
            {
                dbItem.Data = "{}";
            }
            try
            {
                var converter = new ExpandoObjectConverter();    

                var model = JsonConvert.DeserializeObject<EmployeeRecordModel>(dbItem.Data, converter);
                
                model.Id = dbItem.Id;
                model.Name = dbItem.Name;

                result = model;
            }
            catch (Exception ex)
            {
                throw;
            }

            return result;
        }

        public static void UpdateProperties(this EmployeeRecord dbItem, EmployeeRecordModel model)
        {
            var data = JsonConvert.SerializeObject(model);

            dbItem.Data = data;
            dbItem.Deleted = model.Deleted;
            dbItem.Name = model.Name;
            dbItem.LastUpdatedUtc = DateTime.UtcNow;

            //dont forget the id;
            model.Id = dbItem.Id;
        }

        public static EmployeeRecord CreateDbItem(this DbSet<EmployeeRecord> table, EmployeeRecordModel model, int andromedaSiteId, bool attach = true)
        {
            var record = table.Create();

            record.Id = Guid.NewGuid();
            record.CreatedUtc = DateTime.UtcNow;
            record.EmployeeStoreLinkRecords.Add(new EmployeeStoreLinkRecord()
            {
                AdromedaSiteId = andromedaSiteId
            });

            // the rest should be in update properties
            record.UpdateProperties(model);

            if (attach)
            {
                table.Add(record);
            }

            return record;
        }
    }
}