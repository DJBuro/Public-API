
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Gateevent

	/// <summary>
	/// Gateevent form for NHibernate mapped table 'gate_event'.
	/// </summary>
	public partial class GateeventForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Gateevent _Gateevent;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Gateevent Gateevent
		{
			set { _Gateevent = value; }
			get { return _Gateevent; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Gateevent = new OrderTracking.Gps.Dao.Domain.Gateevent ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Gateevent = (OrderTracking.Gps.Dao.Domain.Gateevent)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Gateevent;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("UseridText.Text","Gateevent.Userid");
				BindingManager.AddBinding("CauseText.Text","Gateevent.Cause");
				BindingManager.AddBinding("GateeventexpressionevaluatoridText.Text","Gateevent.Gateeventexpressionevaluatorid");
				BindingManager.AddBinding("StageText.Text","Gateevent.Stage");
				BindingManager.AddBinding("MinlongitudeText.Text","Gateevent.Minlongitude");
				BindingManager.AddBinding("MaxlongitudeText.Text","Gateevent.Maxlongitude");
				BindingManager.AddBinding("MinlatitudeText.Text","Gateevent.Minlatitude");
				BindingManager.AddBinding("MaxlatitudeText.Text","Gateevent.Maxlatitude");
				BindingManager.AddBinding("MinaltitudeText.Text","Gateevent.Minaltitude");
				BindingManager.AddBinding("MaxaltitudeText.Text","Gateevent.Maxaltitude");
				BindingManager.AddBinding("MintimestampText.Text","Gateevent.Mintimestamp");
				BindingManager.AddBinding("MinmillisecondsText.Text","Gateevent.Minmilliseconds");
				BindingManager.AddBinding("MaxtimestampText.Text","Gateevent.Maxtimestamp");
				BindingManager.AddBinding("MaxmillisecondsText.Text","Gateevent.Maxmilliseconds");
				BindingManager.AddBinding("ScheduledText.Text","Gateevent.Scheduled");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindGateeventDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindGateeventDropdownsAndDivSelectRegions();
		}


		
		
		private void BindGateeventDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveGateevent(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Gateevent);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




