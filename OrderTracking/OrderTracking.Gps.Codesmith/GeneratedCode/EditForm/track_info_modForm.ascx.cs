
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Trackinfomod

	/// <summary>
	/// Trackinfomod form for NHibernate mapped table 'track_info_mod'.
	/// </summary>
	public partial class TrackinfomodForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Trackinfomod _Trackinfomod;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Trackinfomod Trackinfomod
		{
			set { _Trackinfomod = value; }
			get { return _Trackinfomod; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Trackinfomod = new OrderTracking.Gps.Dao.Domain.Trackinfomod ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Trackinfomod = (OrderTracking.Gps.Dao.Domain.Trackinfomod)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Trackinfomod;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("TrackinfoidText.Text","Trackinfomod.Trackinfoid");
				BindingManager.AddBinding("NameText.Text","Trackinfomod.Name");
				BindingManager.AddBinding("DescriptionText.Text","Trackinfomod.Description");
				BindingManager.AddBinding("OwneridText.Text","Trackinfomod.Ownerid");
				BindingManager.AddBinding("TrackcategoryidText.Text","Trackinfomod.Trackcategoryid");
				BindingManager.AddBinding("MinlongitudeText.Text","Trackinfomod.Minlongitude");
				BindingManager.AddBinding("MaxlongitudeText.Text","Trackinfomod.Maxlongitude");
				BindingManager.AddBinding("MinlatitudeText.Text","Trackinfomod.Minlatitude");
				BindingManager.AddBinding("MaxlatitudeText.Text","Trackinfomod.Maxlatitude");
				BindingManager.AddBinding("MinaltitudeText.Text","Trackinfomod.Minaltitude");
				BindingManager.AddBinding("MaxaltitudeText.Text","Trackinfomod.Maxaltitude");
				BindingManager.AddBinding("MintimestampText.Text","Trackinfomod.Mintimestamp");
				BindingManager.AddBinding("MinmillisecondsText.Text","Trackinfomod.Minmilliseconds");
				BindingManager.AddBinding("MaxtimestampText.Text","Trackinfomod.Maxtimestamp");
				BindingManager.AddBinding("MaxmillisecondsText.Text","Trackinfomod.Maxmilliseconds");
				BindingManager.AddBinding("TotaldistanceText.Text","Trackinfomod.Totaldistance");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindTrackinfomodDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindTrackinfomodDropdownsAndDivSelectRegions();
		}


		
		
		private void BindTrackinfomodDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveTrackinfomod(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Trackinfomod);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




