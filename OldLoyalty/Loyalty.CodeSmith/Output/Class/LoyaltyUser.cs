
using System;
using System.Collections;


namespace Loyalty.Dao.Domain
{
	#region LoyaltyUser

	/// <summary>
	/// LoyaltyUser object for NHibernate mapped table 'tbl_LoyaltyUser'.
	/// </summary>
	public partial  class LoyaltyUser
		{
		#region Member Variables
		
		protected int? _id;
		protected string _firstName;
		protected string _middleInitial;
		protected string _surName;
		protected DateTime? _dateTimeCreated;
		protected string _emailAddress;
		protected string _password;
		protected LoyaltyAccount _loyaltyAccountId = new LoyaltyAccount();
		protected UserTitle _userTitleId = new UserTitle();
		
		

		#endregion

		#region Constructors

		public LoyaltyUser() { }

		public LoyaltyUser( string firstName, string middleInitial, string surName, DateTime? dateTimeCreated, string emailAddress, string password, LoyaltyAccount loyaltyAccountId, UserTitle userTitleId )
		{
			this._firstName = firstName;
			this._middleInitial = middleInitial;
			this._surName = surName;
			this._dateTimeCreated = dateTimeCreated;
			this._emailAddress = emailAddress;
			this._password = password;
			this._loyaltyAccountId = loyaltyAccountId;
			this._userTitleId = userTitleId;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public string FirstName
		{
			get { return _firstName; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for FirstName", value, value.ToString());
				_firstName = value;
			}
		}

		public string MiddleInitial
		{
			get { return _middleInitial; }
			set
			{
				if ( value != null && value.Length > 1)
					throw new ArgumentOutOfRangeException("Invalid value for MiddleInitial", value, value.ToString());
				_middleInitial = value;
			}
		}

		public string SurName
		{
			get { return _surName; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for SurName", value, value.ToString());
				_surName = value;
			}
		}

		public DateTime? DateTimeCreated
		{
			get { return _dateTimeCreated; }
			set { _dateTimeCreated = value; }
		}

		public string EmailAddress
		{
			get { return _emailAddress; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for EmailAddress", value, value.ToString());
				_emailAddress = value;
			}
		}

		public string Password
		{
			get { return _password; }
			set
			{
				if ( value != null && value.Length > 20)
					throw new ArgumentOutOfRangeException("Invalid value for Password", value, value.ToString());
				_password = value;
			}
		}

		public LoyaltyAccount LoyaltyAccountId
		{
			get { return _loyaltyAccountId; }
			set { _loyaltyAccountId = value; }
		}

		public UserTitle UserTitleId
		{
			get { return _userTitleId; }
			set { _userTitleId = value; }
		}


		#endregion
		
	}

	#endregion
}



