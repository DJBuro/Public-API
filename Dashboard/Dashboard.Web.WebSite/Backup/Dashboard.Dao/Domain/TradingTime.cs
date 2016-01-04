
using System;
using System.Collections;


namespace Dashboard.Dao.Domain
{
	#region TradingTime

	/// <summary>
	/// TradingTime object for NHibernate mapped table 'TradingTimes'.
	/// </summary>
	public class TradingTime : Entity
		{
		#region Member Variables
		
		protected int? _weekDayNumber;
		protected string _openingTime;
		protected string _closingTime;
		protected HeadOffice _headOffice;
		
		

		#endregion

		#region Constructors

		public TradingTime() { }

		public TradingTime( int? weekDayNumber, string openingTime, string closingTime, HeadOffice headOffice )
		{
			this._weekDayNumber = weekDayNumber;
			this._openingTime = openingTime;
			this._closingTime = closingTime;
			this._headOffice = headOffice;
		}

		#endregion

		#region Public Properties

		public int? WeekDayNumber
		{
			get { return _weekDayNumber; }
			set { _weekDayNumber = value; }
		}

		public string OpeningTime
		{
			get { return _openingTime; }
			set
			{
				if ( value != null && value.Length > 10)
					throw new ArgumentOutOfRangeException("Invalid value for OpeningTime", value, value.ToString());
				_openingTime = value;
			}
		}

		public string ClosingTime
		{
			get { return _closingTime; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for ClosingTime", value, value.ToString());
				_closingTime = value;
			}
		}

		public HeadOffice HeadOffice
		{
			get { return _headOffice; }
			set { _headOffice = value; }
		}


		#endregion
		
	}

	#endregion
}



