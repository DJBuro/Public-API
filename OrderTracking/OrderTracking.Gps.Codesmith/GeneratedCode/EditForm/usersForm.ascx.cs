
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region User

	/// <summary>
	/// User form for NHibernate mapped table 'users'.
	/// </summary>
	public partial class UserForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.User _User;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.User User
		{
			set { _User = value; }
			get { return _User; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_User = new OrderTracking.Gps.Dao.Domain.User ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _User = (OrderTracking.Gps.Dao.Domain.User)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _User;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("UsernameText.Text","User.Username");
				BindingManager.AddBinding("NameText.Text","User.Name");
				BindingManager.AddBinding("SurnameText.Text","User.Surname");
				BindingManager.AddBinding("PasswordText.Text","User.Password");
				BindingManager.AddBinding("EmailText.Text","User.Email");
				BindingManager.AddBinding("ActiveText.Text","User.Active");
				BindingManager.AddBinding("DescriptionText.Text","User.Description");
				BindingManager.AddBinding("SourceaddressText.Text","User.Sourceaddress");
				BindingManager.AddBinding("CreatedText.Text","User.Created");
				BindingManager.AddBinding("BotypeText.Text","User.Botype");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindUserDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindUserDropdownsAndDivSelectRegions();
		}


		
		
		private void BindUserDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveUser(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(User);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




