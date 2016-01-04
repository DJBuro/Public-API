
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Trackpostprocessorlog

	/// <summary>
	/// Trackpostprocessorlog object for NHibernate mapped table 'track_post_processor_log'.
	/// </summary>
	public partial  class Trackpostprocessorlog
		{
		#region Member Variables
		
		protected long? _id;
		protected DateTime? _timestampstarted;
		protected int? _timestampstartedms;
		protected DateTime? _timestampdone;
		protected int? _timestampdonems;
		protected DateTime? _trackinfomaxtime;
		protected int? _trackinfomaxtimems;
		protected int? _dirtycount;
		protected Trackinfo _trackinfo = new Trackinfo();
		protected Postprocessor _postprocessor = new Postprocessor();
		
		

		#endregion

		#region Constructors

		public Trackpostprocessorlog() { }

		public Trackpostprocessorlog( DateTime? timestampstarted, int? timestampstartedms, DateTime? timestampdone, int? timestampdonems, DateTime? trackinfomaxtime, int? trackinfomaxtimems, int? dirtycount, Trackinfo trackinfo, Postprocessor postprocessor )
		{
			this._timestampstarted = timestampstarted;
			this._timestampstartedms = timestampstartedms;
			this._timestampdone = timestampdone;
			this._timestampdonems = timestampdonems;
			this._trackinfomaxtime = trackinfomaxtime;
			this._trackinfomaxtimems = trackinfomaxtimems;
			this._dirtycount = dirtycount;
			this._trackinfo = trackinfo;
			this._postprocessor = postprocessor;
		}

		#endregion

		#region Public Properties

		public long? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public DateTime? Timestampstarted
		{
			get { return _timestampstarted; }
			set { _timestampstarted = value; }
		}

		public int? Timestampstartedms
		{
			get { return _timestampstartedms; }
			set { _timestampstartedms = value; }
		}

		public DateTime? Timestampdone
		{
			get { return _timestampdone; }
			set { _timestampdone = value; }
		}

		public int? Timestampdonems
		{
			get { return _timestampdonems; }
			set { _timestampdonems = value; }
		}

		public DateTime? Trackinfomaxtime
		{
			get { return _trackinfomaxtime; }
			set { _trackinfomaxtime = value; }
		}

		public int? Trackinfomaxtimems
		{
			get { return _trackinfomaxtimems; }
			set { _trackinfomaxtimems = value; }
		}

		public int? Dirtycount
		{
			get { return _dirtycount; }
			set { _dirtycount = value; }
		}

		public Trackinfo Trackinfo
		{
			get { return _trackinfo; }
			set { _trackinfo = value; }
		}

		public Postprocessor Postprocessor
		{
			get { return _postprocessor; }
			set { _postprocessor = value; }
		}


		#endregion
		
	}

	#endregion
}



