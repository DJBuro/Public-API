
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Data
{
	#region OrderStatus

	/// <summary>
	/// OrderStatus form for NHibernate mapped table 'tbl_OrderStatus'.
	/// </summary>
	public partial class OrderStatusForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Dao.Domain.OrderStatus _OrderStatus;

			
		#endregion
		
		#region Properties

		public OrderTracking.Dao.Domain.OrderStatus OrderStatus
		{
			set { _OrderStatus = value; }
			get { return _OrderStatus; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_OrderStatus = new OrderTracking.Dao.Domain.OrderStatus ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _OrderStatus = (OrderTracking.Dao.Domain.OrderStatus)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _OrderStatus;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("ProcessorText.Text","OrderStatus.Processor");
				BindingManager.AddBinding("TimeText.Text","OrderStatus.Time");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindOrderStatusDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindOrderStatusDropdownsAndDivSelectRegions();
		}


		
		
		private void BindOrderStatusDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveOrderStatus(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingDAO.Save(OrderStatus);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




