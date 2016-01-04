
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Schedulertaskparameter

	/// <summary>
	/// Schedulertaskparameter form for NHibernate mapped table 'scheduler_task_parameter'.
	/// </summary>
	public partial class SchedulertaskparameterForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Schedulertaskparameter _Schedulertaskparameter;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Schedulertaskparameter Schedulertaskparameter
		{
			set { _Schedulertaskparameter = value; }
			get { return _Schedulertaskparameter; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Schedulertaskparameter = new OrderTracking.Gps.Dao.Domain.Schedulertaskparameter ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Schedulertaskparameter = (OrderTracking.Gps.Dao.Domain.Schedulertaskparameter)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Schedulertaskparameter;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("ParameternameText.Text","Schedulertaskparameter.Parametername");
				BindingManager.AddBinding("ParameterassemblynameText.Text","Schedulertaskparameter.Parameterassemblyname");
				BindingManager.AddBinding("ParametertypenameText.Text","Schedulertaskparameter.Parametertypename");
				BindingManager.AddBinding("ParameterdataText.Text","Schedulertaskparameter.Parameterdata");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindSchedulertaskparameterDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindSchedulertaskparameterDropdownsAndDivSelectRegions();
		}


		
		
		private void BindSchedulertaskparameterDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveSchedulertaskparameter(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Schedulertaskparameter);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




