
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Gateeventexpressionevaluatornotifier

	/// <summary>
	/// Gateeventexpressionevaluatornotifier form for NHibernate mapped table 'gate_event_expression_evaluator_notifier'.
	/// </summary>
	public partial class GateeventexpressionevaluatornotifierForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Gateeventexpressionevaluatornotifier _Gateeventexpressionevaluatornotifier;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Gateeventexpressionevaluatornotifier Gateeventexpressionevaluatornotifier
		{
			set { _Gateeventexpressionevaluatornotifier = value; }
			get { return _Gateeventexpressionevaluatornotifier; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Gateeventexpressionevaluatornotifier = new OrderTracking.Gps.Dao.Domain.Gateeventexpressionevaluatornotifier ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Gateeventexpressionevaluatornotifier = (OrderTracking.Gps.Dao.Domain.Gateeventexpressionevaluatornotifier)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Gateeventexpressionevaluatornotifier;
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
				BindGateeventexpressionevaluatornotifierDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindGateeventexpressionevaluatornotifierDropdownsAndDivSelectRegions();
		}


		
		
		private void BindGateeventexpressionevaluatornotifierDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveGateeventexpressionevaluatornotifier(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Gateeventexpressionevaluatornotifier);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




