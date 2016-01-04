
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Listener

	/// <summary>
	/// Listener object for NHibernate mapped table 'listener'.
	/// </summary>
	public partial  class Listener
		{
		#region Member Variables
		
		protected int? _id;
		protected short _enabled;
		protected string _serveraddress;
		protected int? _serverport;
		protected string _botype;
		protected Loadabletype _loadabletype = new Loadabletype();
		
		

		#endregion

		#region Constructors

		public Listener() { }

		public Listener( short enabled, string serveraddress, int? serverport, string botype, Loadabletype loadabletype )
		{
			this._enabled = enabled;
			this._serveraddress = serveraddress;
			this._serverport = serverport;
			this._botype = botype;
			this._loadabletype = loadabletype;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public short Enabled
		{
			get { return _enabled; }
			set { _enabled = value; }
		}

		public string Serveraddress
		{
			get { return _serveraddress; }
			set
			{
				if ( value != null && value.Length > 128)
					throw new ArgumentOutOfRangeException("Invalid value for Serveraddress", value, value.ToString());
				_serveraddress = value;
			}
		}

		public int? Serverport
		{
			get { return _serverport; }
			set { _serverport = value; }
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

		public Loadabletype Loadabletype
		{
			get { return _loadabletype; }
			set { _loadabletype = value; }
		}


		#endregion
		
	}

	#endregion
}



