
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Gateeventargument

	/// <summary>
	/// Gateeventargument form for NHibernate mapped table 'gate_event_argument'.
	/// </summary>
	public partial class GateeventargumentForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Gateeventargument _Gateeventargument;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Gateeventargument Gateeventargument
		{
			set { _Gateeventargument = value; }
			get { return _Gateeventargument; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Gateeventargument = new OrderTracking.Gps.Dao.Domain.Gateeventargument ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Gateeventargument = (OrderTracking.Gps.Dao.Domain.Gateeventargument)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Gateeventargument;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("BotypeText.Text","Gateeventargument.Botype");
				BindingManager.AddBinding("ArgumentdescriptionText.Text","Gateeventargument.Argumentdescription");
				BindingManager.AddBinding("ValuedataText.Text","Gateeventargument.Valuedata");
				BindingManager.AddBinding("ValuetypeText.Text","Gateeventargument.Valuetype");
				BindingManager.AddBinding("LocalizationkeyText.Text","Gateeventargument.Localizationkey");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindGateeventargumentDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindGateeventargumentDropdownsAndDivSelectRegions();
		}


		
		
		private void BindGateeventargumentDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveGateeventargument(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Gateeventargument);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




