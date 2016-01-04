
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Providermessage

	/// <summary>
	/// Providermessage form for NHibernate mapped table 'provider_message'.
	/// </summary>
	public partial class ProvidermessageForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Providermessage _Providermessage;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Providermessage Providermessage
		{
			set { _Providermessage = value; }
			get { return _Providermessage; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Providermessage = new OrderTracking.Gps.Dao.Domain.Providermessage ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Providermessage = (OrderTracking.Gps.Dao.Domain.Providermessage)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Providermessage;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("ClientdeviceidText.Text","Providermessage.Clientdeviceid");
				BindingManager.AddBinding("ClientaddressText.Text","Providermessage.Clientaddress");
				BindingManager.AddBinding("MessageText.Text","Providermessage.Message");
				BindingManager.AddBinding("DeliverystatusText.Text","Providermessage.Deliverystatus");
				BindingManager.AddBinding("OutgoingText.Text","Providermessage.Outgoing");
				BindingManager.AddBinding("TimestampclientText.Text","Providermessage.Timestampclient");
				BindingManager.AddBinding("TimestampqueuedText.Text","Providermessage.Timestampqueued");
				BindingManager.AddBinding("TimestampdeliveredText.Text","Providermessage.Timestampdelivered");
				BindingManager.AddBinding("TimestamplasttryText.Text","Providermessage.Timestamplasttry");
				BindingManager.AddBinding("RetrycountText.Text","Providermessage.Retrycount");
				BindingManager.AddBinding("TransportText.Text","Providermessage.Transport");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindProvidermessageDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindProvidermessageDropdownsAndDivSelectRegions();
		}


		
		
		private void BindProvidermessageDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveProvidermessage(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Providermessage);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




