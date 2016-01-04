
using System;
using Cacd.Web.Common.Core;


namespace Loyalty.Data
{
	#region RamesesAddressLoyaltyCard

	/// <summary>
	/// RamesesAddressLoyaltyCard form for NHibernate mapped table 'tbl_RamesesAddressLoyaltyCard'.
	/// </summary>
	public partial class RamesesAddressLoyaltyCardForm : BaseUserControl
		{
		#region Fields
		
		private Loyalty.Dao.Domain.RamesesAddressLoyaltyCard _RamesesAddressLoyaltyCard;

			
		#endregion
		
		#region Properties

		public Loyalty.Dao.Domain.RamesesAddressLoyaltyCard RamesesAddressLoyaltyCard
		{
			set { _RamesesAddressLoyaltyCard = value; }
			get { return _RamesesAddressLoyaltyCard; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_RamesesAddressLoyaltyCard = new Loyalty.Dao.Domain.RamesesAddressLoyaltyCard ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _RamesesAddressLoyaltyCard = (Loyalty.Dao.Domain.RamesesAddressLoyaltyCard)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _RamesesAddressLoyaltyCard;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindRamesesAddressLoyaltyCardDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindRamesesAddressLoyaltyCardDropdownsAndDivSelectRegions();
		}


		
		
		private void BindRamesesAddressLoyaltyCardDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveRamesesAddressLoyaltyCard(object sender, EventArgs e)
			{
				if (Validate()){
					LoyaltyDAO.Save(RamesesAddressLoyaltyCard);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




