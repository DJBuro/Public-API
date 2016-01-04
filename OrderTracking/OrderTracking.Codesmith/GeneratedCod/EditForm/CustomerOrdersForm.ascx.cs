
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Data
{
	#region CustomerOrder

	/// <summary>
	/// CustomerOrder form for NHibernate mapped table 'tbl_CustomerOrders'.
	/// </summary>
	public partial class CustomerOrderForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Dao.Domain.CustomerOrder _CustomerOrder;

			
		#endregion
		
		#region Properties

		public OrderTracking.Dao.Domain.CustomerOrder CustomerOrder
		{
			set { _CustomerOrder = value; }
			get { return _CustomerOrder; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_CustomerOrder = new OrderTracking.Dao.Domain.CustomerOrder ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _CustomerOrder = (OrderTracking.Dao.Domain.CustomerOrder)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _CustomerOrder;
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
				BindCustomerOrderDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindCustomerOrderDropdownsAndDivSelectRegions();
		}


		
		
		private void BindCustomerOrderDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveCustomerOrder(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingDAO.Save(CustomerOrder);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




