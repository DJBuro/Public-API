
using System;
using Cacd.Web.Common.Core;


namespace Loyalty.Data
{
	#region LoyaltyCardStatus

	/// <summary>
	/// LoyaltyCardStatus form for NHibernate mapped table 'tbl_LoyaltyCardStatus'.
	/// </summary>
	public partial class LoyaltyCardStatusForm : BaseUserControl
		{
		#region Fields
		
		private Loyalty.Dao.Domain.LoyaltyCardStatus _LoyaltyCardStatus;

			
		#endregion
		
		#region Properties

		public Loyalty.Dao.Domain.LoyaltyCardStatus LoyaltyCardStatus
		{
			set { _LoyaltyCardStatus = value; }
			get { return _LoyaltyCardStatus; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_LoyaltyCardStatus = new Loyalty.Dao.Domain.LoyaltyCardStatus ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _LoyaltyCardStatus = (Loyalty.Dao.Domain.LoyaltyCardStatus)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _LoyaltyCardStatus;
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
				BindLoyaltyCardStatusDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindLoyaltyCardStatusDropdownsAndDivSelectRegions();
		}


		
		
		private void BindLoyaltyCardStatusDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveLoyaltyCardStatus(object sender, EventArgs e)
			{
				if (Validate()){
					LoyaltyDAO.Save(LoyaltyCardStatus);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




