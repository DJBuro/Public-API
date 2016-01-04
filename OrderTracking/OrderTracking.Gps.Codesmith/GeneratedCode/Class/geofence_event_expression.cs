
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Geofenceeventexpression

	/// <summary>
	/// Geofenceeventexpression object for NHibernate mapped table 'geofence_event_expression'.
	/// </summary>
	public partial  class Geofenceeventexpression
		{
		#region Member Variables
		
		protected int? _id;
		protected int? _geofenceaction;
		protected Gateeventexpression _gateeventexpression = new Gateeventexpression();
		protected Tag _tag = new Tag();
		
		

		#endregion

		#region Constructors

		public Geofenceeventexpression() { }

		public Geofenceeventexpression( int? geofenceaction, Gateeventexpression gateeventexpression, Tag tag )
		{
			this._geofenceaction = geofenceaction;
			this._gateeventexpression = gateeventexpression;
			this._tag = tag;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public int? Geofenceaction
		{
			get { return _geofenceaction; }
			set { _geofenceaction = value; }
		}

		public Gateeventexpression Gateeventexpression
		{
			get { return _gateeventexpression; }
			set { _gateeventexpression = value; }
		}

		public Tag Tag
		{
			get { return _tag; }
			set { _tag = value; }
		}


		#endregion
		
	}

	#endregion
}



