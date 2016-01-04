
using System;
using System.Collections;


namespace Dashboard.Dao.Domain
{
	#region InterfaceTextLangauge

	/// <summary>
	/// InterfaceTextLangauge object for NHibernate mapped table 'InterfaceTextLangauge'.
	/// </summary>
	public class InterfaceTextLangauge : Entity
		{
		#region Member Variables

		protected string _translation;
		protected Language _language;
		protected InterfaceText _interfaceText;
		
		
		protected IList _languageHeadOffices;

		#endregion

		#region Constructors

		public InterfaceTextLangauge() { }

		public InterfaceTextLangauge( string translation, Language language, InterfaceText interfaceText )
		{
			this._translation = translation;
			this._language = language;
			this._interfaceText = interfaceText;
		}

		#endregion

		#region Public Properties

		public string Translation
		{
			get { return _translation; }
			set
			{
				if ( value != null && value.Length > 100)
					throw new ArgumentOutOfRangeException("Invalid value for Translation", value, value.ToString());
				_translation = value;
			}
		}

		public Language Language
		{
			get { return _language; }
			set { _language = value; }
		}

		public InterfaceText InterfaceText
		{
			get { return _interfaceText; }
			set { _interfaceText = value; }
		}

		public IList LanguageHeadOffices
		{
			get
			{
				if (_languageHeadOffices==null)
				{
					_languageHeadOffices = new ArrayList();
				}
				return _languageHeadOffices;
			}
			set { _languageHeadOffices = value; }
		}


		#endregion
		
	}

	#endregion
}



