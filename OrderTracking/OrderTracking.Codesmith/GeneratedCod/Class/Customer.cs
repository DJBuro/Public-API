
using System;
using System.Collections;


namespace OrderTracking.Dao.Domain
{
	#region Customer

	/// <summary>
	/// Customer object for NHibernate mapped table 'tbl_Customer'.
	/// </summary>
	public partial  class Customer
		{
		#region Member Variables
		
		protected long? _id;
		protected string _externalId;
		protected string _name;
		protected string _credentials;
		protected Coordinate _coordinates = new Coordinate();
		
		
		protected IList _customerIdCustomerOrderses;

		#endregion

		#region Constructors

		public Customer() { }

		public Customer( string externalId, string name, string credentials, Coordinate coordinates )
		{
			this._externalId = externalId;
			this._name = name;
			this._credentials = credentials;
			this._coordinates = coordinates;
		}

		#endregion

		#region Public Properties

		public long? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public string ExternalId
		{
			get { return _externalId; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for ExternalId", value, value.ToString());
				_externalId = value;
			}
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

		public string Credentials
		{
			get { return _credentials; }
			set
			{
				if ( value != null && value.Length > 20)
					throw new ArgumentOutOfRangeException("Invalid value for Credentials", value, value.ToString());
				_credentials = value;
			}
		}

		public Coordinate Coordinates
		{
			get { return _coordinates; }
			set { _coordinates = value; }
		}

		public IList CustomerIdCustomerOrderses
		{
			get
			{
				if (_customerIdCustomerOrderses==null)
				{
					_customerIdCustomerOrderses = new ArrayList();
				}
				return _customerIdCustomerOrderses;
			}
			set { _customerIdCustomerOrderses = value; }
		}


		#endregion
		
	}

	#endregion
}



