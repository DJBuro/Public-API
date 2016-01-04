
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Mapmetadata

	/// <summary>
	/// Mapmetadata form for NHibernate mapped table 'map_meta_data'.
	/// </summary>
	public partial class MapmetadataForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Mapmetadata _Mapmetadata;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Mapmetadata Mapmetadata
		{
			set { _Mapmetadata = value; }
			get { return _Mapmetadata; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Mapmetadata = new OrderTracking.Gps.Dao.Domain.Mapmetadata ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Mapmetadata = (OrderTracking.Gps.Dao.Domain.Mapmetadata)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Mapmetadata;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("NameText.Text","Mapmetadata.Name");
				BindingManager.AddBinding("ValueText.Text","Mapmetadata.Value");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindMapmetadataDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindMapmetadataDropdownsAndDivSelectRegions();
		}


		
		
		private void BindMapmetadataDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveMapmetadata(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Mapmetadata);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




