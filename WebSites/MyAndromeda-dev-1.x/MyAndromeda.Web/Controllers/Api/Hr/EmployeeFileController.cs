using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using MyAndromeda.Data.DataWarehouse.Models;
using MyAndromeda.Logging;
using MyAndromeda.Web.Controllers.Api.Hr.Models;
using Newtonsoft.Json;
using MyAndromeda.Data.Model.AndroAdmin;

namespace MyAndromeda.Web.Controllers.Api.Hr
{
    [RoutePrefix("hr/{chainId}/employees/{andromedaSiteId}")]
    public class EmployeeFileController : ApiController
    {
        private readonly IMyAndromedaLogger logger;
        private readonly MyAndromeda.Data.Model.AndroAdmin.AndroAdminDbContext androAdminDbContext;

        private readonly DataWarehouseDbContext dbContext;
        private readonly DbSet<EmployeeRecord> employeeTable;
        private readonly DbSet<EmployeeStoreLinkRecord> linkRecords;

        private readonly DbSet<MyAndromeda.Data.Model.AndroAdmin.Store> storeRecords;

        public EmployeeFileController(IMyAndromedaLogger logger,
            MyAndromeda.Data.Model.AndroAdmin.AndroAdminDbContext androAdminDbContext,
            DataWarehouseDbContext dbContext) 
        {
            this.androAdminDbContext = androAdminDbContext;
            this.dbContext = dbContext;
            this.employeeTable = this.dbContext.Set<EmployeeRecord>();
            this.linkRecords = this.dbContext.Set<EmployeeStoreLinkRecord>();
            this.storeRecords = this.androAdminDbContext.Set<MyAndromeda.Data.Model.AndroAdmin.Store>();

            this.logger = logger;
        }

        [HttpGet]
        [Route("list")]
        public async Task<List<EmployeeRecordModel>> List([FromUri]int andromedaSiteId) 
        {
            var query = this.employeeTable
                .Where(e => e.EmployeeStoreLinkRecords.Any(r => r.AndromedaSiteId == andromedaSiteId));

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
            var query = this.employeeTable.Where(e => e.EmployeeStoreLinkRecords.Any(k => k.AndromedaSiteId == andromedaSiteId));

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
                            .Where(e => e.EmployeeStoreLinkRecords.Any(K => K.AndromedaSiteId == andromedaSiteId))
                            .Where(e => e.Id == model.Id);

            var dbItem = await query.FirstOrDefaultAsync();

            //create
            if (dbItem == null)
            {
                dbItem = this.employeeTable.CreateDbItem(model, andromedaSiteId, true);
                await this.dbContext.SaveChangesAsync();
            }
            else //update
            {
                dbItem.UpdateProperties(model);
            }

            
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

        //hr/{0}/employees/{1}/get-store
        [HttpGet]
        [Route("get-store")]
        public async Task<IEnumerable<object>> ListStores([FromUri]int andromedaSiteId)
        {
            //get all stores related to any employee 
            //var employees = await this.linkRecords
            //    .Where(e=> e.EmployeeRecord.EmployeeStoreLinkRecords.Any(k => k.AdromedaSiteId == andromedaSiteId))
            //    .Select(e=> e.AdromedaSiteId).Distinct().ToArrayAsync();

            var stores = await this.storeRecords
                .Where(e => e.AndromedaSiteId == andromedaSiteId)
                .Select(e => new
                {
                    AndromedaSiteId = e.AndromedaSiteId,
                    ChainId = e.ChainId,
                    Name = e.Name
                })
                .ToListAsync();

            return stores.Select(e => e as object);
        }


        //hr/{0}/employees/{1}/list-stores/{2}
        [HttpGet]
        [Route("list-stores/{employeeId}")]
        public async Task<IEnumerable<object>> ListEmployeeStores([FromUri]Guid employeeId) 
        {
            var ids = await this.linkRecords.Where(e => e.EmployeeRecordId == employeeId).Select(e=> e.AndromedaSiteId).ToListAsync();

            var stores = await this.storeRecords
                .Where(e => ids.Contains(e.AndromedaSiteId))
                .Select(e=> new {
                    AndromedaSiteId = e.AndromedaSiteId,
                    ChainId = e.ChainId,
                    Name = e.Name
                })
                .ToListAsync();

            return stores.Select(e=> e as object);
        }
    }
}