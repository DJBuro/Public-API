
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Gateeventmessagespan

	/// <summary>
	/// Gateeventmessagespan form for NHibernate mapped table 'gate_event_message_span'.
	/// </summary>
	public partial class GateeventmessagespanForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Gateeventmessagespan _Gateeventmessagespan;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Gateeventmessagespan Gateeventmessagespan
		{
			set { _Gateeventmessagespan = value; }
			get { return _Gateeventmessagespan; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Gateeventmessagespan = new OrderTracking.Gps.Dao.Domain.Gateeventmessagespan ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Gateeventmessagespan = (OrderTracking.Gps.Dao.Domain.Gateeventmessagespan)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Gateeventmessagespan;
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
				BindGateeventmessagespanDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindGateeventmessagespanDropdownsAndDivSelectRegions();
		}


		
		
		private void BindGateeventmessagespanDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveGateeventmessagespan(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Gateeventmessagespan);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




