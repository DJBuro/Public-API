
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Gateeventexpressionstate

	/// <summary>
	/// Gateeventexpressionstate object for NHibernate mapped table 'gate_event_expression_state'.
	/// </summary>
	public partial  class Gateeventexpressionstate
		{
		#region Member Variables
		
		protected int? _id;
		protected string _customstate;
		protected Gateeventexpression _gateeventexpression = new Gateeventexpression();
		protected User _user = new User();
		
		

		#endregion

		#region Constructors

		public Gateeventexpressionstate() { }

		public Gateeventexpressionstate( string customstate, Gateeventexpression gateeventexpression, User user )
		{
			this._customstate = customstate;
			this._gateeventexpression = gateeventexpression;
			this._user = user;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public string Customstate
		{
			get { return _customstate; }
			set
			{
				if ( value != null && value.Length > 255)
					throw new ArgumentOutOfRangeException("Invalid value for Customstate", value, value.ToString());
				_customstate = value;
			}
		}

		public Gateeventexpression Gateeventexpression
		{
			get { return _gateeventexpression; }
			set { _gateeventexpression = value; }
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



