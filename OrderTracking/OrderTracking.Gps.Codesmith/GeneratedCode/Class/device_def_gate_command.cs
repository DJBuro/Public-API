
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Devicedefgatecommand

	/// <summary>
	/// Devicedefgatecommand object for NHibernate mapped table 'device_def_gate_command'.
	/// </summary>
	public partial  class Devicedefgatecommand
		{
		#region Member Variables
		
		protected string _transport;
		protected Devicedef _devicedef = new Devicedef();
		protected Gatecommand _gatecommand = new Gatecommand();
		
		

		#endregion

		#region Constructors

		public Devicedefgatecommand() { }

		public Devicedefgatecommand( string transport, Devicedef devicedef, Gatecommand gatecommand )
		{
			this._transport = transport;
			this._devicedef = devicedef;
			this._gatecommand = gatecommand;
		}

		#endregion

		#region Public Properties


		public string Transport
		{
			get { return _transport; }
			set
			{
				if ( value != null && value.Length > 128)
					throw new ArgumentOutOfRangeException("Invalid value for Transport", value, value.ToString());
				_transport = value;
			}
		}

		public Devicedef Devicedef
		{
			get { return _devicedef; }
			set { _devicedef = value; }
		}

		public Gatecommand Gatecommand
		{
			get { return _gatecommand; }
			set { _gatecommand = value; }
		}


		#endregion
		
	}

	#endregion
}



