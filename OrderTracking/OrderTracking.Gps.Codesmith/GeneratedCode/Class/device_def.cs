
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Devicedef

	/// <summary>
	/// Devicedef object for NHibernate mapped table 'device_def'.
	/// </summary>
	public partial  class Devicedef
		{
		#region Member Variables
		
		protected int? _id;
		protected string _devicename;
		protected string _description;
		protected int? _templatemsgfielddictid;
		protected string _botype;
		protected short _upgradable;
		protected Msgnamespace _msgnamespace = new Msgnamespace();
		protected Loadabletype _loadabletype = new Loadabletype();
		
		
		protected Devicedeffield _devicedeffield;
		protected Devicedefmsgfielddictionary _devicedefmsgfielddictionary;
		protected Devicedefgatecommand _devicedefgatecommand;

		#endregion

		#region Constructors

		public Devicedef() { }

		public Devicedef( string devicename, string description, int? templatemsgfielddictid, string botype, short upgradable, Msgnamespace msgnamespace, Loadabletype loadabletype )
		{
			this._devicename = devicename;
			this._description = description;
			this._templatemsgfielddictid = templatemsgfielddictid;
			this._botype = botype;
			this._upgradable = upgradable;
			this._msgnamespace = msgnamespace;
			this._loadabletype = loadabletype;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public string Devicename
		{
			get { return _devicename; }
			set
			{
				if ( value != null && value.Length > 64)
					throw new ArgumentOutOfRangeException("Invalid value for Devicename", value, value.ToString());
				_devicename = value;
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

		public int? Templatemsgfielddictid
		{
			get { return _templatemsgfielddictid; }
			set { _templatemsgfielddictid = value; }
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

		public short Upgradable
		{
			get { return _upgradable; }
			set { _upgradable = value; }
		}

		public Msgnamespace Msgnamespace
		{
			get { return _msgnamespace; }
			set { _msgnamespace = value; }
		}

		public Loadabletype Loadabletype
		{
			get { return _loadabletype; }
			set { _loadabletype = value; }
		}

		public Devicedeffield Devicedeffield
		{
			get { return _devicedeffield; }
			set { _devicedeffield = value; }
		}

		public Devicedefmsgfielddictionary Devicedefmsgfielddictionary
		{
			get { return _devicedefmsgfielddictionary; }
			set { _devicedefmsgfielddictionary = value; }
		}

		public Devicedefgatecommand Devicedefgatecommand
		{
			get { return _devicedefgatecommand; }
			set { _devicedefgatecommand = value; }
		}


		#endregion
		
	}

	#endregion
}



