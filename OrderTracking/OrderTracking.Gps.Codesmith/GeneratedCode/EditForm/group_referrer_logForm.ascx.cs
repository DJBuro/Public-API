
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Groupreferrerlog

	/// <summary>
	/// Groupreferrerlog form for NHibernate mapped table 'group_referrer_log'.
	/// </summary>
	public partial class GroupreferrerlogForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Groupreferrerlog _Groupreferrerlog;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Groupreferrerlog Groupreferrerlog
		{
			set { _Groupreferrerlog = value; }
			get { return _Groupreferrerlog; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Groupreferrerlog = new OrderTracking.Gps.Dao.Domain.Groupreferrerlog ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Groupreferrerlog = (OrderTracking.Gps.Dao.Domain.Groupreferrerlog)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Groupreferrerlog;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("RefurlText.Text","Groupreferrerlog.Refurl");
				BindingManager.AddBinding("HitsText.Text","Groupreferrerlog.Hits");
				BindingManager.AddBinding("TimestampText.Text","Groupreferrerlog.Timestamp");
				BindingManager.AddBinding("CreatedText.Text","Groupreferrerlog.Created");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindGroupreferrerlogDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindGroupreferrerlogDropdownsAndDivSelectRegions();
		}


		
		
		private void BindGroupreferrerlogDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveGroupreferrerlog(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Groupreferrerlog);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




