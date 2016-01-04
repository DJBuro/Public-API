
using System;
using Cacd.Web.Common.Core;


namespace Loyalty.Data
{
	#region TblRamesesAddress

	/// <summary>
	/// TblRamesesAddress form for NHibernate mapped table 'tbl_RamesesAddress'.
	/// </summary>
	public partial class TblRamesesAddressForm : BaseUserControl
		{
		#region Fields
		
		private Loyalty.Dao.Domain.TblRamesesAddress _TblRamesesAddress;

			
		#endregion
		
		#region Properties

		public Loyalty.Dao.Domain.TblRamesesAddress TblRamesesAddress
		{
			set { _TblRamesesAddress = value; }
			get { return _TblRamesesAddress; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_TblRamesesAddress = new Loyalty.Dao.Domain.TblRamesesAddress ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _TblRamesesAddress = (Loyalty.Dao.Domain.TblRamesesAddress)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _TblRamesesAddress;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("ContactIDText.Text","TblRamesesAddress.ContactID");
				BindingManager.AddBinding("ContactText.Text","TblRamesesAddress.Contact");
				BindingManager.AddBinding("AddressTypeText.Text","TblRamesesAddress.AddressType");
				BindingManager.AddBinding("OptFlagText.Text","TblRamesesAddress.OptFlag");
				BindingManager.AddBinding("AddressIDText.Text","TblRamesesAddress.AddressID");
				BindingManager.AddBinding("PostOfficeIDText.Text","TblRamesesAddress.PostOfficeID");
				BindingManager.AddBinding("SubAddressText.Text","TblRamesesAddress.SubAddress");
				BindingManager.AddBinding("Org1Text.Text","TblRamesesAddress.Org1");
				BindingManager.AddBinding("Org2Text.Text","TblRamesesAddress.Org2");
				BindingManager.AddBinding("Org3Text.Text","TblRamesesAddress.Org3");
				BindingManager.AddBinding("Prem1Text.Text","TblRamesesAddress.Prem1");
				BindingManager.AddBinding("Prem2Text.Text","TblRamesesAddress.Prem2");
				BindingManager.AddBinding("Prem3Text.Text","TblRamesesAddress.Prem3");
				BindingManager.AddBinding("RoadNumText.Text","TblRamesesAddress.RoadNum");
				BindingManager.AddBinding("RoadNameText.Text","TblRamesesAddress.RoadName");
				BindingManager.AddBinding("LocalityText.Text","TblRamesesAddress.Locality");
				BindingManager.AddBinding("TownText.Text","TblRamesesAddress.Town");
				BindingManager.AddBinding("CountyText.Text","TblRamesesAddress.County");
				BindingManager.AddBinding("PostCodeText.Text","TblRamesesAddress.PostCode");
				BindingManager.AddBinding("GridText.Text","TblRamesesAddress.Grid");
				BindingManager.AddBinding("RefnoText.Text","TblRamesesAddress.Refno");
				BindingManager.AddBinding("DirectionsText.Text","TblRamesesAddress.Directions");
				BindingManager.AddBinding("DpsText.Text","TblRamesesAddress.Dps");
				BindingManager.AddBinding("PafTypeText.Text","TblRamesesAddress.PafType");
				BindingManager.AddBinding("FlagsText.Text","TblRamesesAddress.Flags");
				BindingManager.AddBinding("TimesOrderedText.Text","TblRamesesAddress.TimesOrdered");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindTblRamesesAddressDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindTblRamesesAddressDropdownsAndDivSelectRegions();
		}


		
		
		private void BindTblRamesesAddressDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveTblRamesesAddress(object sender, EventArgs e)
			{
				if (Validate()){
					LoyaltyDAO.Save(TblRamesesAddress);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




