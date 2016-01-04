
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Smsproxy

	/// <summary>
	/// Smsproxy form for NHibernate mapped table 'sms_proxy'.
	/// </summary>
	public partial class SmsproxyForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Smsproxy _Smsproxy;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Smsproxy Smsproxy
		{
			set { _Smsproxy = value; }
			get { return _Smsproxy; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Smsproxy = new OrderTracking.Gps.Dao.Domain.Smsproxy ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Smsproxy = (OrderTracking.Gps.Dao.Domain.Smsproxy)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Smsproxy;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("PhonenumberText.Text","Smsproxy.Phonenumber");
				BindingManager.AddBinding("EnabledText.Text","Smsproxy.Enabled");
				BindingManager.AddBinding("BotypeText.Text","Smsproxy.Botype");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindSmsproxyDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindSmsproxyDropdownsAndDivSelectRegions();
		}


		
		
		private void BindSmsproxyDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveSmsproxy(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Smsproxy);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




