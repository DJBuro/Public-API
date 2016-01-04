
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Gatecommand

	/// <summary>
	/// Gatecommand form for NHibernate mapped table 'gate_command'.
	/// </summary>
	public partial class GatecommandForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Gatecommand _Gatecommand;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Gatecommand Gatecommand
		{
			set { _Gatecommand = value; }
			get { return _Gatecommand; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Gatecommand = new OrderTracking.Gps.Dao.Domain.Gatecommand ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Gatecommand = (OrderTracking.Gps.Dao.Domain.Gatecommand)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Gatecommand;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("NameText.Text","Gatecommand.Name");
				BindingManager.AddBinding("DescriptionText.Text","Gatecommand.Description");
				BindingManager.AddBinding("EnabledText.Text","Gatecommand.Enabled");
				BindingManager.AddBinding("NamespaceText.Text","Gatecommand.Namespace");
				BindingManager.AddBinding("OutgoingText.Text","Gatecommand.Outgoing");
				BindingManager.AddBinding("SpecialText.Text","Gatecommand.Special");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindGatecommandDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindGatecommandDropdownsAndDivSelectRegions();
		}


		
		
		private void BindGatecommandDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveGatecommand(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Gatecommand);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




