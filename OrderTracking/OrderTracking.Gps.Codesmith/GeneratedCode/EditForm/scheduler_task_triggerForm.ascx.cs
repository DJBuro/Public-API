
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Schedulertasktrigger

	/// <summary>
	/// Schedulertasktrigger form for NHibernate mapped table 'scheduler_task_trigger'.
	/// </summary>
	public partial class SchedulertasktriggerForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Schedulertasktrigger _Schedulertasktrigger;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Schedulertasktrigger Schedulertasktrigger
		{
			set { _Schedulertasktrigger = value; }
			get { return _Schedulertasktrigger; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Schedulertasktrigger = new OrderTracking.Gps.Dao.Domain.Schedulertasktrigger ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Schedulertasktrigger = (OrderTracking.Gps.Dao.Domain.Schedulertasktrigger)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Schedulertasktrigger;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("TriggerassemblynameText.Text","Schedulertasktrigger.Triggerassemblyname");
				BindingManager.AddBinding("TriggertypenameText.Text","Schedulertasktrigger.Triggertypename");
				BindingManager.AddBinding("TriggerdataText.Text","Schedulertasktrigger.Triggerdata");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindSchedulertasktriggerDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindSchedulertasktriggerDropdownsAndDivSelectRegions();
		}


		
		
		private void BindSchedulertasktriggerDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveSchedulertasktrigger(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Schedulertasktrigger);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




