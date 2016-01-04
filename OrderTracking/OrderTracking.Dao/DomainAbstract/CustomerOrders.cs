
using System;
using System.Collections;
using OrderTracking.Dao.Domain;
using OrderTracking.Dao.DomainAbstract;


namespace OrderTracking.Dao.DomainAbstract
{
    /// <summary>
    /// CustomerOrder object for NHibernate mapped table 'tbl_CustomerOrders'.
    /// </summary>
    public partial  class CustomerOrder
    {
        #region Member Variables
		
        protected int? _id;
        protected Order _orderId = new Order();
        protected Domain.Customer _customerId = new Domain.Customer();
		
		

        #endregion

        #region Constructors

        public CustomerOrder() { }

        public CustomerOrder( Order orderId, Domain.Customer customerId )
        {
            this._orderId = orderId;
            this._customerId = customerId;
        }

        #endregion

        #region Public Properties

        public int? Id
        {
            get {return _id;}
            set {_id = value;}
        }

        public Order OrderId
        {
            get { return _orderId; }
            set { _orderId = value; }
        }

        public Domain.Customer CustomerId
        {
            get { return _customerId; }
            set { _customerId = value; }
        }


        #endregion
		
    }
}