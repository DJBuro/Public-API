
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Trackdatamod

	/// <summary>
	/// Trackdatamod form for NHibernate mapped table 'track_data_mod'.
	/// </summary>
	public partial class TrackdatamodForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Trackdatamod _Trackdatamod;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Trackdatamod Trackdatamod
		{
			set { _Trackdatamod = value; }
			get { return _Trackdatamod; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Trackdatamod = new OrderTracking.Gps.Dao.Domain.Trackdatamod ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Trackdatamod = (OrderTracking.Gps.Dao.Domain.Trackdatamod)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Trackdatamod;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("LongitudeText.Text","Trackdatamod.Longitude");
				BindingManager.AddBinding("LatitudeText.Text","Trackdatamod.Latitude");
				BindingManager.AddBinding("AltitudeText.Text","Trackdatamod.Altitude");
				BindingManager.AddBinding("HeadingText.Text","Trackdatamod.Heading");
				BindingManager.AddBinding("GroundspeedText.Text","Trackdatamod.Groundspeed");
				BindingManager.AddBinding("TimestampText.Text","Trackdatamod.Timestamp");
				BindingManager.AddBinding("MillisecondsText.Text","Trackdatamod.Milliseconds");
				BindingManager.AddBinding("ValidText.Text","Trackdatamod.Valid");
				BindingManager.AddBinding("DeletedText.Text","Trackdatamod.Deleted");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindTrackdatamodDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindTrackdatamodDropdownsAndDivSelectRegions();
		}


		
		
		private void BindTrackdatamodDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveTrackdatamod(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Trackdatamod);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




