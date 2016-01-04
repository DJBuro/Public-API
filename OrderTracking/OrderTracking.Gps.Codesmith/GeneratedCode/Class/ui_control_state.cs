
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Uicontrolstate

	/// <summary>
	/// Uicontrolstate object for NHibernate mapped table 'ui_control_state'.
	/// </summary>
	public partial  class Uicontrolstate
		{
		#region Member Variables
		
		protected string _state;
		protected Application _application = new Application();
		protected User _user = new User();
		
		

		#endregion

		#region Constructors

		public Uicontrolstate() { }

		public Uicontrolstate( string state, Application application, User user )
		{
			this._state = state;
			this._application = application;
			this._user = user;
		}

		#endregion

		#region Public Properties


		public string State
		{
			get { return _state; }
			set
			{
				if ( value != null && value.Length > 16)
					throw new ArgumentOutOfRangeException("Invalid value for State", value, value.ToString());
				_state = value;
			}
		}

		public Application Application
		{
			get { return _application; }
			set { _application = value; }
		}

		public User User
		{
			get { return _user; }
			set { _user = value; }
		}


		#endregion
		
	}

	#endregion
}



