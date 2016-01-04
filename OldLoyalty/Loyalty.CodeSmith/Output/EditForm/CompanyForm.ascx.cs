
using System;
using Cacd.Web.Common.Core;


namespace Loyalty.Data
{
	#region Company

	/// <summary>
	/// Company form for NHibernate mapped table 'tbl_Company'.
	/// </summary>
	public partial class CompanyForm : BaseUserControl
		{
		#region Fields
		
		private Loyalty.Dao.Domain.Company _Company;

			
		#endregion
		
		#region Properties

		public Loyalty.Dao.Domain.Company Company
		{
			set { _Company = value; }
			get { return _Company; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Company = new Loyalty.Dao.Domain.Company ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Company = (Loyalty.Dao.Domain.Company)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Company;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("RamesesHeadOfficeIdText.Text","Company.RamesesHeadOfficeId");
				BindingManager.AddBinding("RamesesCompanyIdText.Text","Company.RamesesCompanyId");
				BindingManager.AddBinding("NameText.Text","Company.Name");
				BindingManager.AddBinding("RedemptionRatioText.Text","Company.RedemptionRatio");
				BindingManager.AddBinding("CompanyKeyText.Text","Company.CompanyKey");
				BindingManager.AddBinding("CompanyPasswordText.Text","Company.CompanyPassword");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindCompanyDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindCompanyDropdownsAndDivSelectRegions();
		}


		
		
		private void BindCompanyDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveCompany(object sender, EventArgs e)
			{
				if (Validate()){
					LoyaltyDAO.Save(Company);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




