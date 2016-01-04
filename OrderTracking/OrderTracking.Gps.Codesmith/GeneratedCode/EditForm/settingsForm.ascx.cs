
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Setting

	/// <summary>
	/// Setting form for NHibernate mapped table 'settings'.
	/// </summary>
	public partial class SettingForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Setting _Setting;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Setting Setting
		{
			set { _Setting = value; }
			get { return _Setting; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Setting = new OrderTracking.Gps.Dao.Domain.Setting ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Setting = (OrderTracking.Gps.Dao.Domain.Setting)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Setting;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("BotypeText.Text","Setting.Botype");
				BindingManager.AddBinding("NamespaceText.Text","Setting.Namespace");
				BindingManager.AddBinding("ValuenameText.Text","Setting.Valuename");
				BindingManager.AddBinding("ValuetypeText.Text","Setting.Valuetype");
				BindingManager.AddBinding("ValuedataText.Text","Setting.Valuedata");
				BindingManager.AddBinding("DescriptionText.Text","Setting.Description");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindSettingDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindSettingDropdownsAndDivSelectRegions();
		}


		
		
		private void BindSettingDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveSetting(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Setting);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




