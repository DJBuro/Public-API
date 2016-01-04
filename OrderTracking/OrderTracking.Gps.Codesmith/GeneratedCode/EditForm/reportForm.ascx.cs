
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Report

	/// <summary>
	/// Report form for NHibernate mapped table 'report'.
	/// </summary>
	public partial class ReportForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Report _Report;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Report Report
		{
			set { _Report = value; }
			get { return _Report; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Report = new OrderTracking.Gps.Dao.Domain.Report ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Report = (OrderTracking.Gps.Dao.Domain.Report)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Report;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("ReportnameText.Text","Report.Reportname");
				BindingManager.AddBinding("ReportdescriptionText.Text","Report.Reportdescription");
				BindingManager.AddBinding("BotypeText.Text","Report.Botype");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindReportDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindReportDropdownsAndDivSelectRegions();
		}


		
		
		private void BindReportDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveReport(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Report);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




