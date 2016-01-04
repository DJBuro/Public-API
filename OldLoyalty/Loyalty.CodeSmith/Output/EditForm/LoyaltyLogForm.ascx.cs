
using System;
using Cacd.Web.Common.Core;


namespace Loyalty.Data
{
	#region LoyaltyLog

	/// <summary>
	/// LoyaltyLog form for NHibernate mapped table 'tbl_LoyaltyLog'.
	/// </summary>
	public partial class LoyaltyLogForm : BaseUserControl
		{
		#region Fields
		
		private Loyalty.Dao.Domain.LoyaltyLog _LoyaltyLog;

			
		#endregion
		
		#region Properties

		public Loyalty.Dao.Domain.LoyaltyLog LoyaltyLog
		{
			set { _LoyaltyLog = value; }
			get { return _LoyaltyLog; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_LoyaltyLog = new Loyalty.Dao.Domain.LoyaltyLog ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _LoyaltyLog = (Loyalty.Dao.Domain.LoyaltyLog)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _LoyaltyLog;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("DateTimeCreatedText.Text","LoyaltyLog.DateTimeCreated");
				BindingManager.AddBinding("SiteIdText.Text","LoyaltyLog.SiteId");
				BindingManager.AddBinding("SeverityText.Text","LoyaltyLog.Severity");
				BindingManager.AddBinding("MessageText.Text","LoyaltyLog.Message");
				BindingManager.AddBinding("MethodText.Text","LoyaltyLog.Method");
				BindingManager.AddBinding("VariablesText.Text","LoyaltyLog.Variables");
				BindingManager.AddBinding("SourceText.Text","LoyaltyLog.Source");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindLoyaltyLogDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindLoyaltyLogDropdownsAndDivSelectRegions();
		}


		
		
		private void BindLoyaltyLogDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveLoyaltyLog(object sender, EventArgs e)
			{
				if (Validate()){
					LoyaltyDAO.Save(LoyaltyLog);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




