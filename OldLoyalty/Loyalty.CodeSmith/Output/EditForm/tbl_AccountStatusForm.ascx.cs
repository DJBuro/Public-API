
using System;
using Cacd.Web.Common.Core;


namespace Loyalty.Data
{
	#region TblAccountStatus

	/// <summary>
	/// TblAccountStatus form for NHibernate mapped table 'tbl_AccountStatus'.
	/// </summary>
	public partial class TblAccountStatusForm : BaseUserControl
		{
		#region Fields
		
		private Loyalty.Dao.Domain.TblAccountStatus _TblAccountStatus;

			
		#endregion
		
		#region Properties

		public Loyalty.Dao.Domain.TblAccountStatus TblAccountStatus
		{
			set { _TblAccountStatus = value; }
			get { return _TblAccountStatus; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_TblAccountStatus = new Loyalty.Dao.Domain.TblAccountStatus ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _TblAccountStatus = (Loyalty.Dao.Domain.TblAccountStatus)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _TblAccountStatus;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("NameText.Text","TblAccountStatus.Name");
				BindingManager.AddBinding("DescriptionText.Text","TblAccountStatus.Description");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindTblAccountStatusDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindTblAccountStatusDropdownsAndDivSelectRegions();
		}


		
		
		private void BindTblAccountStatusDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveTblAccountStatus(object sender, EventArgs e)
			{
				if (Validate()){
					LoyaltyDAO.Save(TblAccountStatus);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




