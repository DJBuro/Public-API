
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Schedulertask

	/// <summary>
	/// Schedulertask form for NHibernate mapped table 'scheduler_task'.
	/// </summary>
	public partial class SchedulertaskForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Schedulertask _Schedulertask;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Schedulertask Schedulertask
		{
			set { _Schedulertask = value; }
			get { return _Schedulertask; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Schedulertask = new OrderTracking.Gps.Dao.Domain.Schedulertask ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Schedulertask = (OrderTracking.Gps.Dao.Domain.Schedulertask)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Schedulertask;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("TasknameText.Text","Schedulertask.Taskname");
				BindingManager.AddBinding("TaskgroupText.Text","Schedulertask.Taskgroup");
				BindingManager.AddBinding("ExeccountText.Text","Schedulertask.Execcount");
				BindingManager.AddBinding("LastexectimestampText.Text","Schedulertask.Lastexectimestamp");
				BindingManager.AddBinding("NextexectimestampText.Text","Schedulertask.Nextexectimestamp");
				BindingManager.AddBinding("StateText.Text","Schedulertask.State");
				BindingManager.AddBinding("TaskassemblynameText.Text","Schedulertask.Taskassemblyname");
				BindingManager.AddBinding("TasktypenameText.Text","Schedulertask.Tasktypename");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindSchedulertaskDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindSchedulertaskDropdownsAndDivSelectRegions();
		}


		
		
		private void BindSchedulertaskDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveSchedulertask(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Schedulertask);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




