
using System;
using Cacd.Web.Common.Core;


namespace Loyalty.Data
{
	#region TblTransactionHistory

	/// <summary>
	/// TblTransactionHistory form for NHibernate mapped table 'tbl_TransactionHistory'.
	/// </summary>
	public partial class TblTransactionHistoryForm : BaseUserControl
		{
		#region Fields
		
		private Loyalty.Dao.Domain.TblTransactionHistory _TblTransactionHistory;

			
		#endregion
		
		#region Properties

		public Loyalty.Dao.Domain.TblTransactionHistory TblTransactionHistory
		{
			set { _TblTransactionHistory = value; }
			get { return _TblTransactionHistory; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_TblTransactionHistory = new Loyalty.Dao.Domain.TblTransactionHistory ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _TblTransactionHistory = (Loyalty.Dao.Domain.TblTransactionHistory)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _TblTransactionHistory;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("DateTimeOrderedText.Text","TblTransactionHistory.DateTimeOrdered");
				BindingManager.AddBinding("OrderIdText.Text","TblTransactionHistory.OrderId");
				BindingManager.AddBinding("OrderTypeIdText.Text","TblTransactionHistory.OrderTypeId");
				BindingManager.AddBinding("LoyaltyPointsRedeemedText.Text","TblTransactionHistory.LoyaltyPointsRedeemed");
				BindingManager.AddBinding("LoyaltyPointsAddedText.Text","TblTransactionHistory.LoyaltyPointsAdded");
				BindingManager.AddBinding("LoyaltyPointsValueText.Text","TblTransactionHistory.LoyaltyPointsValue");
				BindingManager.AddBinding("OrderTotalText.Text","TblTransactionHistory.OrderTotal");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindTblTransactionHistoryDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindTblTransactionHistoryDropdownsAndDivSelectRegions();
		}


		
		
		private void BindTblTransactionHistoryDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveTblTransactionHistory(object sender, EventArgs e)
			{
				if (Validate()){
					LoyaltyDAO.Save(TblTransactionHistory);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




