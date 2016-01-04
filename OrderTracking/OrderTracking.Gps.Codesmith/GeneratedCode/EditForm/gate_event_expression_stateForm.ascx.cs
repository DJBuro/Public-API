
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Gateeventexpressionstate

	/// <summary>
	/// Gateeventexpressionstate form for NHibernate mapped table 'gate_event_expression_state'.
	/// </summary>
	public partial class GateeventexpressionstateForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Gateeventexpressionstate _Gateeventexpressionstate;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Gateeventexpressionstate Gateeventexpressionstate
		{
			set { _Gateeventexpressionstate = value; }
			get { return _Gateeventexpressionstate; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Gateeventexpressionstate = new OrderTracking.Gps.Dao.Domain.Gateeventexpressionstate ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Gateeventexpressionstate = (OrderTracking.Gps.Dao.Domain.Gateeventexpressionstate)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Gateeventexpressionstate;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("CustomstateText.Text","Gateeventexpressionstate.Customstate");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindGateeventexpressionstateDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindGateeventexpressionstateDropdownsAndDivSelectRegions();
		}


		
		
		private void BindGateeventexpressionstateDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveGateeventexpressionstate(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Gateeventexpressionstate);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




