
using System;
using Cacd.Web.Common.Core;


namespace Loyalty.Data
{
	#region AccountStatus

	/// <summary>
	/// AccountStatus form for NHibernate mapped table 'tbl_AccountStatus'.
	/// </summary>
	public partial class AccountStatusForm : BaseUserControl
		{
		#region Fields
		
		private Loyalty.Dao.Domain.AccountStatus _AccountStatus;

			
		#endregion
		
		#region Properties

		public Loyalty.Dao.Domain.AccountStatus AccountStatus
		{
			set { _AccountStatus = value; }
			get { return _AccountStatus; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_AccountStatus = new Loyalty.Dao.Domain.AccountStatus ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _AccountStatus = (Loyalty.Dao.Domain.AccountStatus)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _AccountStatus;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("NameText.Text","AccountStatus.Name");
				BindingManager.AddBinding("DescriptionText.Text","AccountStatus.Description");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindAccountStatusDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindAccountStatusDropdownsAndDivSelectRegions();
		}


		
		
		private void BindAccountStatusDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveAccountStatus(object sender, EventArgs e)
			{
				if (Validate()){
					LoyaltyDAO.Save(AccountStatus);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




