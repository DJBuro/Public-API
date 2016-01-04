
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Messagetemplate

	/// <summary>
	/// Messagetemplate object for NHibernate mapped table 'message_template'.
	/// </summary>
	public partial  class Messagetemplate
		{
		#region Member Variables
		
		protected int? _id;
		protected string _templatetext;
		protected string _botype;
		protected Notifier _notifier = new Notifier();
		
		

		#endregion

		#region Constructors

		public Messagetemplate() { }

		public Messagetemplate( string templatetext, string botype, Notifier notifier )
		{
			this._templatetext = templatetext;
			this._botype = botype;
			this._notifier = notifier;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public string Templatetext
		{
			get { return _templatetext; }
			set
			{
				if ( value != null && value.Length > 16)
					throw new ArgumentOutOfRangeException("Invalid value for Templatetext", value, value.ToString());
				_templatetext = value;
			}
		}

		public string Botype
		{
			get { return _botype; }
			set
			{
				if ( value != null && value.Length > 255)
					throw new ArgumentOutOfRangeException("Invalid value for Botype", value, value.ToString());
				_botype = value;
			}
		}

		public Notifier Notifier
		{
			get { return _notifier; }
			set { _notifier = value; }
		}


		#endregion
		
	}

	#endregion
}



