
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Scriptplugin

	/// <summary>
	/// Scriptplugin form for NHibernate mapped table 'script_plugin'.
	/// </summary>
	public partial class ScriptpluginForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Scriptplugin _Scriptplugin;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Scriptplugin Scriptplugin
		{
			set { _Scriptplugin = value; }
			get { return _Scriptplugin; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Scriptplugin = new OrderTracking.Gps.Dao.Domain.Scriptplugin ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Scriptplugin = (OrderTracking.Gps.Dao.Domain.Scriptplugin)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Scriptplugin;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("NameText.Text","Scriptplugin.Name");
				BindingManager.AddBinding("CategoryText.Text","Scriptplugin.Category");
				BindingManager.AddBinding("DescriptionText.Text","Scriptplugin.Description");
				BindingManager.AddBinding("FilepathText.Text","Scriptplugin.Filepath");
				BindingManager.AddBinding("VersionText.Text","Scriptplugin.Version");
				BindingManager.AddBinding("LoadorderText.Text","Scriptplugin.Loadorder");
				BindingManager.AddBinding("DeletedText.Text","Scriptplugin.Deleted");
				BindingManager.AddBinding("CreatedText.Text","Scriptplugin.Created");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindScriptpluginDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindScriptpluginDropdownsAndDivSelectRegions();
		}


		
		
		private void BindScriptpluginDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveScriptplugin(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Scriptplugin);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




