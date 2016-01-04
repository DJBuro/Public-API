
using System;
using System.Collections;


namespace Dashboard.Dao.Domain
{
	#region DashboardData

	/// <summary>
	/// DashboardData object for NHibernate mapped table 'DashboardData'.
	/// </summary>
	public class DashboardData : Entity
		{
		#region Member Variables
		
		protected DateTime? _lastUpdated;
		protected int? _column1;
		protected int? _column2;
		protected int? _column3;
		protected int? _column4;
		protected int? _column5;
		protected int? _column6;
		protected int? _column7;
		protected int? _column8;
		protected int? _column9;
		protected int? _column10;
		protected int? _column11;
		protected int? _column12;
		protected int? _column13;
		protected int? _column14;
		protected int? _column15;
		protected int? _column16;
		protected int? _column17;
		protected int? _column18;
		protected int? _column19;
		protected int? _column20;
		protected Site _site;
		
		

		#endregion

		#region Constructors

		public DashboardData() { }

		public DashboardData( DateTime? lastUpdated, int? column1, int? column2, int? column3, int? column4, int? column5, int? column6, int? column7, int? column8, int? column9, int? column10, int? column11, int? column12, int? column13, int? column14, int? column15, int? column16, int? column17, int? column18, int? column19, int? column20, Site site )
		{
			this._lastUpdated = lastUpdated;
			this._column1 = column1;
			this._column2 = column2;
			this._column3 = column3;
			this._column4 = column4;
			this._column5 = column5;
			this._column6 = column6;
			this._column7 = column7;
			this._column8 = column8;
			this._column9 = column9;
			this._column10 = column10;
			this._column11 = column11;
			this._column12 = column12;
			this._column13 = column13;
			this._column14 = column14;
			this._column15 = column15;
			this._column16 = column16;
			this._column17 = column17;
			this._column18 = column18;
			this._column19 = column19;
			this._column20 = column20;
			this._site = site;
		}

		#endregion

		#region Public Properties

		public DateTime? LastUpdated
		{
			get { return _lastUpdated; }
			set { _lastUpdated = value; }
		}

		public int? Column1
		{
			get { return _column1; }
			set { _column1 = value; }
		}

		public int? Column2
		{
			get { return _column2; }
			set { _column2 = value; }
		}

		public int? Column3
		{
			get { return _column3; }
			set { _column3 = value; }
		}

		public int? Column4
		{
			get { return _column4; }
			set { _column4 = value; }
		}

		public int? Column5
		{
			get { return _column5; }
			set { _column5 = value; }
		}

		public int? Column6
		{
			get { return _column6; }
			set { _column6 = value; }
		}

		public int? Column7
		{
			get { return _column7; }
			set { _column7 = value; }
		}

		public int? Column8
		{
			get { return _column8; }
			set { _column8 = value; }
		}

		public int? Column9
		{
			get { return _column9; }
			set { _column9 = value; }
		}

		public int? Column10
		{
			get { return _column10; }
			set { _column10 = value; }
		}

		public int? Column11
		{
			get { return _column11; }
			set { _column11 = value; }
		}

		public int? Column12
		{
			get { return _column12; }
			set { _column12 = value; }
		}

		public int? Column13
		{
			get { return _column13; }
			set { _column13 = value; }
		}

		public int? Column14
		{
			get { return _column14; }
			set { _column14 = value; }
		}

		public int? Column15
		{
			get { return _column15; }
			set { _column15 = value; }
		}

		public int? Column16
		{
			get { return _column16; }
			set { _column16 = value; }
		}

		public int? Column17
		{
			get { return _column17; }
			set { _column17 = value; }
		}

		public int? Column18
		{
			get { return _column18; }
			set { _column18 = value; }
		}

		public int? Column19
		{
			get { return _column19; }
			set { _column19 = value; }
		}

		public int? Column20
		{
			get { return _column20; }
			set { _column20 = value; }
		}

		public Site Site
		{
			get { return _site; }
			set { _site = value; }
		}


		#endregion
		
	}

	#endregion
}



