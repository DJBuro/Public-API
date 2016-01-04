using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MyAndromeda.Framework.Tokens;
using MyAndromedaDataAccess.Domain.Marketing;

namespace MyAndromeda.Web.Areas.Marketing.ViewModels
{
    public class EditEmailCampaignViewModel
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the reference.
        /// </summary>
        /// <value>The reference.</value>
        [Required]
        public string Reference { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        [Required]
        public string Subject { get; set; }
        
        /// <summary>
        /// Gets or sets the email template.
        /// </summary>
        /// <value>The email template.</value>
        public string EmailTemplate { get; set; }

        /// <summary>
        /// Gets or sets the created.
        /// </summary>
        /// <value>The created.</value>
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the modified.
        /// </summary>
        /// <value>The modified.</value>
        public DateTime Modified { get; set; }

        /// <summary>
        /// Gets or sets the message tokens to place on the editor.
        /// </summary>
        /// <value>The tokens.</value>
        public IEnumerable<Token> Tokens { get; set; }
        
        /// <summary>
        /// Gets or sets the site id.
        /// </summary>
        /// <value>The site id.</value>
        public int ChainId { get; set; }
    }

    public static class EditEmailCampaignExtensions 
    {
        /// <summary>
        /// Updates from data model.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <param name="dataModel">The data model.</param>
        public static void UpdateFromDataModel(this EditEmailCampaignViewModel viewModel, EmailCampaign dataModel) 
        {
            var m = viewModel;
            m.Id = dataModel.Id;
            m.Created = dataModel.Created;
            m.Modified = dataModel.Modified;
            m.Reference = dataModel.Reference;
            m.Subject = dataModel.Subject;
            m.EmailTemplate = dataModel.EmailTemplate;
        }

        /// <summary>
        /// Updates from view model.
        /// </summary>
        /// <param name="dataModel">The data model.</param>
        /// <param name="viewModel">The view model.</param>
        public static void UpdateFromViewModel(this EmailCampaign dataModel, EditEmailCampaignViewModel viewModel) 
        {
            var m = dataModel;
            m.Id = viewModel.Id;
            m.Created = viewModel.Created;
            m.Modified = viewModel.Modified;
            m.Reference = viewModel.Reference;
            m.Subject = viewModel.Subject;
            m.EmailTemplate = viewModel.EmailTemplate;
        }
    }

    
}