
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Gateeventargument

	/// <summary>
	/// Gateeventargument object for NHibernate mapped table 'gate_event_argument'.
	/// </summary>
	public partial  class Gateeventargument
		{
		#region Member Variables
		
		protected int? _id;
		protected string _botype;
		protected string _argumentdescription;
		protected string _valuedata;
		protected string _valuetype;
		protected string _localizationkey;
		protected Gateevent _gateevent = new Gateevent();
		
		
		protected Gateeventargumentgeneric _gateeventargumentgeneric;
		protected Geofenceeventargument _geofenceeventargument;

		#endregion

		#region Constructors

		public Gateeventargument() { }

		public Gateeventargument( string botype, string argumentdescription, string valuedata, string valuetype, string localizationkey, Gateevent gateevent )
		{
			this._botype = botype;
			this._argumentdescription = argumentdescription;
			this._valuedata = valuedata;
			this._valuetype = valuetype;
			this._localizationkey = localizationkey;
			this._gateevent = gateevent;
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
				if ( value != null && value.Length > 255)
					throw new ArgumentOutOfRangeException("Invalid value for Botype", value, value.ToString());
				_botype = value;
			}
		}

		public string Argumentdescription
		{
			get { return _argumentdescription; }
			set
			{
				if ( value != null && value.Length > 255)
					throw new ArgumentOutOfRangeException("Invalid value for Argumentdescription", value, value.ToString());
				_argumentdescription = value;
			}
		}

		public string Valuedata
		{
			get { return _valuedata; }
			set
			{
				if ( value != null && value.Length > 255)
					throw new ArgumentOutOfRangeException("Invalid value for Valuedata", value, value.ToString());
				_valuedata = value;
			}
		}

		public string Valuetype
		{
			get { return _valuetype; }
			set
			{
				if ( value != null && value.Length > 64)
					throw new ArgumentOutOfRangeException("Invalid value for Valuetype", value, value.ToString());
				_valuetype = value;
			}
		}

		public string Localizationkey
		{
			get { return _localizationkey; }
			set
			{
				if ( value != null && value.Length > 71)
					throw new ArgumentOutOfRangeException("Invalid value for Localizationkey", value, value.ToString());
				_localizationkey = value;
			}
		}

		public Gateevent Gateevent
		{
			get { return _gateevent; }
			set { _gateevent = value; }
		}

		public Gateeventargumentgeneric Gateeventargumentgeneric
		{
			get { return _gateeventargumentgeneric; }
			set { _gateeventargumentgeneric = value; }
		}

		public Geofenceeventargument Geofenceeventargument
		{
			get { return _geofenceeventargument; }
			set { _geofenceeventargument = value; }
		}


		#endregion
		
	}

	#endregion
}



