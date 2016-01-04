
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Devicedeffield

	/// <summary>
	/// Devicedeffield form for NHibernate mapped table 'device_def_field'.
	/// </summary>
	public partial class DevicedeffieldForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Devicedeffield _Devicedeffield;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Devicedeffield Devicedeffield
		{
			set { _Devicedeffield = value; }
			get { return _Devicedeffield; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Devicedeffield = new OrderTracking.Gps.Dao.Domain.Devicedeffield ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Devicedeffield = (OrderTracking.Gps.Dao.Domain.Devicedeffield)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Devicedeffield;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("SavechangesonlyText.Text","Devicedeffield.Savechangesonly");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindDevicedeffieldDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindDevicedeffieldDropdownsAndDivSelectRegions();
		}


		
		
		private void BindDevicedeffieldDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveDevicedeffield(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Devicedeffield);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




