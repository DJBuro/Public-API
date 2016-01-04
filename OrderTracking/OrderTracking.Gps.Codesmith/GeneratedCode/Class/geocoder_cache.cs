
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Geocodercache

	/// <summary>
	/// Geocodercache object for NHibernate mapped table 'geocoder_cache'.
	/// </summary>
	public partial  class Geocodercache
		{
		#region Member Variables
		
		protected int? _id;
		protected double? _lon;
		protected double? _lat;
		protected string _countryname;
		protected string _cityname;
		protected string _postalcodenumber;
		protected string _streetname;
		protected string _streetnumber;
		protected string _streetbox;
		protected string _address;
		protected long? _lonlathash;
		protected Geocoderprovider _geocoderprovider = new Geocoderprovider();
		
		

		#endregion

		#region Constructors

		public Geocodercache() { }

		public Geocodercache( double? lon, double? lat, string countryname, string cityname, string postalcodenumber, string streetname, string streetnumber, string streetbox, string address, long? lonlathash, Geocoderprovider geocoderprovider )
		{
			this._lon = lon;
			this._lat = lat;
			this._countryname = countryname;
			this._cityname = cityname;
			this._postalcodenumber = postalcodenumber;
			this._streetname = streetname;
			this._streetnumber = streetnumber;
			this._streetbox = streetbox;
			this._address = address;
			this._lonlathash = lonlathash;
			this._geocoderprovider = geocoderprovider;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public double? Lon
		{
			get { return _lon; }
			set { _lon = value; }
		}

		public double? Lat
		{
			get { return _lat; }
			set { _lat = value; }
		}

		public string Countryname
		{
			get { return _countryname; }
			set
			{
				if ( value != null && value.Length > 128)
					throw new ArgumentOutOfRangeException("Invalid value for Countryname", value, value.ToString());
				_countryname = value;
			}
		}

		public string Cityname
		{
			get { return _cityname; }
			set
			{
				if ( value != null && value.Length > 128)
					throw new ArgumentOutOfRangeException("Invalid value for Cityname", value, value.ToString());
				_cityname = value;
			}
		}

		public string Postalcodenumber
		{
			get { return _postalcodenumber; }
			set
			{
				if ( value != null && value.Length > 45)
					throw new ArgumentOutOfRangeException("Invalid value for Postalcodenumber", value, value.ToString());
				_postalcodenumber = value;
			}
		}

		public string Streetname
		{
			get { return _streetname; }
			set
			{
				if ( value != null && value.Length > 128)
					throw new ArgumentOutOfRangeException("Invalid value for Streetname", value, value.ToString());
				_streetname = value;
			}
		}

		public string Streetnumber
		{
			get { return _streetnumber; }
			set
			{
				if ( value != null && value.Length > 45)
					throw new ArgumentOutOfRangeException("Invalid value for Streetnumber", value, value.ToString());
				_streetnumber = value;
			}
		}

		public string Streetbox
		{
			get { return _streetbox; }
			set
			{
				if ( value != null && value.Length > 45)
					throw new ArgumentOutOfRangeException("Invalid value for Streetbox", value, value.ToString());
				_streetbox = value;
			}
		}

		public string Address
		{
			get { return _address; }
			set
			{
				if ( value != null && value.Length > 128)
					throw new ArgumentOutOfRangeException("Invalid value for Address", value, value.ToString());
				_address = value;
			}
		}

		public long? Lonlathash
		{
			get { return _lonlathash; }
			set { _lonlathash = value; }
		}

		public Geocoderprovider Geocoderprovider
		{
			get { return _geocoderprovider; }
			set { _geocoderprovider = value; }
		}


		#endregion
		
	}

	#endregion
}



