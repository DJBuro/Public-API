
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Notifier

	/// <summary>
	/// Notifier object for NHibernate mapped table 'notifier'.
	/// </summary>
	public partial  class Notifier
		{
		#region Member Variables
		
		protected int? _id;
		protected string _name;
		protected string _botype;
		protected DateTime? _created;
		protected Loadabletype _loadabletype = new Loadabletype();
		protected Application _application = new Application();
		
		
		protected IList _messagetemplates;
		protected IList _emailnotifiers;
		protected IList _channelnotifiers;
		protected IList _userattributenotifiers;

		#endregion

		#region Constructors

		public Notifier() { }

		public Notifier( string name, string botype, DateTime? created, Loadabletype loadabletype, Application application )
		{
			this._name = name;
			this._botype = botype;
			this._created = created;
			this._loadabletype = loadabletype;
			this._application = application;
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
				if ( value != null && value.Length > 64)
					throw new ArgumentOutOfRangeException("Invalid value for Name", value, value.ToString());
				_name = value;
			}
		}

		public string Botype
		{
			get { return _botype; }
			set
			{
				if ( value != null && value.Length > 255)
					throw new ArgumentOutOfRangeException("Invalid value for Botype", value, value.ToString());
				_botype = value;
			}
		}

		public DateTime? Created
		{
			get { return _created; }
			set { _created = value; }
		}

		public Loadabletype Loadabletype
		{
			get { return _loadabletype; }
			set { _loadabletype = value; }
		}

		public Application Application
		{
			get { return _application; }
			set { _application = value; }
		}

		public IList message_templates
		{
			get
			{
				if (_messagetemplates==null)
				{
					_messagetemplates = new ArrayList();
				}
				return _messagetemplates;
			}
			set { _messagetemplates = value; }
		}

		public IList email_notifiers
		{
			get
			{
				if (_emailnotifiers==null)
				{
					_emailnotifiers = new ArrayList();
				}
				return _emailnotifiers;
			}
			set { _emailnotifiers = value; }
		}

		public IList channel_notifiers
		{
			get
			{
				if (_channelnotifiers==null)
				{
					_channelnotifiers = new ArrayList();
				}
				return _channelnotifiers;
			}
			set { _channelnotifiers = value; }
		}

		public IList user_attribute_notifiers
		{
			get
			{
				if (_userattributenotifiers==null)
				{
					_userattributenotifiers = new ArrayList();
				}
				return _userattributenotifiers;
			}
			set { _userattributenotifiers = value; }
		}


		#endregion
		
	}

	#endregion
}



