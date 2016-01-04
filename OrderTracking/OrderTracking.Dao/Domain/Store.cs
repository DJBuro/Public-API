
using System;
using System.Collections;


namespace OrderTracking.Dao.Domain
{
	#region Store

	/// <summary>
	/// Store object for NHibernate mapped table 'tbl_Store'.
	/// </summary>
    public class Store : Entity.Entity
		{
		#region Member Variables
		
		protected string _externalStoreId;
		protected string _name;
		protected short _deliveryRadius;
		protected Coordinates _coordinates;
		
		
		protected IList _storeIdDrivers;
		protected IList _storeIdOrders;
		protected IList _storeIdTrackers;

		#endregion

		#region Constructors

		public Store() { }

        /// <summary>
        /// note: delivery Radius is in km !
        /// </summary>
        /// <param name="externalStoreId"></param>
        /// <param name="name"></param>
        /// <param name="deliveryRadius"></param>
        /// <param name="coordinates"></param>
		public Store( string externalStoreId, string name,short deliveryRadius, Coordinates coordinates )
		{
			this._externalStoreId = externalStoreId;
			this._name = name;
			this._coordinates = coordinates;
		}

		#endregion

		#region Public Properties

		public string ExternalStoreId
		{
			get { return _externalStoreId; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for ExternalStoreId", value, value.ToString());
				_externalStoreId = value;
			}
		}

		public string Name
		{
			get { return _name; }
			set
			{
				if ( value != null && value.Length > 100)
					throw new ArgumentOutOfRangeException("Invalid value for Name", value, value.ToString());
				_name = value;
			}
		}

		public short DeliveryRadius
		{
			get { return _deliveryRadius; }
			set { _deliveryRadius = value; }
		}

		public Coordinates Coordinates
		{
			get { return _coordinates; }
			set { _coordinates = value; }
		}

		public IList Orders
		{
			get
			{
				if (_storeIdOrders==null)
				{
					_storeIdOrders = new ArrayList();
				}
				return _storeIdOrders;
			}
			set { _storeIdOrders = value; }
		}

		public IList Drivers
		{
			get
			{
				if (_storeIdDrivers==null)
				{
					_storeIdDrivers = new ArrayList();
				}
				return _storeIdDrivers;
			}
			set { _storeIdDrivers = value; }
		}

		public IList Trackers
		{
			get
			{
				if (_storeIdTrackers==null)
				{
					_storeIdTrackers = new ArrayList();
				}
				return _storeIdTrackers;
			}
			set { _storeIdTrackers = value; }
		}


		#endregion
		
	}

	#endregion
}



