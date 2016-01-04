
using System;
using Cacd.Web.Common.Core;


namespace Loyalty.Data
{
	#region TblCompanyLoyaltyAccount

	/// <summary>
	/// TblCompanyLoyaltyAccount form for NHibernate mapped table 'tbl_CompanyLoyaltyAccount'.
	/// </summary>
	public partial class TblCompanyLoyaltyAccountForm : BaseUserControl
		{
		#region Fields
		
		private Loyalty.Dao.Domain.TblCompanyLoyaltyAccount _TblCompanyLoyaltyAccount;

			
		#endregion
		
		#region Properties

		public Loyalty.Dao.Domain.TblCompanyLoyaltyAccount TblCompanyLoyaltyAccount
		{
			set { _TblCompanyLoyaltyAccount = value; }
			get { return _TblCompanyLoyaltyAccount; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_TblCompanyLoyaltyAccount = new Loyalty.Dao.Domain.TblCompanyLoyaltyAccount ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _TblCompanyLoyaltyAccount = (Loyalty.Dao.Domain.TblCompanyLoyaltyAccount)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _TblCompanyLoyaltyAccount;
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
				BindTblCompanyLoyaltyAccountDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindTblCompanyLoyaltyAccountDropdownsAndDivSelectRegions();
		}


		
		
		private void BindTblCompanyLoyaltyAccountDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveTblCompanyLoyaltyAccount(object sender, EventArgs e)
			{
				if (Validate()){
					LoyaltyDAO.Save(TblCompanyLoyaltyAccount);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




