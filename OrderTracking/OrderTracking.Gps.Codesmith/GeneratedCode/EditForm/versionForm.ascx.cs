
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Version

	/// <summary>
	/// Version form for NHibernate mapped table 'version'.
	/// </summary>
	public partial class VersionForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Version _Version;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Version Version
		{
			set { _Version = value; }
			get { return _Version; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Version = new OrderTracking.Gps.Dao.Domain.Version ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Version = (OrderTracking.Gps.Dao.Domain.Version)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Version;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("ModuledescriptionText.Text","Version.Moduledescription");
				BindingManager.AddBinding("CreatedText.Text","Version.Created");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindVersionDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindVersionDropdownsAndDivSelectRegions();
		}


		
		
		private void BindVersionDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveVersion(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Version);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




