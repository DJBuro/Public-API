
using System;
using System.Collections;


namespace OrderTracking.Dao.Domain
{
	#region TrackerType

	/// <summary>
	/// TrackerType object for NHibernate mapped table 'tbl_TrackerType'.
	/// </summary>
	public partial  class TrackerType
		{
		#region Member Variables
		
		protected long? _id;
		protected string _name;
		protected int? _gpsGateId;
		
		
		protected IList _typeIdTrackers;
		protected IList _trackerTypeIdTrackerCommandses;

		#endregion

		#region Constructors

		public TrackerType() { }

		public TrackerType( string name, int? gpsGateId )
		{
			this._name = name;
			this._gpsGateId = gpsGateId;
		}

		#endregion

		#region Public Properties

		public long? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public string Name
		{
			get { return _name; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for Name", value, value.ToString());
				_name = value;
			}
		}

		public int? GpsGateId
		{
			get { return _gpsGateId; }
			set { _gpsGateId = value; }
		}

		public IList TypeIdTrackers
		{
			get
			{
				if (_typeIdTrackers==null)
				{
					_typeIdTrackers = new ArrayList();
				}
				return _typeIdTrackers;
			}
			set { _typeIdTrackers = value; }
		}

		public IList TrackerTypeIdTrackerCommandses
		{
			get
			{
				if (_trackerTypeIdTrackerCommandses==null)
				{
					_trackerTypeIdTrackerCommandses = new ArrayList();
				}
				return _trackerTypeIdTrackerCommandses;
			}
			set { _trackerTypeIdTrackerCommandses = value; }
		}


		#endregion
		
	}

	#endregion
}



