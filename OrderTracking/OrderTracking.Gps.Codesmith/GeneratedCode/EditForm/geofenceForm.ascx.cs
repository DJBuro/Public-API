
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Geofence

	/// <summary>
	/// Geofence form for NHibernate mapped table 'geofence'.
	/// </summary>
	public partial class GeofenceForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Geofence _Geofence;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Geofence Geofence
		{
			set { _Geofence = value; }
			get { return _Geofence; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Geofence = new OrderTracking.Gps.Dao.Domain.Geofence ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Geofence = (OrderTracking.Gps.Dao.Domain.Geofence)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Geofence;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("GeofencenameText.Text","Geofence.Geofencename");
				BindingManager.AddBinding("GeofencedescriptionText.Text","Geofence.Geofencedescription");
				BindingManager.AddBinding("MinlongitudeText.Text","Geofence.Minlongitude");
				BindingManager.AddBinding("MaxlongitudeText.Text","Geofence.Maxlongitude");
				BindingManager.AddBinding("MinlatitudeText.Text","Geofence.Minlatitude");
				BindingManager.AddBinding("MaxlatitudeText.Text","Geofence.Maxlatitude");
				BindingManager.AddBinding("MinaltitudeText.Text","Geofence.Minaltitude");
				BindingManager.AddBinding("MaxaltitudeText.Text","Geofence.Maxaltitude");
				BindingManager.AddBinding("BotypeText.Text","Geofence.Botype");
				BindingManager.AddBinding("CreatedText.Text","Geofence.Created");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindGeofenceDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindGeofenceDropdownsAndDivSelectRegions();
		}


		
		
		private void BindGeofenceDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveGeofence(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Geofence);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




