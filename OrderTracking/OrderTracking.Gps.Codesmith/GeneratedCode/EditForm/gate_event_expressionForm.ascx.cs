
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Gateeventexpression

	/// <summary>
	/// Gateeventexpression form for NHibernate mapped table 'gate_event_expression'.
	/// </summary>
	public partial class GateeventexpressionForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Gateeventexpression _Gateeventexpression;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Gateeventexpression Gateeventexpression
		{
			set { _Gateeventexpression = value; }
			get { return _Gateeventexpression; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Gateeventexpression = new OrderTracking.Gps.Dao.Domain.Gateeventexpression ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Gateeventexpression = (OrderTracking.Gps.Dao.Domain.Gateeventexpression)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Gateeventexpression;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("ValuedoubleText.Text","Gateeventexpression.Valuedouble");
				BindingManager.AddBinding("ValuebooleanText.Text","Gateeventexpression.Valueboolean");
				BindingManager.AddBinding("MindeltayfilterText.Text","Gateeventexpression.Mindeltayfilter");
				BindingManager.AddBinding("DenominatorText.Text","Gateeventexpression.Denominator");
				BindingManager.AddBinding("RelationaloperatorText.Text","Gateeventexpression.Relationaloperator");
				BindingManager.AddBinding("MathoperatorText.Text","Gateeventexpression.Mathoperator");
				BindingManager.AddBinding("ValuestringText.Text","Gateeventexpression.Valuestring");
				BindingManager.AddBinding("IsendexpressionText.Text","Gateeventexpression.Isendexpression");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindGateeventexpressionDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindGateeventexpressionDropdownsAndDivSelectRegions();
		}


		
		
		private void BindGateeventexpressionDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveGateeventexpression(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Gateeventexpression);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




