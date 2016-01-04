
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Geocoder

	/// <summary>
	/// Geocoder object for NHibernate mapped table 'geocoder'.
	/// </summary>
	public partial  class Geocoder
		{
		#region Member Variables
		
		protected int? _id;
		protected string _name;
		protected string _description;
		protected string _botype;
		
		
		protected Geocoderapplication _geocoderapplication;
		protected IList _geocoderproviders;

		#endregion

		#region Constructors

		public Geocoder() { }

		public Geocoder( string name, string description, string botype )
		{
			this._name = name;
			this._description = description;
			this._botype = botype;
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
				if ( value != null && value.Length > 128)
					throw new ArgumentOutOfRangeException("Invalid value for Description", value, value.ToString());
				_description = value;
			}
		}

		public string Botype
		{
			get { return _botype; }
			set
			{
				if ( value != null && value.Length > 128)
					throw new ArgumentOutOfRangeException("Invalid value for Botype", value, value.ToString());
				_botype = value;
			}
		}

		public Geocoderapplication Geocoderapplication
		{
			get { return _geocoderapplication; }
			set { _geocoderapplication = value; }
		}

		public IList geocoder_providers
		{
			get
			{
				if (_geocoderproviders==null)
				{
					_geocoderproviders = new ArrayList();
				}
				return _geocoderproviders;
			}
			set { _geocoderproviders = value; }
		}


		#endregion
		
	}

	#endregion
}



