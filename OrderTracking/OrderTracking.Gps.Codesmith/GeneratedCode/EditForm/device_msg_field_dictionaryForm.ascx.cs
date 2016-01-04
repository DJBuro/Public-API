
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Devicemsgfielddictionary

	/// <summary>
	/// Devicemsgfielddictionary form for NHibernate mapped table 'device_msg_field_dictionary'.
	/// </summary>
	public partial class DevicemsgfielddictionaryForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Devicemsgfielddictionary _Devicemsgfielddictionary;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Devicemsgfielddictionary Devicemsgfielddictionary
		{
			set { _Devicemsgfielddictionary = value; }
			get { return _Devicemsgfielddictionary; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Devicemsgfielddictionary = new OrderTracking.Gps.Dao.Domain.Devicemsgfielddictionary ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Devicemsgfielddictionary = (OrderTracking.Gps.Dao.Domain.Devicemsgfielddictionary)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Devicemsgfielddictionary;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindDevicemsgfielddictionaryDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindDevicemsgfielddictionaryDropdownsAndDivSelectRegions();
		}


		
		
		private void BindDevicemsgfielddictionaryDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveDevicemsgfielddictionary(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Devicemsgfielddictionary);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




