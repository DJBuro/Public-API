
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Rulechain

	/// <summary>
	/// Rulechain object for NHibernate mapped table 'rule_chain'.
	/// </summary>
	public partial  class Rulechain
		{
		#region Member Variables
		
		protected int? _id;
		protected string _name;
		protected string _description;
		protected string _botype;
		
		
		protected IList _recorderrules;
		protected IList _applicationrules;

		#endregion

		#region Constructors

		public Rulechain() { }

		public Rulechain( string name, string description, string botype )
		{
			this._name = name;
			this._description = description;
			this._botype = botype;
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

		public IList recorder_rules
		{
			get
			{
				if (_recorderrules==null)
				{
					_recorderrules = new ArrayList();
				}
				return _recorderrules;
			}
			set { _recorderrules = value; }
		}

		public IList application_rules
		{
			get
			{
				if (_applicationrules==null)
				{
					_applicationrules = new ArrayList();
				}
				return _applicationrules;
			}
			set { _applicationrules = value; }
		}


		#endregion
		
	}

	#endregion
}



