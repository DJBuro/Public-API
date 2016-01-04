
using System;
using System.Collections;


namespace WebDashboard.Dao.Domain
{
	#region ValueType

	/// <summary>
	/// ValueType object for NHibernate mapped table 'tbl_ValueType'.
	/// </summary>
    public class ValueType : Entity.Entity
		{
		#region Member Variables
		
		protected string _name;
		protected string _value;
		protected bool _isPrefix;
		
		
		protected IList _valueTypeIdIndicators;

		#endregion

		#region Constructors

		public ValueType() { }

		public ValueType( string name, string value, bool isPrefix )
		{
			this._name = name;
			this._value = value;
			this._isPrefix = isPrefix;
		}

		#endregion

		#region Public Properties

		public string Name
		{
			get { return _name; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for Name", value, value.ToString());
				_name = value;
			}
		}

		public string Value
		{
			get { return _value; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for Value", value, value.ToString());
				_value = value;
			}
		}

		public bool IsPrefix
		{
			get { return _isPrefix; }
			set { _isPrefix = value; }
		}

		public IList ValueTypeIdIndicators
		{
			get
			{
				if (_valueTypeIdIndicators==null)
				{
					_valueTypeIdIndicators = new ArrayList();
				}
				return _valueTypeIdIndicators;
			}
			set { _valueTypeIdIndicators = value; }
		}


		#endregion
		
	}

	#endregion
}



