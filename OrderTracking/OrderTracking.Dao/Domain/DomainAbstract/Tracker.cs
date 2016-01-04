
using System;
using System.Collections;


namespace OrderTracking.Dao.Domain
{
	#region Tracker

	/// <summary>
	/// Tracker object for NHibernate mapped table 'tbl_Tracker'.
	/// </summary>
	public partial  class Tracker
		{
		#region Member Variables
		
		protected int? _id;
		protected string _name;
		protected double? _longitude;
		protected double? _latitude;
		protected string _status;
		protected Driver _driverId = new Driver();
		
		

		#endregion

		#region Constructors

		public Tracker() { }

		public Tracker( string name, double? longitude, double? latitude, string status, Driver driverId )
		{
			this._name = name;
			this._longitude = longitude;
			this._latitude = latitude;
			this._status = status;
			this._driverId = driverId;
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
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for Name", value, value.ToString());
				_name = value;
			}
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

		public string Status
		{
			get { return _status; }
			set
			{
				if ( value != null && value.Length > 20)
					throw new ArgumentOutOfRangeException("Invalid value for Status", value, value.ToString());
				_status = value;
			}
		}

		public Driver DriverId
		{
			get { return _driverId; }
			set { _driverId = value; }
		}


		#endregion
		
	}

	#endregion
}



