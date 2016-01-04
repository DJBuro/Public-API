
using System;
using System.Collections;


namespace OrderTracking.Dao.DomainAbstract
{
    /// <summary>
    /// Abstract Customer object for NHibernate mapped table 'tbl_Customer'.
    /// </summary>
    public abstract class Customer : Entity.Entity
    {
        #region Member Variables
		
        protected string _name;
        protected double? _longitude;
        protected double? _latitude;
        protected string _credentials;
		
		
        protected IList _customerIdCustomerOrderses;

        #endregion

        #region Constructors

        public Customer() {this._name = "base hello"; }

        public Customer( string name, double? longitude, double? latitude, string credentials )
        {
            this._name = name;
            this._longitude = longitude;
            this._latitude = latitude;
            this._credentials = credentials;
        }

        #endregion

        #region Public Properties

        public virtual string Name
        {
            get { return _name; }
            set
            {
                if ( value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Invalid value for Name", value, value.ToString());
                _name = value;
            }
        }

        public double? Longitude
        {
            get { return _longitude; }
            set { _longitude = value; }
        }

        public double? Latitude
        {
            get { return _latitude; }
            set { _latitude = value; }
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

        public virtual IList CustomerIdCustomerOrderses
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
}