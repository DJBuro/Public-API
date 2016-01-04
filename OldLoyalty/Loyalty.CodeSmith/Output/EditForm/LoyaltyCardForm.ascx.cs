
using System;
using Cacd.Web.Common.Core;


namespace Loyalty.Data
{
	#region LoyaltyCard

	/// <summary>
	/// LoyaltyCard form for NHibernate mapped table 'tbl_LoyaltyCard'.
	/// </summary>
	public partial class LoyaltyCardForm : BaseUserControl
		{
		#region Fields
		
		private Loyalty.Dao.Domain.LoyaltyCard _LoyaltyCard;

			
		#endregion
		
		#region Properties

		public Loyalty.Dao.Domain.LoyaltyCard LoyaltyCard
		{
			set { _LoyaltyCard = value; }
			get { return _LoyaltyCard; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_LoyaltyCard = new Loyalty.Dao.Domain.LoyaltyCard ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _LoyaltyCard = (Loyalty.Dao.Domain.LoyaltyCard)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _LoyaltyCard;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("CardNumberText.Text","LoyaltyCard.CardNumber");
				BindingManager.AddBinding("DateTimeCreatedText.Text","LoyaltyCard.DateTimeCreated");
				BindingManager.AddBinding("PinText.Text","LoyaltyCard.Pin");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindLoyaltyCardDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindLoyaltyCardDropdownsAndDivSelectRegions();
		}


		
		
		private void BindLoyaltyCardDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveLoyaltyCard(object sender, EventArgs e)
			{
				if (Validate()){
					LoyaltyDAO.Save(LoyaltyCard);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




