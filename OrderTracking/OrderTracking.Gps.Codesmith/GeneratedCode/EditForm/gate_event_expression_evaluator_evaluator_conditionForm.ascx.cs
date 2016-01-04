
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Gateeventexpressionevaluatorevaluatorcondition

	/// <summary>
	/// Gateeventexpressionevaluatorevaluatorcondition form for NHibernate mapped table 'gate_event_expression_evaluator_evaluator_condition'.
	/// </summary>
	public partial class GateeventexpressionevaluatorevaluatorconditionForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Gateeventexpressionevaluatorevaluatorcondition _Gateeventexpressionevaluatorevaluatorcondition;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Gateeventexpressionevaluatorevaluatorcondition Gateeventexpressionevaluatorevaluatorcondition
		{
			set { _Gateeventexpressionevaluatorevaluatorcondition = value; }
			get { return _Gateeventexpressionevaluatorevaluatorcondition; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Gateeventexpressionevaluatorevaluatorcondition = new OrderTracking.Gps.Dao.Domain.Gateeventexpressionevaluatorevaluatorcondition ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Gateeventexpressionevaluatorevaluatorcondition = (OrderTracking.Gps.Dao.Domain.Gateeventexpressionevaluatorevaluatorcondition)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Gateeventexpressionevaluatorevaluatorcondition;
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
				BindGateeventexpressionevaluatorevaluatorconditionDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindGateeventexpressionevaluatorevaluatorconditionDropdownsAndDivSelectRegions();
		}


		
		
		private void BindGateeventexpressionevaluatorevaluatorconditionDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveGateeventexpressionevaluatorevaluatorcondition(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Gateeventexpressionevaluatorevaluatorcondition);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




