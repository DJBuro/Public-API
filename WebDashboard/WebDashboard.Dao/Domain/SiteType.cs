
using System;
using System.Collections;


namespace WebDashboard.Dao.Domain
{
	#region SiteType

	/// <summary>
	/// SiteType object for NHibernate mapped table 'tbl_SiteType'.
	/// </summary>
    public class SiteType : Entity.Entity
		{
		#region Member Variables
		
		protected string _name;
		protected string _description;
		
		
		protected IList _siteTypeIdSites;

		#endregion

		#region Constructors

		public SiteType() { }

		public SiteType( string name, string description )
		{
			this._name = name;
			this._description = description;
		}

		#endregion

		#region Public Properties

		public string Name
		{
			get { return _name; }
			set
			{
				if ( value != null && value.Length > 10)
					throw new ArgumentOutOfRangeException("Invalid value for Name", value, value.ToString());
				_name = value;
			}
		}

		public string Description
		{
			get { return _description; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for Description", value, value.ToString());
				_description = value;
			}
		}

		public IList SiteTypeIdSites
		{
			get
			{
				if (_siteTypeIdSites==null)
				{
					_siteTypeIdSites = new ArrayList();
				}
				return _siteTypeIdSites;
			}
			set { _siteTypeIdSites = value; }
		}


		#endregion
		
	}

	#endregion
}



