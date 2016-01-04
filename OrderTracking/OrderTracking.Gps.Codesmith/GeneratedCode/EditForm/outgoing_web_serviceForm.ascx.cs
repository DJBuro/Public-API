
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Outgoingwebservice

	/// <summary>
	/// Outgoingwebservice form for NHibernate mapped table 'outgoing_web_service'.
	/// </summary>
	public partial class OutgoingwebserviceForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Outgoingwebservice _Outgoingwebservice;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Outgoingwebservice Outgoingwebservice
		{
			set { _Outgoingwebservice = value; }
			get { return _Outgoingwebservice; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Outgoingwebservice = new OrderTracking.Gps.Dao.Domain.Outgoingwebservice ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Outgoingwebservice = (OrderTracking.Gps.Dao.Domain.Outgoingwebservice)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Outgoingwebservice;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("NamespaceText.Text","Outgoingwebservice.Namespace");
				BindingManager.AddBinding("UrlText.Text","Outgoingwebservice.Url");
				BindingManager.AddBinding("UsernameText.Text","Outgoingwebservice.Username");
				BindingManager.AddBinding("PasswordText.Text","Outgoingwebservice.Password");
				BindingManager.AddBinding("CallintervalText.Text","Outgoingwebservice.Callinterval");
				BindingManager.AddBinding("CustomlongText.Text","Outgoingwebservice.Customlong");
				BindingManager.AddBinding("CustomstringText.Text","Outgoingwebservice.Customstring");
				BindingManager.AddBinding("TimeoutText.Text","Outgoingwebservice.Timeout");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindOutgoingwebserviceDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindOutgoingwebserviceDropdownsAndDivSelectRegions();
		}


		
		
		private void BindOutgoingwebserviceDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveOutgoingwebservice(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Outgoingwebservice);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




