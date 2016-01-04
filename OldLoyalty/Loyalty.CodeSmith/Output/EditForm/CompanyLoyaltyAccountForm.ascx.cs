
using System;
using Cacd.Web.Common.Core;


namespace Loyalty.Data
{
	#region CompanyLoyaltyAccount

	/// <summary>
	/// CompanyLoyaltyAccount form for NHibernate mapped table 'tbl_CompanyLoyaltyAccount'.
	/// </summary>
	public partial class CompanyLoyaltyAccountForm : BaseUserControl
		{
		#region Fields
		
		private Loyalty.Dao.Domain.CompanyLoyaltyAccount _CompanyLoyaltyAccount;

			
		#endregion
		
		#region Properties

		public Loyalty.Dao.Domain.CompanyLoyaltyAccount CompanyLoyaltyAccount
		{
			set { _CompanyLoyaltyAccount = value; }
			get { return _CompanyLoyaltyAccount; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_CompanyLoyaltyAccount = new Loyalty.Dao.Domain.CompanyLoyaltyAccount ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _CompanyLoyaltyAccount = (Loyalty.Dao.Domain.CompanyLoyaltyAccount)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _CompanyLoyaltyAccount;
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
				BindCompanyLoyaltyAccountDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindCompanyLoyaltyAccountDropdownsAndDivSelectRegions();
		}


		
		
		private void BindCompanyLoyaltyAccountDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveCompanyLoyaltyAccount(object sender, EventArgs e)
			{
				if (Validate()){
					LoyaltyDAO.Save(CompanyLoyaltyAccount);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




