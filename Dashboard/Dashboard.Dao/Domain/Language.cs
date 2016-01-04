
using System;
using System.Collections;


namespace Dashboard.Dao.Domain
{
	#region Language

	/// <summary>
	/// Language object for NHibernate mapped table 'Language'.
	/// </summary>
	public class Language : Entity
		{
		#region Member Variables
		
		protected string _languageName;
		
		protected IList _interfaceTextLangauges;
		protected IList _indicatorTranslationses;

		#endregion

		#region Constructors

		public Language() { }

		public Language( string languageName )
		{
			this._languageName = languageName;
		}

		#endregion

		#region Public Properties

		public string LanguageName
		{
			get { return _languageName; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for LanguageName", value, value.ToString());
				_languageName = value;
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

		public IList IndicatorTranslations
		{
			get
			{
				if (_indicatorTranslationses==null)
				{
					_indicatorTranslationses = new ArrayList();
				}
				return _indicatorTranslationses;
			}
			set { _indicatorTranslationses = value; }
		}


		#endregion
		
	}

	#endregion
}



