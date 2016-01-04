
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Data
{
	#region Customer

	/// <summary>
	/// Customer form for NHibernate mapped table 'tbl_Customer'.
	/// </summary>
	public partial class CustomerForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Dao.Domain.Customer _Customer;

			
		#endregion
		
		#region Properties

		public OrderTracking.Dao.Domain.Customer Customer
		{
			set { _Customer = value; }
			get { return _Customer; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Customer = new OrderTracking.Dao.Domain.Customer ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Customer = (OrderTracking.Dao.Domain.Customer)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Customer;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("ExternalIdText.Text","Customer.ExternalId");
				BindingManager.AddBinding("NameText.Text","Customer.Name");
				BindingManager.AddBinding("CredentialsText.Text","Customer.Credentials");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindCustomerDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindCustomerDropdownsAndDivSelectRegions();
		}


		
		
		private void BindCustomerDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveCustomer(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingDAO.Save(Customer);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




