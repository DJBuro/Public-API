
using System;
using System.Collections;


namespace Loyalty.Dao.Domain
{
	#region Status

	/// <summary>
	/// Status object for NHibernate mapped table 'tbl_Status'.
	/// </summary>
	public partial  class Status
		{
		#region Member Variables
		
		protected int? _id;
		protected string _name;
		protected string _description;
		
		
		protected IList _statusIdLoyaltyCardStatuses;

		#endregion

		#region Constructors

		public Status() { }

		public Status( string name, string description )
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

		public IList StatusIdLoyaltyCardStatuses
		{
			get
			{
				if (_statusIdLoyaltyCardStatuses==null)
				{
					_statusIdLoyaltyCardStatuses = new ArrayList();
				}
				return _statusIdLoyaltyCardStatuses;
			}
			set { _statusIdLoyaltyCardStatuses = value; }
		}


		#endregion
		
	}

	#endregion
}



