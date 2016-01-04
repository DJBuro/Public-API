
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Evaluatorconditioneventduration

	/// <summary>
	/// Evaluatorconditioneventduration form for NHibernate mapped table 'evaluator_condition_event_duration'.
	/// </summary>
	public partial class EvaluatorconditioneventdurationForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Evaluatorconditioneventduration _Evaluatorconditioneventduration;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Evaluatorconditioneventduration Evaluatorconditioneventduration
		{
			set { _Evaluatorconditioneventduration = value; }
			get { return _Evaluatorconditioneventduration; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Evaluatorconditioneventduration = new OrderTracking.Gps.Dao.Domain.Evaluatorconditioneventduration ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Evaluatorconditioneventduration = (OrderTracking.Gps.Dao.Domain.Evaluatorconditioneventduration)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Evaluatorconditioneventduration;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("EventdurationText.Text","Evaluatorconditioneventduration.Eventduration");
				BindingManager.AddBinding("RelationaloperatorText.Text","Evaluatorconditioneventduration.Relationaloperator");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindEvaluatorconditioneventdurationDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindEvaluatorconditioneventdurationDropdownsAndDivSelectRegions();
		}


		
		
		private void BindEvaluatorconditioneventdurationDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveEvaluatorconditioneventduration(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Evaluatorconditioneventduration);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




