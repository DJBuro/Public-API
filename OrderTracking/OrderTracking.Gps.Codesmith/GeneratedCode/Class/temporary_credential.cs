
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Temporarycredential

	/// <summary>
	/// Temporarycredential object for NHibernate mapped table 'temporary_credential'.
	/// </summary>
	public partial  class Temporarycredential
		{
		#region Member Variables
		
		protected int? _id;
		protected DateTime? _expire;
		protected string _botype;
		protected User _user = new User();
		protected Application _application = new Application();
		
		

		#endregion

		#region Constructors

		public Temporarycredential() { }

		public Temporarycredential( DateTime? expire, string botype, User user, Application application )
		{
			this._expire = expire;
			this._botype = botype;
			this._user = user;
			this._application = application;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public DateTime? Expire
		{
			get { return _expire; }
			set { _expire = value; }
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

		public User User
		{
			get { return _user; }
			set { _user = value; }
		}

		public Application Application
		{
			get { return _application; }
			set { _application = value; }
		}


		#endregion
		
	}

	#endregion
}



