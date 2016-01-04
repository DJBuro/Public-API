
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Gateuser

	/// <summary>
	/// Gateuser form for NHibernate mapped table 'gate_user'.
	/// </summary>
	public partial class GateuserForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Gateuser _Gateuser;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Gateuser Gateuser
		{
			set { _Gateuser = value; }
			get { return _Gateuser; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Gateuser = new OrderTracking.Gps.Dao.Domain.Gateuser ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Gateuser = (OrderTracking.Gps.Dao.Domain.Gateuser)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Gateuser;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("LongitudeText.Text","Gateuser.Longitude");
				BindingManager.AddBinding("LatitudeText.Text","Gateuser.Latitude");
				BindingManager.AddBinding("GroundspeedText.Text","Gateuser.Groundspeed");
				BindingManager.AddBinding("AltitudeText.Text","Gateuser.Altitude");
				BindingManager.AddBinding("HeadingText.Text","Gateuser.Heading");
				BindingManager.AddBinding("TimestampText.Text","Gateuser.Timestamp");
				BindingManager.AddBinding("ServertimestampText.Text","Gateuser.Servertimestamp");
				BindingManager.AddBinding("DeviceactivityText.Text","Gateuser.Deviceactivity");
				BindingManager.AddBinding("DelayText.Text","Gateuser.Delay");
				BindingManager.AddBinding("LasttransportText.Text","Gateuser.Lasttransport");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindGateuserDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindGateuserDropdownsAndDivSelectRegions();
		}


		
		
		private void BindGateuserDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveGateuser(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Gateuser);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




