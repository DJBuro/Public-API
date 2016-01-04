
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Geocodercache

	/// <summary>
	/// Geocodercache form for NHibernate mapped table 'geocoder_cache'.
	/// </summary>
	public partial class GeocodercacheForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Geocodercache _Geocodercache;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Geocodercache Geocodercache
		{
			set { _Geocodercache = value; }
			get { return _Geocodercache; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Geocodercache = new OrderTracking.Gps.Dao.Domain.Geocodercache ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Geocodercache = (OrderTracking.Gps.Dao.Domain.Geocodercache)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Geocodercache;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("LonText.Text","Geocodercache.Lon");
				BindingManager.AddBinding("LatText.Text","Geocodercache.Lat");
				BindingManager.AddBinding("CountrynameText.Text","Geocodercache.Countryname");
				BindingManager.AddBinding("CitynameText.Text","Geocodercache.Cityname");
				BindingManager.AddBinding("PostalcodenumberText.Text","Geocodercache.Postalcodenumber");
				BindingManager.AddBinding("StreetnameText.Text","Geocodercache.Streetname");
				BindingManager.AddBinding("StreetnumberText.Text","Geocodercache.Streetnumber");
				BindingManager.AddBinding("StreetboxText.Text","Geocodercache.Streetbox");
				BindingManager.AddBinding("AddressText.Text","Geocodercache.Address");
				BindingManager.AddBinding("LonlathashText.Text","Geocodercache.Lonlathash");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindGeocodercacheDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindGeocodercacheDropdownsAndDivSelectRegions();
		}


		
		
		private void BindGeocodercacheDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveGeocodercache(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Geocodercache);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




