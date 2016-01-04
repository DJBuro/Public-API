
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Referrer

	/// <summary>
	/// Referrer form for NHibernate mapped table 'referrer'.
	/// </summary>
	public partial class ReferrerForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Referrer _Referrer;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Referrer Referrer
		{
			set { _Referrer = value; }
			get { return _Referrer; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Referrer = new OrderTracking.Gps.Dao.Domain.Referrer ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Referrer = (OrderTracking.Gps.Dao.Domain.Referrer)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Referrer;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("RefurlText.Text","Referrer.Refurl");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindReferrerDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindReferrerDropdownsAndDivSelectRegions();
		}


		
		
		private void BindReferrerDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveReferrer(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Referrer);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




