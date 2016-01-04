
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Scriptfile

	/// <summary>
	/// Scriptfile form for NHibernate mapped table 'script_file'.
	/// </summary>
	public partial class ScriptfileForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Scriptfile _Scriptfile;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Scriptfile Scriptfile
		{
			set { _Scriptfile = value; }
			get { return _Scriptfile; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Scriptfile = new OrderTracking.Gps.Dao.Domain.Scriptfile ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Scriptfile = (OrderTracking.Gps.Dao.Domain.Scriptfile)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Scriptfile;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("UrlText.Text","Scriptfile.Url");
				BindingManager.AddBinding("LanguageText.Text","Scriptfile.Language");
				BindingManager.AddBinding("LoadorderText.Text","Scriptfile.Loadorder");
				BindingManager.AddBinding("CreatedText.Text","Scriptfile.Created");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindScriptfileDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindScriptfileDropdownsAndDivSelectRegions();
		}


		
		
		private void BindScriptfileDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveScriptfile(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Scriptfile);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




