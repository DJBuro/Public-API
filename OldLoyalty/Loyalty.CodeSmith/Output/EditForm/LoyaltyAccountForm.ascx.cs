
using System;
using Cacd.Web.Common.Core;


namespace Loyalty.Data
{
	#region LoyaltyAccount

	/// <summary>
	/// LoyaltyAccount form for NHibernate mapped table 'tbl_LoyaltyAccount'.
	/// </summary>
	public partial class LoyaltyAccountForm : BaseUserControl
		{
		#region Fields
		
		private Loyalty.Dao.Domain.LoyaltyAccount _LoyaltyAccount;

			
		#endregion
		
		#region Properties

		public Loyalty.Dao.Domain.LoyaltyAccount LoyaltyAccount
		{
			set { _LoyaltyAccount = value; }
			get { return _LoyaltyAccount; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_LoyaltyAccount = new Loyalty.Dao.Domain.LoyaltyAccount ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _LoyaltyAccount = (Loyalty.Dao.Domain.LoyaltyAccount)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _LoyaltyAccount;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("PointsText.Text","LoyaltyAccount.Points");
				BindingManager.AddBinding("DateTimeCreatedText.Text","LoyaltyAccount.DateTimeCreated");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindLoyaltyAccountDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindLoyaltyAccountDropdownsAndDivSelectRegions();
		}


		
		
		private void BindLoyaltyAccountDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveLoyaltyAccount(object sender, EventArgs e)
			{
				if (Validate()){
					LoyaltyDAO.Save(LoyaltyAccount);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




