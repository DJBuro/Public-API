
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Data
{
	#region Order

	/// <summary>
	/// Order form for NHibernate mapped table 'tbl_Order'.
	/// </summary>
	public partial class OrderForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Dao.Domain.Order _Order;

			
		#endregion
		
		#region Properties

		public OrderTracking.Dao.Domain.Order Order
		{
			set { _Order = value; }
			get { return _Order; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Order = new OrderTracking.Dao.Domain.Order ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Order = (OrderTracking.Dao.Domain.Order)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Order;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("ExternalOrderIdText.Text","Order.ExternalOrderId");
				BindingManager.AddBinding("NameText.Text","Order.Name");
				BindingManager.AddBinding("TicketNumberText.Text","Order.TicketNumber");
				BindingManager.AddBinding("ExtraInformationText.Text","Order.ExtraInformation");
				BindingManager.AddBinding("ProximityDeliveredText.Text","Order.ProximityDelivered");
				BindingManager.AddBinding("CreatedText.Text","Order.Created");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindOrderDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindOrderDropdownsAndDivSelectRegions();
		}


		
		
		private void BindOrderDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveOrder(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingDAO.Save(Order);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




