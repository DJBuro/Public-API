
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Gateeventlog

	/// <summary>
	/// Gateeventlog form for NHibernate mapped table 'gate_event_log'.
	/// </summary>
	public partial class GateeventlogForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Gateeventlog _Gateeventlog;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Gateeventlog Gateeventlog
		{
			set { _Gateeventlog = value; }
			get { return _Gateeventlog; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Gateeventlog = new OrderTracking.Gps.Dao.Domain.Gateeventlog ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Gateeventlog = (OrderTracking.Gps.Dao.Domain.Gateeventlog)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Gateeventlog;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("ModifiedbyuseridText.Text","Gateeventlog.Modifiedbyuserid");
				BindingManager.AddBinding("ServertimestampText.Text","Gateeventlog.Servertimestamp");
				BindingManager.AddBinding("ServertimestampmsText.Text","Gateeventlog.Servertimestampms");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindGateeventlogDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindGateeventlogDropdownsAndDivSelectRegions();
		}


		
		
		private void BindGateeventlogDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveGateeventlog(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Gateeventlog);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




