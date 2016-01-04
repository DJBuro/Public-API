
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Gateeventargumentgeneric

	/// <summary>
	/// Gateeventargumentgeneric form for NHibernate mapped table 'gate_event_argument_generic'.
	/// </summary>
	public partial class GateeventargumentgenericForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Gateeventargumentgeneric _Gateeventargumentgeneric;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Gateeventargumentgeneric Gateeventargumentgeneric
		{
			set { _Gateeventargumentgeneric = value; }
			get { return _Gateeventargumentgeneric; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Gateeventargumentgeneric = new OrderTracking.Gps.Dao.Domain.Gateeventargumentgeneric ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Gateeventargumentgeneric = (OrderTracking.Gps.Dao.Domain.Gateeventargumentgeneric)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Gateeventargumentgeneric;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("IntvalueText.Text","Gateeventargumentgeneric.Intvalue");
				BindingManager.AddBinding("DblvalueText.Text","Gateeventargumentgeneric.Dblvalue");
				BindingManager.AddBinding("BoolvalueText.Text","Gateeventargumentgeneric.Boolvalue");
				BindingManager.AddBinding("StrvalueText.Text","Gateeventargumentgeneric.Strvalue");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindGateeventargumentgenericDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindGateeventargumentgenericDropdownsAndDivSelectRegions();
		}


		
		
		private void BindGateeventargumentgenericDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveGateeventargumentgeneric(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Gateeventargumentgeneric);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




