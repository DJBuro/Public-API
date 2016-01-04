
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Gateeventchanneldictionaryentry

	/// <summary>
	/// Gateeventchanneldictionaryentry object for NHibernate mapped table 'gate_event_channel_dictionary_entry'.
	/// </summary>
	public partial  class Gateeventchanneldictionaryentry
		{
		#region Member Variables
		
		protected int? _id;
		protected Gateeventchanneldictionary _gateeventchanneldictionary = new Gateeventchanneldictionary();
		protected Gateeventchannel _gateeventchannel = new Gateeventchannel();
		protected Msgfield _msgfield = new Msgfield();
		
		

		#endregion

		#region Constructors

		public Gateeventchanneldictionaryentry() { }

		public Gateeventchanneldictionaryentry( Gateeventchanneldictionary gateeventchanneldictionary, Gateeventchannel gateeventchannel, Msgfield msgfield )
		{
			this._gateeventchanneldictionary = gateeventchanneldictionary;
			this._gateeventchannel = gateeventchannel;
			this._msgfield = msgfield;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public Gateeventchanneldictionary Gateeventchanneldictionary
		{
			get { return _gateeventchanneldictionary; }
			set { _gateeventchanneldictionary = value; }
		}

		public Gateeventchannel Gateeventchannel
		{
			get { return _gateeventchannel; }
			set { _gateeventchannel = value; }
		}

		public Msgfield Msgfield
		{
			get { return _msgfield; }
			set { _msgfield = value; }
		}


		#endregion
		
	}

	#endregion
}



