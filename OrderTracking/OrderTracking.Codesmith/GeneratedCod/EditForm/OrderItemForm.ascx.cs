
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Data
{
	#region OrderItem

	/// <summary>
	/// OrderItem form for NHibernate mapped table 'tbl_OrderItem'.
	/// </summary>
	public partial class OrderItemForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Dao.Domain.OrderItem _OrderItem;

			
		#endregion
		
		#region Properties

		public OrderTracking.Dao.Domain.OrderItem OrderItem
		{
			set { _OrderItem = value; }
			get { return _OrderItem; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_OrderItem = new OrderTracking.Dao.Domain.OrderItem ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _OrderItem = (OrderTracking.Dao.Domain.OrderItem)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _OrderItem;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindOrderItemDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindOrderItemDropdownsAndDivSelectRegions();
		}


		
		
		private void BindOrderItemDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveOrderItem(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingDAO.Save(OrderItem);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




