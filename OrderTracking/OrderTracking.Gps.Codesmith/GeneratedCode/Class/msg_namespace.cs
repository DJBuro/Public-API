
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Msgnamespace

	/// <summary>
	/// Msgnamespace object for NHibernate mapped table 'msg_namespace'.
	/// </summary>
	public partial  class Msgnamespace
		{
		#region Member Variables
		
		protected int? _id;
		protected string _name;
		protected string _description;
		protected string _protocolid;
		
		
		protected IList _devicedefs;
		protected IList _msgfields;

		#endregion

		#region Constructors

		public Msgnamespace() { }

		public Msgnamespace( string name, string description, string protocolid )
		{
			this._name = name;
			this._description = description;
			this._protocolid = protocolid;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public string Name
		{
			get { return _name; }
			set
			{
				if ( value != null && value.Length > 128)
					throw new ArgumentOutOfRangeException("Invalid value for Name", value, value.ToString());
				_name = value;
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

		public IList device_defs
		{
			get
			{
				if (_devicedefs==null)
				{
					_devicedefs = new ArrayList();
				}
				return _devicedefs;
			}
			set { _devicedefs = value; }
		}

		public IList msg_fields
		{
			get
			{
				if (_msgfields==null)
				{
					_msgfields = new ArrayList();
				}
				return _msgfields;
			}
			set { _msgfields = value; }
		}


		#endregion
		
	}

	#endregion
}



