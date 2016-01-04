
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Geofenceeventexpression

	/// <summary>
	/// Geofenceeventexpression form for NHibernate mapped table 'geofence_event_expression'.
	/// </summary>
	public partial class GeofenceeventexpressionForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Geofenceeventexpression _Geofenceeventexpression;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Geofenceeventexpression Geofenceeventexpression
		{
			set { _Geofenceeventexpression = value; }
			get { return _Geofenceeventexpression; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Geofenceeventexpression = new OrderTracking.Gps.Dao.Domain.Geofenceeventexpression ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Geofenceeventexpression = (OrderTracking.Gps.Dao.Domain.Geofenceeventexpression)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Geofenceeventexpression;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("GeofenceactionText.Text","Geofenceeventexpression.Geofenceaction");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindGeofenceeventexpressionDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindGeofenceeventexpressionDropdownsAndDivSelectRegions();
		}


		
		
		private void BindGeofenceeventexpressionDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveGeofenceeventexpression(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Geofenceeventexpression);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




