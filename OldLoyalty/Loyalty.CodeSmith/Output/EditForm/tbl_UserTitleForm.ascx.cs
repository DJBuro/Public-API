
using System;
using Cacd.Web.Common.Core;


namespace Loyalty.Data
{
	#region TblUserTitle

	/// <summary>
	/// TblUserTitle form for NHibernate mapped table 'tbl_UserTitle'.
	/// </summary>
	public partial class TblUserTitleForm : BaseUserControl
		{
		#region Fields
		
		private Loyalty.Dao.Domain.TblUserTitle _TblUserTitle;

			
		#endregion
		
		#region Properties

		public Loyalty.Dao.Domain.TblUserTitle TblUserTitle
		{
			set { _TblUserTitle = value; }
			get { return _TblUserTitle; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_TblUserTitle = new Loyalty.Dao.Domain.TblUserTitle ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _TblUserTitle = (Loyalty.Dao.Domain.TblUserTitle)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _TblUserTitle;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("TitleText.Text","TblUserTitle.Title");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindTblUserTitleDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindTblUserTitleDropdownsAndDivSelectRegions();
		}


		
		
		private void BindTblUserTitleDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveTblUserTitle(object sender, EventArgs e)
			{
				if (Validate()){
					LoyaltyDAO.Save(TblUserTitle);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




