
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Geocoder

	/// <summary>
	/// Geocoder form for NHibernate mapped table 'geocoder'.
	/// </summary>
	public partial class GeocoderForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Geocoder _Geocoder;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Geocoder Geocoder
		{
			set { _Geocoder = value; }
			get { return _Geocoder; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Geocoder = new OrderTracking.Gps.Dao.Domain.Geocoder ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Geocoder = (OrderTracking.Gps.Dao.Domain.Geocoder)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Geocoder;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("NameText.Text","Geocoder.Name");
				BindingManager.AddBinding("DescriptionText.Text","Geocoder.Description");
				BindingManager.AddBinding("BotypeText.Text","Geocoder.Botype");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindGeocoderDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindGeocoderDropdownsAndDivSelectRegions();
		}


		
		
		private void BindGeocoderDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveGeocoder(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Geocoder);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




