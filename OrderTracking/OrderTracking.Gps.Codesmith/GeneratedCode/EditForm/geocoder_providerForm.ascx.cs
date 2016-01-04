
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Geocoderprovider

	/// <summary>
	/// Geocoderprovider form for NHibernate mapped table 'geocoder_provider'.
	/// </summary>
	public partial class GeocoderproviderForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Geocoderprovider _Geocoderprovider;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Geocoderprovider Geocoderprovider
		{
			set { _Geocoderprovider = value; }
			get { return _Geocoderprovider; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Geocoderprovider = new OrderTracking.Gps.Dao.Domain.Geocoderprovider ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Geocoderprovider = (OrderTracking.Gps.Dao.Domain.Geocoderprovider)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Geocoderprovider;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("TypeidText.Text","Geocoderprovider.Typeid");
				BindingManager.AddBinding("PriorityText.Text","Geocoderprovider.Priority");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindGeocoderproviderDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindGeocoderproviderDropdownsAndDivSelectRegions();
		}


		
		
		private void BindGeocoderproviderDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveGeocoderprovider(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Geocoderprovider);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




