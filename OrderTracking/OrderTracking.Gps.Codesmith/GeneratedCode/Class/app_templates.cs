
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Apptemplate

	/// <summary>
	/// Apptemplate object for NHibernate mapped table 'app_templates'.
	/// </summary>
	public partial  class Apptemplate
		{
		#region Member Variables
		
		protected int? _id;
		protected string _botype;
		protected string _name;
		protected string _description;
		protected Application _application = new Application();
		
		

		#endregion

		#region Constructors

		public Apptemplate() { }

		public Apptemplate( string botype, string name, string description, Application application )
		{
			this._botype = botype;
			this._name = name;
			this._description = description;
			this._application = application;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
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
				if ( value != null && value.Length > 255)
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

		public Application Application
		{
			get { return _application; }
			set { _application = value; }
		}


		#endregion
		
	}

	#endregion
}



