
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Mobilenetwork

	/// <summary>
	/// Mobilenetwork form for NHibernate mapped table 'mobile_network'.
	/// </summary>
	public partial class MobilenetworkForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Mobilenetwork _Mobilenetwork;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Mobilenetwork Mobilenetwork
		{
			set { _Mobilenetwork = value; }
			get { return _Mobilenetwork; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Mobilenetwork = new OrderTracking.Gps.Dao.Domain.Mobilenetwork ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Mobilenetwork = (OrderTracking.Gps.Dao.Domain.Mobilenetwork)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Mobilenetwork;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("OperatorText.Text","Mobilenetwork.Operator");
				BindingManager.AddBinding("UsernameText.Text","Mobilenetwork.Username");
				BindingManager.AddBinding("PasswordText.Text","Mobilenetwork.Password");
				BindingManager.AddBinding("ApnText.Text","Mobilenetwork.Apn");
				BindingManager.AddBinding("Dns1Text.Text","Mobilenetwork.Dns1");
				BindingManager.AddBinding("Dns2Text.Text","Mobilenetwork.Dns2");
				BindingManager.AddBinding("DescriptionText.Text","Mobilenetwork.Description");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindMobilenetworkDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindMobilenetworkDropdownsAndDivSelectRegions();
		}


		
		
		private void BindMobilenetworkDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveMobilenetwork(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Mobilenetwork);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




