
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Postprocessor

	/// <summary>
	/// Postprocessor object for NHibernate mapped table 'post_processor'.
	/// </summary>
	public partial  class Postprocessor
		{
		#region Member Variables
		
		protected int? _id;
		protected string _name;
		protected string _description;
		protected string _botype;
		protected Loadabletype _loadabletype = new Loadabletype();
		
		
		protected IList _trackpostprocessorlogs;
		protected IList _trackdatamods;
		protected IList _trackinfomods;

		#endregion

		#region Constructors

		public Postprocessor() { }

		public Postprocessor( string name, string description, string botype, Loadabletype loadabletype )
		{
			this._name = name;
			this._description = description;
			this._botype = botype;
			this._loadabletype = loadabletype;
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
				if ( value != null && value.Length > 32)
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

		public Loadabletype Loadabletype
		{
			get { return _loadabletype; }
			set { _loadabletype = value; }
		}

		public IList track_post_processor_logs
		{
			get
			{
				if (_trackpostprocessorlogs==null)
				{
					_trackpostprocessorlogs = new ArrayList();
				}
				return _trackpostprocessorlogs;
			}
			set { _trackpostprocessorlogs = value; }
		}

		public IList track_data_mods
		{
			get
			{
				if (_trackdatamods==null)
				{
					_trackdatamods = new ArrayList();
				}
				return _trackdatamods;
			}
			set { _trackdatamods = value; }
		}

		public IList track_info_mods
		{
			get
			{
				if (_trackinfomods==null)
				{
					_trackinfomods = new ArrayList();
				}
				return _trackinfomods;
			}
			set { _trackinfomods = value; }
		}


		#endregion
		
	}

	#endregion
}



