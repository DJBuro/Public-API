
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Trackpostprocessorlog

	/// <summary>
	/// Trackpostprocessorlog form for NHibernate mapped table 'track_post_processor_log'.
	/// </summary>
	public partial class TrackpostprocessorlogForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Trackpostprocessorlog _Trackpostprocessorlog;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Trackpostprocessorlog Trackpostprocessorlog
		{
			set { _Trackpostprocessorlog = value; }
			get { return _Trackpostprocessorlog; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Trackpostprocessorlog = new OrderTracking.Gps.Dao.Domain.Trackpostprocessorlog ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Trackpostprocessorlog = (OrderTracking.Gps.Dao.Domain.Trackpostprocessorlog)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Trackpostprocessorlog;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("TimestampstartedText.Text","Trackpostprocessorlog.Timestampstarted");
				BindingManager.AddBinding("TimestampstartedmsText.Text","Trackpostprocessorlog.Timestampstartedms");
				BindingManager.AddBinding("TimestampdoneText.Text","Trackpostprocessorlog.Timestampdone");
				BindingManager.AddBinding("TimestampdonemsText.Text","Trackpostprocessorlog.Timestampdonems");
				BindingManager.AddBinding("TrackinfomaxtimeText.Text","Trackpostprocessorlog.Trackinfomaxtime");
				BindingManager.AddBinding("TrackinfomaxtimemsText.Text","Trackpostprocessorlog.Trackinfomaxtimems");
				BindingManager.AddBinding("DirtycountText.Text","Trackpostprocessorlog.Dirtycount");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindTrackpostprocessorlogDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindTrackpostprocessorlogDropdownsAndDivSelectRegions();
		}


		
		
		private void BindTrackpostprocessorlogDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveTrackpostprocessorlog(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Trackpostprocessorlog);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




