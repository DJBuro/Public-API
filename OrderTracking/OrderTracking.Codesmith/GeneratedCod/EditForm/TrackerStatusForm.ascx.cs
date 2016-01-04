
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Data
{
	#region TrackerStatus

	/// <summary>
	/// TrackerStatus form for NHibernate mapped table 'tbl_TrackerStatus'.
	/// </summary>
	public partial class TrackerStatusForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Dao.Domain.TrackerStatus _TrackerStatus;

			
		#endregion
		
		#region Properties

		public OrderTracking.Dao.Domain.TrackerStatus TrackerStatus
		{
			set { _TrackerStatus = value; }
			get { return _TrackerStatus; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_TrackerStatus = new OrderTracking.Dao.Domain.TrackerStatus ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _TrackerStatus = (OrderTracking.Dao.Domain.TrackerStatus)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _TrackerStatus;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("NameText.Text","TrackerStatus.Name");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindTrackerStatusDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindTrackerStatusDropdownsAndDivSelectRegions();
		}


		
		
		private void BindTrackerStatusDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveTrackerStatus(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingDAO.Save(TrackerStatus);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




