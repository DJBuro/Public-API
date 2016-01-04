
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Data
{
	#region TrackerType

	/// <summary>
	/// TrackerType form for NHibernate mapped table 'tbl_TrackerType'.
	/// </summary>
	public partial class TrackerTypeForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Dao.Domain.TrackerType _TrackerType;

			
		#endregion
		
		#region Properties

		public OrderTracking.Dao.Domain.TrackerType TrackerType
		{
			set { _TrackerType = value; }
			get { return _TrackerType; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_TrackerType = new OrderTracking.Dao.Domain.TrackerType ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _TrackerType = (OrderTracking.Dao.Domain.TrackerType)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _TrackerType;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("NameText.Text","TrackerType.Name");
				BindingManager.AddBinding("GpsGateIdText.Text","TrackerType.GpsGateId");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindTrackerTypeDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindTrackerTypeDropdownsAndDivSelectRegions();
		}


		
		
		private void BindTrackerTypeDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveTrackerType(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingDAO.Save(TrackerType);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




