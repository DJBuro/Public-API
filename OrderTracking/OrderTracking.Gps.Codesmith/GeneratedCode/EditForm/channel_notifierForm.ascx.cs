
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Channelnotifier

	/// <summary>
	/// Channelnotifier form for NHibernate mapped table 'channel_notifier'.
	/// </summary>
	public partial class ChannelnotifierForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Channelnotifier _Channelnotifier;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Channelnotifier Channelnotifier
		{
			set { _Channelnotifier = value; }
			get { return _Channelnotifier; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Channelnotifier = new OrderTracking.Gps.Dao.Domain.Channelnotifier ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Channelnotifier = (OrderTracking.Gps.Dao.Domain.Channelnotifier)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Channelnotifier;
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
				BindChannelnotifierDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindChannelnotifierDropdownsAndDivSelectRegions();
		}


		
		
		private void BindChannelnotifierDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveChannelnotifier(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Channelnotifier);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




