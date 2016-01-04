
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Gatemessagerecord

	/// <summary>
	/// Gatemessagerecord object for NHibernate mapped table 'gate_message_record'.
	/// </summary>
	public partial  class Gatemessagerecord
		{
		#region Member Variables
		
		protected long? _id;
		protected short _databool;
		protected int? _dataint;
		protected double? _datadouble;
		protected string _datatext;
		protected string _datalongtext;
		protected DateTime? _datadatetime;
		protected short _savechangesonly;
		protected Gatemessage _gatemessage = new Gatemessage();
		protected Msgfield _msgfield = new Msgfield();
		
		
		protected IList _gaterecordlatests;

		#endregion

		#region Constructors

		public Gatemessagerecord() { }

		public Gatemessagerecord( short databool, int? dataint, double? datadouble, string datatext, string datalongtext, DateTime? datadatetime, short savechangesonly, Gatemessage gatemessage, Msgfield msgfield )
		{
			this._databool = databool;
			this._dataint = dataint;
			this._datadouble = datadouble;
			this._datatext = datatext;
			this._datalongtext = datalongtext;
			this._datadatetime = datadatetime;
			this._savechangesonly = savechangesonly;
			this._gatemessage = gatemessage;
			this._msgfield = msgfield;
		}

		#endregion

		#region Public Properties

		public long? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public short Databool
		{
			get { return _databool; }
			set { _databool = value; }
		}

		public int? Dataint
		{
			get { return _dataint; }
			set { _dataint = value; }
		}

		public double? Datadouble
		{
			get { return _datadouble; }
			set { _datadouble = value; }
		}

		public string Datatext
		{
			get { return _datatext; }
			set
			{
				if ( value != null && value.Length > 255)
					throw new ArgumentOutOfRangeException("Invalid value for Datatext", value, value.ToString());
				_datatext = value;
			}
		}

		public string Datalongtext
		{
			get { return _datalongtext; }
			set
			{
				if ( value != null && value.Length > 16)
					throw new ArgumentOutOfRangeException("Invalid value for Datalongtext", value, value.ToString());
				_datalongtext = value;
			}
		}

		public DateTime? Datadatetime
		{
			get { return _datadatetime; }
			set { _datadatetime = value; }
		}

		public short Savechangesonly
		{
			get { return _savechangesonly; }
			set { _savechangesonly = value; }
		}

		public Gatemessage Gatemessage
		{
			get { return _gatemessage; }
			set { _gatemessage = value; }
		}

		public Msgfield Msgfield
		{
			get { return _msgfield; }
			set { _msgfield = value; }
		}

		public IList gate_record_latests
		{
			get
			{
				if (_gaterecordlatests==null)
				{
					_gaterecordlatests = new ArrayList();
				}
				return _gaterecordlatests;
			}
			set { _gaterecordlatests = value; }
		}


		#endregion
		
	}

	#endregion
}



