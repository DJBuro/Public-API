
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Devicedef

	/// <summary>
	/// Devicedef form for NHibernate mapped table 'device_def'.
	/// </summary>
	public partial class DevicedefForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Devicedef _Devicedef;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Devicedef Devicedef
		{
			set { _Devicedef = value; }
			get { return _Devicedef; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Devicedef = new OrderTracking.Gps.Dao.Domain.Devicedef ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Devicedef = (OrderTracking.Gps.Dao.Domain.Devicedef)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Devicedef;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("DevicenameText.Text","Devicedef.Devicename");
				BindingManager.AddBinding("DescriptionText.Text","Devicedef.Description");
				BindingManager.AddBinding("TemplatemsgfielddictidText.Text","Devicedef.Templatemsgfielddictid");
				BindingManager.AddBinding("BotypeText.Text","Devicedef.Botype");
				BindingManager.AddBinding("UpgradableText.Text","Devicedef.Upgradable");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindDevicedefDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindDevicedefDropdownsAndDivSelectRegions();
		}


		
		
		private void BindDevicedefDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveDevicedef(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Devicedef);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




