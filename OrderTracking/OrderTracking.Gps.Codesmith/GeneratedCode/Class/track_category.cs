
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Trackcategory

	/// <summary>
	/// Trackcategory object for NHibernate mapped table 'track_category'.
	/// </summary>
	public partial  class Trackcategory
		{
		#region Member Variables
		
		protected string _id;
		protected string _botype;
		protected string _name;
		protected string _description;
		
		
		protected IList _trackinfos;

		#endregion

		#region Constructors

		public Trackcategory() { }

		public Trackcategory( string botype, string name, string description )
		{
			this._botype = botype;
			this._name = name;
			this._description = description;
		}

		#endregion

		#region Public Properties

		public string Id
		{
			get {return _id;}
			set
			{
				if ( value != null && value.Length > 16)
					throw new ArgumentOutOfRangeException("Invalid value for Id", value, value.ToString());
				_id = value;
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

		public string Name
		{
			get { return _name; }
			set
			{
				if ( value != null && value.Length > 64)
					throw new ArgumentOutOfRangeException("Invalid value for Name", value, value.ToString());
				_name = value;
			}
		}

		public string Description
		{
			get { return _description; }
			set
			{
				if ( value != null && value.Length > 255)
					throw new ArgumentOutOfRangeException("Invalid value for Description", value, value.ToString());
				_description = value;
			}
		}

		public IList track_infos
		{
			get
			{
				if (_trackinfos==null)
				{
					_trackinfos = new ArrayList();
				}
				return _trackinfos;
			}
			set { _trackinfos = value; }
		}


		#endregion
		
	}

	#endregion
}



