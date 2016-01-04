
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region License

	/// <summary>
	/// License object for NHibernate mapped table 'license'.
	/// </summary>
	public partial  class License
		{
		#region Member Variables
		
		protected int? _id;
		protected int? _licensedusers;
		protected string _licenseid;
		protected string _customerid;
		protected string _description;
		protected string _signature;
		protected string _email;
		protected string _botype;
		
		

		#endregion

		#region Constructors

		public License() { }

		public License( int? licensedusers, string licenseid, string customerid, string description, string signature, string email, string botype )
		{
			this._licensedusers = licensedusers;
			this._licenseid = licenseid;
			this._customerid = customerid;
			this._description = description;
			this._signature = signature;
			this._email = email;
			this._botype = botype;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public int? Licensedusers
		{
			get { return _licensedusers; }
			set { _licensedusers = value; }
		}

		public string Licenseid
		{
			get { return _licenseid; }
			set
			{
				if ( value != null && value.Length > 36)
					throw new ArgumentOutOfRangeException("Invalid value for Licenseid", value, value.ToString());
				_licenseid = value;
			}
		}

		public string Customerid
		{
			get { return _customerid; }
			set
			{
				if ( value != null && value.Length > 36)
					throw new ArgumentOutOfRangeException("Invalid value for Customerid", value, value.ToString());
				_customerid = value;
			}
		}

		public string Description
		{
			get { return _description; }
			set
			{
				if ( value != null && value.Length > 100)
					throw new ArgumentOutOfRangeException("Invalid value for Description", value, value.ToString());
				_description = value;
			}
		}

		public string Signature
		{
			get { return _signature; }
			set
			{
				if ( value != null && value.Length > 172)
					throw new ArgumentOutOfRangeException("Invalid value for Signature", value, value.ToString());
				_signature = value;
			}
		}

		public string Email
		{
			get { return _email; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for Email", value, value.ToString());
				_email = value;
			}
		}

		public string Botype
		{
			get { return _botype; }
			set
			{
				if ( value != null && value.Length > 64)
					throw new ArgumentOutOfRangeException("Invalid value for Botype", value, value.ToString());
				_botype = value;
			}
		}


		#endregion
		
	}

	#endregion
}



