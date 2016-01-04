
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Cmdqueue

	/// <summary>
	/// Cmdqueue form for NHibernate mapped table 'cmd_queue'.
	/// </summary>
	public partial class CmdqueueForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Cmdqueue _Cmdqueue;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Cmdqueue Cmdqueue
		{
			set { _Cmdqueue = value; }
			get { return _Cmdqueue; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Cmdqueue = new OrderTracking.Gps.Dao.Domain.Cmdqueue ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Cmdqueue = (OrderTracking.Gps.Dao.Domain.Cmdqueue)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Cmdqueue;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("BotypeText.Text","Cmdqueue.Botype");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindCmdqueueDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindCmdqueueDropdownsAndDivSelectRegions();
		}


		
		
		private void BindCmdqueueDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveCmdqueue(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Cmdqueue);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




