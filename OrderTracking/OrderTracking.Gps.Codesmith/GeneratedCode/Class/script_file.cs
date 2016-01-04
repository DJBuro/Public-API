
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Scriptfile

	/// <summary>
	/// Scriptfile object for NHibernate mapped table 'script_file'.
	/// </summary>
	public partial  class Scriptfile
		{
		#region Member Variables
		
		protected int? _id;
		protected string _url;
		protected string _language;
		protected int? _loadorder;
		protected DateTime? _created;
		protected Scriptplugin _scriptplugin = new Scriptplugin();
		
		

		#endregion

		#region Constructors

		public Scriptfile() { }

		public Scriptfile( string url, string language, int? loadorder, DateTime? created, Scriptplugin scriptplugin )
		{
			this._url = url;
			this._language = language;
			this._loadorder = loadorder;
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

		public string Language
		{
			get { return _language; }
			set
			{
				if ( value != null && value.Length > 255)
					throw new ArgumentOutOfRangeException("Invalid value for Language", value, value.ToString());
				_language = value;
			}
		}

		public int? Loadorder
		{
			get { return _loadorder; }
			set { _loadorder = value; }
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



