
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Data
{
	#region Country

	/// <summary>
	/// Country form for NHibernate mapped table 'tbl_Country'.
	/// </summary>
	public partial class CountryForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Dao.Domain.Country _Country;

			
		#endregion
		
		#region Properties

		public OrderTracking.Dao.Domain.Country Country
		{
			set { _Country = value; }
			get { return _Country; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Country = new OrderTracking.Dao.Domain.Country ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Country = (OrderTracking.Dao.Domain.Country)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Country;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("NameText.Text","Country.Name");
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
					OrderTrackingDAO.Save(Country);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




