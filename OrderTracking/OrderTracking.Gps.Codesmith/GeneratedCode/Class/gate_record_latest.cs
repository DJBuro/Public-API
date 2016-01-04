
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Gaterecordlatest

	/// <summary>
	/// Gaterecordlatest object for NHibernate mapped table 'gate_record_latest'.
	/// </summary>
	public partial  class Gaterecordlatest
		{
		#region Member Variables
		
		protected long? _id;
		protected Device _device = new Device();
		protected Gateuser _gateuser = new Gateuser();
		protected Gatemessagerecord _gatemessagerecord = new Gatemessagerecord();
		protected Msgfield _msgfield = new Msgfield();
		
		

		#endregion

		#region Constructors

		public Gaterecordlatest() { }

		public Gaterecordlatest( Device device, Gateuser gateuser, Gatemessagerecord gatemessagerecord, Msgfield msgfield )
		{
			this._device = device;
			this._gateuser = gateuser;
			this._gatemessagerecord = gatemessagerecord;
			this._msgfield = msgfield;
		}

		#endregion

		#region Public Properties

		public long? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public Device Device
		{
			get { return _device; }
			set { _device = value; }
		}

		public Gateuser Gateuser
		{
			get { return _gateuser; }
			set { _gateuser = value; }
		}

		public Gatemessagerecord Gatemessagerecord
		{
			get { return _gatemessagerecord; }
			set { _gatemessagerecord = value; }
		}

		public Msgfield Msgfield
		{
			get { return _msgfield; }
			set { _msgfield = value; }
		}


		#endregion
		
	}

	#endregion
}



