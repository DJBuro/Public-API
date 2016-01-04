
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Scriptpluginapplication

	/// <summary>
	/// Scriptpluginapplication form for NHibernate mapped table 'script_plugin_application'.
	/// </summary>
	public partial class ScriptpluginapplicationForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Scriptpluginapplication _Scriptpluginapplication;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Scriptpluginapplication Scriptpluginapplication
		{
			set { _Scriptpluginapplication = value; }
			get { return _Scriptpluginapplication; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Scriptpluginapplication = new OrderTracking.Gps.Dao.Domain.Scriptpluginapplication ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Scriptpluginapplication = (OrderTracking.Gps.Dao.Domain.Scriptpluginapplication)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Scriptpluginapplication;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindScriptpluginapplicationDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindScriptpluginapplicationDropdownsAndDivSelectRegions();
		}


		
		
		private void BindScriptpluginapplicationDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveScriptpluginapplication(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Scriptpluginapplication);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




