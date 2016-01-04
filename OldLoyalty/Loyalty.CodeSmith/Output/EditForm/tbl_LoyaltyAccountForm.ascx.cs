
using System;
using Cacd.Web.Common.Core;


namespace Loyalty.Data
{
	#region TblLoyaltyAccount

	/// <summary>
	/// TblLoyaltyAccount form for NHibernate mapped table 'tbl_LoyaltyAccount'.
	/// </summary>
	public partial class TblLoyaltyAccountForm : BaseUserControl
		{
		#region Fields
		
		private Loyalty.Dao.Domain.TblLoyaltyAccount _TblLoyaltyAccount;

			
		#endregion
		
		#region Properties

		public Loyalty.Dao.Domain.TblLoyaltyAccount TblLoyaltyAccount
		{
			set { _TblLoyaltyAccount = value; }
			get { return _TblLoyaltyAccount; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_TblLoyaltyAccount = new Loyalty.Dao.Domain.TblLoyaltyAccount ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _TblLoyaltyAccount = (Loyalty.Dao.Domain.TblLoyaltyAccount)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _TblLoyaltyAccount;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("PointsText.Text","TblLoyaltyAccount.Points");
				BindingManager.AddBinding("DateTimeCreatedText.Text","TblLoyaltyAccount.DateTimeCreated");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindTblLoyaltyAccountDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindTblLoyaltyAccountDropdownsAndDivSelectRegions();
		}


		
		
		private void BindTblLoyaltyAccountDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveTblLoyaltyAccount(object sender, EventArgs e)
			{
				if (Validate()){
					LoyaltyDAO.Save(TblLoyaltyAccount);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




