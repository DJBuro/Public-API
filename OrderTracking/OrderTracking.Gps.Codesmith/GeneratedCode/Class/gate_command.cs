
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Gatecommand

	/// <summary>
	/// Gatecommand object for NHibernate mapped table 'gate_command'.
	/// </summary>
	public partial  class Gatecommand
		{
		#region Member Variables
		
		protected int? _id;
		protected string _name;
		protected string _description;
		protected short _enabled;
		protected string _namespace;
		protected short _outgoing;
		protected int? _special;
		protected Loadabletype _loadabletype = new Loadabletype();
		
		
		protected IList _templatecmdsteps;
		protected Devicedefgatecommand _devicedefgatecommand;

		#endregion

		#region Constructors

		public Gatecommand() { }

		public Gatecommand( string name, string description, short enabled, string namespace, short outgoing, int? special, Loadabletype loadabletype )
		{
			this._name = name;
			this._description = description;
			this._enabled = enabled;
			this._namespace = namespace;
			this._outgoing = outgoing;
			this._special = special;
			this._loadabletype = loadabletype;
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

		public short Enabled
		{
			get { return _enabled; }
			set { _enabled = value; }
		}

		public string Namespace
		{
			get { return _namespace; }
			set
			{
				if ( value != null && value.Length > 128)
					throw new ArgumentOutOfRangeException("Invalid value for Namespace", value, value.ToString());
				_namespace = value;
			}
		}

		public short Outgoing
		{
			get { return _outgoing; }
			set { _outgoing = value; }
		}

		public int? Special
		{
			get { return _special; }
			set { _special = value; }
		}

		public Loadabletype Loadabletype
		{
			get { return _loadabletype; }
			set { _loadabletype = value; }
		}

		public IList template_cmd_steps
		{
			get
			{
				if (_templatecmdsteps==null)
				{
					_templatecmdsteps = new ArrayList();
				}
				return _templatecmdsteps;
			}
			set { _templatecmdsteps = value; }
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



