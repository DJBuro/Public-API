
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Devicedeffield

	/// <summary>
	/// Devicedeffield object for NHibernate mapped table 'device_def_field'.
	/// </summary>
	public partial  class Devicedeffield
		{
		#region Member Variables
		
		protected short _savechangesonly;
		protected Devicedef _devicedef = new Devicedef();
		protected Msgfield _msgfield = new Msgfield();
		
		

		#endregion

		#region Constructors

		public Devicedeffield() { }

		public Devicedeffield( short savechangesonly, Devicedef devicedef, Msgfield msgfield )
		{
			this._savechangesonly = savechangesonly;
			this._devicedef = devicedef;
			this._msgfield = msgfield;
		}

		#endregion

		#region Public Properties


		public short Savechangesonly
		{
			get { return _savechangesonly; }
			set { _savechangesonly = value; }
		}

		public Devicedef Devicedef
		{
			get { return _devicedef; }
			set { _devicedef = value; }
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



