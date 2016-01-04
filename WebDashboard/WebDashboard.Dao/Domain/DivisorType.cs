
using System;
using System.Collections;


namespace WebDashboard.Dao.Domain
{
	#region DivisorType

	/// <summary>
	/// DivisorType object for NHibernate mapped table 'tbl_DivisorType'.
	/// </summary>
	public class DivisorType : Entity.Entity
		{
		#region Member Variables
		
		protected string _name;
		protected string _description;
		
		
		//protected IList _divisorTypeIdIndicators;

		#endregion

		#region Constructors

		public DivisorType() { }

		public DivisorType( string name, string description )
		{
			this._name = name;
			this._description = description;
		}

		#endregion

		#region Public Properties

		public string Name
		{
			get { return _name; }
			set
			{
				if ( value != null && value.Length > 20)
					throw new ArgumentOutOfRangeException("Invalid value for Name", value, value.ToString());
				_name = value;
			}
		}

		public string Description
		{
			get { return _description; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for Description", value, value.ToString());
				_description = value;
			}
		}
        /*
		public IList DivisorTypeIdIndicators
		{
			get
			{
				if (_divisorTypeIdIndicators==null)
				{
					_divisorTypeIdIndicators = new ArrayList();
				}
				return _divisorTypeIdIndicators;
			}
			set { _divisorTypeIdIndicators = value; }
		}
        */

		#endregion
		
	}

	#endregion
}



