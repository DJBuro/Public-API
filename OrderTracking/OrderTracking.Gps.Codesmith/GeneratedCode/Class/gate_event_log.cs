
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Gateeventlog

	/// <summary>
	/// Gateeventlog object for NHibernate mapped table 'gate_event_log'.
	/// </summary>
	public partial  class Gateeventlog
		{
		#region Member Variables
		
		protected long? _id;
		protected int? _modifiedbyuserid;
		protected DateTime? _servertimestamp;
		protected int? _servertimestampms;
		protected Gateeventstate _gateeventstate = new Gateeventstate();
		protected Gateevent _gateevent = new Gateevent();
		
		

		#endregion

		#region Constructors

		public Gateeventlog() { }

		public Gateeventlog( int? modifiedbyuserid, DateTime? servertimestamp, int? servertimestampms, Gateeventstate gateeventstate, Gateevent gateevent )
		{
			this._modifiedbyuserid = modifiedbyuserid;
			this._servertimestamp = servertimestamp;
			this._servertimestampms = servertimestampms;
			this._gateeventstate = gateeventstate;
			this._gateevent = gateevent;
		}

		#endregion

		#region Public Properties

		public long? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public int? Modifiedbyuserid
		{
			get { return _modifiedbyuserid; }
			set { _modifiedbyuserid = value; }
		}

		public DateTime? Servertimestamp
		{
			get { return _servertimestamp; }
			set { _servertimestamp = value; }
		}

		public int? Servertimestampms
		{
			get { return _servertimestampms; }
			set { _servertimestampms = value; }
		}

		public Gateeventstate Gateeventstate
		{
			get { return _gateeventstate; }
			set { _gateeventstate = value; }
		}

		public Gateevent Gateevent
		{
			get { return _gateevent; }
			set { _gateevent = value; }
		}


		#endregion
		
	}

	#endregion
}



