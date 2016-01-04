
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Trackdata

	/// <summary>
	/// Trackdata form for NHibernate mapped table 'track_data'.
	/// </summary>
	public partial class TrackdataForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Trackdata _Trackdata;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Trackdata Trackdata
		{
			set { _Trackdata = value; }
			get { return _Trackdata; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Trackdata = new OrderTracking.Gps.Dao.Domain.Trackdata ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Trackdata = (OrderTracking.Gps.Dao.Domain.Trackdata)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Trackdata;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("LongitudeText.Text","Trackdata.Longitude");
				BindingManager.AddBinding("LatitudeText.Text","Trackdata.Latitude");
				BindingManager.AddBinding("AltitudeText.Text","Trackdata.Altitude");
				BindingManager.AddBinding("HeadingText.Text","Trackdata.Heading");
				BindingManager.AddBinding("GroundspeedText.Text","Trackdata.Groundspeed");
				BindingManager.AddBinding("TimestampText.Text","Trackdata.Timestamp");
				BindingManager.AddBinding("MillisecondsText.Text","Trackdata.Milliseconds");
				BindingManager.AddBinding("DistancenextText.Text","Trackdata.Distancenext");
				BindingManager.AddBinding("ValidText.Text","Trackdata.Valid");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindTrackdataDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindTrackdataDropdownsAndDivSelectRegions();
		}


		
		
		private void BindTrackdataDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveTrackdata(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Trackdata);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




