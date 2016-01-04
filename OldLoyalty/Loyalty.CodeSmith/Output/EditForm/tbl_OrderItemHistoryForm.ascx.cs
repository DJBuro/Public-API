
using System;
using Cacd.Web.Common.Core;


namespace Loyalty.Data
{
	#region TblOrderItemHistory

	/// <summary>
	/// TblOrderItemHistory form for NHibernate mapped table 'tbl_OrderItemHistory'.
	/// </summary>
	public partial class TblOrderItemHistoryForm : BaseUserControl
		{
		#region Fields
		
		private Loyalty.Dao.Domain.TblOrderItemHistory _TblOrderItemHistory;

			
		#endregion
		
		#region Properties

		public Loyalty.Dao.Domain.TblOrderItemHistory TblOrderItemHistory
		{
			set { _TblOrderItemHistory = value; }
			get { return _TblOrderItemHistory; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_TblOrderItemHistory = new Loyalty.Dao.Domain.TblOrderItemHistory ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _TblOrderItemHistory = (Loyalty.Dao.Domain.TblOrderItemHistory)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _TblOrderItemHistory;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("NameText.Text","TblOrderItemHistory.Name");
				BindingManager.AddBinding("ItemPriceText.Text","TblOrderItemHistory.ItemPrice");
				BindingManager.AddBinding("ItemLoyaltyPointsText.Text","TblOrderItemHistory.ItemLoyaltyPoints");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindTblOrderItemHistoryDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindTblOrderItemHistoryDropdownsAndDivSelectRegions();
		}


		
		
		private void BindTblOrderItemHistoryDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveTblOrderItemHistory(object sender, EventArgs e)
			{
				if (Validate()){
					LoyaltyDAO.Save(TblOrderItemHistory);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




