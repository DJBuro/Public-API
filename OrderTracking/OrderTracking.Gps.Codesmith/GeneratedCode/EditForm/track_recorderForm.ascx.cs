
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Trackrecorder

	/// <summary>
	/// Trackrecorder form for NHibernate mapped table 'track_recorder'.
	/// </summary>
	public partial class TrackrecorderForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Trackrecorder _Trackrecorder;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Trackrecorder Trackrecorder
		{
			set { _Trackrecorder = value; }
			get { return _Trackrecorder; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Trackrecorder = new OrderTracking.Gps.Dao.Domain.Trackrecorder ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Trackrecorder = (OrderTracking.Gps.Dao.Domain.Trackrecorder)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Trackrecorder;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("BotypeText.Text","Trackrecorder.Botype");
				BindingManager.AddBinding("NameText.Text","Trackrecorder.Name");
				BindingManager.AddBinding("TrackinfoidText.Text","Trackrecorder.Trackinfoid");
				BindingManager.AddBinding("RecordingText.Text","Trackrecorder.Recording");
				BindingManager.AddBinding("LasttrackdataidText.Text","Trackrecorder.Lasttrackdataid");
				BindingManager.AddBinding("TimefilterText.Text","Trackrecorder.Timefilter");
				BindingManager.AddBinding("DistancefilterText.Text","Trackrecorder.Distancefilter");
				BindingManager.AddBinding("DirectionfilterText.Text","Trackrecorder.Directionfilter");
				BindingManager.AddBinding("DirectionthresholdText.Text","Trackrecorder.Directionthreshold");
				BindingManager.AddBinding("SpeedfilterText.Text","Trackrecorder.Speedfilter");
				BindingManager.AddBinding("RestarttimeText.Text","Trackrecorder.Restarttime");
				BindingManager.AddBinding("RestartdistanceText.Text","Trackrecorder.Restartdistance");
				BindingManager.AddBinding("TrackcategoryidText.Text","Trackrecorder.Trackcategoryid");
				BindingManager.AddBinding("LastuncertaintrackdataidText.Text","Trackrecorder.Lastuncertaintrackdataid");
				BindingManager.AddBinding("RestartintervalText.Text","Trackrecorder.Restartinterval");
				BindingManager.AddBinding("RestartintervaloffsetText.Text","Trackrecorder.Restartintervaloffset");
				BindingManager.AddBinding("SmstimefilterText.Text","Trackrecorder.Smstimefilter");
				BindingManager.AddBinding("MotionText.Text","Trackrecorder.Motion");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindTrackrecorderDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindTrackrecorderDropdownsAndDivSelectRegions();
		}


		
		
		private void BindTrackrecorderDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveTrackrecorder(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Trackrecorder);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




