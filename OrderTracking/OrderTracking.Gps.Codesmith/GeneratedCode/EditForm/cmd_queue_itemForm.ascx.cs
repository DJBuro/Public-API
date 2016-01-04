
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Cmdqueueitem

	/// <summary>
	/// Cmdqueueitem form for NHibernate mapped table 'cmd_queue_item'.
	/// </summary>
	public partial class CmdqueueitemForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Cmdqueueitem _Cmdqueueitem;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Cmdqueueitem Cmdqueueitem
		{
			set { _Cmdqueueitem = value; }
			get { return _Cmdqueueitem; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Cmdqueueitem = new OrderTracking.Gps.Dao.Domain.Cmdqueueitem ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Cmdqueueitem = (OrderTracking.Gps.Dao.Domain.Cmdqueueitem)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Cmdqueueitem;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("GatecommandidText.Text","Cmdqueueitem.Gatecommandid");
				BindingManager.AddBinding("GatecommandnameText.Text","Cmdqueueitem.Gatecommandname");
				BindingManager.AddBinding("StepcurrentText.Text","Cmdqueueitem.Stepcurrent");
				BindingManager.AddBinding("StepmaxText.Text","Cmdqueueitem.Stepmax");
				BindingManager.AddBinding("StepdescText.Text","Cmdqueueitem.Stepdesc");
				BindingManager.AddBinding("ErrordescText.Text","Cmdqueueitem.Errordesc");
				BindingManager.AddBinding("CustomstateText.Text","Cmdqueueitem.Customstate");
				BindingManager.AddBinding("DeliverystatusText.Text","Cmdqueueitem.Deliverystatus");
				BindingManager.AddBinding("OutgoingText.Text","Cmdqueueitem.Outgoing");
				BindingManager.AddBinding("TimestampclientText.Text","Cmdqueueitem.Timestampclient");
				BindingManager.AddBinding("TimestampqueuedText.Text","Cmdqueueitem.Timestampqueued");
				BindingManager.AddBinding("TimestampdeliveredText.Text","Cmdqueueitem.Timestampdelivered");
				BindingManager.AddBinding("TimestamplasttryText.Text","Cmdqueueitem.Timestamplasttry");
				BindingManager.AddBinding("RetrycountText.Text","Cmdqueueitem.Retrycount");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindCmdqueueitemDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindCmdqueueitemDropdownsAndDivSelectRegions();
		}


		
		
		private void BindCmdqueueitemDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveCmdqueueitem(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Cmdqueueitem);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




