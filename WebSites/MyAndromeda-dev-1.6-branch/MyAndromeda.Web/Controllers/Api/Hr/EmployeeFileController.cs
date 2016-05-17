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
using MyAndromeda.Framework.Dates;

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
        private readonly IDateServices dateServices;

        public EmployeeFileController(IMyAndromedaLogger logger,
            IDateServices dateServices,
            MyAndromeda.Data.Model.AndroAdmin.AndroAdminDbContext androAdminDbContext,
            DataWarehouseDbContext dbContext) 
        {
            this.dateServices = dateServices;
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
            IQueryable<EmployeeRecord> query = this.employeeTable
                .Where(e => e.EmployeeStoreLinkRecords.Any(r => r.AndromedaSiteId == andromedaSiteId));

            List<EmployeeRecord> records = await query.ToListAsync();

            List<EmployeeRecordModel> models = records
                .Select((record) => record.ToViewModel())
                .ToList();

            // i dislike multiple regions editing the datetiem. 
            models.ForEach((model) => {
                //convert to local time 
                model.DateOfBirth = this.dateServices.ConvertToLocalFromUtc(model.DateOfBirth).GetValueOrDefault();
            });

            return models; 
        }

        [HttpGet]
        [Route("get/{id}")]
        public async Task<EmployeeRecordModel> Get([FromUri]
                                                   int andromedaSiteId, [FromUri]
                                                   Guid id) 
        {
            IQueryable<EmployeeRecord> query = this.employeeTable.Where(e => e.EmployeeStoreLinkRecords.Any(k => k.AndromedaSiteId == andromedaSiteId));

            EmployeeRecord record = await query.FirstOrDefaultAsync();

            return record.ToViewModel();
        }

        [HttpPost]
        [Route("update")]
        public async Task<EmployeeRecordModel> Update([FromUri] int andromedaSiteId) 
        {
            string content = await this.Request.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject< EmployeeRecordModel>(content);

            IQueryable<EmployeeRecord> query = this.employeeTable
                            .Where(e => e.EmployeeStoreLinkRecords.Any(K => K.AndromedaSiteId == andromedaSiteId))
                            .Where(e => e.Id == model.Id);

            EmployeeRecord dbItem = await query.FirstOrDefaultAsync();

            //create
            if (dbItem == null)
            {
                dbItem = this.employeeTable.CreateDbItem(model, andromedaSiteId, attach: true);
                await this.dbContext.SaveChangesAsync();
            }
            else //update
            {
                dbItem.UpdateProperties(model);
            }
            
            EmployeeRecordModel result = dbItem.ToViewModel();


            result.DateOfBirth = dateServices.ConvertToLocalFromUtc(result.DateOfBirth).GetValueOrDefault();

            await this.dbContext.SaveChangesAsync();

            return result;
        }

        [HttpPost]
        [Route("create")]
        public async Task<EmployeeRecordModel> Create(
            [FromUri] int andromedaSiteId)
        {
            string content = await this.Request.Content.ReadAsStringAsync();
            EmployeeRecordModel result = null;
            try
            {
                EmployeeRecordModel model = JsonConvert.DeserializeObject<EmployeeRecordModel>(content);

                EmployeeRecord dbRecord = this.employeeTable.CreateDbItem(model, andromedaSiteId, attach: true);

                await this.dbContext.SaveChangesAsync();

                result = dbRecord.ToViewModel();
                result.DateOfBirth = dateServices.ConvertToLocalFromUtc(result.DateOfBirth).GetValueOrDefault();
            }
            catch (Exception ex)
            {
                this.logger.Error(message: "failed to create EmployeeRecordModel");
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
            List<int> ids = await this.linkRecords.Where(e => e.EmployeeRecordId == employeeId).Select(e=> e.AndromedaSiteId).ToListAsync();

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