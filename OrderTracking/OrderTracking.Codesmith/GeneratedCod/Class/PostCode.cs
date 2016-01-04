
using System;
using System.Collections;


namespace OrderTracking.Dao.Domain
{
	#region PostCode

	/// <summary>
	/// PostCode object for NHibernate mapped table 'tbl_PostCode'.
	/// </summary>
	public partial  class PostCode
		{
		#region Member Variables
		
		protected long? _id;
		protected string _postCode;
		protected long? _del1;
		protected double? _longitude;
		protected double? _latitude;
		protected string _del2;
		protected string _del3;
		protected string _del4;
		protected string _del5;
		protected string _del6;
		protected string _del7;
		
		

		#endregion

		#region Constructors

		public PostCode() { }

		public PostCode( string postCode, long? del1, double? longitude, double? latitude, string del2, string del3, string del4, string del5, string del6, string del7 )
		{
			this._postCode = postCode;
			this._del1 = del1;
			this._longitude = longitude;
			this._latitude = latitude;
			this._del2 = del2;
			this._del3 = del3;
			this._del4 = del4;
			this._del5 = del5;
			this._del6 = del6;
			this._del7 = del7;
		}

		#endregion

		#region Public Properties

		public long? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public string PostCode
		{
			get { return _postCode; }
			set
			{
				if ( value != null && value.Length > 12)
					throw new ArgumentOutOfRangeException("Invalid value for PostCode", value, value.ToString());
				_postCode = value;
			}
		}

		public long? Del1
		{
			get { return _del1; }
			set { _del1 = value; }
		}

		public double? Longitude
		{
			get { return _longitude; }
			set { _longitude = value; }
		}

		public double? Latitude
		{
			get { return _latitude; }
			set { _latitude = value; }
		}

		public string Del2
		{
			get { return _del2; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for Del2", value, value.ToString());
				_del2 = value;
			}
		}

		public string Del3
		{
			get { return _del3; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for Del3", value, value.ToString());
				_del3 = value;
			}
		}

		public string Del4
		{
			get { return _del4; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for Del4", value, value.ToString());
				_del4 = value;
			}
		}

		public string Del5
		{
			get { return _del5; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for Del5", value, value.ToString());
				_del5 = value;
			}
		}

		public string Del6
		{
			get { return _del6; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for Del6", value, value.ToString());
				_del6 = value;
			}
		}

		public string Del7
		{
			get { return _del7; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for Del7", value, value.ToString());
				_del7 = value;
			}
		}


		#endregion
		
	}

	#endregion
}



