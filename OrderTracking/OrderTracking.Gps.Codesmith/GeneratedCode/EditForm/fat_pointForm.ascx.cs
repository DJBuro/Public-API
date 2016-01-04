
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Fatpoint

	/// <summary>
	/// Fatpoint form for NHibernate mapped table 'fat_point'.
	/// </summary>
	public partial class FatpointForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Fatpoint _Fatpoint;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Fatpoint Fatpoint
		{
			set { _Fatpoint = value; }
			get { return _Fatpoint; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Fatpoint = new OrderTracking.Gps.Dao.Domain.Fatpoint ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Fatpoint = (OrderTracking.Gps.Dao.Domain.Fatpoint)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Fatpoint;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("StarttrackdataidText.Text","Fatpoint.Starttrackdataid");
				BindingManager.AddBinding("EndtrackdataidText.Text","Fatpoint.Endtrackdataid");
				BindingManager.AddBinding("LongitudeText.Text","Fatpoint.Longitude");
				BindingManager.AddBinding("LatitudeText.Text","Fatpoint.Latitude");
				BindingManager.AddBinding("AltitudeText.Text","Fatpoint.Altitude");
				BindingManager.AddBinding("StarttimeText.Text","Fatpoint.Starttime");
				BindingManager.AddBinding("StarttimemsText.Text","Fatpoint.Starttimems");
				BindingManager.AddBinding("EndtimeText.Text","Fatpoint.Endtime");
				BindingManager.AddBinding("EndtimemsText.Text","Fatpoint.Endtimems");
				BindingManager.AddBinding("ErrorradiusText.Text","Fatpoint.Errorradius");
				BindingManager.AddBinding("BuilddotsText.Text","Fatpoint.Builddots");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindFatpointDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindFatpointDropdownsAndDivSelectRegions();
		}


		
		
		private void BindFatpointDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveFatpoint(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Fatpoint);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




