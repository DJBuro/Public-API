
using System;
using Cacd.Web.Common.Core;


namespace Loyalty.Data
{
	#region TblAccountAddress

	/// <summary>
	/// TblAccountAddress form for NHibernate mapped table 'tbl_AccountAddress'.
	/// </summary>
	public partial class TblAccountAddressForm : BaseUserControl
		{
		#region Fields
		
		private Loyalty.Dao.Domain.TblAccountAddress _TblAccountAddress;

			
		#endregion
		
		#region Properties

		public Loyalty.Dao.Domain.TblAccountAddress TblAccountAddress
		{
			set { _TblAccountAddress = value; }
			get { return _TblAccountAddress; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_TblAccountAddress = new Loyalty.Dao.Domain.TblAccountAddress ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _TblAccountAddress = (Loyalty.Dao.Domain.TblAccountAddress)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _TblAccountAddress;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("AddressLineOneText.Text","TblAccountAddress.AddressLineOne");
				BindingManager.AddBinding("AddressLineTwoText.Text","TblAccountAddress.AddressLineTwo");
				BindingManager.AddBinding("AddressLineThreeText.Text","TblAccountAddress.AddressLineThree");
				BindingManager.AddBinding("AddressLineFourText.Text","TblAccountAddress.AddressLineFour");
				BindingManager.AddBinding("TownCityText.Text","TblAccountAddress.TownCity");
				BindingManager.AddBinding("CountyProvinceText.Text","TblAccountAddress.CountyProvince");
				BindingManager.AddBinding("PostCodeText.Text","TblAccountAddress.PostCode");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindTblAccountAddressDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindTblAccountAddressDropdownsAndDivSelectRegions();
		}


		
		
		private void BindTblAccountAddressDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveTblAccountAddress(object sender, EventArgs e)
			{
				if (Validate()){
					LoyaltyDAO.Save(TblAccountAddress);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




