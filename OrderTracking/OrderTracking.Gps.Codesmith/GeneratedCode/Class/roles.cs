
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Role

	/// <summary>
	/// Role object for NHibernate mapped table 'roles'.
	/// </summary>
	public partial  class Role
		{
		#region Member Variables
		
		protected int? _id;
		protected string _rolename;
		protected string _roledescription;
		protected string _botype;
		
		

		#endregion

		#region Constructors

		public Role() { }

		public Role( string rolename, string roledescription, string botype )
		{
			this._rolename = rolename;
			this._roledescription = roledescription;
			this._botype = botype;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public string Rolename
		{
			get { return _rolename; }
			set
			{
				if ( value != null && value.Length > 15)
					throw new ArgumentOutOfRangeException("Invalid value for Rolename", value, value.ToString());
				_rolename = value;
			}
		}

		public string Roledescription
		{
			get { return _roledescription; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for Roledescription", value, value.ToString());
				_roledescription = value;
			}
		}

		public string Botype
		{
			get { return _botype; }
			set
			{
				if ( value != null && value.Length > 64)
					throw new ArgumentOutOfRangeException("Invalid value for Botype", value, value.ToString());
				_botype = value;
			}
		}


		#endregion
		
	}

	#endregion
}



