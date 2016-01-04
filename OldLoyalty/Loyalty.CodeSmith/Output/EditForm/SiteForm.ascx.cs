
using System;
using Cacd.Web.Common.Core;


namespace Loyalty.Data
{
	#region Site

	/// <summary>
	/// Site form for NHibernate mapped table 'tbl_Site'.
	/// </summary>
	public partial class SiteForm : BaseUserControl
		{
		#region Fields
		
		private Loyalty.Dao.Domain.Site _Site;

			
		#endregion
		
		#region Properties

		public Loyalty.Dao.Domain.Site Site
		{
			set { _Site = value; }
			get { return _Site; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Site = new Loyalty.Dao.Domain.Site ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Site = (Loyalty.Dao.Domain.Site)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Site;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("RamesesSiteIdText.Text","Site.RamesesSiteId");
				BindingManager.AddBinding("NameText.Text","Site.Name");
				BindingManager.AddBinding("SiteKeyText.Text","Site.SiteKey");
				BindingManager.AddBinding("SitePasswordText.Text","Site.SitePassword");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindSiteDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindSiteDropdownsAndDivSelectRegions();
		}


		
		
		private void BindSiteDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveSite(object sender, EventArgs e)
			{
				if (Validate()){
					LoyaltyDAO.Save(Site);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




