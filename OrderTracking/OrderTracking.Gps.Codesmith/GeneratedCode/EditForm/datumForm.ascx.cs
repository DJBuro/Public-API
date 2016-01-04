
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Datum

	/// <summary>
	/// Datum form for NHibernate mapped table 'datum'.
	/// </summary>
	public partial class DatumForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Datum _Datum;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Datum Datum
		{
			set { _Datum = value; }
			get { return _Datum; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Datum = new OrderTracking.Gps.Dao.Domain.Datum ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Datum = (OrderTracking.Gps.Dao.Domain.Datum)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Datum;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("NameText.Text","Datum.Name");
				BindingManager.AddBinding("SemimajoraxisText.Text","Datum.Semimajoraxis");
				BindingManager.AddBinding("E2Text.Text","Datum.E2");
				BindingManager.AddBinding("DeltaxText.Text","Datum.Deltax");
				BindingManager.AddBinding("DeltayText.Text","Datum.Deltay");
				BindingManager.AddBinding("DeltazText.Text","Datum.Deltaz");
				BindingManager.AddBinding("RotxText.Text","Datum.Rotx");
				BindingManager.AddBinding("RotyText.Text","Datum.Roty");
				BindingManager.AddBinding("RotzText.Text","Datum.Rotz");
				BindingManager.AddBinding("ScaleText.Text","Datum.Scale");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindDatumDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindDatumDropdownsAndDivSelectRegions();
		}


		
		
		private void BindDatumDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveDatum(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Datum);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




