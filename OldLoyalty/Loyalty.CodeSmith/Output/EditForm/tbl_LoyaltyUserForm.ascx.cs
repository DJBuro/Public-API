
using System;
using Cacd.Web.Common.Core;


namespace Loyalty.Data
{
	#region TblLoyaltyUser

	/// <summary>
	/// TblLoyaltyUser form for NHibernate mapped table 'tbl_LoyaltyUser'.
	/// </summary>
	public partial class TblLoyaltyUserForm : BaseUserControl
		{
		#region Fields
		
		private Loyalty.Dao.Domain.TblLoyaltyUser _TblLoyaltyUser;

			
		#endregion
		
		#region Properties

		public Loyalty.Dao.Domain.TblLoyaltyUser TblLoyaltyUser
		{
			set { _TblLoyaltyUser = value; }
			get { return _TblLoyaltyUser; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_TblLoyaltyUser = new Loyalty.Dao.Domain.TblLoyaltyUser ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _TblLoyaltyUser = (Loyalty.Dao.Domain.TblLoyaltyUser)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _TblLoyaltyUser;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("FirstNameText.Text","TblLoyaltyUser.FirstName");
				BindingManager.AddBinding("MiddleInitialText.Text","TblLoyaltyUser.MiddleInitial");
				BindingManager.AddBinding("SurNameText.Text","TblLoyaltyUser.SurName");
				BindingManager.AddBinding("DateTimeCreatedText.Text","TblLoyaltyUser.DateTimeCreated");
				BindingManager.AddBinding("EmailAddressText.Text","TblLoyaltyUser.EmailAddress");
				BindingManager.AddBinding("PasswordText.Text","TblLoyaltyUser.Password");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindTblLoyaltyUserDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindTblLoyaltyUserDropdownsAndDivSelectRegions();
		}


		
		
		private void BindTblLoyaltyUserDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveTblLoyaltyUser(object sender, EventArgs e)
			{
				if (Validate()){
					LoyaltyDAO.Save(TblLoyaltyUser);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




