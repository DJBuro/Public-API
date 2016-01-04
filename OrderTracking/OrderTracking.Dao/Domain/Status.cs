
using System;
using System.Collections;


namespace OrderTracking.Dao.Domain
{
	#region Status

	/// <summary>
	/// Status object for NHibernate mapped table 'tbl_Status'.
	/// </summary>
    public class Status : Entity.Entity
		{
		#region Member Variables
		
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

        //public IList OrderStatus
        //{
        //    get
        //    {
        //        if (_statusIdOrderStatuses==null)
        //        {
        //            _statusIdOrderStatuses = new ArrayList();
        //        }
        //        return _statusIdOrderStatuses;
        //    }
        //    set { _statusIdOrderStatuses = value; }
        //}


		#endregion
		
	}

	#endregion
}



