
using System;
using Cacd.Web.Common.Core;


namespace Loyalty.Data
{
	#region OrderItemHistory

	/// <summary>
	/// OrderItemHistory form for NHibernate mapped table 'tbl_OrderItemHistory'.
	/// </summary>
	public partial class OrderItemHistoryForm : BaseUserControl
		{
		#region Fields
		
		private Loyalty.Dao.Domain.OrderItemHistory _OrderItemHistory;

			
		#endregion
		
		#region Properties

		public Loyalty.Dao.Domain.OrderItemHistory OrderItemHistory
		{
			set { _OrderItemHistory = value; }
			get { return _OrderItemHistory; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_OrderItemHistory = new Loyalty.Dao.Domain.OrderItemHistory ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _OrderItemHistory = (Loyalty.Dao.Domain.OrderItemHistory)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _OrderItemHistory;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("NameText.Text","OrderItemHistory.Name");
				BindingManager.AddBinding("ItemPriceText.Text","OrderItemHistory.ItemPrice");
				BindingManager.AddBinding("ItemLoyaltyPointsText.Text","OrderItemHistory.ItemLoyaltyPoints");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindOrderItemHistoryDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindOrderItemHistoryDropdownsAndDivSelectRegions();
		}


		
		
		private void BindOrderItemHistoryDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveOrderItemHistory(object sender, EventArgs e)
			{
				if (Validate()){
					LoyaltyDAO.Save(OrderItemHistory);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




