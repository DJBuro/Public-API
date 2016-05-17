using System.Threading.Tasks;
using MyAndromeda.Core;
using MyAndromeda.SendGridService.MarketingApi.Models;
using MyAndromeda.SendGridService.MarketingApi.Models.Template;

namespace MyAndromeda.SendGridService.MarketingApi
{
    public interface ITemplateService : IDependency 
    {
        Task<GetResponseTemplateModel> GetAsync(GetRequestTemplateModel model);

        Task<SendGridMessage> AddAsync(AddTemplateModel model);

        Task<SendGridMessage> EditAsync(EditTemplateModel model);
    }
}