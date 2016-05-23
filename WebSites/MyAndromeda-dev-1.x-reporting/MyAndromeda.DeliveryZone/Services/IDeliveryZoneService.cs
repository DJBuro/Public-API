using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MyAndromeda.Data.Model.AndroAdmin;
using MyAndromeda.Core;

namespace MyAndromeda.DeliveryZone.Services
{
    public interface IDeliveryZoneService : IDependency
    { 
        /// <summary>
        /// Gets the specified store id.
        /// </summary>
        /// <param name="storeId">The store id.</param>
        /// <returns></returns>
        IList<DeliveryArea> Get(int storeId);

        /// <summary>
        /// Gets the list by expression.
        /// </summary>
        /// <param name="storeId">The store id.</param>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        IList<DeliveryArea> GetListByExpression(int storeId, Expression<Func<DeliveryArea, bool>> query);

        /// <summary>
        /// Lists this instance.
        /// </summary>
        /// <returns></returns>
        IQueryable<DeliveryArea> List();

        /// <summary>
        /// Creates the specified delivery area.
        /// </summary>
        /// <param name="deliveryArea">The delivery area.</param>
        void Create(DeliveryArea deliveryArea);

        /// <summary>
        /// Updates the specified delivery area.
        /// </summary>
        /// <param name="deliveryArea">The delivery area.</param>
        void Update(DeliveryArea deliveryArea);

        /// <summary>
        /// Deletes the specified delivery area.
        /// </summary>
        /// <param name="deliveryArea">The delivery area.</param>
        /// <returns></returns>
        bool Delete(DeliveryArea deliveryArea);

        /// <summary>
        /// Deletes the specified store id.
        /// </summary>
        /// <param name="storeId">The store id.</param>
        /// <returns></returns>
        bool Delete(int storeId);

        /// <summary>
        /// Gets the delivery zones by radius.
        /// </summary>
        /// <param name="storeId">The store id.</param>
        /// <returns></returns>
        DeliveryZoneName GetDeliveryZonesByRadius(int storeId);

        /// <summary>
        /// Saves the delivery zones.
        /// </summary>
        /// <param name="deliveryZone">The delivery zone.</param>
        /// <returns></returns>
        bool SaveDeliveryZones(DeliveryZoneName deliveryZone);
    }
}
