
using System;
using Cacd.Web.Common.Core;


namespace Loyalty.Data
{
	#region TransactionHistory

	/// <summary>
	/// TransactionHistory form for NHibernate mapped table 'tbl_TransactionHistory'.
	/// </summary>
	public partial class TransactionHistoryForm : BaseUserControl
		{
		#region Fields
		
		private Loyalty.Dao.Domain.TransactionHistory _TransactionHistory;

			
		#endregion
		
		#region Properties

		public Loyalty.Dao.Domain.TransactionHistory TransactionHistory
		{
			set { _TransactionHistory = value; }
			get { return _TransactionHistory; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_TransactionHistory = new Loyalty.Dao.Domain.TransactionHistory ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _TransactionHistory = (Loyalty.Dao.Domain.TransactionHistory)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _TransactionHistory;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("DateTimeOrderedText.Text","TransactionHistory.DateTimeOrdered");
				BindingManager.AddBinding("OrderIdText.Text","TransactionHistory.OrderId");
				BindingManager.AddBinding("LoyaltyPointsRedeemedText.Text","TransactionHistory.LoyaltyPointsRedeemed");
				BindingManager.AddBinding("LoyaltyPointsAddedText.Text","TransactionHistory.LoyaltyPointsAdded");
				BindingManager.AddBinding("LoyaltyPointsValueText.Text","TransactionHistory.LoyaltyPointsValue");
				BindingManager.AddBinding("OrderTotalText.Text","TransactionHistory.OrderTotal");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindTransactionHistoryDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindTransactionHistoryDropdownsAndDivSelectRegions();
		}


		
		
		private void BindTransactionHistoryDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveTransactionHistory(object sender, EventArgs e)
			{
				if (Validate()){
					LoyaltyDAO.Save(TransactionHistory);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




