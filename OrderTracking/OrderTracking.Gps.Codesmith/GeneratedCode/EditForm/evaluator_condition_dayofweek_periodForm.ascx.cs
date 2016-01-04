
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Evaluatorconditiondayofweekperiod

	/// <summary>
	/// Evaluatorconditiondayofweekperiod form for NHibernate mapped table 'evaluator_condition_dayofweek_period'.
	/// </summary>
	public partial class EvaluatorconditiondayofweekperiodForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Evaluatorconditiondayofweekperiod _Evaluatorconditiondayofweekperiod;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Evaluatorconditiondayofweekperiod Evaluatorconditiondayofweekperiod
		{
			set { _Evaluatorconditiondayofweekperiod = value; }
			get { return _Evaluatorconditiondayofweekperiod; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Evaluatorconditiondayofweekperiod = new OrderTracking.Gps.Dao.Domain.Evaluatorconditiondayofweekperiod ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Evaluatorconditiondayofweekperiod = (OrderTracking.Gps.Dao.Domain.Evaluatorconditiondayofweekperiod)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Evaluatorconditiondayofweekperiod;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("StarttimeofdayText.Text","Evaluatorconditiondayofweekperiod.Starttimeofday");
				BindingManager.AddBinding("StoptimeofdayText.Text","Evaluatorconditiondayofweekperiod.Stoptimeofday");
				BindingManager.AddBinding("DayofweekText.Text","Evaluatorconditiondayofweekperiod.Dayofweek");
				BindingManager.AddBinding("EvaluationmethodText.Text","Evaluatorconditiondayofweekperiod.Evaluationmethod");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindEvaluatorconditiondayofweekperiodDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindEvaluatorconditiondayofweekperiodDropdownsAndDivSelectRegions();
		}


		
		
		private void BindEvaluatorconditiondayofweekperiodDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveEvaluatorconditiondayofweekperiod(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Evaluatorconditiondayofweekperiod);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




