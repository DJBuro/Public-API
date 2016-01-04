
using System;
using Cacd.Web.Common.Core;


namespace Loyalty.Data
{
	#region TblCountry

	/// <summary>
	/// TblCountry form for NHibernate mapped table 'tbl_Country'.
	/// </summary>
	public partial class TblCountryForm : BaseUserControl
		{
		#region Fields
		
		private Loyalty.Dao.Domain.TblCountry _TblCountry;

			
		#endregion
		
		#region Properties

		public Loyalty.Dao.Domain.TblCountry TblCountry
		{
			set { _TblCountry = value; }
			get { return _TblCountry; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_TblCountry = new Loyalty.Dao.Domain.TblCountry ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _TblCountry = (Loyalty.Dao.Domain.TblCountry)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _TblCountry;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("NameText.Text","TblCountry.Name");
				BindingManager.AddBinding("ISOCodeText.Text","TblCountry.ISOCode");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindTblCountryDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindTblCountryDropdownsAndDivSelectRegions();
		}


		
		
		private void BindTblCountryDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveTblCountry(object sender, EventArgs e)
			{
				if (Validate()){
					LoyaltyDAO.Save(TblCountry);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




