
using System;


namespace WebDashboard.Dao.Domain
{
	#region Definition

	/// <summary>
	/// Definition object for NHibernate mapped table 'tbl_Definition'.
	/// </summary>
	public class Definition : Entity.Entity
		{
		#region Member Variables
		
		protected int _columnNumber;
		protected string _name;
		protected string _shortName;
		
		
		//protected IList _definitionIdIndicators;

		#endregion

		#region Constructors

		public Definition() { }

		public Definition( int columnNumber, string name, string shortName)
		{
			this._columnNumber = columnNumber;
			this._name = name;
			this._shortName = shortName;
		}

		#endregion

		#region Public Properties

		public int ColumnNumber
		{
			get { return _columnNumber; }
			set { _columnNumber = value; }
		}

		public string Name
		{
			get { return _name; }
			set
			{
				if ( value != null && value.Length > 100)
					throw new ArgumentOutOfRangeException("Invalid value for Name", value, value.ToString());
				_name = value;
			}
		}

		public string ShortName
		{
			get { return _shortName; }
			set
			{
				if ( value != null && value.Length > 10)
					throw new ArgumentOutOfRangeException("Invalid value for ShortName", value, value.ToString());
				_shortName = value;
			}
		}
/*
		public IList DefinitionIdIndicators
		{
			get
			{
				if (_definitionIdIndicators==null)
				{
					_definitionIdIndicators = new ArrayList();
				}
				return _definitionIdIndicators;
			}
			set { _definitionIdIndicators = value; }
		}
        */

		#endregion
		
	}

	#endregion
}



