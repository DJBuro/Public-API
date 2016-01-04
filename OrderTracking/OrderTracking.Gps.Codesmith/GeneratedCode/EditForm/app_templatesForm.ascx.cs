
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Apptemplate

	/// <summary>
	/// Apptemplate form for NHibernate mapped table 'app_templates'.
	/// </summary>
	public partial class ApptemplateForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Apptemplate _Apptemplate;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Apptemplate Apptemplate
		{
			set { _Apptemplate = value; }
			get { return _Apptemplate; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Apptemplate = new OrderTracking.Gps.Dao.Domain.Apptemplate ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Apptemplate = (OrderTracking.Gps.Dao.Domain.Apptemplate)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Apptemplate;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("BotypeText.Text","Apptemplate.Botype");
				BindingManager.AddBinding("NameText.Text","Apptemplate.Name");
				BindingManager.AddBinding("DescriptionText.Text","Apptemplate.Description");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindApptemplateDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindApptemplateDropdownsAndDivSelectRegions();
		}


		
		
		private void BindApptemplateDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveApptemplate(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Apptemplate);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




