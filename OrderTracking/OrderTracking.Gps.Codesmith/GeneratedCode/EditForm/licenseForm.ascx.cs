
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region License

	/// <summary>
	/// License form for NHibernate mapped table 'license'.
	/// </summary>
	public partial class LicenseForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.License _License;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.License License
		{
			set { _License = value; }
			get { return _License; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_License = new OrderTracking.Gps.Dao.Domain.License ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _License = (OrderTracking.Gps.Dao.Domain.License)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _License;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("LicensedusersText.Text","License.Licensedusers");
				BindingManager.AddBinding("LicenseidText.Text","License.Licenseid");
				BindingManager.AddBinding("CustomeridText.Text","License.Customerid");
				BindingManager.AddBinding("DescriptionText.Text","License.Description");
				BindingManager.AddBinding("SignatureText.Text","License.Signature");
				BindingManager.AddBinding("EmailText.Text","License.Email");
				BindingManager.AddBinding("BotypeText.Text","License.Botype");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindLicenseDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindLicenseDropdownsAndDivSelectRegions();
		}


		
		
		private void BindLicenseDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveLicense(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(License);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




