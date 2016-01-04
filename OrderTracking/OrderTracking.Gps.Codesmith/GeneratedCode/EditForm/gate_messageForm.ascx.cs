
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Gatemessage

	/// <summary>
	/// Gatemessage form for NHibernate mapped table 'gate_message'.
	/// </summary>
	public partial class GatemessageForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Gatemessage _Gatemessage;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Gatemessage Gatemessage
		{
			set { _Gatemessage = value; }
			get { return _Gatemessage; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Gatemessage = new OrderTracking.Gps.Dao.Domain.Gatemessage ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Gatemessage = (OrderTracking.Gps.Dao.Domain.Gatemessage)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Gatemessage;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("TrackdataidText.Text","Gatemessage.Trackdataid");
				BindingManager.AddBinding("ServertimestampText.Text","Gatemessage.Servertimestamp");
				BindingManager.AddBinding("ServertimestampmsText.Text","Gatemessage.Servertimestampms");
				BindingManager.AddBinding("DeviceidText.Text","Gatemessage.Deviceid");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindGatemessageDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindGatemessageDropdownsAndDivSelectRegions();
		}


		
		
		private void BindGatemessageDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveGatemessage(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Gatemessage);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




