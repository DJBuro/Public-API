
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Userattributenotifier

	/// <summary>
	/// Userattributenotifier object for NHibernate mapped table 'user_attribute_notifier'.
	/// </summary>
	public partial  class Userattributenotifier
		{
		#region Member Variables
		
		protected int? _id;
		protected string _header;
		protected string _type;
		protected string _attributekey;
		protected string _attributevaluestart;
		protected string _attributevalueend;
		protected int? _applicationid;
		protected Notifier _notifier = new Notifier();
		
		

		#endregion

		#region Constructors

		public Userattributenotifier() { }

		public Userattributenotifier( string header, string type, string attributekey, string attributevaluestart, string attributevalueend, int? applicationid, Notifier notifier )
		{
			this._header = header;
			this._type = type;
			this._attributekey = attributekey;
			this._attributevaluestart = attributevaluestart;
			this._attributevalueend = attributevalueend;
			this._applicationid = applicationid;
			this._notifier = notifier;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public string Header
		{
			get { return _header; }
			set
			{
				if ( value != null && value.Length > 255)
					throw new ArgumentOutOfRangeException("Invalid value for Header", value, value.ToString());
				_header = value;
			}
		}

		public string Type
		{
			get { return _type; }
			set
			{
				if ( value != null && value.Length > 255)
					throw new ArgumentOutOfRangeException("Invalid value for Type", value, value.ToString());
				_type = value;
			}
		}

		public string Attributekey
		{
			get { return _attributekey; }
			set
			{
				if ( value != null && value.Length > 45)
					throw new ArgumentOutOfRangeException("Invalid value for Attributekey", value, value.ToString());
				_attributekey = value;
			}
		}

		public string Attributevaluestart
		{
			get { return _attributevaluestart; }
			set
			{
				if ( value != null && value.Length > 255)
					throw new ArgumentOutOfRangeException("Invalid value for Attributevaluestart", value, value.ToString());
				_attributevaluestart = value;
			}
		}

		public string Attributevalueend
		{
			get { return _attributevalueend; }
			set
			{
				if ( value != null && value.Length > 255)
					throw new ArgumentOutOfRangeException("Invalid value for Attributevalueend", value, value.ToString());
				_attributevalueend = value;
			}
		}

		public int? Applicationid
		{
			get { return _applicationid; }
			set { _applicationid = value; }
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



