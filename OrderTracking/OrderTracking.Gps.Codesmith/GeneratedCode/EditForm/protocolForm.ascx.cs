
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Protocol

	/// <summary>
	/// Protocol form for NHibernate mapped table 'protocol'.
	/// </summary>
	public partial class ProtocolForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Protocol _Protocol;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Protocol Protocol
		{
			set { _Protocol = value; }
			get { return _Protocol; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Protocol = new OrderTracking.Gps.Dao.Domain.Protocol ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Protocol = (OrderTracking.Gps.Dao.Domain.Protocol)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Protocol;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("BotypeText.Text","Protocol.Botype");
				BindingManager.AddBinding("NameText.Text","Protocol.Name");
				BindingManager.AddBinding("DescriptionText.Text","Protocol.Description");
				BindingManager.AddBinding("AdapterbotypeText.Text","Protocol.Adapterbotype");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindProtocolDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindProtocolDropdownsAndDivSelectRegions();
		}


		
		
		private void BindProtocolDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveProtocol(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Protocol);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




