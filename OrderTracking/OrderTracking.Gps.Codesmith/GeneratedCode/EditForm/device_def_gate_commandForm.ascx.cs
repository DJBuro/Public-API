
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Devicedefgatecommand

	/// <summary>
	/// Devicedefgatecommand form for NHibernate mapped table 'device_def_gate_command'.
	/// </summary>
	public partial class DevicedefgatecommandForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Devicedefgatecommand _Devicedefgatecommand;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Devicedefgatecommand Devicedefgatecommand
		{
			set { _Devicedefgatecommand = value; }
			get { return _Devicedefgatecommand; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Devicedefgatecommand = new OrderTracking.Gps.Dao.Domain.Devicedefgatecommand ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Devicedefgatecommand = (OrderTracking.Gps.Dao.Domain.Devicedefgatecommand)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Devicedefgatecommand;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("TransportText.Text","Devicedefgatecommand.Transport");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindDevicedefgatecommandDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindDevicedefgatecommandDropdownsAndDivSelectRegions();
		}


		
		
		private void BindDevicedefgatecommandDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveDevicedefgatecommand(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Devicedefgatecommand);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




