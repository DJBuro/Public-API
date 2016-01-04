
using System;
using Cacd.Web.Common.Core;


namespace Loyalty.Data
{
	#region LoyaltyAccountStatus

	/// <summary>
	/// LoyaltyAccountStatus form for NHibernate mapped table 'tbl_LoyaltyAccountStatus'.
	/// </summary>
	public partial class LoyaltyAccountStatusForm : BaseUserControl
		{
		#region Fields
		
		private Loyalty.Dao.Domain.LoyaltyAccountStatus _LoyaltyAccountStatus;

			
		#endregion
		
		#region Properties

		public Loyalty.Dao.Domain.LoyaltyAccountStatus LoyaltyAccountStatus
		{
			set { _LoyaltyAccountStatus = value; }
			get { return _LoyaltyAccountStatus; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_LoyaltyAccountStatus = new Loyalty.Dao.Domain.LoyaltyAccountStatus ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _LoyaltyAccountStatus = (Loyalty.Dao.Domain.LoyaltyAccountStatus)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _LoyaltyAccountStatus;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("ReasonText.Text","LoyaltyAccountStatus.Reason");
				BindingManager.AddBinding("DateTimeCreatedText.Text","LoyaltyAccountStatus.DateTimeCreated");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindLoyaltyAccountStatusDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindLoyaltyAccountStatusDropdownsAndDivSelectRegions();
		}


		
		
		private void BindLoyaltyAccountStatusDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveLoyaltyAccountStatus(object sender, EventArgs e)
			{
				if (Validate()){
					LoyaltyDAO.Save(LoyaltyAccountStatus);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




