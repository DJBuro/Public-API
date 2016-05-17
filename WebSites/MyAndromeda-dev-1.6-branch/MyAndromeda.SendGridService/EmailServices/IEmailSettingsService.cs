using System;
using System.Linq;
using MyAndromeda.Core;
using MyAndromeda.Data.Domain.Marketing;

namespace MyAndromeda.SendGridService.EmailServices
{
    public interface IEmailSettingsService : IDependency
    {
        /// <summary>
        /// Gets the best email settings available - either stored by chain or default web.config.
        /// </summary>
        /// <param name="chainId">The chain id.</param>
        /// <returns></returns>
        EmailSettings GetBestSettings(int chainId);

        /// <summary>
        /// Gets the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        EmailSettings Get(int id);

        /// <summary>
        /// Destroys the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        void Destroy(int id);

        /// <summary>
        /// Updates the specified settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        void Update(EmailSettings settings);

        /// <summary>
        /// Creates the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        void Create(EmailSettings settings);
    }
}
