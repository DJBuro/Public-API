
using System;
using System.Collections;


namespace AndroAdmin.Dao.Domain
{
	#region Language

	/// <summary>
	/// Language object for NHibernate mapped table 'tbl_Language'.
	/// </summary>
	public class Language : Entity.Entity
		{
		#region Member Variables

		protected string _name;
		protected string _code;
		
		
		protected IList _languageIdAndroUsers;

		#endregion

		#region Constructors

		public Language() { }

		public Language( string name, string code )
		{
			this._name = name;
			this._code = code;
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

		public string Code
		{
			get { return _code; }
			set
			{
				if ( value != null && value.Length > 2)
					throw new ArgumentOutOfRangeException("Invalid value for Code", value, value.ToString());
				_code = value;
			}
		}
/*
		public IList LanguageIdAndroUsers
		{
			get
			{
				if (_languageIdAndroUsers==null)
				{
					_languageIdAndroUsers = new ArrayList();
				}
				return _languageIdAndroUsers;
			}
			set { _languageIdAndroUsers = value; }
		}

        */
		#endregion
		
	}

	#endregion
}



