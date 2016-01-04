
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Data
{
	#region Coordinate

	/// <summary>
	/// Coordinate form for NHibernate mapped table 'tbl_Coordinates'.
	/// </summary>
	public partial class CoordinateForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Dao.Domain.Coordinate _Coordinate;

			
		#endregion
		
		#region Properties

		public OrderTracking.Dao.Domain.Coordinate Coordinate
		{
			set { _Coordinate = value; }
			get { return _Coordinate; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Coordinate = new OrderTracking.Dao.Domain.Coordinate ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Coordinate = (OrderTracking.Dao.Domain.Coordinate)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Coordinate;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("LongitudeText.Text","Coordinate.Longitude");
				BindingManager.AddBinding("LatitudeText.Text","Coordinate.Latitude");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindCoordinateDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindCoordinateDropdownsAndDivSelectRegions();
		}


		
		
		private void BindCoordinateDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveCoordinate(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingDAO.Save(Coordinate);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




