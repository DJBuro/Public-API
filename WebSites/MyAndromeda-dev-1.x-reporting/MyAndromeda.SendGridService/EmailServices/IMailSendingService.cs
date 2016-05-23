using MyAndromeda.Core;
using MyAndromeda.Data.Domain.Marketing;

namespace MyAndromeda.SendGridService.EmailServices
{
    public interface IMailSendingService : ITransientDependency  
    {
        //void SendEmails(EmailCampaign model, EmailSettings settings = null, IEnumerable<Customer> users, string[] overideTo = null);
        /// <summary>
        /// Sends the preview of the email.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="emailTo">The email to.</param>
        void SendPreviewEmail(EmailCampaign model, EmailSettings settings, string emailTo);

        /// <summary>
        /// Sends the campaign email.
        /// </summary>
        /// <param name="model">The model.</param>
        void SendCampaignEmail(EmailCampaign model, EmailSettings settings);
    }
}