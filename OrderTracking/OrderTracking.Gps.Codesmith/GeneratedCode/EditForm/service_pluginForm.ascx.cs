
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Serviceplugin

	/// <summary>
	/// Serviceplugin form for NHibernate mapped table 'service_plugin'.
	/// </summary>
	public partial class ServicepluginForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Serviceplugin _Serviceplugin;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Serviceplugin Serviceplugin
		{
			set { _Serviceplugin = value; }
			get { return _Serviceplugin; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Serviceplugin = new OrderTracking.Gps.Dao.Domain.Serviceplugin ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Serviceplugin = (OrderTracking.Gps.Dao.Domain.Serviceplugin)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Serviceplugin;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("EnabledText.Text","Serviceplugin.Enabled");
				BindingManager.AddBinding("BotypeText.Text","Serviceplugin.Botype");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindServicepluginDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindServicepluginDropdownsAndDivSelectRegions();
		}


		
		
		private void BindServicepluginDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveServiceplugin(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Serviceplugin);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




