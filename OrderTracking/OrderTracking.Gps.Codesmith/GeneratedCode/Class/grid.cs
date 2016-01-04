
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Grid

	/// <summary>
	/// Grid object for NHibernate mapped table 'grid'.
	/// </summary>
	public partial  class Grid
		{
		#region Member Variables
		
		protected int? _id;
		protected string _name;
		protected int? _algorithm;
		protected double? _falseeasting;
		protected double? _falsenorthing;
		protected double? _origolongitude;
		protected double? _origolatitude;
		protected double? _scale;
		protected double? _latitudesp1;
		protected double? _latitudesp2;
		
		

		#endregion

		#region Constructors

		public Grid() { }

		public Grid( string name, int? algorithm, double? falseeasting, double? falsenorthing, double? origolongitude, double? origolatitude, double? scale, double? latitudesp1, double? latitudesp2 )
		{
			this._name = name;
			this._algorithm = algorithm;
			this._falseeasting = falseeasting;
			this._falsenorthing = falsenorthing;
			this._origolongitude = origolongitude;
			this._origolatitude = origolatitude;
			this._scale = scale;
			this._latitudesp1 = latitudesp1;
			this._latitudesp2 = latitudesp2;
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
				if ( value != null && value.Length > 255)
					throw new ArgumentOutOfRangeException("Invalid value for Name", value, value.ToString());
				_name = value;
			}
		}

		public int? Algorithm
		{
			get { return _algorithm; }
			set { _algorithm = value; }
		}

		public double? Falseeasting
		{
			get { return _falseeasting; }
			set { _falseeasting = value; }
		}

		public double? Falsenorthing
		{
			get { return _falsenorthing; }
			set { _falsenorthing = value; }
		}

		public double? Origolongitude
		{
			get { return _origolongitude; }
			set { _origolongitude = value; }
		}

		public double? Origolatitude
		{
			get { return _origolatitude; }
			set { _origolatitude = value; }
		}

		public double? Scale
		{
			get { return _scale; }
			set { _scale = value; }
		}

		public double? Latitudesp1
		{
			get { return _latitudesp1; }
			set { _latitudesp1 = value; }
		}

		public double? Latitudesp2
		{
			get { return _latitudesp2; }
			set { _latitudesp2 = value; }
		}


		#endregion
		
	}

	#endregion
}



