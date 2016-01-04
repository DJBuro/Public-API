
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Applicationdefreportviewer

	/// <summary>
	/// Applicationdefreportviewer form for NHibernate mapped table 'application_def_report_viewer'.
	/// </summary>
	public partial class ApplicationdefreportviewerForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Applicationdefreportviewer _Applicationdefreportviewer;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Applicationdefreportviewer Applicationdefreportviewer
		{
			set { _Applicationdefreportviewer = value; }
			get { return _Applicationdefreportviewer; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Applicationdefreportviewer = new OrderTracking.Gps.Dao.Domain.Applicationdefreportviewer ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Applicationdefreportviewer = (OrderTracking.Gps.Dao.Domain.Applicationdefreportviewer)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Applicationdefreportviewer;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindApplicationdefreportviewerDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindApplicationdefreportviewerDropdownsAndDivSelectRegions();
		}


		
		
		private void BindApplicationdefreportviewerDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveApplicationdefreportviewer(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Applicationdefreportviewer);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




