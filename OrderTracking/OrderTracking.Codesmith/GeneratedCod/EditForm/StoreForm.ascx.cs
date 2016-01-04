
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Data
{
	#region Store

	/// <summary>
	/// Store form for NHibernate mapped table 'tbl_Store'.
	/// </summary>
	public partial class StoreForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Dao.Domain.Store _Store;

			
		#endregion
		
		#region Properties

		public OrderTracking.Dao.Domain.Store Store
		{
			set { _Store = value; }
			get { return _Store; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Store = new OrderTracking.Dao.Domain.Store ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Store = (OrderTracking.Dao.Domain.Store)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Store;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("ExternalStoreIdText.Text","Store.ExternalStoreId");
				BindingManager.AddBinding("NameText.Text","Store.Name");
				BindingManager.AddBinding("DeliveryRadiusText.Text","Store.DeliveryRadius");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindStoreDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindStoreDropdownsAndDivSelectRegions();
		}


		
		
		private void BindStoreDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveStore(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingDAO.Save(Store);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




