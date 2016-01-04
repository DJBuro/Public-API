
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Data
{
	#region Apn

	/// <summary>
	/// Apn form for NHibernate mapped table 'tbl_Apn'.
	/// </summary>
	public partial class ApnForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Dao.Domain.Apn _Apn;

			
		#endregion
		
		#region Properties

		public OrderTracking.Dao.Domain.Apn Apn
		{
			set { _Apn = value; }
			get { return _Apn; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Apn = new OrderTracking.Dao.Domain.Apn ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Apn = (OrderTracking.Dao.Domain.Apn)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Apn;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("ProviderText.Text","Apn.Provider");
				BindingManager.AddBinding("NameText.Text","Apn.Name");
				BindingManager.AddBinding("UsernameText.Text","Apn.Username");
				BindingManager.AddBinding("PasswordText.Text","Apn.Password");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindApnDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindApnDropdownsAndDivSelectRegions();
		}


		
		
		private void BindApnDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveApn(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingDAO.Save(Apn);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




