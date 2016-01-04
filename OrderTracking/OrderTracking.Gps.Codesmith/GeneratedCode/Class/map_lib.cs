
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Maplib

	/// <summary>
	/// Maplib object for NHibernate mapped table 'map_lib'.
	/// </summary>
	public partial  class Maplib
		{
		#region Member Variables
		
		protected int? _id;
		protected string _name;
		protected string _botype;
		protected DateTime? _created;
		
		
		protected Mapsgroup _mapsgroup;

		#endregion

		#region Constructors

		public Maplib() { }

		public Maplib( string name, string botype, DateTime? created )
		{
			this._name = name;
			this._botype = botype;
			this._created = created;
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

		public string Botype
		{
			get { return _botype; }
			set
			{
				if ( value != null && value.Length > 128)
					throw new ArgumentOutOfRangeException("Invalid value for Botype", value, value.ToString());
				_botype = value;
			}
		}

		public DateTime? Created
		{
			get { return _created; }
			set { _created = value; }
		}

		public Mapsgroup Mapsgroup
		{
			get { return _mapsgroup; }
			set { _mapsgroup = value; }
		}


		#endregion
		
	}

	#endregion
}



