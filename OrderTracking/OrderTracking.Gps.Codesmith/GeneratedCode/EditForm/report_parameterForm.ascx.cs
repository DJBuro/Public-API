
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Reportparameter

	/// <summary>
	/// Reportparameter form for NHibernate mapped table 'report_parameter'.
	/// </summary>
	public partial class ReportparameterForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Reportparameter _Reportparameter;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Reportparameter Reportparameter
		{
			set { _Reportparameter = value; }
			get { return _Reportparameter; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Reportparameter = new OrderTracking.Gps.Dao.Domain.Reportparameter ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Reportparameter = (OrderTracking.Gps.Dao.Domain.Reportparameter)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Reportparameter;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("ParameternameText.Text","Reportparameter.Parametername");
				BindingManager.AddBinding("ParametertypenameText.Text","Reportparameter.Parametertypename");
				BindingManager.AddBinding("ParameterassemblynameText.Text","Reportparameter.Parameterassemblyname");
				BindingManager.AddBinding("ParameterdataText.Text","Reportparameter.Parameterdata");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindReportparameterDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindReportparameterDropdownsAndDivSelectRegions();
		}


		
		
		private void BindReportparameterDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveReportparameter(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Reportparameter);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




