
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Rulechain

	/// <summary>
	/// Rulechain form for NHibernate mapped table 'rule_chain'.
	/// </summary>
	public partial class RulechainForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Rulechain _Rulechain;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Rulechain Rulechain
		{
			set { _Rulechain = value; }
			get { return _Rulechain; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Rulechain = new OrderTracking.Gps.Dao.Domain.Rulechain ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Rulechain = (OrderTracking.Gps.Dao.Domain.Rulechain)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Rulechain;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("NameText.Text","Rulechain.Name");
				BindingManager.AddBinding("DescriptionText.Text","Rulechain.Description");
				BindingManager.AddBinding("BotypeText.Text","Rulechain.Botype");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindRulechainDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindRulechainDropdownsAndDivSelectRegions();
		}


		
		
		private void BindRulechainDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveRulechain(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Rulechain);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




