
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Msgfield

	/// <summary>
	/// Msgfield object for NHibernate mapped table 'msg_field'.
	/// </summary>
	public partial  class Msgfield
		{
		#region Member Variables
		
		protected int? _id;
		protected string _type;
		protected string _name;
		protected string _description;
		protected string _localizationkey;
		protected Msgnamespace _msgnamespace = new Msgnamespace();
		protected Unit _unit = new Unit();
		
		
		protected Devicedeffield _devicedeffield;
		protected IList _gaterecordlatests;
		protected IList _inmsgfielddictionaryentries;
		protected IList _outmsgfielddictionaryentries;
		protected IList _gatemessagerecords;
		protected IList _gateeventexpressions;
		protected IList _gateeventchanneldictionaryentries;

		#endregion

		#region Constructors

		public Msgfield() { }

		public Msgfield( string type, string name, string description, string localizationkey, Msgnamespace msgnamespace, Unit unit )
		{
			this._type = type;
			this._name = name;
			this._description = description;
			this._localizationkey = localizationkey;
			this._msgnamespace = msgnamespace;
			this._unit = unit;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public string Type
		{
			get { return _type; }
			set
			{
				if ( value != null && value.Length > 45)
					throw new ArgumentOutOfRangeException("Invalid value for Type", value, value.ToString());
				_type = value;
			}
		}

		public string Name
		{
			get { return _name; }
			set
			{
				if ( value != null && value.Length > 64)
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

		public string Localizationkey
		{
			get { return _localizationkey; }
			set
			{
				if ( value != null && value.Length > 71)
					throw new ArgumentOutOfRangeException("Invalid value for Localizationkey", value, value.ToString());
				_localizationkey = value;
			}
		}

		public Msgnamespace Msgnamespace
		{
			get { return _msgnamespace; }
			set { _msgnamespace = value; }
		}

		public Unit Unit
		{
			get { return _unit; }
			set { _unit = value; }
		}

		public Devicedeffield Devicedeffield
		{
			get { return _devicedeffield; }
			set { _devicedeffield = value; }
		}

		public IList gate_record_latests
		{
			get
			{
				if (_gaterecordlatests==null)
				{
					_gaterecordlatests = new ArrayList();
				}
				return _gaterecordlatests;
			}
			set { _gaterecordlatests = value; }
		}

		public IList in_msg_field_dictionary_entries
		{
			get
			{
				if (_inmsgfielddictionaryentries==null)
				{
					_inmsgfielddictionaryentries = new ArrayList();
				}
				return _inmsgfielddictionaryentries;
			}
			set { _inmsgfielddictionaryentries = value; }
		}

		public IList out_msg_field_dictionary_entries
		{
			get
			{
				if (_outmsgfielddictionaryentries==null)
				{
					_outmsgfielddictionaryentries = new ArrayList();
				}
				return _outmsgfielddictionaryentries;
			}
			set { _outmsgfielddictionaryentries = value; }
		}

		public IList gate_message_records
		{
			get
			{
				if (_gatemessagerecords==null)
				{
					_gatemessagerecords = new ArrayList();
				}
				return _gatemessagerecords;
			}
			set { _gatemessagerecords = value; }
		}

		public IList gate_event_expressions
		{
			get
			{
				if (_gateeventexpressions==null)
				{
					_gateeventexpressions = new ArrayList();
				}
				return _gateeventexpressions;
			}
			set { _gateeventexpressions = value; }
		}

		public IList gate_event_channel_dictionary_entries
		{
			get
			{
				if (_gateeventchanneldictionaryentries==null)
				{
					_gateeventchanneldictionaryentries = new ArrayList();
				}
				return _gateeventchanneldictionaryentries;
			}
			set { _gateeventchanneldictionaryentries = value; }
		}


		#endregion
		
	}

	#endregion
}



