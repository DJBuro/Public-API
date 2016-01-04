
using System;
using System.Collections;


namespace OrderTracking.Dao.Domain
{
	#region Customer

	/// <summary>
	/// Customer object for NHibernate mapped table 'tbl_Customer'.
	/// </summary>
	public class Customer : Entity.Entity
		{
		#region Member Variables
		
		protected string _externalId;
        protected string _name;
		protected string _credentials;
		protected Coordinates _coordinates;
        protected string _houseNumber;
        protected string _buildingName;
        protected string _roadName;
        protected string _townCity;
        protected string _postCode;
        protected string _country;
	
		protected IList _customerIdCustomerOrderses;

		#endregion

		#region Constructors

		public Customer() { }

		public Customer( 
		    string externalId, 
		    string name, 
		    string credentials, 
		    Coordinates coordinates, 
		    string houseNumber,
        	string buildingName,
        	string roadName,
        	string townCity,
        	string postCode,
        	string country)
		{
			this._externalId = externalId;
			this._name = name;
			this._credentials = credentials;
			this._coordinates = coordinates;
			this._houseNumber = houseNumber;
        	this._buildingName = buildingName;
        	this._roadName = roadName;
        	this._townCity = townCity;
        	this._postCode = postCode;
        	this._country = country;
		}

		#endregion

		#region Public Properties

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

		public Coordinates Coordinates
		{
			get { return _coordinates; }
			set { _coordinates = value; }
		}

        public string HouseNumber
        {
            get { return _houseNumber; }
            set { _houseNumber = value; }
        }

        public string BuildingName
        {
            get { return _buildingName; }
            set { _buildingName = value; }
        }

        public string RoadName
        {
            get { return _roadName; }
            set { _roadName = value; }
        }

        public string TownCity
        {
            get { return _townCity; }
            set { _townCity = value; }
        }

        public string PostCode
        {
            get { return _postCode; }
            set { _postCode = value; }
        }

        public string Country
        {
            get { return _country; }
            set { _country = value; }
        }

        public IList CustomerOrders
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



