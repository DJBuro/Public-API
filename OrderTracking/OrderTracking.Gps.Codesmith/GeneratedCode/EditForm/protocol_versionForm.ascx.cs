
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Protocolversion

	/// <summary>
	/// Protocolversion form for NHibernate mapped table 'protocol_version'.
	/// </summary>
	public partial class ProtocolversionForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Protocolversion _Protocolversion;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Protocolversion Protocolversion
		{
			set { _Protocolversion = value; }
			get { return _Protocolversion; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Protocolversion = new OrderTracking.Gps.Dao.Domain.Protocolversion ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Protocolversion = (OrderTracking.Gps.Dao.Domain.Protocolversion)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Protocolversion;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("BotypeText.Text","Protocolversion.Botype");
				BindingManager.AddBinding("ProtocolidText.Text","Protocolversion.Protocolid");
				BindingManager.AddBinding("MajorversionText.Text","Protocolversion.Majorversion");
				BindingManager.AddBinding("MinorversionText.Text","Protocolversion.Minorversion");
				BindingManager.AddBinding("ClientnameText.Text","Protocolversion.Clientname");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindProtocolversionDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindProtocolversionDropdownsAndDivSelectRegions();
		}


		
		
		private void BindProtocolversionDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveProtocolversion(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Protocolversion);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




