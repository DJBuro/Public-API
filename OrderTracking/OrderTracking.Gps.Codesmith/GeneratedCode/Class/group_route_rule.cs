
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Grouprouterule

	/// <summary>
	/// Grouprouterule object for NHibernate mapped table 'group_route_rule'.
	/// </summary>
	public partial  class Grouprouterule
		{
		#region Member Variables
		
		protected int? _id;
		protected string _serverroutelabel;
		protected string _providerroutelabel;
		protected string _transport;
		protected int? _autoadd;
		protected string _botype;
		protected Group _group = new Group();
		
		

		#endregion

		#region Constructors

		public Grouprouterule() { }

		public Grouprouterule( string serverroutelabel, string providerroutelabel, string transport, int? autoadd, string botype, Group group )
		{
			this._serverroutelabel = serverroutelabel;
			this._providerroutelabel = providerroutelabel;
			this._transport = transport;
			this._autoadd = autoadd;
			this._botype = botype;
			this._group = group;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public string Serverroutelabel
		{
			get { return _serverroutelabel; }
			set
			{
				if ( value != null && value.Length > 255)
					throw new ArgumentOutOfRangeException("Invalid value for Serverroutelabel", value, value.ToString());
				_serverroutelabel = value;
			}
		}

		public string Providerroutelabel
		{
			get { return _providerroutelabel; }
			set
			{
				if ( value != null && value.Length > 255)
					throw new ArgumentOutOfRangeException("Invalid value for Providerroutelabel", value, value.ToString());
				_providerroutelabel = value;
			}
		}

		public string Transport
		{
			get { return _transport; }
			set
			{
				if ( value != null && value.Length > 16)
					throw new ArgumentOutOfRangeException("Invalid value for Transport", value, value.ToString());
				_transport = value;
			}
		}

		public int? Autoadd
		{
			get { return _autoadd; }
			set { _autoadd = value; }
		}

		public string Botype
		{
			get { return _botype; }
			set
			{
				if ( value != null && value.Length > 128)
					throw new ArgumentOutOfRangeException("Invalid value for Botype", value, value.ToString());
				_botype = value;
			}
		}

		public Group Group
		{
			get { return _group; }
			set { _group = value; }
		}


		#endregion
		
	}

	#endregion
}



