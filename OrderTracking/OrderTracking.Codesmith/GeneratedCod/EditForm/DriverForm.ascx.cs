
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Data
{
	#region Driver

	/// <summary>
	/// Driver form for NHibernate mapped table 'tbl_Driver'.
	/// </summary>
	public partial class DriverForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Dao.Domain.Driver _Driver;

			
		#endregion
		
		#region Properties

		public OrderTracking.Dao.Domain.Driver Driver
		{
			set { _Driver = value; }
			get { return _Driver; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Driver = new OrderTracking.Dao.Domain.Driver ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Driver = (OrderTracking.Dao.Domain.Driver)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Driver;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("NameText.Text","Driver.Name");
				BindingManager.AddBinding("ExternalDriverIdText.Text","Driver.ExternalDriverId");
				BindingManager.AddBinding("VehicleText.Text","Driver.Vehicle");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindDriverDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindDriverDropdownsAndDivSelectRegions();
		}


		
		
		private void BindDriverDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveDriver(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingDAO.Save(Driver);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




