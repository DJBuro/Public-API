
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Smsproxy

	/// <summary>
	/// Smsproxy object for NHibernate mapped table 'sms_proxy'.
	/// </summary>
	public partial  class Smsproxy
		{
		#region Member Variables
		
		protected int? _id;
		protected string _phonenumber;
		protected short _enabled;
		protected string _botype;
		protected User _ownerid = new User();
		
		

		#endregion

		#region Constructors

		public Smsproxy() { }

		public Smsproxy( string phonenumber, short enabled, string botype, User ownerid )
		{
			this._phonenumber = phonenumber;
			this._enabled = enabled;
			this._botype = botype;
			this._ownerid = ownerid;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public string Phonenumber
		{
			get { return _phonenumber; }
			set
			{
				if ( value != null && value.Length > 32)
					throw new ArgumentOutOfRangeException("Invalid value for Phonenumber", value, value.ToString());
				_phonenumber = value;
			}
		}

		public short Enabled
		{
			get { return _enabled; }
			set { _enabled = value; }
		}

		public string Botype
		{
			get { return _botype; }
			set
			{
				if ( value != null && value.Length > 128)
					throw new ArgumentOutOfRangeException("Invalid value for Botype", value, value.ToString());
				_botype = value;
			}
		}

		public User owner_id
		{
			get { return _ownerid; }
			set { _ownerid = value; }
		}


		#endregion
		
	}

	#endregion
}



