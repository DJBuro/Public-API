using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using MyAndromeda.Data.DataWarehouse.Models;
using MyAndromeda.Logging;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json.Converters;


namespace MyAndromeda.Web.Controllers.Api.Hr
{
    [RoutePrefix("hr/{chainId}/employees/{andromedaSiteId}")]
    public class EmployeeFileController : ApiController
    {
        private readonly IMyAndromedaLogger logger;
        private readonly DataWarehouseDbContext dbContext;
        private readonly DbSet<EmployeeRecord> employeeTable;

        public EmployeeFileController(IMyAndromedaLogger logger, DataWarehouseDbContext dbContext) 
        {
            this.dbContext = dbContext;
            this.employeeTable = this.dbContext.Set<EmployeeRecord>();
            this.logger = logger;
        }


        [HttpGet]
        [Route("list")]
        public async Task<List<EmployeeRecordModel>> List([FromUri]int andromedaSiteId) 
        {
            var query = this.employeeTable
                .Where(e => e.EmployeeStoreLinkRecords.Any(r => r.AdromedaSiteId == andromedaSiteId));

            var records = await query.ToListAsync();

            var models = records
                .Select((record) => record.ToViewModel())
                .ToList();

            //var models2 = models.Select((r) => r as dynamic);


            //var k = JsonConvert.SerializeObject(models);
            //var k2 = JsonConvert.SerializeObject(models2);

            //var converters = new List<Newtonsoft.Json.JsonConverter>();

            //var k3 = JsonConvert.SerializeObject(models2, new JsonSerializerSettings() { 
            //    CheckAdditionalContent=true,
            //    Converters = converters
            //});


            //var response = Request.CreateResponse(HttpStatusCode.OK, k2, Configuration.Formatters.JsonFormatter);

            return models; 

            //return response;
        }

        [HttpGet]
        [Route("get/{id}")]
        public async Task<EmployeeRecordModel> Get([FromUri]
                                                   int andromedaSiteId, [FromUri]
                                                   Guid id) 
        {
            var query = this.employeeTable.Where(e => e.EmployeeStoreLinkRecords.Any(k => k.AdromedaSiteId == andromedaSiteId));

            var record = await query.FirstOrDefaultAsync();

            return record.ToViewModel();
        }

        [HttpPost]
        [Route("update")]
        public async Task<EmployeeRecordModel> Update(
            [FromUri]
            int andromedaSiteId,
            [FromBody]
            EmployeeRecordModel model) 
        {

            var query = this.employeeTable
                            .Where(e => e.EmployeeStoreLinkRecords.Any(K => K.AdromedaSiteId == andromedaSiteId))
                            .Where(e => e.Id == model.Id);

            var dbItem = await query.FirstOrDefaultAsync();

            dbItem.UpdateProperties(model);

            var vm = dbItem.ToViewModel();

            await this.dbContext.SaveChangesAsync();

            return vm;
        }

        [HttpPost]
        [Route("create")]
        public async Task<EmployeeRecordModel> Create(
            [FromUri] int andromedaSiteId)
        {
            var content = await this.Request.Content.ReadAsStringAsync();
            EmployeeRecordModel result = null;
            try
            {
                var model = JsonConvert.DeserializeObject<EmployeeRecordModel>(content);

                var dbRecord = this.employeeTable.CreateDbItem(model, andromedaSiteId, true);

                await this.dbContext.SaveChangesAsync();

                result = dbRecord.ToViewModel();
            }
            catch (Exception ex)
            {
                this.logger.Error("failed to create EmployeeRecordModel");
                this.logger.Error(ex);   
                throw ex;
            }

            return result;
        }
    }

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
            record.EmployeeStoreLinkRecords.Add(new EmployeeStoreLinkRecord(){
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

    public class EmployeeRecordModel
    {
        public Guid Id { get; set; }
        public bool Deleted { get; set; }

        public string Code { get; set; }
        public string ShortName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Gender { get; set; }

        public string Department { get; set; }
        public string PrimaryRole { get; set; }
        //public string[] Roles { get; set; }
        //public string[] Skills { get; set; }

        public string DrivingLicense { get; set; }
        public string PayrollNumber { get; set; }
        public string NationalInsurance { get; set; }

        public List<EmployeeDocumentModel> Documents { get; set; }

        //readonly Dictionary<string, object> properties = new Dictionary<string, object>();

        //public override bool TryGetMember(GetMemberBinder binder, out object result)
        //{
        //    if (properties.ContainsKey(binder.Name))
        //    {
        //        result = properties[binder.Name];
        //        return true;
        //    }
        //    else
        //    {
        //        result = "Invalid Property!";
        //        return false;
        //    }
        //}

        //public override bool TrySetMember(SetMemberBinder binder, object value)
        //{
        //    properties[binder.Name] = value;
        //    return true;
        //}

        //public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        //{
        //    dynamic method = properties[binder.Name];
        //    result = method(args[0].ToString(), args[1].ToString());
        //    return true;
        //}

        //public override IEnumerable<string> GetDynamicMemberNames()
        //{
        //    return this.properties.Keys;
        //}
    }

    public class EmployeeDocumentModel 
    {
        public string Name { get; set; }
        public string DocumentUrl { get; set; }

        //readonly Dictionary<string, object> properties = new Dictionary<string, object>();

        //public override bool TryGetMember(GetMemberBinder binder, out object result)
        //{
        //    if (properties.ContainsKey(binder.Name))
        //    {
        //        result = properties[binder.Name];
        //        return true;
        //    }
        //    else
        //    {
        //        result = "Invalid Property!";
        //        return false;
        //    }
        //}

        //public override bool TrySetMember(SetMemberBinder binder, object value)
        //{
        //    properties[binder.Name] = value;
        //    return true;
        //}

        //public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        //{
        //    dynamic method = properties[binder.Name];
        //    result = method(args[0].ToString(), args[1].ToString());
        //    return true;
        //}

        //public override IEnumerable<string> GetDynamicMemberNames()
        //{
        //    return this.properties.Keys;
        //}
    }

}