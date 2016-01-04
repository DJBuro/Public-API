
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Serviceplugin

	/// <summary>
	/// Serviceplugin object for NHibernate mapped table 'service_plugin'.
	/// </summary>
	public partial  class Serviceplugin
		{
		#region Member Variables
		
		protected int? _id;
		protected int? _enabled;
		protected string _botype;
		protected Loadabletype _loadabletype = new Loadabletype();
		
		

		#endregion

		#region Constructors

		public Serviceplugin() { }

		public Serviceplugin( int? enabled, string botype, Loadabletype loadabletype )
		{
			this._enabled = enabled;
			this._botype = botype;
			this._loadabletype = loadabletype;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public int? Enabled
		{
			get { return _enabled; }
			set { _enabled = value; }
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

		public Loadabletype Loadabletype
		{
			get { return _loadabletype; }
			set { _loadabletype = value; }
		}


		#endregion
		
	}

	#endregion
}



