
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace OrderTracking.Dao.Domain
{
	#region Driver

	/// <summary>
	/// Driver object for NHibernate mapped table 'tbl_Driver'.
	/// </summary>
    public class Driver : Entity.Entity
		{
		#region Member Variables
		
		protected string _name;
		protected string _externalDriverId;
		protected string _vehicle;
		protected Store _storeId;
		
		
		protected IList _driverIdTrackers;
		protected IList _driverIdOrders;
        //protected List<DriverOrder> _driverIdOrders;
		
		#endregion

		#region Constructors

		public Driver() { }

		public Driver( string name, string externalDriverId, string vehicle, Store store )
		{
			this._name = name;
			this._externalDriverId = externalDriverId;
			this._vehicle = vehicle;
			this._storeId = store;
		}

		#endregion

		#region Public Properties

        public string Name
        {
            get { return _name; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Invalid value for Name", value, value.ToString());
                _name = value;
            }
        }

		public string ExternalDriverId
		{
			get { return _externalDriverId; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for ExternalDriverId", value, value.ToString());
				_externalDriverId = value;
			}
		}

		public string Vehicle
		{
			get { return _vehicle; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for Vehicle", value, value.ToString());
				_vehicle = value;
			}
		}

		public Store Store
		{
			get { return _storeId; }
			set { _storeId = value; }
		}
/*
        public List<DriverOrder> DriverOrder
		{
			get
			{
				if (_driverIdOrders==null)
				{
                    _driverIdOrders = new List<DriverOrder>();
				}
				return _driverIdOrders;
			}
			set { _driverIdOrders = value; }
		}
*/
        
        public IList DriverOrder
        {
            get
            {
                if (_driverIdOrders == null)
                {
                    _driverIdOrders = new ArrayList();
                }
                return _driverIdOrders;
            }
            set { _driverIdOrders = value; }
        }

		public IList Trackers
		{
			get
			{
				if (_driverIdTrackers==null)
				{
					_driverIdTrackers = new ArrayList();
				}
				return _driverIdTrackers;
			}
			set { _driverIdTrackers = value; }
		}



		#endregion
		
	}

	#endregion
}



