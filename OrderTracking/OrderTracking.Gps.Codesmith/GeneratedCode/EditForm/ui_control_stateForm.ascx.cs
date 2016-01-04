
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Uicontrolstate

	/// <summary>
	/// Uicontrolstate form for NHibernate mapped table 'ui_control_state'.
	/// </summary>
	public partial class UicontrolstateForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Uicontrolstate _Uicontrolstate;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Uicontrolstate Uicontrolstate
		{
			set { _Uicontrolstate = value; }
			get { return _Uicontrolstate; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Uicontrolstate = new OrderTracking.Gps.Dao.Domain.Uicontrolstate ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Uicontrolstate = (OrderTracking.Gps.Dao.Domain.Uicontrolstate)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Uicontrolstate;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("StateText.Text","Uicontrolstate.State");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindUicontrolstateDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindUicontrolstateDropdownsAndDivSelectRegions();
		}


		
		
		private void BindUicontrolstateDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveUicontrolstate(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Uicontrolstate);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




