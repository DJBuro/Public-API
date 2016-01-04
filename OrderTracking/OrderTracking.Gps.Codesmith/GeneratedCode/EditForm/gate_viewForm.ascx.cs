
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Gateview

	/// <summary>
	/// Gateview form for NHibernate mapped table 'gate_view'.
	/// </summary>
	public partial class GateviewForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Gateview _Gateview;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Gateview Gateview
		{
			set { _Gateview = value; }
			get { return _Gateview; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Gateview = new OrderTracking.Gps.Dao.Domain.Gateview ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Gateview = (OrderTracking.Gps.Dao.Domain.Gateview)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Gateview;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("ViewnameText.Text","Gateview.Viewname");
				BindingManager.AddBinding("ViewdescriptionText.Text","Gateview.Viewdescription");
				BindingManager.AddBinding("StatusfilterText.Text","Gateview.Statusfilter");
				BindingManager.AddBinding("MatchalltagsText.Text","Gateview.Matchalltags");
				BindingManager.AddBinding("BotypeText.Text","Gateview.Botype");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindGateviewDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindGateviewDropdownsAndDivSelectRegions();
		}


		
		
		private void BindGateviewDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveGateview(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Gateview);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




