
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Device

	/// <summary>
	/// Device form for NHibernate mapped table 'device'.
	/// </summary>
	public partial class DeviceForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Device _Device;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Device Device
		{
			set { _Device = value; }
			get { return _Device; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Device = new OrderTracking.Gps.Dao.Domain.Device ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Device = (OrderTracking.Gps.Dao.Domain.Device)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Device;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("DevicenameText.Text","Device.Devicename");
				BindingManager.AddBinding("BotypeText.Text","Device.Botype");
				BindingManager.AddBinding("CreatedText.Text","Device.Created");
				BindingManager.AddBinding("HidepositionText.Text","Device.Hideposition");
				BindingManager.AddBinding("ProximityText.Text","Device.Proximity");
				BindingManager.AddBinding("IMEIText.Text","Device.IMEI");
				BindingManager.AddBinding("PhonenumberText.Text","Device.Phonenumber");
				BindingManager.AddBinding("LastipText.Text","Device.Lastip");
				BindingManager.AddBinding("LastportText.Text","Device.Lastport");
				BindingManager.AddBinding("StaticipText.Text","Device.Staticip");
				BindingManager.AddBinding("StaticportText.Text","Device.Staticport");
				BindingManager.AddBinding("LongitudeText.Text","Device.Longitude");
				BindingManager.AddBinding("LatitudeText.Text","Device.Latitude");
				BindingManager.AddBinding("GroundspeedText.Text","Device.Groundspeed");
				BindingManager.AddBinding("AltitudeText.Text","Device.Altitude");
				BindingManager.AddBinding("HeadingText.Text","Device.Heading");
				BindingManager.AddBinding("TimestampText.Text","Device.Timestamp");
				BindingManager.AddBinding("MillisecondsText.Text","Device.Milliseconds");
				BindingManager.AddBinding("ProtocolidText.Text","Device.Protocolid");
				BindingManager.AddBinding("ProtocolversionidText.Text","Device.Protocolversionid");
				BindingManager.AddBinding("ValidText.Text","Device.Valid");
				BindingManager.AddBinding("ApnText.Text","Device.Apn");
				BindingManager.AddBinding("GprsusernameText.Text","Device.Gprsusername");
				BindingManager.AddBinding("GprspasswordText.Text","Device.Gprspassword");
				BindingManager.AddBinding("DevdefidText.Text","Device.Devdefid");
				BindingManager.AddBinding("OutgoingtransportText.Text","Device.Outgoingtransport");
				BindingManager.AddBinding("EmailText.Text","Device.Email");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindDeviceDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindDeviceDropdownsAndDivSelectRegions();
		}


		
		
		private void BindDeviceDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveDevice(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Device);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




