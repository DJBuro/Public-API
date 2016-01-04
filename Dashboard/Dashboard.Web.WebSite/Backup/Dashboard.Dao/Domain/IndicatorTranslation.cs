
using System;
using System.Collections;


namespace Dashboard.Dao.Domain
{
	#region IndicatorTranslation

	/// <summary>
	/// IndicatorTranslation object for NHibernate mapped table 'IndicatorTranslations'.
	/// </summary>
	public class IndicatorTranslation : Entity
		{
		#region Member Variables
		
		protected string _translatedIndicatorName;
		protected Language _language;
		protected IndicatorDefinition _indicatorDefinition;

		#endregion

		#region Constructors

		public IndicatorTranslation() { }

		public IndicatorTranslation( string translatedIndicatorName, Language language, IndicatorDefinition indicatorDefinition )
		{
			this._translatedIndicatorName = translatedIndicatorName;
			this._language = language;
			this._indicatorDefinition = indicatorDefinition;
		}

		#endregion

		#region Public Properties

        public string TranslatedIndicatorName
		{
			get { return _translatedIndicatorName; }
			set
			{
				if ( value != null && value.Length > 100)
					throw new ArgumentOutOfRangeException("Invalid value for TranslatedIndicatorName", value, value.ToString());
				_translatedIndicatorName = value;
			}
		}

		public Language Language
		{
			get { return _language; }
			set { _language = value; }
		}

		public IndicatorDefinition IndicatorDefinition
		{
			get { return _indicatorDefinition; }
			set { _indicatorDefinition = value; }
		}


		#endregion
		
	}

	#endregion
}



