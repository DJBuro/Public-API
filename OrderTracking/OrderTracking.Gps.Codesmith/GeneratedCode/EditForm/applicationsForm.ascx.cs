
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Application

	/// <summary>
	/// Application form for NHibernate mapped table 'applications'.
	/// </summary>
	public partial class ApplicationForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Application _Application;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Application Application
		{
			set { _Application = value; }
			get { return _Application; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Application = new OrderTracking.Gps.Dao.Domain.Application ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Application = (OrderTracking.Gps.Dao.Domain.Application)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Application;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("BotypeText.Text","Application.Botype");
				BindingManager.AddBinding("NameText.Text","Application.Name");
				BindingManager.AddBinding("DescriptionText.Text","Application.Description");
				BindingManager.AddBinding("MaxusersText.Text","Application.Maxusers");
				BindingManager.AddBinding("ExpireText.Text","Application.Expire");
				BindingManager.AddBinding("CreatedText.Text","Application.Created");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindApplicationDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindApplicationDropdownsAndDivSelectRegions();
		}


		
		
		private void BindApplicationDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveApplication(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Application);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




