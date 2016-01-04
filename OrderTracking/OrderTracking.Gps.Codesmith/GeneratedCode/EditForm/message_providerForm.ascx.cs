
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Messageprovider

	/// <summary>
	/// Messageprovider form for NHibernate mapped table 'message_provider'.
	/// </summary>
	public partial class MessageproviderForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Messageprovider _Messageprovider;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Messageprovider Messageprovider
		{
			set { _Messageprovider = value; }
			get { return _Messageprovider; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Messageprovider = new OrderTracking.Gps.Dao.Domain.Messageprovider ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Messageprovider = (OrderTracking.Gps.Dao.Domain.Messageprovider)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Messageprovider;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("BotypeText.Text","Messageprovider.Botype");
				BindingManager.AddBinding("MsgprovnameText.Text","Messageprovider.Msgprovname");
				BindingManager.AddBinding("EnabledText.Text","Messageprovider.Enabled");
				BindingManager.AddBinding("CreatedText.Text","Messageprovider.Created");
				BindingManager.AddBinding("UrlText.Text","Messageprovider.Url");
				BindingManager.AddBinding("UsernameText.Text","Messageprovider.Username");
				BindingManager.AddBinding("PasswordText.Text","Messageprovider.Password");
				BindingManager.AddBinding("CallintervalText.Text","Messageprovider.Callinterval");
				BindingManager.AddBinding("CustomlongText.Text","Messageprovider.Customlong");
				BindingManager.AddBinding("CustomstringText.Text","Messageprovider.Customstring");
				BindingManager.AddBinding("CalltimeoutText.Text","Messageprovider.Calltimeout");
				BindingManager.AddBinding("RoutelabelText.Text","Messageprovider.Routelabel");
				BindingManager.AddBinding("DefaultproviderText.Text","Messageprovider.Defaultprovider");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindMessageproviderDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindMessageproviderDropdownsAndDivSelectRegions();
		}


		
		
		private void BindMessageproviderDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveMessageprovider(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Messageprovider);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




