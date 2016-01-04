
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Scriptservice

	/// <summary>
	/// Scriptservice form for NHibernate mapped table 'script_service'.
	/// </summary>
	public partial class ScriptserviceForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Scriptservice _Scriptservice;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Scriptservice Scriptservice
		{
			set { _Scriptservice = value; }
			get { return _Scriptservice; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Scriptservice = new OrderTracking.Gps.Dao.Domain.Scriptservice ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Scriptservice = (OrderTracking.Gps.Dao.Domain.Scriptservice)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Scriptservice;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("UrlText.Text","Scriptservice.Url");
				BindingManager.AddBinding("NamespaceText.Text","Scriptservice.Namespace");
				BindingManager.AddBinding("MethodText.Text","Scriptservice.Method");
				BindingManager.AddBinding("CreatedText.Text","Scriptservice.Created");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindScriptserviceDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindScriptserviceDropdownsAndDivSelectRegions();
		}


		
		
		private void BindScriptserviceDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveScriptservice(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Scriptservice);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




