
using System;
using Cacd.Web.Common.Core;


namespace Loyalty.Data
{
	#region RamesesAddress

	/// <summary>
	/// RamesesAddress form for NHibernate mapped table 'tbl_RamesesAddress'.
	/// </summary>
	public partial class RamesesAddressForm : BaseUserControl
		{
		#region Fields
		
		private Loyalty.Dao.Domain.RamesesAddress _RamesesAddress;

			
		#endregion
		
		#region Properties

		public Loyalty.Dao.Domain.RamesesAddress RamesesAddress
		{
			set { _RamesesAddress = value; }
			get { return _RamesesAddress; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_RamesesAddress = new Loyalty.Dao.Domain.RamesesAddress ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _RamesesAddress = (Loyalty.Dao.Domain.RamesesAddress)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _RamesesAddress;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("ContactIDText.Text","RamesesAddress.ContactID");
				BindingManager.AddBinding("ContactText.Text","RamesesAddress.Contact");
				BindingManager.AddBinding("AddressTypeText.Text","RamesesAddress.AddressType");
				BindingManager.AddBinding("OptFlagText.Text","RamesesAddress.OptFlag");
				BindingManager.AddBinding("AddressIDText.Text","RamesesAddress.AddressID");
				BindingManager.AddBinding("PostOfficeIDText.Text","RamesesAddress.PostOfficeID");
				BindingManager.AddBinding("SubAddressText.Text","RamesesAddress.SubAddress");
				BindingManager.AddBinding("Org1Text.Text","RamesesAddress.Org1");
				BindingManager.AddBinding("Org2Text.Text","RamesesAddress.Org2");
				BindingManager.AddBinding("Org3Text.Text","RamesesAddress.Org3");
				BindingManager.AddBinding("Prem1Text.Text","RamesesAddress.Prem1");
				BindingManager.AddBinding("Prem2Text.Text","RamesesAddress.Prem2");
				BindingManager.AddBinding("Prem3Text.Text","RamesesAddress.Prem3");
				BindingManager.AddBinding("RoadNumText.Text","RamesesAddress.RoadNum");
				BindingManager.AddBinding("RoadNameText.Text","RamesesAddress.RoadName");
				BindingManager.AddBinding("LocalityText.Text","RamesesAddress.Locality");
				BindingManager.AddBinding("TownText.Text","RamesesAddress.Town");
				BindingManager.AddBinding("CountyText.Text","RamesesAddress.County");
				BindingManager.AddBinding("PostCodeText.Text","RamesesAddress.PostCode");
				BindingManager.AddBinding("GridText.Text","RamesesAddress.Grid");
				BindingManager.AddBinding("RefnoText.Text","RamesesAddress.Refno");
				BindingManager.AddBinding("DirectionsText.Text","RamesesAddress.Directions");
				BindingManager.AddBinding("DpsText.Text","RamesesAddress.Dps");
				BindingManager.AddBinding("PafTypeText.Text","RamesesAddress.PafType");
				BindingManager.AddBinding("FlagsText.Text","RamesesAddress.Flags");
				BindingManager.AddBinding("TimesOrderedText.Text","RamesesAddress.TimesOrdered");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindRamesesAddressDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindRamesesAddressDropdownsAndDivSelectRegions();
		}


		
		
		private void BindRamesesAddressDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveRamesesAddress(object sender, EventArgs e)
			{
				if (Validate()){
					LoyaltyDAO.Save(RamesesAddress);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




