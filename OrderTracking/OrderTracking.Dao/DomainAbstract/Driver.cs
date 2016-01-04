
using System;
using System.Collections;


namespace OrderTracking.Dao.Domain
{
	#region Driver

	/// <summary>
	/// Driver object for NHibernate mapped table 'tbl_Driver'.
	/// </summary>
	public partial  class Driver
		{
		#region Member Variables
		
		protected int? _id;
		protected string _name;
		protected string _vehicle;
		protected Store _storeId = new Store();
		
		
		protected IList _driverIdTrackers;
		protected IList _driverIdDriverOrderses;

		#endregion

		#region Constructors

		public Driver() { }

		public Driver( string name, string vehicle, Store storeId )
		{
			this._name = name;
			this._vehicle = vehicle;
			this._storeId = storeId;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public string Name
		{
			get { return _name; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for Name", value, value.ToString());
				_name = value;
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

		public Store StoreId
		{
			get { return _storeId; }
			set { _storeId = value; }
		}

		public IList DriverIdTrackers
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

		public IList DriverIdDriverOrderses
		{
			get
			{
				if (_driverIdDriverOrderses==null)
				{
					_driverIdDriverOrderses = new ArrayList();
				}
				return _driverIdDriverOrderses;
			}
			set { _driverIdDriverOrderses = value; }
		}


		#endregion
		
	}

	#endregion
}



