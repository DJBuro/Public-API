
using System;
using System.Collections;


namespace OrderTracking.Dao.Domain
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
		
		
		protected IList _statusIdOrderStatuses;

		#endregion

		#region Constructors

		public Status() { }

		public Status( string name )
		{
			this._name = name;
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
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for Name", value, value.ToString());
				_name = value;
			}
		}

		public IList StatusIdOrderStatuses
		{
			get
			{
				if (_statusIdOrderStatuses==null)
				{
					_statusIdOrderStatuses = new ArrayList();
				}
				return _statusIdOrderStatuses;
			}
			set { _statusIdOrderStatuses = value; }
		}


		#endregion
		
	}

	#endregion
}



