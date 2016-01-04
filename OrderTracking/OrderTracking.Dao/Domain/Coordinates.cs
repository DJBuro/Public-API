
namespace OrderTracking.Dao.Domain
{
	#region Coordinates

	/// <summary>
	/// Coordinates object for NHibernate mapped table 'tbl_Coordinates'.
	/// </summary>
	public class Coordinates : Entity.Entity
		{
		#region Member Variables
		
		protected float _longitude;
        protected float _latitude;
		
		/*
		protected IList _coordinatesStores;
		protected IList _coordinatesTrackers;
		protected IList _coordinatesCustomers;
*/
		#endregion

		#region Constructors

		public Coordinates() { }

		public Coordinates(float longitude, float latitude)
		{
			this._longitude = longitude;
			this._latitude = latitude;
		}

		#endregion

		#region Public Properties

        public float Longitude
		{
			get { return _longitude; }
			set { _longitude = value; }
		}

        public float Latitude
		{
			get { return _latitude; }
			set { _latitude = value; }
		}

	/*	public IList CoordinatesStores
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
        */

		#endregion
		
	}

	#endregion
}



