
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Setting

	/// <summary>
	/// Setting object for NHibernate mapped table 'settings'.
	/// </summary>
	public partial  class Setting
		{
		#region Member Variables
		
		protected int? _id;
		protected string _botype;
		protected string _namespace;
		protected string _valuename;
		protected string _valuetype;
		protected string _valuedata;
		protected string _description;
		
		

		#endregion

		#region Constructors

		public Setting() { }

		public Setting( string botype, string namespace, string valuename, string valuetype, string valuedata, string description )
		{
			this._botype = botype;
			this._namespace = namespace;
			this._valuename = valuename;
			this._valuetype = valuetype;
			this._valuedata = valuedata;
			this._description = description;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public string Botype
		{
			get { return _botype; }
			set
			{
				if ( value != null && value.Length > 64)
					throw new ArgumentOutOfRangeException("Invalid value for Botype", value, value.ToString());
				_botype = value;
			}
		}

		public string Namespace
		{
			get { return _namespace; }
			set
			{
				if ( value != null && value.Length > 64)
					throw new ArgumentOutOfRangeException("Invalid value for Namespace", value, value.ToString());
				_namespace = value;
			}
		}

		public string Valuename
		{
			get { return _valuename; }
			set
			{
				if ( value != null && value.Length > 64)
					throw new ArgumentOutOfRangeException("Invalid value for Valuename", value, value.ToString());
				_valuename = value;
			}
		}

		public string Valuetype
		{
			get { return _valuetype; }
			set
			{
				if ( value != null && value.Length > 64)
					throw new ArgumentOutOfRangeException("Invalid value for Valuetype", value, value.ToString());
				_valuetype = value;
			}
		}

		public string Valuedata
		{
			get { return _valuedata; }
			set
			{
				if ( value != null && value.Length > 255)
					throw new ArgumentOutOfRangeException("Invalid value for Valuedata", value, value.ToString());
				_valuedata = value;
			}
		}

		public string Description
		{
			get { return _description; }
			set
			{
				if ( value != null && value.Length > 255)
					throw new ArgumentOutOfRangeException("Invalid value for Description", value, value.ToString());
				_description = value;
			}
		}


		#endregion
		
	}

	#endregion

	#region Groupsetting

	/// <summary>
	/// Groupsetting object for NHibernate mapped table 'group_settings'.
	/// </summary>
	public class Groupsetting : Setting
	{
		#region Member Variables

		protected Group _group;

		#endregion

		#region Constructors

		public Groupsetting() : base() { }

		public Groupsetting( string botype, string namespace, string valuename, string valuetype, string valuedata, string description, Group group ) : base(botype, namespace, valuename, valuetype, valuedata, description)
		{
			this.Group = group;
		}

		#endregion

		#region Public Properties

		public Group Group
		{
			get { return _group; }
			set { _group = value; }
		}

		#endregion
	}

	#endregion

	#region Msgprovsetting

	/// <summary>
	/// Msgprovsetting object for NHibernate mapped table 'msg_prov_settings'.
	/// </summary>
	public class Msgprovsetting : Setting
	{
		#region Member Variables

		protected Messageprovider _messageprovider;

		#endregion

		#region Constructors

		public Msgprovsetting() : base() { }

		public Msgprovsetting( string botype, string namespace, string valuename, string valuetype, string valuedata, string description, Messageprovider messageprovider ) : base(botype, namespace, valuename, valuetype, valuedata, description)
		{
			this.Messageprovider = messageprovider;
		}

		#endregion

		#region Public Properties

		public Messageprovider Messageprovider
		{
			get { return _messageprovider; }
			set { _messageprovider = value; }
		}

		#endregion
	}

	#endregion

	#region Appsetting

	/// <summary>
	/// Appsetting object for NHibernate mapped table 'app_settings'.
	/// </summary>
	public class Appsetting : Setting
	{
		#region Member Variables

		protected Application _application;

		#endregion

		#region Constructors

		public Appsetting() : base() { }

		public Appsetting( string botype, string namespace, string valuename, string valuetype, string valuedata, string description, Application application ) : base(botype, namespace, valuename, valuetype, valuedata, description)
		{
			this.Application = application;
		}

		#endregion

		#region Public Properties

		public Application Application
		{
			get { return _application; }
			set { _application = value; }
		}

		#endregion
	}

	#endregion

	#region Sitesetting

	/// <summary>
	/// Sitesetting object for NHibernate mapped table 'site_settings'.
	/// </summary>
	public class Sitesetting : Setting
	{
		#region Member Variables


		#endregion

		#region Constructors

		public Sitesetting() : base() { }

		public Sitesetting( string botype, string namespace, string valuename, string valuetype, string valuedata, string description ) : base(botype, namespace, valuename, valuetype, valuedata, description)
		{
		}

		#endregion

		#region Public Properties

		#endregion
	}

	#endregion

	#region Usersetting

	/// <summary>
	/// Usersetting object for NHibernate mapped table 'user_settings'.
	/// </summary>
	public class Usersetting : Setting
	{
		#region Member Variables

		protected User _user;

		#endregion

		#region Constructors

		public Usersetting() : base() { }

		public Usersetting( string botype, string namespace, string valuename, string valuetype, string valuedata, string description, User user ) : base(botype, namespace, valuename, valuetype, valuedata, description)
		{
			this.User = user;
		}

		#endregion

		#region Public Properties

		public User User
		{
			get { return _user; }
			set { _user = value; }
		}

		#endregion
	}

	#endregion
}



