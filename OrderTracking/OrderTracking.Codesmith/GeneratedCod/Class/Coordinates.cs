
using System;
using System.Collections;


namespace OrderTracking.Dao.Domain
{
	#region Coordinate

	/// <summary>
	/// Coordinate object for NHibernate mapped table 'tbl_Coordinates'.
	/// </summary>
	public partial  class Coordinate
		{
		#region Member Variables
		
		protected long? _id;
		protected double? _longitude;
		protected double? _latitude;
		
		
		protected IList _coordinatesStores;
		protected IList _coordinatesCustomers;
		protected IList _coordinatesTrackers;

		#endregion

		#region Constructors

		public Coordinate() { }

		public Coordinate( double? longitude, double? latitude )
		{
			this._longitude = longitude;
			this._latitude = latitude;
		}

		#endregion

		#region Public Properties

		public long? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public double? Longitude
		{
			get { return _longitude; }
			set { _longitude = value; }
		}

		public double? Latitude
		{
			get { return _latitude; }
			set { _latitude = value; }
		}

		public IList CoordinatesStores
		{
			get
			{
				if (_coordinatesStores==null)
				{
					_coordinatesStores = new ArrayList();
				}
				return _coordinatesStores;
			}
			set { _coordinatesStores = value; }
		}

		public IList CoordinatesCustomers
		{
			get
			{
				if (_coordinatesCustomers==null)
				{
					_coordinatesCustomers = new ArrayList();
				}
				return _coordinatesCustomers;
			}
			set { _coordinatesCustomers = value; }
		}

		public IList CoordinatesTrackers
		{
			get
			{
				if (_coordinatesTrackers==null)
				{
					_coordinatesTrackers = new ArrayList();
				}
				return _coordinatesTrackers;
			}
			set { _coordinatesTrackers = value; }
		}


		#endregion
		
	}

	#endregion
}



