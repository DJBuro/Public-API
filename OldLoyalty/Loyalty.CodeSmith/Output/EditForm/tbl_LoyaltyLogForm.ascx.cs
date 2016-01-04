
using System;
using Cacd.Web.Common.Core;


namespace Loyalty.Data
{
	#region TblLoyaltyLog

	/// <summary>
	/// TblLoyaltyLog form for NHibernate mapped table 'tbl_LoyaltyLog'.
	/// </summary>
	public partial class TblLoyaltyLogForm : BaseUserControl
		{
		#region Fields
		
		private Loyalty.Dao.Domain.TblLoyaltyLog _TblLoyaltyLog;

			
		#endregion
		
		#region Properties

		public Loyalty.Dao.Domain.TblLoyaltyLog TblLoyaltyLog
		{
			set { _TblLoyaltyLog = value; }
			get { return _TblLoyaltyLog; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_TblLoyaltyLog = new Loyalty.Dao.Domain.TblLoyaltyLog ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _TblLoyaltyLog = (Loyalty.Dao.Domain.TblLoyaltyLog)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _TblLoyaltyLog;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("DateTimeCreatedText.Text","TblLoyaltyLog.DateTimeCreated");
				BindingManager.AddBinding("SiteIdText.Text","TblLoyaltyLog.SiteId");
				BindingManager.AddBinding("SeverityText.Text","TblLoyaltyLog.Severity");
				BindingManager.AddBinding("MessageText.Text","TblLoyaltyLog.Message");
				BindingManager.AddBinding("MethodText.Text","TblLoyaltyLog.Method");
				BindingManager.AddBinding("VariablesText.Text","TblLoyaltyLog.Variables");
				BindingManager.AddBinding("SourceText.Text","TblLoyaltyLog.Source");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindTblLoyaltyLogDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindTblLoyaltyLogDropdownsAndDivSelectRegions();
		}


		
		
		private void BindTblLoyaltyLogDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveTblLoyaltyLog(object sender, EventArgs e)
			{
				if (Validate()){
					LoyaltyDAO.Save(TblLoyaltyLog);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




