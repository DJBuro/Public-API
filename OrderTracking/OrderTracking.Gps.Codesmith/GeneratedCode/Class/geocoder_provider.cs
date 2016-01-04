
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Geocoderprovider

	/// <summary>
	/// Geocoderprovider object for NHibernate mapped table 'geocoder_provider'.
	/// </summary>
	public partial  class Geocoderprovider
		{
		#region Member Variables
		
		protected int? _id;
		protected int? _typeid;
		protected int? _priority;
		protected Geocoder _geocoder = new Geocoder();
		
		

		#endregion

		#region Constructors

		public Geocoderprovider() { }

		public Geocoderprovider( int? typeid, int? priority, Geocoder geocoder )
		{
			this._typeid = typeid;
			this._priority = priority;
			this._geocoder = geocoder;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public int? Typeid
		{
			get { return _typeid; }
			set { _typeid = value; }
		}

		public int? Priority
		{
			get { return _priority; }
			set { _priority = value; }
		}

		public Geocoder Geocoder
		{
			get { return _geocoder; }
			set { _geocoder = value; }
		}


		#endregion
		
	}

	#endregion
}



