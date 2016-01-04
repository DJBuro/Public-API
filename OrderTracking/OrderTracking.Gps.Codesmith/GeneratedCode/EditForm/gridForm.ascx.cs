
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Grid

	/// <summary>
	/// Grid form for NHibernate mapped table 'grid'.
	/// </summary>
	public partial class GridForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Grid _Grid;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Grid Grid
		{
			set { _Grid = value; }
			get { return _Grid; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Grid = new OrderTracking.Gps.Dao.Domain.Grid ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Grid = (OrderTracking.Gps.Dao.Domain.Grid)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Grid;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("NameText.Text","Grid.Name");
				BindingManager.AddBinding("AlgorithmText.Text","Grid.Algorithm");
				BindingManager.AddBinding("FalseeastingText.Text","Grid.Falseeasting");
				BindingManager.AddBinding("FalsenorthingText.Text","Grid.Falsenorthing");
				BindingManager.AddBinding("OrigolongitudeText.Text","Grid.Origolongitude");
				BindingManager.AddBinding("OrigolatitudeText.Text","Grid.Origolatitude");
				BindingManager.AddBinding("ScaleText.Text","Grid.Scale");
				BindingManager.AddBinding("Latitudesp1Text.Text","Grid.Latitudesp1");
				BindingManager.AddBinding("Latitudesp2Text.Text","Grid.Latitudesp2");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindGridDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindGridDropdownsAndDivSelectRegions();
		}


		
		
		private void BindGridDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveGrid(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Grid);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




