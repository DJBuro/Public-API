
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Msgnamespace

	/// <summary>
	/// Msgnamespace form for NHibernate mapped table 'msg_namespace'.
	/// </summary>
	public partial class MsgnamespaceForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Msgnamespace _Msgnamespace;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Msgnamespace Msgnamespace
		{
			set { _Msgnamespace = value; }
			get { return _Msgnamespace; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Msgnamespace = new OrderTracking.Gps.Dao.Domain.Msgnamespace ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Msgnamespace = (OrderTracking.Gps.Dao.Domain.Msgnamespace)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Msgnamespace;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("NameText.Text","Msgnamespace.Name");
				BindingManager.AddBinding("DescriptionText.Text","Msgnamespace.Description");
				BindingManager.AddBinding("ProtocolidText.Text","Msgnamespace.Protocolid");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindMsgnamespaceDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindMsgnamespaceDropdownsAndDivSelectRegions();
		}


		
		
		private void BindMsgnamespaceDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveMsgnamespace(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Msgnamespace);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




