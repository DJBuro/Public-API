
using System;
using System.Collections;


namespace OrderTracking.Dao.Domain
{
	#region OrderStatus

	/// <summary>
	/// OrderStatus object for NHibernate mapped table 'tbl_OrderStatus'.
	/// </summary>
	public class OrderStatus : Entity.Entity
		{
		#region Member Variables
		
		protected string _processor;
		protected DateTime _time;
		protected Status _statusId;
		protected Order _orderId;
		protected DateTime? createdDateTime;
		protected DateTime? dispatchedDateTime;

		#endregion

		#region Constructors

		public OrderStatus() { }

		public OrderStatus( string processor, DateTime time, Status status, Order order )
		{
			this._processor = processor;
			this._time = time;
			this._statusId = status;
			this._orderId = order;
		}

		#endregion

		#region Public Properties

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

		public DateTime Time
		{
			get { return _time; }
			set { _time = value; }
		}

		public Status Status
		{
			get { return _statusId; }
			set { _statusId = value; }
		}

		public Order Order
		{
			get { return _orderId; }
			set { _orderId = value; }
		}

        public DateTime? CreatedDateTime
        {
            get { return this.createdDateTime; }
            set { this.createdDateTime = value; }
        }

        public DateTime? DispatchedDateTime
        {
            get { return this.dispatchedDateTime; }
            set { this.dispatchedDateTime = value; }
        }

		#endregion
		
	}

	#endregion
}



