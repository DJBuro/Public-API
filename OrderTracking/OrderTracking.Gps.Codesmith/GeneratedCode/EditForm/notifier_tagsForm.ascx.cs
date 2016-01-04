
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Notifiertag

	/// <summary>
	/// Notifiertag form for NHibernate mapped table 'notifier_tags'.
	/// </summary>
	public partial class NotifiertagForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Notifiertag _Notifiertag;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Notifiertag Notifiertag
		{
			set { _Notifiertag = value; }
			get { return _Notifiertag; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Notifiertag = new OrderTracking.Gps.Dao.Domain.Notifiertag ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Notifiertag = (OrderTracking.Gps.Dao.Domain.Notifiertag)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Notifiertag;
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
				BindNotifiertagDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindNotifiertagDropdownsAndDivSelectRegions();
		}


		
		
		private void BindNotifiertagDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveNotifiertag(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Notifiertag);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




