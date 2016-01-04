
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Gateeventchannel

	/// <summary>
	/// Gateeventchannel object for NHibernate mapped table 'gate_event_channel'.
	/// </summary>
	public partial  class Gateeventchannel
		{
		#region Member Variables
		
		protected int? _id;
		protected string _name;
		protected string _description;
		
		
		protected Gateeventexpressionevaluatorchannel _gateeventexpressionevaluatorchannel;
		protected IList _gateevents;
		protected IList _channelnotifiers;
		protected Applicationdefgateeventchannel _applicationdefgateeventchannel;
		protected IList _gateeventchanneldictionaryentries;

		#endregion

		#region Constructors

		public Gateeventchannel() { }

		public Gateeventchannel( string name, string description )
		{
			this._name = name;
			this._description = description;
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

		public Gateeventexpressionevaluatorchannel Gateeventexpressionevaluatorchannel
		{
			get { return _gateeventexpressionevaluatorchannel; }
			set { _gateeventexpressionevaluatorchannel = value; }
		}

		public IList gate_events
		{
			get
			{
				if (_gateevents==null)
				{
					_gateevents = new ArrayList();
				}
				return _gateevents;
			}
			set { _gateevents = value; }
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

		public Applicationdefgateeventchannel Applicationdefgateeventchannel
		{
			get { return _applicationdefgateeventchannel; }
			set { _applicationdefgateeventchannel = value; }
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



