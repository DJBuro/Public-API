
using System;
using Cacd.Web.Common.Core;


namespace Loyalty.Data
{
	#region TblCompany

	/// <summary>
	/// TblCompany form for NHibernate mapped table 'tbl_Company'.
	/// </summary>
	public partial class TblCompanyForm : BaseUserControl
		{
		#region Fields
		
		private Loyalty.Dao.Domain.TblCompany _TblCompany;

			
		#endregion
		
		#region Properties

		public Loyalty.Dao.Domain.TblCompany TblCompany
		{
			set { _TblCompany = value; }
			get { return _TblCompany; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_TblCompany = new Loyalty.Dao.Domain.TblCompany ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _TblCompany = (Loyalty.Dao.Domain.TblCompany)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _TblCompany;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("NameText.Text","TblCompany.Name");
				BindingManager.AddBinding("RedemptionRatioText.Text","TblCompany.RedemptionRatio");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindTblCompanyDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindTblCompanyDropdownsAndDivSelectRegions();
		}


		
		
		private void BindTblCompanyDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveTblCompany(object sender, EventArgs e)
			{
				if (Validate()){
					LoyaltyDAO.Save(TblCompany);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




