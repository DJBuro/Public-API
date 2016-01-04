
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Protocol

	/// <summary>
	/// Protocol object for NHibernate mapped table 'protocol'.
	/// </summary>
	public partial  class Protocol
		{
		#region Member Variables
		
		protected string _id;
		protected string _botype;
		protected string _name;
		protected string _description;
		protected string _adapterbotype;
		
		

		#endregion

		#region Constructors

		public Protocol() { }

		public Protocol( string botype, string name, string description, string adapterbotype )
		{
			this._botype = botype;
			this._name = name;
			this._description = description;
			this._adapterbotype = adapterbotype;
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

		public string Adapterbotype
		{
			get { return _adapterbotype; }
			set
			{
				if ( value != null && value.Length > 64)
					throw new ArgumentOutOfRangeException("Invalid value for Adapterbotype", value, value.ToString());
				_adapterbotype = value;
			}
		}


		#endregion
		
	}

	#endregion
}



