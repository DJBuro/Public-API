
using System;
using System.Collections;
using System.Collections.Generic;


namespace WebDashboard.Dao.Domain
{
    #region HeadOffice

    /// <summary>
    /// HeadOffice object for NHibernate mapped table 'tbl_HeadOffice'.
    /// </summary>
    public class HeadOffice : Entity.Entity
    {
        #region Public Properties

        protected string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (value != null && value.Length > 100)
                    throw new ArgumentOutOfRangeException("Invalid value for Name", value, value.ToString());
                _name = value;
            }
        }

        protected string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                if (value != null && value.Length > 1000)
                    throw new ArgumentOutOfRangeException("Invalid value for Message", value, value.ToString());
                _message = value;
            }
        }

        protected int? _groupId;
        public int? GroupId
        {
            get { return _groupId; }
            set { _groupId = value; }
        }

        protected string _currencyCode;
        public string CurrencyCode
        {
            get { return _currencyCode; }
            set { _currencyCode = value; }
        }

        protected string _currencySymbol;
        public string CurrencySymbol
        {
            get { return _currencySymbol; }
            set { _currencySymbol = value; }
        }

        protected IList _headOfficeIdUsers;
        public IList Users
        {
            get
            {
                if (_headOfficeIdUsers == null)
                {
                    _headOfficeIdUsers = new ArrayList();
                }
                return _headOfficeIdUsers;
            }
            set { _headOfficeIdUsers = value; }
        }

        protected IList _headOfficeRegions;
        public IList Regions
        {
            get
            {
                if (_headOfficeRegions == null)
                {
                    _headOfficeRegions = new ArrayList();
                }
                return _headOfficeRegions;
            }
            set { _headOfficeRegions = value; }
        }

        protected IList<Site> _headOfficeSites;
        public IList<Site> Sites
        {
            get
            {
                if (_headOfficeSites == null)
                {
                    _headOfficeSites = new List<Site>();
                }
                return _headOfficeSites;
            }
            set { _headOfficeSites = value; }
        }

        protected IList _headOfficeIdIndicators;
        public IList Indicators
        {
            get
            {
                if (_headOfficeIdIndicators == null)
                {
                    _headOfficeIdIndicators = new ArrayList();
                }
                return _headOfficeIdIndicators;
            }
            set { _headOfficeIdIndicators = value; }
        }


        #endregion

        public float ExchangeRate { get; set; }

        #region Constructors

        public HeadOffice() 
        {
            this.ExchangeRate = 1;
        }

        public HeadOffice(string name, string message): this()
        {
            this._name = name;
            this._message = message;

        }

        #endregion
    }

    #endregion
}



