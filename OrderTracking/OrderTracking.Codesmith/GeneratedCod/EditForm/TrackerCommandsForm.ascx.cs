
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Data
{
	#region TrackerCommand

	/// <summary>
	/// TrackerCommand form for NHibernate mapped table 'tbl_TrackerCommands'.
	/// </summary>
	public partial class TrackerCommandForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Dao.Domain.TrackerCommand _TrackerCommand;

			
		#endregion
		
		#region Properties

		public OrderTracking.Dao.Domain.TrackerCommand TrackerCommand
		{
			set { _TrackerCommand = value; }
			get { return _TrackerCommand; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_TrackerCommand = new OrderTracking.Dao.Domain.TrackerCommand ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _TrackerCommand = (OrderTracking.Dao.Domain.TrackerCommand)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _TrackerCommand;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("PriorityText.Text","TrackerCommand.Priority");
				BindingManager.AddBinding("NameText.Text","TrackerCommand.Name");
				BindingManager.AddBinding("CommandText.Text","TrackerCommand.Command");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindTrackerCommandDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindTrackerCommandDropdownsAndDivSelectRegions();
		}


		
		
		private void BindTrackerCommandDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveTrackerCommand(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingDAO.Save(TrackerCommand);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




