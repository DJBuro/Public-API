
using System;
using System.Collections;


namespace OrderTracking.Dao.Domain
{
	#region OrderStatus

	/// <summary>
	/// OrderStatus object for NHibernate mapped table 'tbl_OrderStatus'.
	/// </summary>
	public partial  class OrderStatus
		{
		#region Member Variables
		
		protected int? _id;
		protected string _processor;
		protected DateTime? _time;
		protected Status _statusId = new Status();
		protected Order _orderId = new Order();
		
		

		#endregion

		#region Constructors

		public OrderStatus() { }

		public OrderStatus( string processor, DateTime? time, Status statusId, Order orderId )
		{
			this._processor = processor;
			this._time = time;
			this._statusId = statusId;
			this._orderId = orderId;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public string Processor
		{
			get { return _processor; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for Processor", value, value.ToString());
				_processor = value;
			}
		}

		public DateTime? Time
		{
			get { return _time; }
			set { _time = value; }
		}

		public Status StatusId
		{
			get { return _statusId; }
			set { _statusId = value; }
		}

		public Order OrderId
		{
			get { return _orderId; }
			set { _orderId = value; }
		}


		#endregion
		
	}

	#endregion
}



