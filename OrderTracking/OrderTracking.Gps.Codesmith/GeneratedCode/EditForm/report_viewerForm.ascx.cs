
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Reportviewer

	/// <summary>
	/// Reportviewer form for NHibernate mapped table 'report_viewer'.
	/// </summary>
	public partial class ReportviewerForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Reportviewer _Reportviewer;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Reportviewer Reportviewer
		{
			set { _Reportviewer = value; }
			get { return _Reportviewer; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Reportviewer = new OrderTracking.Gps.Dao.Domain.Reportviewer ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Reportviewer = (OrderTracking.Gps.Dao.Domain.Reportviewer)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Reportviewer;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("NameText.Text","Reportviewer.Name");
				BindingManager.AddBinding("DescriptionText.Text","Reportviewer.Description");
				BindingManager.AddBinding("ReportviewertypeText.Text","Reportviewer.Reportviewertype");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindReportviewerDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindReportviewerDropdownsAndDivSelectRegions();
		}


		
		
		private void BindReportviewerDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveReportviewer(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Reportviewer);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




