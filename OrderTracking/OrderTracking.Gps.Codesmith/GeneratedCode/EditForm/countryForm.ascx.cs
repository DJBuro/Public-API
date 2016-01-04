
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Country

	/// <summary>
	/// Country form for NHibernate mapped table 'country'.
	/// </summary>
	public partial class CountryForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Country _Country;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Country Country
		{
			set { _Country = value; }
			get { return _Country; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Country = new OrderTracking.Gps.Dao.Domain.Country ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Country = (OrderTracking.Gps.Dao.Domain.Country)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Country;
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
					OrderTrackingGpsDAO.Save(Country);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




