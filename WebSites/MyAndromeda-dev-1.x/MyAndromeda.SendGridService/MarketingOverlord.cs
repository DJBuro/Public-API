using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyAndromeda.Data.Model.AndroAdmin;
using MyAndromeda.Data.Model.MyAndromeda;
using MyAndromeda.Logging;
using MyAndromeda.SendGridService.MarketingApi;
using MyAndromeda.SendGridService.MarketingApi.Models.Recipients;
using MyAndromeda.SendGridService.Models;
using System.Data;
using System.Data.Entity;
using MyAndromedaDataAccessEntityFramework.DataAccess.Sites;
using Postal;
using MyAndromeda.SendGridService.MarketingApi.Models.Template;
using MyAndromeda.SendGridService.MarketingApi.Models;
using MyAndromeda.Data.DataAccess.WebOrdering;
namespace MyAndromeda.SendGridService
{
    public class MarketingOverlord : IMarketingOverlord 
    {
        //sending
        private readonly IMyAndromedaLogger logger;
        private readonly IScheduleService schedulingSerivce;
        private readonly ITemplateService templateService;
        private readonly IRecipientListService recipientListService;
        private readonly IMarketingEmailRecipientsService emailRecipientListService;
        private readonly IMarketingTemplateDataService marketingTemplateDataService;
        private readonly ICategoryService categoryService;
        
        public MarketingOverlord(IScheduleService schedulingSerivce, ITemplateService templateService, IRecipientListService recipientListService, IMarketingEmailRecipientsService emailRecipientListService, ICategoryService categoryService, IMarketingTemplateDataService marketingTemplateDataService, IMyAndromedaLogger logger)
        {
            this.logger = logger;
            this.marketingTemplateDataService = marketingTemplateDataService;
            this.categoryService = categoryService;
            this.emailRecipientListService = emailRecipientListService;
            this.recipientListService = recipientListService;
            this.templateService = templateService;
            this.schedulingSerivce = schedulingSerivce;
        }

        public async Task<MarketingTemplateModel> GetMarketingInfoAsync(MarketingEventCampaign campaign, string emailTemplate) 
        {
            //string emailTemplate = campaign.GetNameOfTemplate();

            var getModel = campaign.ConvertToGetTemplateModel();
            getModel.Name = emailTemplate;
            
            var marketingEmailResult = await this.templateService.GetAsync(getModel);
            
            //no bother checking if the email campaign has not been saved yet. 
            if (marketingEmailResult == null)
            {
                return null;
            }

            //get mailing lists
            //var getEmailRecipientListModel = new MarketingApi.Models.Recipients.GetRequestModel()
            //{
            //    MarketingEmailName = emailTemplate
            //};

            //var recipientListResult = await this.emailRecipientListService.ListAsync(getEmailRecipientListModel);

            var model = new MarketingTemplateModel()
            {
                Template = marketingEmailResult
            //    RecipientList = recipientListResult
            };

            return model;
        }

        public async Task<bool> AddRecipientListsToTemplateAsync(MarketingEventCampaign campaign, string templateName) 
        {
            var recipientListName = campaign.GetNameOfRecipientList();

            var addToMarketing = await this.emailRecipientListService.AddAsync(new AddOrRemoveRequestModel()
                {
                    MarketingEmailName = templateName,
                    RecipientListName = recipientListName
                });

            return addToMarketing;
        }

        public async Task<bool> AddPeopleToRecipientListAsync<TModel>(MarketingEventCampaign campaign, List<TModel> allPeople)
            where TModel : MarketingApi.Models.Recipients.Person
        {
            //warning: it may or may not take more than a 1000 users. 
            var recipientListName = campaign.GetNameOfRecipientList();

            this.logger.Debug("Get/Create recipient list:" + recipientListName);
            var people = allPeople.ToList();

            while (people.Count > 0)
            {
                var group = people.Take(1000).ToArray();
                var recipientListResult = await this.recipientListService.GetAsync(new MarketingApi.Models.Lists.GetListRequestModel() { 
                    ListName = recipientListName
                });

                if (recipientListResult == null) 
                {
                    await this.recipientListService.CreateAsync(new MarketingApi.Models.Lists.CreateListModel() {
                        ListName = recipientListName
                    });
                }

                /*
                 * Clear all current residents of the campaign.  
                 */

                var currentParticipients = await this.recipientListService.GetPeopleAsync<MyAndromeda.SendGridService.MarketingApi.Models.Recipients.Person>(new GetPeopleRequestModel() { 
                    ListName = recipientListName
                });

                if (currentParticipients.Count > 0) 
                {
                    var removeUserModel = new MarketingApi.Models.Lists.RemoveEmailsRequestModel()
                    {
                        ListName = recipientListName,
                        Emails = currentParticipients.Select(e => e.Email).ToArray()
                    };

                    var response = await this.recipientListService.RemoveEmailsAsync(removeUserModel);
                    this.logger.Debug("Target: {0}; Removed {1} users from the {2} mailing list", currentParticipients.Count, response.Removed, recipientListName);
                }

                
                /*
                 * Add the current batch to the campaign
                 */

                var addPeopleModel = new AddPeopleRequestModel<TModel>()
                {
                    ListName = recipientListName,
                    Data = new List<TModel>(group)
                };

                addPeopleModel.Data.ForEach((person) => { 
                    people.Remove(person);
                });

                var addPeopleTask = await this.recipientListService.AddPeopleAsync(addPeopleModel);

                this.logger.Debug("Recipients inserted: " + addPeopleTask.Inserted);

                this.logger.Debug("Recipient list size: " + addPeopleModel.Data.Count);
            }

            return true;
        }

        //public async Task<bool> AddPeopleToMarketingEmailAsync<TModel>(MarketingEventCampaign campaign, List<TModel> people) 
        //    where TModel : MarketingApi.Models.Recipients.Person
        //{
        //    string emailTemplate = campaign.GetNameOfTemplate();

        //    var getEmailRecipientListModel = new MarketingApi.Models.Recipients.GetRequestModel()
        //    {
        //        MarketingEmailName = emailTemplate
        //    };

        //    //go get people 
        //    var recipientListResult = await this.emailRecipientListService.ListAsync(getEmailRecipientListModel);

        //    var tasks = recipientListResult.Select(e => this.recipientListService.GetPersonCountAsync(new GetPeopleRequestModel()
        //    {
        //        ListName = e.ListName
        //    })).ToList();

        //    await Task.WhenAll(tasks);

        //    //for-each of the recipient lists that exist ... fill them up. 
        //    foreach (var task in tasks) 
        //    {
        //        int canAdd = 1000 - task.Result.Count;
                
        //        var addThesePeople = people.Take(canAdd).ToArray();
                
        //        //List<TModel> addPeople = new List<TModel>();
        //        var addPeople = new AddPeopleRequestModel<TModel>()
        //        {
        //            ListName = task.Result.ListName,
        //            Data = new List<TModel>()
        //        };

        //        foreach (var person in addThesePeople) 
        //        {
        //            addPeople.Data.Add(person);
        //            people.Remove(person);
        //        }

        //        var response = await this.recipientListService.AddPeopleAsync(addPeople);

        //        if (people.Count == 0)
        //        {
        //            break;
        //        }
        //    }

        //    //take 1000 
        //    while (people.Count > 0) 
        //    { 
        //        //to-do ... create the new list
        //        var take = people.Take(1000).ToList();
        //        var recipientListName = campaign.GetNameOfRecipientList(recipientListResult.Count);

        //        //create a new recipient list - adding a flag per thousand people 
        //        await this.recipientListService.CreateAsync(new MarketingApi.Models.Lists.CreateListModel()
        //        {
        //            ListName = recipientListName
        //        });

        //        recipientListResult.Add(new ListMetaModel()
        //        {
        //            ListName = recipientListName
        //        });

        //        var addPeople = new AddPeopleRequestModel<TModel>()
        //        {
        //            ListName = recipientListName,
        //            Data = new List<TModel>()
        //        };

        //        foreach (var person in take)
        //        {
        //            addPeople.Data.Add(person);
        //            people.Remove(person);
        //        }

        //        await this.recipientListService.AddPeopleAsync(addPeople);

        //        //to-do add list to the marketing email. 
        //        var addToMarketing = await this.emailRecipientListService.AddAsync(new AddOrRemoveRequestModel()
        //        {
        //            MarketingEmailName = emailTemplate,
        //            RecipientListName = recipientListName
        //        });

        //        if (!addToMarketing)
        //        {
        //            var message = string.Format("Add recipient list to marketing campaign failed: {0} to {1}", emailTemplate, recipientListName);
        //            throw new Exception(message);
        //        }
        //    }

        //    return true;
        //}

        public async Task<bool> RemovePeopleFromMarketingEmailAsync(MarketingEventCampaign campaign, List<string> emailAddresses)
        {
            string emailTemplate = campaign.GetNameOfTemplate();

            var getEmailRecipientListModel = new MarketingApi.Models.Recipients.GetRequestModel()
            {
                MarketingEmailName = emailTemplate
            };

            var recipientListResult = await this.emailRecipientListService.ListAsync(getEmailRecipientListModel);

            var tasks = recipientListResult.Select(e => this.recipientListService.GetPersonCountAsync(new GetPeopleRequestModel()
            {
                ListName = e.ListName
            })).ToList();

            await Task.WhenAll(tasks);

            var deleteTasks = tasks.Select(e =>
            {
                var model = new MarketingApi.Models.Lists.RemoveEmailsRequestModel()
                {
                    ListName = e.Result.ListName,
                    Emails = emailAddresses.ToArray()
                };
                return this.recipientListService.RemoveEmailsAsync(model);
            });

            await Task.WhenAll(deleteTasks);

            return true;
        }

        public async Task<bool> AddCategoryAsync(string campaignTemplateName, IEnumerable<string> categories) 
        {
            var savedCategories = await this.categoryService.ListAsync();

            //create the categories if they do not exist. 
            var addCategories = categories
                                          .Where(e => !savedCategories.Any(c => c.Category.Equals(e, StringComparison.CurrentCultureIgnoreCase)))
                                          .ToArray();

            var createTasks = addCategories.Select(e => this.categoryService.CreateAsync(new MarketingApi.Models.Categories.CategoryCreateRequestModel()
            {
                Category = e
            }));
            
            await Task.WhenAll(createTasks);

            //add the categories to the email
            var addCategoryTasks = categories
                .Select(e => 
                    this.categoryService.AddCategoryToMarketingEmail(new MarketingApi.Models.Categories.CategoryAddOrRemoveRequestModel()
                    {
                        Category = e,
                        Name = campaignTemplateName
                    })
                );

            await Task.WhenAll(addCategoryTasks);

            return true;
        }

        public async Task<bool> SendCampaignAsync(MarketingEventCampaign campaign, string emailTemplateName)
        {
            //string emailTemplate = campaign.GetNameOfTemplate();
            var sendModel = new MarketingApi.Models.Schedule.ScheduleAddRequestModel()
            {
                Name = emailTemplateName
            };
            
            return await this.schedulingSerivce.AddAsync(sendModel);
        }


        public async Task<MarketingApi.Models.Template.GetResponseTemplateModel> UpdateSendGridTemplate(
            MarketingEventCampaign entity, 
            string html)
        {
            var contactEntity = await this.marketingTemplateDataService.MarketingContacts
                .FirstOrDefaultAsync(e => e.AndromedaSiteId == entity.AndromedaSiteId);

            //var template = this.GenerateEmailMessage(entity);

            GetRequestTemplateModel model = entity.ConvertToGetTemplateModel();
            GetResponseTemplateModel result = null;

            try
            {
                SendGridMessage addOrUpdateResult = null;

                result = await this.templateService.GetAsync(model);

                if (result == null)
                {
                    var templateModel = entity.ConvertToAddModel(contactEntity, model.Name, html);
                    addOrUpdateResult = await this.templateService.AddAsync(templateModel);

                    //go fetch that model again.
                    result = await this.templateService.GetAsync(model);
                }
                else
                {
                    var templateModel = entity.ConvertToEditModel(contactEntity, model.Name, html);
                    addOrUpdateResult = await this.templateService.EditAsync(templateModel);
                }

                if (!addOrUpdateResult.IsSuccessful)
                {
                    this.logger.Error("Could not update sendgrid");
                }
            }
            catch (Exception e)
            {
                this.logger.Error("updating sendgrid problem.");
                throw;
            }
            
            return result;
        }

        
    }
}