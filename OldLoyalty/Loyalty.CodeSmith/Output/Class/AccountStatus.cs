
using System;
using System.Collections;


namespace Loyalty.Dao.Domain
{
	#region AccountStatus

	/// <summary>
	/// AccountStatus object for NHibernate mapped table 'tbl_AccountStatus'.
	/// </summary>
	public partial  class AccountStatus
		{
		#region Member Variables
		
		protected int? _id;
		protected string _name;
		protected string _description;
		
		
		protected IList _accountStatusIdLoyaltyAccountStatuses;

		#endregion

		#region Constructors

		public AccountStatus() { }

		public AccountStatus( string name, string description )
		{
			this._name = name;
			this._description = description;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public string Name
		{
			get { return _name; }
			set
			{
				if ( value != null && value.Length > 20)
					throw new ArgumentOutOfRangeException("Invalid value for Name", value, value.ToString());
				_name = value;
			}
		}

		public string Description
		{
			get { return _description; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for Description", value, value.ToString());
				_description = value;
			}
		}

		public IList AccountStatusIdLoyaltyAccountStatuses
		{
			get
			{
				if (_accountStatusIdLoyaltyAccountStatuses==null)
				{
					_accountStatusIdLoyaltyAccountStatuses = new ArrayList();
				}
				return _accountStatusIdLoyaltyAccountStatuses;
			}
			set { _accountStatusIdLoyaltyAccountStatuses = value; }
		}


		#endregion
		
	}

	#endregion
}



