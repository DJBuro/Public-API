
using System;
using Cacd.Web.Common.Core;


namespace Loyalty.Data
{
	#region LoyaltyUser

	/// <summary>
	/// LoyaltyUser form for NHibernate mapped table 'tbl_LoyaltyUser'.
	/// </summary>
	public partial class LoyaltyUserForm : BaseUserControl
		{
		#region Fields
		
		private Loyalty.Dao.Domain.LoyaltyUser _LoyaltyUser;

			
		#endregion
		
		#region Properties

		public Loyalty.Dao.Domain.LoyaltyUser LoyaltyUser
		{
			set { _LoyaltyUser = value; }
			get { return _LoyaltyUser; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_LoyaltyUser = new Loyalty.Dao.Domain.LoyaltyUser ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _LoyaltyUser = (Loyalty.Dao.Domain.LoyaltyUser)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _LoyaltyUser;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("FirstNameText.Text","LoyaltyUser.FirstName");
				BindingManager.AddBinding("MiddleInitialText.Text","LoyaltyUser.MiddleInitial");
				BindingManager.AddBinding("SurNameText.Text","LoyaltyUser.SurName");
				BindingManager.AddBinding("DateTimeCreatedText.Text","LoyaltyUser.DateTimeCreated");
				BindingManager.AddBinding("EmailAddressText.Text","LoyaltyUser.EmailAddress");
				BindingManager.AddBinding("PasswordText.Text","LoyaltyUser.Password");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindLoyaltyUserDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindLoyaltyUserDropdownsAndDivSelectRegions();
		}


		
		
		private void BindLoyaltyUserDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveLoyaltyUser(object sender, EventArgs e)
			{
				if (Validate()){
					LoyaltyDAO.Save(LoyaltyUser);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




