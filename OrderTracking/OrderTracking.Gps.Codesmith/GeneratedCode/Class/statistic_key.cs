
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Statistickey

	/// <summary>
	/// Statistickey object for NHibernate mapped table 'statistic_key'.
	/// </summary>
	public partial  class Statistickey
		{
		#region Member Variables
		
		protected int? _id;
		protected string _name;
		protected string _type;
		
		

		#endregion

		#region Constructors

		public Statistickey() { }

		public Statistickey( string name, string type )
		{
			this._name = name;
			this._type = type;
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
				if ( value != null && value.Length > 45)
					throw new ArgumentOutOfRangeException("Invalid value for Name", value, value.ToString());
				_name = value;
			}
		}

		public string Type
		{
			get { return _type; }
			set
			{
				if ( value != null && value.Length > 45)
					throw new ArgumentOutOfRangeException("Invalid value for Type", value, value.ToString());
				_type = value;
			}
		}


		#endregion
		
	}

	#endregion
}



