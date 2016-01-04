
using System;
using System.Collections;


namespace OrderTracking.Dao.Domain
{
	#region Log

	/// <summary>
	/// Log object for NHibernate mapped table 'tbl_Log'.
	/// </summary>
	public partial  class Log
		{
		#region Member Variables
		
		protected long? _id;
		protected string _storeId;
		protected string _severity;
		protected string _message;
		protected string _method;
		protected string _source;
		protected DateTime? _created;
		
		

		#endregion

		#region Constructors

		public Log() { }

		public Log( string storeId, string severity, string message, string method, string source, DateTime? created )
		{
			this._storeId = storeId;
			this._severity = severity;
			this._message = message;
			this._method = method;
			this._source = source;
			this._created = created;
		}

		#endregion

		#region Public Properties

		public long? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public string StoreId
		{
			get { return _storeId; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for StoreId", value, value.ToString());
				_storeId = value;
			}
		}

		public string Severity
		{
			get { return _severity; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for Severity", value, value.ToString());
				_severity = value;
			}
		}

		public string Message
		{
			get { return _message; }
			set
			{
				if ( value != null && value.Length > 500)
					throw new ArgumentOutOfRangeException("Invalid value for Message", value, value.ToString());
				_message = value;
			}
		}

		public string Method
		{
			get { return _method; }
			set
			{
				if ( value != null && value.Length > 200)
					throw new ArgumentOutOfRangeException("Invalid value for Method", value, value.ToString());
				_method = value;
			}
		}

		public string Source
		{
			get { return _source; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for Source", value, value.ToString());
				_source = value;
			}
		}

		public DateTime? Created
		{
			get { return _created; }
			set { _created = value; }
		}


		#endregion
		
	}

	#endregion
}



