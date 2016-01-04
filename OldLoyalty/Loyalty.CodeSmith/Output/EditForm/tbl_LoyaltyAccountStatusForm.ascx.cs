
using System;
using Cacd.Web.Common.Core;


namespace Loyalty.Data
{
	#region TblLoyaltyAccountStatus

	/// <summary>
	/// TblLoyaltyAccountStatus form for NHibernate mapped table 'tbl_LoyaltyAccountStatus'.
	/// </summary>
	public partial class TblLoyaltyAccountStatusForm : BaseUserControl
		{
		#region Fields
		
		private Loyalty.Dao.Domain.TblLoyaltyAccountStatus _TblLoyaltyAccountStatus;

			
		#endregion
		
		#region Properties

		public Loyalty.Dao.Domain.TblLoyaltyAccountStatus TblLoyaltyAccountStatus
		{
			set { _TblLoyaltyAccountStatus = value; }
			get { return _TblLoyaltyAccountStatus; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_TblLoyaltyAccountStatus = new Loyalty.Dao.Domain.TblLoyaltyAccountStatus ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _TblLoyaltyAccountStatus = (Loyalty.Dao.Domain.TblLoyaltyAccountStatus)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _TblLoyaltyAccountStatus;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("ReasonText.Text","TblLoyaltyAccountStatus.Reason");
				BindingManager.AddBinding("DateTimeCreatedText.Text","TblLoyaltyAccountStatus.DateTimeCreated");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindTblLoyaltyAccountStatusDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindTblLoyaltyAccountStatusDropdownsAndDivSelectRegions();
		}


		
		
		private void BindTblLoyaltyAccountStatusDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveTblLoyaltyAccountStatus(object sender, EventArgs e)
			{
				if (Validate()){
					LoyaltyDAO.Save(TblLoyaltyAccountStatus);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




