
using System;
using System.Collections;
using System.Xml.Serialization;


namespace OrderTracking.Dao.Domain
{
    #region ClientWebsiteCustomContent

    /// <summary>
	/// ClientWebsiteCustomContent object for NHibernate mapped table 'ClientWebsiteCustomContent'.
	/// </summary>
    public class ClientWebsiteCustomContent : Entity.Entity
		{
		#region Member Variables
		
		protected Int64 _clientId;
		protected string _key;
        protected string _value;

		#endregion

		#region Constructors

		public ClientWebsiteCustomContent() 
		{ 
		}

        public ClientWebsiteCustomContent(
            Int64 clientId,
            string key,
            string value)
		{
            this._clientId = clientId;
            this._key = key;
            this._value = value;
		}

		#endregion

		#region Public Properties

        public Int64 ClientId
		{
            get { return _clientId; }
			set
			{
                _clientId = value;
			}
		}

        public string Key
		{
            get { return _key; }
			set
			{
				if ( value != null && value.Length > 255)
                    throw new ArgumentOutOfRangeException("Invalid value for Key", value, value.ToString());
                _key = value;
			}
		}

        public string Value
        {
            get { return _value; }
            set
            {
                if (value != null && value.Length > 2000)
                    throw new ArgumentOutOfRangeException("Invalid value for _value", value, value.ToString());
                _value = value;
            }
        }

		#endregion
		
	}

	#endregion
}



