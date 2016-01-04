
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Scheduler

	/// <summary>
	/// Scheduler form for NHibernate mapped table 'scheduler'.
	/// </summary>
	public partial class SchedulerForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Scheduler _Scheduler;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Scheduler Scheduler
		{
			set { _Scheduler = value; }
			get { return _Scheduler; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Scheduler = new OrderTracking.Gps.Dao.Domain.Scheduler ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Scheduler = (OrderTracking.Gps.Dao.Domain.Scheduler)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Scheduler;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("SchedulernameText.Text","Scheduler.Schedulername");
				BindingManager.AddBinding("BotypeText.Text","Scheduler.Botype");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindSchedulerDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindSchedulerDropdownsAndDivSelectRegions();
		}


		
		
		private void BindSchedulerDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveScheduler(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Scheduler);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




