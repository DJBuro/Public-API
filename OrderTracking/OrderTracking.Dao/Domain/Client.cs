
using System;
using System.Collections;
using System.Xml.Serialization;


namespace OrderTracking.Dao.Domain
{
	#region Client

	/// <summary>
    /// Client object for NHibernate mapped table 'Client'.
	/// </summary>
    public class Client : Entity.Entity
		{
		#region Member Variables
		
		protected string _name;
		protected string _webKey;
        protected string _websiteTemplateName;

		#endregion

		#region Constructors

		public Client() 
		{ 
		}

        public Client( 
		    string name, 
		    string webKey,
            string websiteTemplateName)
		{
			this._name = name;
            this._webKey = webKey;
            this._websiteTemplateName = websiteTemplateName;
		}

		#endregion

		#region Public Properties

		public string Name
		{
			get { return _name; }
			set
			{
				if ( value != null && value.Length > 255)
					throw new ArgumentOutOfRangeException("Invalid value for Name", value, value.ToString());
				_name = value;
			}
		}

        public string WebKey
		{
            get { return _webKey; }
			set
			{
				if ( value != null && value.Length > 255)
                    throw new ArgumentOutOfRangeException("Invalid value for WebKey", value, value.ToString());
                _webKey = value;
			}
		}

        public string WebsiteTemplateName
        {
            get { return _websiteTemplateName; }
            set
            {
                if (value != null && value.Length > 255)
                    throw new ArgumentOutOfRangeException("Invalid value for _websiteTemplateName", value, value.ToString());
                _websiteTemplateName = value;
            }
        }

		#endregion
		
	}

	#endregion
}



