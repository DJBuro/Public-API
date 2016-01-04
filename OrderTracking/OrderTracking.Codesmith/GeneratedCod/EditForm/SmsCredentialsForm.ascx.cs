
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Data
{
	#region SmsCredential

	/// <summary>
	/// SmsCredential form for NHibernate mapped table 'tbl_SmsCredentials'.
	/// </summary>
	public partial class SmsCredentialForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Dao.Domain.SmsCredential _SmsCredential;

			
		#endregion
		
		#region Properties

		public OrderTracking.Dao.Domain.SmsCredential SmsCredential
		{
			set { _SmsCredential = value; }
			get { return _SmsCredential; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_SmsCredential = new OrderTracking.Dao.Domain.SmsCredential ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _SmsCredential = (OrderTracking.Dao.Domain.SmsCredential)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _SmsCredential;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("UsernameText.Text","SmsCredential.Username");
				BindingManager.AddBinding("PasswordText.Text","SmsCredential.Password");
				BindingManager.AddBinding("SmsFromText.Text","SmsCredential.SmsFrom");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindSmsCredentialDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindSmsCredentialDropdownsAndDivSelectRegions();
		}


		
		
		private void BindSmsCredentialDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveSmsCredential(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingDAO.Save(SmsCredential);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




