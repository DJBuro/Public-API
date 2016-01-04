
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Data
{
	#region DriverOrder

	/// <summary>
	/// DriverOrder form for NHibernate mapped table 'tbl_DriverOrders'.
	/// </summary>
	public partial class DriverOrderForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Dao.Domain.DriverOrder _DriverOrder;

			
		#endregion
		
		#region Properties

		public OrderTracking.Dao.Domain.DriverOrder DriverOrder
		{
			set { _DriverOrder = value; }
			get { return _DriverOrder; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_DriverOrder = new OrderTracking.Dao.Domain.DriverOrder ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _DriverOrder = (OrderTracking.Dao.Domain.DriverOrder)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _DriverOrder;
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
				BindDriverOrderDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindDriverOrderDropdownsAndDivSelectRegions();
		}


		
		
		private void BindDriverOrderDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveDriverOrder(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingDAO.Save(DriverOrder);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




