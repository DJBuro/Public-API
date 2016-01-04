
using System;
using Cacd.Web.Common.Core;


namespace Loyalty.Data
{
	#region AccountAddress

	/// <summary>
	/// AccountAddress form for NHibernate mapped table 'tbl_AccountAddress'.
	/// </summary>
	public partial class AccountAddressForm : BaseUserControl
		{
		#region Fields
		
		private Loyalty.Dao.Domain.AccountAddress _AccountAddress;

			
		#endregion
		
		#region Properties

		public Loyalty.Dao.Domain.AccountAddress AccountAddress
		{
			set { _AccountAddress = value; }
			get { return _AccountAddress; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_AccountAddress = new Loyalty.Dao.Domain.AccountAddress ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _AccountAddress = (Loyalty.Dao.Domain.AccountAddress)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _AccountAddress;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("AddressLineOneText.Text","AccountAddress.AddressLineOne");
				BindingManager.AddBinding("AddressLineTwoText.Text","AccountAddress.AddressLineTwo");
				BindingManager.AddBinding("AddressLineThreeText.Text","AccountAddress.AddressLineThree");
				BindingManager.AddBinding("AddressLineFourText.Text","AccountAddress.AddressLineFour");
				BindingManager.AddBinding("TownCityText.Text","AccountAddress.TownCity");
				BindingManager.AddBinding("CountyProvinceText.Text","AccountAddress.CountyProvince");
				BindingManager.AddBinding("PostCodeText.Text","AccountAddress.PostCode");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindAccountAddressDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindAccountAddressDropdownsAndDivSelectRegions();
		}


		
		
		private void BindAccountAddressDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveAccountAddress(object sender, EventArgs e)
			{
				if (Validate()){
					LoyaltyDAO.Save(AccountAddress);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




