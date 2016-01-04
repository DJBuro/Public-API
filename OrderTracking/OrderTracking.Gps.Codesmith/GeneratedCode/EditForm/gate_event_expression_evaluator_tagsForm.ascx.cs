
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Gateeventexpressionevaluatortag

	/// <summary>
	/// Gateeventexpressionevaluatortag form for NHibernate mapped table 'gate_event_expression_evaluator_tags'.
	/// </summary>
	public partial class GateeventexpressionevaluatortagForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Gateeventexpressionevaluatortag _Gateeventexpressionevaluatortag;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Gateeventexpressionevaluatortag Gateeventexpressionevaluatortag
		{
			set { _Gateeventexpressionevaluatortag = value; }
			get { return _Gateeventexpressionevaluatortag; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Gateeventexpressionevaluatortag = new OrderTracking.Gps.Dao.Domain.Gateeventexpressionevaluatortag ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Gateeventexpressionevaluatortag = (OrderTracking.Gps.Dao.Domain.Gateeventexpressionevaluatortag)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Gateeventexpressionevaluatortag;
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
				BindGateeventexpressionevaluatortagDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindGateeventexpressionevaluatortagDropdownsAndDivSelectRegions();
		}


		
		
		private void BindGateeventexpressionevaluatortagDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveGateeventexpressionevaluatortag(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Gateeventexpressionevaluatortag);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




