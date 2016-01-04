
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Gateeventexpressionevaluator

	/// <summary>
	/// Gateeventexpressionevaluator form for NHibernate mapped table 'gate_event_expression_evaluator'.
	/// </summary>
	public partial class GateeventexpressionevaluatorForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Gateeventexpressionevaluator _Gateeventexpressionevaluator;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Gateeventexpressionevaluator Gateeventexpressionevaluator
		{
			set { _Gateeventexpressionevaluator = value; }
			get { return _Gateeventexpressionevaluator; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Gateeventexpressionevaluator = new OrderTracking.Gps.Dao.Domain.Gateeventexpressionevaluator ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Gateeventexpressionevaluator = (OrderTracking.Gps.Dao.Domain.Gateeventexpressionevaluator)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Gateeventexpressionevaluator;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("NameText.Text","Gateeventexpressionevaluator.Name");
				BindingManager.AddBinding("DescriptionText.Text","Gateeventexpressionevaluator.Description");
				BindingManager.AddBinding("DeletedText.Text","Gateeventexpressionevaluator.Deleted");
				BindingManager.AddBinding("ExpressionbooloperatorText.Text","Gateeventexpressionevaluator.Expressionbooloperator");
				BindingManager.AddBinding("CreatedText.Text","Gateeventexpressionevaluator.Created");
				BindingManager.AddBinding("EndexpressionbooloperatorText.Text","Gateeventexpressionevaluator.Endexpressionbooloperator");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindGateeventexpressionevaluatorDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindGateeventexpressionevaluatorDropdownsAndDivSelectRegions();
		}


		
		
		private void BindGateeventexpressionevaluatorDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveGateeventexpressionevaluator(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Gateeventexpressionevaluator);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




