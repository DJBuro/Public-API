
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Evaluatorcondition

	/// <summary>
	/// Evaluatorcondition form for NHibernate mapped table 'evaluator_condition'.
	/// </summary>
	public partial class EvaluatorconditionForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Evaluatorcondition _Evaluatorcondition;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Evaluatorcondition Evaluatorcondition
		{
			set { _Evaluatorcondition = value; }
			get { return _Evaluatorcondition; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Evaluatorcondition = new OrderTracking.Gps.Dao.Domain.Evaluatorcondition ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Evaluatorcondition = (OrderTracking.Gps.Dao.Domain.Evaluatorcondition)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Evaluatorcondition;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("BotypeText.Text","Evaluatorcondition.Botype");
				BindingManager.AddBinding("NameText.Text","Evaluatorcondition.Name");
				BindingManager.AddBinding("CreatedText.Text","Evaluatorcondition.Created");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindEvaluatorconditionDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindEvaluatorconditionDropdownsAndDivSelectRegions();
		}


		
		
		private void BindEvaluatorconditionDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveEvaluatorcondition(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Evaluatorcondition);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




