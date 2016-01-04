
using System;
using Cacd.Web.Common.Core;


namespace Loyalty.Data
{
	#region TblLoyaltyCardStatus

	/// <summary>
	/// TblLoyaltyCardStatus form for NHibernate mapped table 'tbl_LoyaltyCardStatus'.
	/// </summary>
	public partial class TblLoyaltyCardStatusForm : BaseUserControl
		{
		#region Fields
		
		private Loyalty.Dao.Domain.TblLoyaltyCardStatus _TblLoyaltyCardStatus;

			
		#endregion
		
		#region Properties

		public Loyalty.Dao.Domain.TblLoyaltyCardStatus TblLoyaltyCardStatus
		{
			set { _TblLoyaltyCardStatus = value; }
			get { return _TblLoyaltyCardStatus; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_TblLoyaltyCardStatus = new Loyalty.Dao.Domain.TblLoyaltyCardStatus ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _TblLoyaltyCardStatus = (Loyalty.Dao.Domain.TblLoyaltyCardStatus)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _TblLoyaltyCardStatus;
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
				BindTblLoyaltyCardStatusDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindTblLoyaltyCardStatusDropdownsAndDivSelectRegions();
		}


		
		
		private void BindTblLoyaltyCardStatusDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveTblLoyaltyCardStatus(object sender, EventArgs e)
			{
				if (Validate()){
					LoyaltyDAO.Save(TblLoyaltyCardStatus);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




