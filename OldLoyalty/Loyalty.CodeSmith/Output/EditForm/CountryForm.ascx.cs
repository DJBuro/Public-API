
using System;
using Cacd.Web.Common.Core;


namespace Loyalty.Data
{
	#region Country

	/// <summary>
	/// Country form for NHibernate mapped table 'tbl_Country'.
	/// </summary>
	public partial class CountryForm : BaseUserControl
		{
		#region Fields
		
		private Loyalty.Dao.Domain.Country _Country;

			
		#endregion
		
		#region Properties

		public Loyalty.Dao.Domain.Country Country
		{
			set { _Country = value; }
			get { return _Country; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Country = new Loyalty.Dao.Domain.Country ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Country = (Loyalty.Dao.Domain.Country)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Country;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("NameText.Text","Country.Name");
				BindingManager.AddBinding("ISOCodeText.Text","Country.ISOCode");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindCountryDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindCountryDropdownsAndDivSelectRegions();
		}


		
		
		private void BindCountryDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveCountry(object sender, EventArgs e)
			{
				if (Validate()){
					LoyaltyDAO.Save(Country);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




