using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using MyAndromeda.Data.DataWarehouse.Models;
using MyAndromeda.Logging;
using MyAndromeda.Web.Controllers.Api.Hr;
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
        public async Task<HttpResponseMessage> List([FromUri]int andromedaSiteId) 
        {
            var query = this.employeeTable.Where(e => e.AndromedaSiteId == andromedaSiteId);

            var records = await query.ToListAsync();

            var models = records
                .Select((record) => record.ToViewModel() as dynamic)
                .ToList();

            var models2 = models.Select((r) => r as dynamic);


            var k = JsonConvert.SerializeObject(models);
            var k2 = JsonConvert.SerializeObject(models2);

            var response = Request.CreateResponse(HttpStatusCode.OK, k2, Configuration.Formatters.JsonFormatter);

            return response;
        }

        [HttpGet]
        [Route("get/{id}")]
        public async Task<EmployeeRecordModel> Get([FromUri]
                                                   int andromedaSiteId, [FromUri]
                                                   Guid id) 
        {
            var query = this.employeeTable.Where(e => e.AndromedaSiteId == andromedaSiteId && e.Id == id);

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
                            .Where(e => e.AndromedaSiteId == andromedaSiteId)
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
            [FromUri] int andromedaSiteId,
            [FromBody] EmployeeRecordModel model)
        {
            var dbRecord = this.employeeTable.CreateDbItem(model, andromedaSiteId, true);
            
            await this.dbContext.SaveChangesAsync();
            
            return dbRecord.ToViewModel();
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
            record.AndromedaSiteId = andromedaSiteId;

            // the rest should be in update properties
            record.UpdateProperties(model);

            if (attach)
            {
                table.Add(record);
            }

            return record;
        }
    }

    public class EmployeeRecordModel : DynamicObject
    {
        public Guid Id { get; set; }

        public bool Deleted { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string PHone { get; set; }

        public DateTime DateOfBirth { get; set; }


        public string ShortName { get; set; }
        public string Gender { get; set; }
        public string PrimaryRole { get; set; }

        public string DrivingLicense { get; set; }
        public string PayrollNumber { get; set; }
        public string NationalInsurance { get; set; }

        public string[] Skills { get; set; }

        public List<EmployeeDocumentModel> Documents { get; set; }

        readonly Dictionary<string, object> properties = new Dictionary<string, object>();

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (properties.ContainsKey(binder.Name))
            {
                result = properties[binder.Name];
                return true;
            }
            else
            {
                result = "Invalid Property!";
                return false;
            }
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            properties[binder.Name] = value;
            return true;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            dynamic method = properties[binder.Name];
            result = method(args[0].ToString(), args[1].ToString());
            return true;
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return this.properties.Keys;
        }
    }

    public class EmployeeDocumentModel : DynamicObject
    {
        public string Name { get; set; }
        public string DocumentUrl { get; set; }

        readonly Dictionary<string, object> properties = new Dictionary<string, object>();

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (properties.ContainsKey(binder.Name))
            {
                result = properties[binder.Name];
                return true;
            }
            else
            {
                result = "Invalid Property!";
                return false;
            }
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            properties[binder.Name] = value;
            return true;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            dynamic method = properties[binder.Name];
            result = method(args[0].ToString(), args[1].ToString());
            return true;
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return this.properties.Keys;
        }
    }

}