
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Applicationrule

	/// <summary>
	/// Applicationrule object for NHibernate mapped table 'application_rule'.
	/// </summary>
	public partial  class Applicationrule
		{
		#region Member Variables
		
		protected int? _id;
		protected string _name;
		protected string _description;
		protected short _enabled;
		protected int? _exeorder;
		protected string _botype;
		protected Loadabletype _loadabletype = new Loadabletype();
		protected Rulechain _rulechain = new Rulechain();
		
		

		#endregion

		#region Constructors

		public Applicationrule() { }

		public Applicationrule( string name, string description, short enabled, int? exeorder, string botype, Loadabletype loadabletype, Rulechain rulechain )
		{
			this._name = name;
			this._description = description;
			this._enabled = enabled;
			this._exeorder = exeorder;
			this._botype = botype;
			this._loadabletype = loadabletype;
			this._rulechain = rulechain;
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
				if ( value != null && value.Length > 128)
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

		public short Enabled
		{
			get { return _enabled; }
			set { _enabled = value; }
		}

		public int? Exeorder
		{
			get { return _exeorder; }
			set { _exeorder = value; }
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

		public Rulechain Rulechain
		{
			get { return _rulechain; }
			set { _rulechain = value; }
		}


		#endregion
		
	}

	#endregion
}



