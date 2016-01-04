
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Mapdata

	/// <summary>
	/// Mapdata object for NHibernate mapped table 'map_data'.
	/// </summary>
	public partial  class Mapdata
		{
		#region Member Variables
		
		protected int? _id;
		protected string _botype;
		protected string _name;
		protected int? _bmtilewidth;
		protected int? _bmtileheight;
		protected int? _bmtotalwidth;
		protected int? _bmtotalheight;
		protected int? _gridid;
		protected int? _datumid;
		protected double? _geomine;
		protected double? _geominn;
		protected double? _geomaxe;
		protected double? _geomaxn;
		protected string _projtype;
		protected double? _projorigoe;
		protected double? _projorigon;
		protected double? _projdedx;
		protected double? _projdedy;
		protected double? _projdndx;
		protected double? _projdndy;
		protected string _xmlfilepath;
		protected string _virtualpath;
		protected DateTime? _created;
		protected double? _projorigox;
		protected double? _projorigoy;
		protected double? _projdvde;
		protected double? _projdrdn;
		
		

		#endregion

		#region Constructors

		public Mapdata() { }

		public Mapdata( string botype, string name, int? bmtilewidth, int? bmtileheight, int? bmtotalwidth, int? bmtotalheight, int? gridid, int? datumid, double? geomine, double? geominn, double? geomaxe, double? geomaxn, string projtype, double? projorigoe, double? projorigon, double? projdedx, double? projdedy, double? projdndx, double? projdndy, string xmlfilepath, string virtualpath, DateTime? created, double? projorigox, double? projorigoy, double? projdvde, double? projdrdn )
		{
			this._botype = botype;
			this._name = name;
			this._bmtilewidth = bmtilewidth;
			this._bmtileheight = bmtileheight;
			this._bmtotalwidth = bmtotalwidth;
			this._bmtotalheight = bmtotalheight;
			this._gridid = gridid;
			this._datumid = datumid;
			this._geomine = geomine;
			this._geominn = geominn;
			this._geomaxe = geomaxe;
			this._geomaxn = geomaxn;
			this._projtype = projtype;
			this._projorigoe = projorigoe;
			this._projorigon = projorigon;
			this._projdedx = projdedx;
			this._projdedy = projdedy;
			this._projdndx = projdndx;
			this._projdndy = projdndy;
			this._xmlfilepath = xmlfilepath;
			this._virtualpath = virtualpath;
			this._created = created;
			this._projorigox = projorigox;
			this._projorigoy = projorigoy;
			this._projdvde = projdvde;
			this._projdrdn = projdrdn;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
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

		public int? Bmtilewidth
		{
			get { return _bmtilewidth; }
			set { _bmtilewidth = value; }
		}

		public int? Bmtileheight
		{
			get { return _bmtileheight; }
			set { _bmtileheight = value; }
		}

		public int? Bmtotalwidth
		{
			get { return _bmtotalwidth; }
			set { _bmtotalwidth = value; }
		}

		public int? Bmtotalheight
		{
			get { return _bmtotalheight; }
			set { _bmtotalheight = value; }
		}

		public int? Gridid
		{
			get { return _gridid; }
			set { _gridid = value; }
		}

		public int? Datumid
		{
			get { return _datumid; }
			set { _datumid = value; }
		}

		public double? Geomine
		{
			get { return _geomine; }
			set { _geomine = value; }
		}

		public double? Geominn
		{
			get { return _geominn; }
			set { _geominn = value; }
		}

		public double? Geomaxe
		{
			get { return _geomaxe; }
			set { _geomaxe = value; }
		}

		public double? Geomaxn
		{
			get { return _geomaxn; }
			set { _geomaxn = value; }
		}

		public string Projtype
		{
			get { return _projtype; }
			set
			{
				if ( value != null && value.Length > 64)
					throw new ArgumentOutOfRangeException("Invalid value for Projtype", value, value.ToString());
				_projtype = value;
			}
		}

		public double? Projorigoe
		{
			get { return _projorigoe; }
			set { _projorigoe = value; }
		}

		public double? Projorigon
		{
			get { return _projorigon; }
			set { _projorigon = value; }
		}

		public double? Projdedx
		{
			get { return _projdedx; }
			set { _projdedx = value; }
		}

		public double? Projdedy
		{
			get { return _projdedy; }
			set { _projdedy = value; }
		}

		public double? Projdndx
		{
			get { return _projdndx; }
			set { _projdndx = value; }
		}

		public double? Projdndy
		{
			get { return _projdndy; }
			set { _projdndy = value; }
		}

		public string Xmlfilepath
		{
			get { return _xmlfilepath; }
			set
			{
				if ( value != null && value.Length > 16)
					throw new ArgumentOutOfRangeException("Invalid value for Xmlfilepath", value, value.ToString());
				_xmlfilepath = value;
			}
		}

		public string Virtualpath
		{
			get { return _virtualpath; }
			set
			{
				if ( value != null && value.Length > 255)
					throw new ArgumentOutOfRangeException("Invalid value for Virtualpath", value, value.ToString());
				_virtualpath = value;
			}
		}

		public DateTime? Created
		{
			get { return _created; }
			set { _created = value; }
		}

		public double? Projorigox
		{
			get { return _projorigox; }
			set { _projorigox = value; }
		}

		public double? Projorigoy
		{
			get { return _projorigoy; }
			set { _projorigoy = value; }
		}

		public double? Projdvde
		{
			get { return _projdvde; }
			set { _projdvde = value; }
		}

		public double? Projdrdn
		{
			get { return _projdrdn; }
			set { _projdrdn = value; }
		}


		#endregion
		
	}

	#endregion
}



