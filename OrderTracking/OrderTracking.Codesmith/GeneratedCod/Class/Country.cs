
using System;
using System.Collections;


namespace OrderTracking.Dao.Domain
{
	#region Country

	/// <summary>
	/// Country object for NHibernate mapped table 'tbl_Country'.
	/// </summary>
	public partial  class Country
		{
		#region Member Variables
		
		protected long? _id;
		protected string _name;
		
		
		protected IList _countryIdApns;

		#endregion

		#region Constructors

		public Country() { }

		public Country( string name )
		{
			this._name = name;
		}

		#endregion

		#region Public Properties

		public long? Id
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

		public IList CountryIdApns
		{
			get
			{
				if (_countryIdApns==null)
				{
					_countryIdApns = new ArrayList();
				}
				return _countryIdApns;
			}
			set { _countryIdApns = value; }
		}


		#endregion
		
	}

	#endregion
}



