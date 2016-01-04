
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Smsproxyqueue

	/// <summary>
	/// Smsproxyqueue form for NHibernate mapped table 'sms_proxy_queue'.
	/// </summary>
	public partial class SmsproxyqueueForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Smsproxyqueue _Smsproxyqueue;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Smsproxyqueue Smsproxyqueue
		{
			set { _Smsproxyqueue = value; }
			get { return _Smsproxyqueue; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Smsproxyqueue = new OrderTracking.Gps.Dao.Domain.Smsproxyqueue ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Smsproxyqueue = (OrderTracking.Gps.Dao.Domain.Smsproxyqueue)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Smsproxyqueue;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("SmsproxyidText.Text","Smsproxyqueue.Smsproxyid");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindSmsproxyqueueDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindSmsproxyqueueDropdownsAndDivSelectRegions();
		}


		
		
		private void BindSmsproxyqueueDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveSmsproxyqueue(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Smsproxyqueue);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




