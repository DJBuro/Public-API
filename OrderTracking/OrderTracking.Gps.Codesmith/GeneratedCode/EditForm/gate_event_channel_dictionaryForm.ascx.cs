
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Gateeventchanneldictionary

	/// <summary>
	/// Gateeventchanneldictionary form for NHibernate mapped table 'gate_event_channel_dictionary'.
	/// </summary>
	public partial class GateeventchanneldictionaryForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Gateeventchanneldictionary _Gateeventchanneldictionary;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Gateeventchanneldictionary Gateeventchanneldictionary
		{
			set { _Gateeventchanneldictionary = value; }
			get { return _Gateeventchanneldictionary; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Gateeventchanneldictionary = new OrderTracking.Gps.Dao.Domain.Gateeventchanneldictionary ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Gateeventchanneldictionary = (OrderTracking.Gps.Dao.Domain.Gateeventchanneldictionary)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Gateeventchanneldictionary;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindGateeventchanneldictionaryDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindGateeventchanneldictionaryDropdownsAndDivSelectRegions();
		}


		
		
		private void BindGateeventchanneldictionaryDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveGateeventchanneldictionary(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Gateeventchanneldictionary);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




