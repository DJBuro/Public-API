
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Gateeventchanneldictionary

	/// <summary>
	/// Gateeventchanneldictionary object for NHibernate mapped table 'gate_event_channel_dictionary'.
	/// </summary>
	public partial  class Gateeventchanneldictionary
		{
		#region Member Variables
		
		protected int? _id;
		protected Application _application = new Application();
		
		
		protected IList _gateeventchanneldictionaryentries;

		#endregion

		#region Constructors

		public Gateeventchanneldictionary() { }

		public Gateeventchanneldictionary( Application application )
		{
			this._application = application;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public Application Application
		{
			get { return _application; }
			set { _application = value; }
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



