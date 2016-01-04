
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Emailnotifier

	/// <summary>
	/// Emailnotifier object for NHibernate mapped table 'email_notifier'.
	/// </summary>
	public partial  class Emailnotifier
		{
		#region Member Variables
		
		protected int? _id;
		protected string _subject;
		protected byte? _ishtml;
		protected Notifier _notifier = new Notifier();
		
		

		#endregion

		#region Constructors

		public Emailnotifier() { }

		public Emailnotifier( string subject, byte? ishtml, Notifier notifier )
		{
			this._subject = subject;
			this._ishtml = ishtml;
			this._notifier = notifier;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public string Subject
		{
			get { return _subject; }
			set
			{
				if ( value != null && value.Length > 255)
					throw new ArgumentOutOfRangeException("Invalid value for Subject", value, value.ToString());
				_subject = value;
			}
		}

		public byte? Ishtml
		{
			get { return _ishtml; }
			set { _ishtml = value; }
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



