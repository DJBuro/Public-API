
using System;
using System.Collections;


namespace Dashboard.Dao.Domain
{
	#region InterfaceText

	/// <summary>
	/// InterfaceText object for NHibernate mapped table 'InterfaceText'.
	/// </summary>
	public class InterfaceText : Entity
		{
		#region Member Variables
		
		protected string _definition;
		
		
		protected IList _interfaceTextLangauges;

		#endregion

		#region Constructors

		public InterfaceText() { }

		public InterfaceText( string definition )
		{
			this._definition = definition;
		}

		#endregion

		#region Public Properties


		public string Definition
		{
			get { return _definition; }
			set
			{
				if ( value != null && value.Length > 100)
					throw new ArgumentOutOfRangeException("Invalid value for Definition", value, value.ToString());
				_definition = value;
			}
		}

		public IList InterfaceTextLangauges
		{
			get
			{
				if (_interfaceTextLangauges==null)
				{
					_interfaceTextLangauges = new ArrayList();
				}
				return _interfaceTextLangauges;
			}
			set { _interfaceTextLangauges = value; }
		}


		#endregion
		
	}

	#endregion
}



