
using System;
using Cacd.Web.Common.Core;


namespace Loyalty.Data
{
	#region TblSite

	/// <summary>
	/// TblSite form for NHibernate mapped table 'tbl_Site'.
	/// </summary>
	public partial class TblSiteForm : BaseUserControl
		{
		#region Fields
		
		private Loyalty.Dao.Domain.TblSite _TblSite;

			
		#endregion
		
		#region Properties

		public Loyalty.Dao.Domain.TblSite TblSite
		{
			set { _TblSite = value; }
			get { return _TblSite; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_TblSite = new Loyalty.Dao.Domain.TblSite ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _TblSite = (Loyalty.Dao.Domain.TblSite)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _TblSite;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("NameText.Text","TblSite.Name");
				BindingManager.AddBinding("SiteKeyText.Text","TblSite.SiteKey");
				BindingManager.AddBinding("SitePasswordText.Text","TblSite.SitePassword");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindTblSiteDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindTblSiteDropdownsAndDivSelectRegions();
		}


		
		
		private void BindTblSiteDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveTblSite(object sender, EventArgs e)
			{
				if (Validate()){
					LoyaltyDAO.Save(TblSite);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




