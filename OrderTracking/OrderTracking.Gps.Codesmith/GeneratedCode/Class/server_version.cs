
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Serverversion

	/// <summary>
	/// Serverversion object for NHibernate mapped table 'server_version'.
	/// </summary>
	public partial  class Serverversion
		{
		#region Member Variables
		
		protected string _id;
		protected DateTime? _installed;
		
		

		#endregion

		#region Constructors

		public Serverversion() { }

		public Serverversion( DateTime? installed )
		{
			this._installed = installed;
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

		public DateTime? Installed
		{
			get { return _installed; }
			set { _installed = value; }
		}


		#endregion
		
	}

	#endregion
}



