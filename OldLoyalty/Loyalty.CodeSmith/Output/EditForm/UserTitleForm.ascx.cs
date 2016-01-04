
using System;
using Cacd.Web.Common.Core;


namespace Loyalty.Data
{
	#region UserTitle

	/// <summary>
	/// UserTitle form for NHibernate mapped table 'tbl_UserTitle'.
	/// </summary>
	public partial class UserTitleForm : BaseUserControl
		{
		#region Fields
		
		private Loyalty.Dao.Domain.UserTitle _UserTitle;

			
		#endregion
		
		#region Properties

		public Loyalty.Dao.Domain.UserTitle UserTitle
		{
			set { _UserTitle = value; }
			get { return _UserTitle; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_UserTitle = new Loyalty.Dao.Domain.UserTitle ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _UserTitle = (Loyalty.Dao.Domain.UserTitle)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _UserTitle;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("TitleText.Text","UserTitle.Title");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindUserTitleDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindUserTitleDropdownsAndDivSelectRegions();
		}


		
		
		private void BindUserTitleDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveUserTitle(object sender, EventArgs e)
			{
				if (Validate()){
					LoyaltyDAO.Save(UserTitle);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




