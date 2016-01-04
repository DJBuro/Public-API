
using System;
using System.Collections;


namespace OrderTracking.Dao.Domain
{
	#region TrackerStatus

	/// <summary>
	/// TrackerStatus object for NHibernate mapped table 'tbl_TrackerStatus'.
	/// </summary>
	public partial  class TrackerStatus
		{
		#region Member Variables
		
		protected long? _id;
		protected string _name;
		
		
		protected IList _statusTrackers;

		#endregion

		#region Constructors

		public TrackerStatus() { }

		public TrackerStatus( string name )
		{
			this._name = name;
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

		public IList StatusTrackers
		{
			get
			{
				if (_statusTrackers==null)
				{
					_statusTrackers = new ArrayList();
				}
				return _statusTrackers;
			}
			set { _statusTrackers = value; }
		}


		#endregion
		
	}

	#endregion
}



