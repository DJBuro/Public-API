
using System;
using System.Collections;


namespace Loyalty.Dao.Domain
{
	#region UserTitle

	/// <summary>
	/// UserTitle object for NHibernate mapped table 'tbl_UserTitle'.
	/// </summary>
	public partial  class UserTitle
		{
		#region Member Variables
		
		protected int? _id;
		protected string _title;
		
		
		protected IList _userTitleIdLoyaltyUsers;
		protected IList _userTitleIdCompanyUserTitleses;

		#endregion

		#region Constructors

		public UserTitle() { }

		public UserTitle( string title )
		{
			this._title = title;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public string Title
		{
			get { return _title; }
			set
			{
				if ( value != null && value.Length > 5)
					throw new ArgumentOutOfRangeException("Invalid value for Title", value, value.ToString());
				_title = value;
			}
		}

		public IList UserTitleIdLoyaltyUsers
		{
			get
			{
				if (_userTitleIdLoyaltyUsers==null)
				{
					_userTitleIdLoyaltyUsers = new ArrayList();
				}
				return _userTitleIdLoyaltyUsers;
			}
			set { _userTitleIdLoyaltyUsers = value; }
		}

		public IList UserTitleIdCompanyUserTitleses
		{
			get
			{
				if (_userTitleIdCompanyUserTitleses==null)
				{
					_userTitleIdCompanyUserTitleses = new ArrayList();
				}
				return _userTitleIdCompanyUserTitleses;
			}
			set { _userTitleIdCompanyUserTitleses = value; }
		}


		#endregion
		
	}

	#endregion
}



