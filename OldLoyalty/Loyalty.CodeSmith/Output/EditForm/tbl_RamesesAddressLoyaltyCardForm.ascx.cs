
using System;
using Cacd.Web.Common.Core;


namespace Loyalty.Data
{
	#region TblRamesesAddressLoyaltyCard

	/// <summary>
	/// TblRamesesAddressLoyaltyCard form for NHibernate mapped table 'tbl_RamesesAddressLoyaltyCard'.
	/// </summary>
	public partial class TblRamesesAddressLoyaltyCardForm : BaseUserControl
		{
		#region Fields
		
		private Loyalty.Dao.Domain.TblRamesesAddressLoyaltyCard _TblRamesesAddressLoyaltyCard;

			
		#endregion
		
		#region Properties

		public Loyalty.Dao.Domain.TblRamesesAddressLoyaltyCard TblRamesesAddressLoyaltyCard
		{
			set { _TblRamesesAddressLoyaltyCard = value; }
			get { return _TblRamesesAddressLoyaltyCard; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_TblRamesesAddressLoyaltyCard = new Loyalty.Dao.Domain.TblRamesesAddressLoyaltyCard ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _TblRamesesAddressLoyaltyCard = (Loyalty.Dao.Domain.TblRamesesAddressLoyaltyCard)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _TblRamesesAddressLoyaltyCard;
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
				BindTblRamesesAddressLoyaltyCardDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindTblRamesesAddressLoyaltyCardDropdownsAndDivSelectRegions();
		}


		
		
		private void BindTblRamesesAddressLoyaltyCardDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveTblRamesesAddressLoyaltyCard(object sender, EventArgs e)
			{
				if (Validate()){
					LoyaltyDAO.Save(TblRamesesAddressLoyaltyCard);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




