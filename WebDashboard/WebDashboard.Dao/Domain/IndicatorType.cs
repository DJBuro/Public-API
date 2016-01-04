
using System;

namespace WebDashboard.Dao.Domain
{
	#region IndicatorType

	/// <summary>
	/// IndicatorType object for NHibernate mapped table 'tbl_IndicatorType'.
	/// </summary>
    public class IndicatorType : Entity.Entity
		{
		#region Member Variables
		
        protected string _name;
		protected string _description;

        //protected IList _indicatorTypeIdIndicators;

		#endregion

		#region Constructors

		public IndicatorType() { }

		public IndicatorType( string name, string description )
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
		public IList IndicatorTypeIdIndicators
		{
			get
			{
				if (_indicatorTypeIdIndicators==null)
				{
					_indicatorTypeIdIndicators = new ArrayList();
				}
				return _indicatorTypeIdIndicators;
			}
			set { _indicatorTypeIdIndicators = value; }
		}
        */

		#endregion
		
	}

	#endregion
}



