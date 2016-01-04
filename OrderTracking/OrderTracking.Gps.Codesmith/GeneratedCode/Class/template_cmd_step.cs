
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Templatecmdstep

	/// <summary>
	/// Templatecmdstep object for NHibernate mapped table 'template_cmd_step'.
	/// </summary>
	public partial  class Templatecmdstep
		{
		#region Member Variables
		
		protected int? _id;
		protected string _template;
		protected string _transport;
		protected string _stepdescription;
		protected Gatecommand _gatecommand = new Gatecommand();
		
		

		#endregion

		#region Constructors

		public Templatecmdstep() { }

		public Templatecmdstep( string template, string transport, string stepdescription, Gatecommand gatecommand )
		{
			this._template = template;
			this._transport = transport;
			this._stepdescription = stepdescription;
			this._gatecommand = gatecommand;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public string Template
		{
			get { return _template; }
			set
			{
				if ( value != null && value.Length > 16)
					throw new ArgumentOutOfRangeException("Invalid value for Template", value, value.ToString());
				_template = value;
			}
		}

		public string Transport
		{
			get { return _transport; }
			set
			{
				if ( value != null && value.Length > 255)
					throw new ArgumentOutOfRangeException("Invalid value for Transport", value, value.ToString());
				_transport = value;
			}
		}

		public string Stepdescription
		{
			get { return _stepdescription; }
			set
			{
				if ( value != null && value.Length > 255)
					throw new ArgumentOutOfRangeException("Invalid value for Stepdescription", value, value.ToString());
				_stepdescription = value;
			}
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



