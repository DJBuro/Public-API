
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Datum

	/// <summary>
	/// Datum object for NHibernate mapped table 'datum'.
	/// </summary>
	public partial  class Datum
		{
		#region Member Variables
		
		protected int? _id;
		protected string _name;
		protected double? _semimajoraxis;
		protected double? _e2;
		protected double? _deltax;
		protected double? _deltay;
		protected double? _deltaz;
		protected double? _rotx;
		protected double? _roty;
		protected double? _rotz;
		protected double? _scale;
		
		

		#endregion

		#region Constructors

		public Datum() { }

		public Datum( string name, double? semimajoraxis, double? e2, double? deltax, double? deltay, double? deltaz, double? rotx, double? roty, double? rotz, double? scale )
		{
			this._name = name;
			this._semimajoraxis = semimajoraxis;
			this._e2 = e2;
			this._deltax = deltax;
			this._deltay = deltay;
			this._deltaz = deltaz;
			this._rotx = rotx;
			this._roty = roty;
			this._rotz = rotz;
			this._scale = scale;
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

		public double? Semimajoraxis
		{
			get { return _semimajoraxis; }
			set { _semimajoraxis = value; }
		}

		public double? E2
		{
			get { return _e2; }
			set { _e2 = value; }
		}

		public double? Deltax
		{
			get { return _deltax; }
			set { _deltax = value; }
		}

		public double? Deltay
		{
			get { return _deltay; }
			set { _deltay = value; }
		}

		public double? Deltaz
		{
			get { return _deltaz; }
			set { _deltaz = value; }
		}

		public double? Rotx
		{
			get { return _rotx; }
			set { _rotx = value; }
		}

		public double? Roty
		{
			get { return _roty; }
			set { _roty = value; }
		}

		public double? Rotz
		{
			get { return _rotz; }
			set { _rotz = value; }
		}

		public double? Scale
		{
			get { return _scale; }
			set { _scale = value; }
		}


		#endregion
		
	}

	#endregion
}



