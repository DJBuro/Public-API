
using System;
using Cacd.Web.Common.Core;


namespace Loyalty.Data
{
	#region TblLoyaltyCard

	/// <summary>
	/// TblLoyaltyCard form for NHibernate mapped table 'tbl_LoyaltyCard'.
	/// </summary>
	public partial class TblLoyaltyCardForm : BaseUserControl
		{
		#region Fields
		
		private Loyalty.Dao.Domain.TblLoyaltyCard _TblLoyaltyCard;

			
		#endregion
		
		#region Properties

		public Loyalty.Dao.Domain.TblLoyaltyCard TblLoyaltyCard
		{
			set { _TblLoyaltyCard = value; }
			get { return _TblLoyaltyCard; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_TblLoyaltyCard = new Loyalty.Dao.Domain.TblLoyaltyCard ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _TblLoyaltyCard = (Loyalty.Dao.Domain.TblLoyaltyCard)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _TblLoyaltyCard;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("CardNumberText.Text","TblLoyaltyCard.CardNumber");
				BindingManager.AddBinding("DateTimeCreatedText.Text","TblLoyaltyCard.DateTimeCreated");
				BindingManager.AddBinding("PinText.Text","TblLoyaltyCard.Pin");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindTblLoyaltyCardDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindTblLoyaltyCardDropdownsAndDivSelectRegions();
		}


		
		
		private void BindTblLoyaltyCardDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveTblLoyaltyCard(object sender, EventArgs e)
			{
				if (Validate()){
					LoyaltyDAO.Save(TblLoyaltyCard);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




