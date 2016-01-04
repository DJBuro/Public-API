
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Listener

	/// <summary>
	/// Listener form for NHibernate mapped table 'listener'.
	/// </summary>
	public partial class ListenerForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Listener _Listener;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Listener Listener
		{
			set { _Listener = value; }
			get { return _Listener; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Listener = new OrderTracking.Gps.Dao.Domain.Listener ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Listener = (OrderTracking.Gps.Dao.Domain.Listener)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Listener;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("EnabledText.Text","Listener.Enabled");
				BindingManager.AddBinding("ServeraddressText.Text","Listener.Serveraddress");
				BindingManager.AddBinding("ServerportText.Text","Listener.Serverport");
				BindingManager.AddBinding("BotypeText.Text","Listener.Botype");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindListenerDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindListenerDropdownsAndDivSelectRegions();
		}


		
		
		private void BindListenerDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveListener(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Listener);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




