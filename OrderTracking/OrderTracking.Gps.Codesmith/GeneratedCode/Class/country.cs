
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Country

	/// <summary>
	/// Country object for NHibernate mapped table 'country'.
	/// </summary>
	public partial  class Country
		{
		#region Member Variables
		
		protected string _id;
		
		
		protected IList _mobilenetworks;

		#endregion

		#region Constructors

		public Country() { }

		public Country(  )
		{
		}

		#endregion

		#region Public Properties

		public string Id
		{
			get {return _id;}
			set
			{
				if ( value != null && value.Length > 64)
					throw new ArgumentOutOfRangeException("Invalid value for Id", value, value.ToString());
				_id = value;
			}
		}

		public IList mobile_networks
		{
			get
			{
				if (_mobilenetworks==null)
				{
					_mobilenetworks = new ArrayList();
				}
				return _mobilenetworks;
			}
			set { _mobilenetworks = value; }
		}


		#endregion
		
	}

	#endregion
}



