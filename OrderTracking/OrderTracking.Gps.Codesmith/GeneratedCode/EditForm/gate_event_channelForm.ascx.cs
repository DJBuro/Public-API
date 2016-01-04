
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Gateeventchannel

	/// <summary>
	/// Gateeventchannel form for NHibernate mapped table 'gate_event_channel'.
	/// </summary>
	public partial class GateeventchannelForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Gateeventchannel _Gateeventchannel;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Gateeventchannel Gateeventchannel
		{
			set { _Gateeventchannel = value; }
			get { return _Gateeventchannel; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Gateeventchannel = new OrderTracking.Gps.Dao.Domain.Gateeventchannel ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Gateeventchannel = (OrderTracking.Gps.Dao.Domain.Gateeventchannel)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Gateeventchannel;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("NameText.Text","Gateeventchannel.Name");
				BindingManager.AddBinding("DescriptionText.Text","Gateeventchannel.Description");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindGateeventchannelDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindGateeventchannelDropdownsAndDivSelectRegions();
		}


		
		
		private void BindGateeventchannelDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveGateeventchannel(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Gateeventchannel);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




