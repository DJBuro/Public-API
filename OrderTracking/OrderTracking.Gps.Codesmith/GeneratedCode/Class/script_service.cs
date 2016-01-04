
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Scriptservice

	/// <summary>
	/// Scriptservice object for NHibernate mapped table 'script_service'.
	/// </summary>
	public partial  class Scriptservice
		{
		#region Member Variables
		
		protected int? _id;
		protected string _url;
		protected string _namespace;
		protected string _method;
		protected DateTime? _created;
		protected Scriptplugin _scriptplugin = new Scriptplugin();
		
		

		#endregion

		#region Constructors

		public Scriptservice() { }

		public Scriptservice( string url, string namespace, string method, DateTime? created, Scriptplugin scriptplugin )
		{
			this._url = url;
			this._namespace = namespace;
			this._method = method;
			this._created = created;
			this._scriptplugin = scriptplugin;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public string Url
		{
			get { return _url; }
			set
			{
				if ( value != null && value.Length > 255)
					throw new ArgumentOutOfRangeException("Invalid value for Url", value, value.ToString());
				_url = value;
			}
		}

		public string Namespace
		{
			get { return _namespace; }
			set
			{
				if ( value != null && value.Length > 255)
					throw new ArgumentOutOfRangeException("Invalid value for Namespace", value, value.ToString());
				_namespace = value;
			}
		}

		public string Method
		{
			get { return _method; }
			set
			{
				if ( value != null && value.Length > 32)
					throw new ArgumentOutOfRangeException("Invalid value for Method", value, value.ToString());
				_method = value;
			}
		}

		public DateTime? Created
		{
			get { return _created; }
			set { _created = value; }
		}

		public Scriptplugin Scriptplugin
		{
			get { return _scriptplugin; }
			set { _scriptplugin = value; }
		}


		#endregion
		
	}

	#endregion
}



