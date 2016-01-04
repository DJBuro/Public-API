
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Protocolversion

	/// <summary>
	/// Protocolversion object for NHibernate mapped table 'protocol_version'.
	/// </summary>
	public partial  class Protocolversion
		{
		#region Member Variables
		
		protected int? _id;
		protected string _botype;
		protected string _protocolid;
		protected int? _majorversion;
		protected int? _minorversion;
		protected string _clientname;
		
		

		#endregion

		#region Constructors

		public Protocolversion() { }

		public Protocolversion( string botype, string protocolid, int? majorversion, int? minorversion, string clientname )
		{
			this._botype = botype;
			this._protocolid = protocolid;
			this._majorversion = majorversion;
			this._minorversion = minorversion;
			this._clientname = clientname;
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

		public string Protocolid
		{
			get { return _protocolid; }
			set
			{
				if ( value != null && value.Length > 16)
					throw new ArgumentOutOfRangeException("Invalid value for Protocolid", value, value.ToString());
				_protocolid = value;
			}
		}

		public int? Majorversion
		{
			get { return _majorversion; }
			set { _majorversion = value; }
		}

		public int? Minorversion
		{
			get { return _minorversion; }
			set { _minorversion = value; }
		}

		public string Clientname
		{
			get { return _clientname; }
			set
			{
				if ( value != null && value.Length > 64)
					throw new ArgumentOutOfRangeException("Invalid value for Clientname", value, value.ToString());
				_clientname = value;
			}
		}


		#endregion
		
	}

	#endregion
}



