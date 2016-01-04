
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Version

	/// <summary>
	/// Version object for NHibernate mapped table 'version'.
	/// </summary>
	public partial  class Version
		{
		#region Member Variables
		
		protected string _moduledescription;
		protected DateTime? _created;
		
		

		#endregion

		#region Constructors

		public Version() { }

		public Version( string moduledescription, DateTime? created )
		{
			this._moduledescription = moduledescription;
			this._created = created;
		}

		#endregion

		#region Public Properties


		public string Moduledescription
		{
			get { return _moduledescription; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for Moduledescription", value, value.ToString());
				_moduledescription = value;
			}
		}

		public DateTime? Created
		{
			get { return _created; }
			set { _created = value; }
		}


		#endregion
		
	}

	#endregion
}



