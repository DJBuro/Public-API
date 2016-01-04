
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Gateeventstate

	/// <summary>
	/// Gateeventstate form for NHibernate mapped table 'gate_event_state'.
	/// </summary>
	public partial class GateeventstateForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Gateeventstate _Gateeventstate;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Gateeventstate Gateeventstate
		{
			set { _Gateeventstate = value; }
			get { return _Gateeventstate; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Gateeventstate = new OrderTracking.Gps.Dao.Domain.Gateeventstate ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Gateeventstate = (OrderTracking.Gps.Dao.Domain.Gateeventstate)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Gateeventstate;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("NameText.Text","Gateeventstate.Name");
				BindingManager.AddBinding("DescriptionText.Text","Gateeventstate.Description");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindGateeventstateDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindGateeventstateDropdownsAndDivSelectRegions();
		}


		
		
		private void BindGateeventstateDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveGateeventstate(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Gateeventstate);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




