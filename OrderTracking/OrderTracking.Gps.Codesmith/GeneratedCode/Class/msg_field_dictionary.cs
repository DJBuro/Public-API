
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Msgfielddictionary

	/// <summary>
	/// Msgfielddictionary object for NHibernate mapped table 'msg_field_dictionary'.
	/// </summary>
	public partial  class Msgfielddictionary
		{
		#region Member Variables
		
		protected int? _id;
		protected string _name;
		protected string _description;
		protected string _botype;
		protected Group _group = new Group();
		
		
		protected Devicedefmsgfielddictionary _devicedefmsgfielddictionary;
		protected IList _devices;
		protected IList _msgfielddictionaryentries;

		#endregion

		#region Constructors

		public Msgfielddictionary() { }

		public Msgfielddictionary( string name, string description, string botype, Group group )
		{
			this._name = name;
			this._description = description;
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

		public Group Group
		{
			get { return _group; }
			set { _group = value; }
		}

		public Devicedefmsgfielddictionary Devicedefmsgfielddictionary
		{
			get { return _devicedefmsgfielddictionary; }
			set { _devicedefmsgfielddictionary = value; }
		}

		public IList devices
		{
			get
			{
				if (_devices==null)
				{
					_devices = new ArrayList();
				}
				return _devices;
			}
			set { _devices = value; }
		}

		public IList msg_field_dictionary_entries
		{
			get
			{
				if (_msgfielddictionaryentries==null)
				{
					_msgfielddictionaryentries = new ArrayList();
				}
				return _msgfielddictionaryentries;
			}
			set { _msgfielddictionaryentries = value; }
		}


		#endregion
		
	}

	#endregion
}



