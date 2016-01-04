
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Mapmetadata

	/// <summary>
	/// Mapmetadata object for NHibernate mapped table 'map_meta_data'.
	/// </summary>
	public partial  class Mapmetadata
		{
		#region Member Variables
		
		protected int? _id;
		protected string _name;
		protected string _value;
		
		

		#endregion

		#region Constructors

		public Mapmetadata() { }

		public Mapmetadata( string name, string value )
		{
			this._name = name;
			this._value = value;
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
				if ( value != null && value.Length > 255)
					throw new ArgumentOutOfRangeException("Invalid value for Name", value, value.ToString());
				_name = value;
			}
		}

		public string Value
		{
			get { return _value; }
			set
			{
				if ( value != null && value.Length > 255)
					throw new ArgumentOutOfRangeException("Invalid value for Value", value, value.ToString());
				_value = value;
			}
		}


		#endregion
		
	}

	#endregion
}



