
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Data
{
	#region Account

	/// <summary>
	/// Account form for NHibernate mapped table 'tbl_Account'.
	/// </summary>
	public partial class AccountForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Dao.Domain.Account _Account;

			
		#endregion
		
		#region Properties

		public OrderTracking.Dao.Domain.Account Account
		{
			set { _Account = value; }
			get { return _Account; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Account = new OrderTracking.Dao.Domain.Account ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Account = (OrderTracking.Dao.Domain.Account)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Account;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("UserNameText.Text","Account.UserName");
				BindingManager.AddBinding("PasswordText.Text","Account.Password");
				BindingManager.AddBinding("GpsEnabledText.Text","Account.GpsEnabled");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindAccountDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindAccountDropdownsAndDivSelectRegions();
		}


		
		
		private void BindAccountDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveAccount(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingDAO.Save(Account);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




