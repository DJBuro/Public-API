
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Session

	/// <summary>
	/// Session form for NHibernate mapped table 'sessions'.
	/// </summary>
	public partial class SessionForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Session _Session;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Session Session
		{
			set { _Session = value; }
			get { return _Session; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Session = new OrderTracking.Gps.Dao.Domain.Session ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Session = (OrderTracking.Gps.Dao.Domain.Session)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Session;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("UseridText.Text","Session.Userid");
				BindingManager.AddBinding("TimestampText.Text","Session.Timestamp");
				BindingManager.AddBinding("ExpireText.Text","Session.Expire");
				BindingManager.AddBinding("CreatedText.Text","Session.Created");
				BindingManager.AddBinding("IpaddressText.Text","Session.Ipaddress");
				BindingManager.AddBinding("BotypeText.Text","Session.Botype");
				BindingManager.AddBinding("DeviceidText.Text","Session.Deviceid");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindSessionDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindSessionDropdownsAndDivSelectRegions();
		}


		
		
		private void BindSessionDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveSession(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Session);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




