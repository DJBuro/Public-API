
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Serverversion

	/// <summary>
	/// Serverversion form for NHibernate mapped table 'server_version'.
	/// </summary>
	public partial class ServerversionForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Serverversion _Serverversion;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Serverversion Serverversion
		{
			set { _Serverversion = value; }
			get { return _Serverversion; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Serverversion = new OrderTracking.Gps.Dao.Domain.Serverversion ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Serverversion = (OrderTracking.Gps.Dao.Domain.Serverversion)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Serverversion;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("InstalledText.Text","Serverversion.Installed");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindServerversionDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindServerversionDropdownsAndDivSelectRegions();
		}


		
		
		private void BindServerversionDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveServerversion(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Serverversion);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




