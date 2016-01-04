
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Data
{
	#region Tracker

	/// <summary>
	/// Tracker form for NHibernate mapped table 'tbl_Tracker'.
	/// </summary>
	public partial class TrackerForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Dao.Domain.Tracker _Tracker;

			
		#endregion
		
		#region Properties

		public OrderTracking.Dao.Domain.Tracker Tracker
		{
			set { _Tracker = value; }
			get { return _Tracker; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Tracker = new OrderTracking.Dao.Domain.Tracker ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Tracker = (OrderTracking.Dao.Domain.Tracker)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Tracker;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("NameText.Text","Tracker.Name");
				BindingManager.AddBinding("BatteryLevelText.Text","Tracker.BatteryLevel");
				BindingManager.AddBinding("IMEIText.Text","Tracker.IMEI");
				BindingManager.AddBinding("SerialNumberText.Text","Tracker.SerialNumber");
				BindingManager.AddBinding("PhoneNumberText.Text","Tracker.PhoneNumber");
				BindingManager.AddBinding("ActiveText.Text","Tracker.Active");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindTrackerDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindTrackerDropdownsAndDivSelectRegions();
		}


		
		
		private void BindTrackerDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveTracker(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingDAO.Save(Tracker);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




