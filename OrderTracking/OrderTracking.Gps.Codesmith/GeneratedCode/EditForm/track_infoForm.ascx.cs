
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Trackinfo

	/// <summary>
	/// Trackinfo form for NHibernate mapped table 'track_info'.
	/// </summary>
	public partial class TrackinfoForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Trackinfo _Trackinfo;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Trackinfo Trackinfo
		{
			set { _Trackinfo = value; }
			get { return _Trackinfo; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Trackinfo = new OrderTracking.Gps.Dao.Domain.Trackinfo ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Trackinfo = (OrderTracking.Gps.Dao.Domain.Trackinfo)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Trackinfo;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("BotypeText.Text","Trackinfo.Botype");
				BindingManager.AddBinding("NameText.Text","Trackinfo.Name");
				BindingManager.AddBinding("DescriptionText.Text","Trackinfo.Description");
				BindingManager.AddBinding("MinlongitudeText.Text","Trackinfo.Minlongitude");
				BindingManager.AddBinding("MaxlongitudeText.Text","Trackinfo.Maxlongitude");
				BindingManager.AddBinding("MinlatitudeText.Text","Trackinfo.Minlatitude");
				BindingManager.AddBinding("MaxlatitudeText.Text","Trackinfo.Maxlatitude");
				BindingManager.AddBinding("MinaltitudeText.Text","Trackinfo.Minaltitude");
				BindingManager.AddBinding("MaxaltitudeText.Text","Trackinfo.Maxaltitude");
				BindingManager.AddBinding("MintimestampText.Text","Trackinfo.Mintimestamp");
				BindingManager.AddBinding("MinmillisecondsText.Text","Trackinfo.Minmilliseconds");
				BindingManager.AddBinding("MaxtimestampText.Text","Trackinfo.Maxtimestamp");
				BindingManager.AddBinding("MaxmillisecondsText.Text","Trackinfo.Maxmilliseconds");
				BindingManager.AddBinding("TotaldistanceText.Text","Trackinfo.Totaldistance");
				BindingManager.AddBinding("DeletedText.Text","Trackinfo.Deleted");
				BindingManager.AddBinding("DirtycountText.Text","Trackinfo.Dirtycount");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindTrackinfoDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindTrackinfoDropdownsAndDivSelectRegions();
		}


		
		
		private void BindTrackinfoDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveTrackinfo(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Trackinfo);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




