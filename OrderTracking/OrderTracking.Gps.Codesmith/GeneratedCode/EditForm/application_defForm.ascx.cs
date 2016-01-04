
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Applicationdef

	/// <summary>
	/// Applicationdef form for NHibernate mapped table 'application_def'.
	/// </summary>
	public partial class ApplicationdefForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Applicationdef _Applicationdef;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Applicationdef Applicationdef
		{
			set { _Applicationdef = value; }
			get { return _Applicationdef; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Applicationdef = new OrderTracking.Gps.Dao.Domain.Applicationdef ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Applicationdef = (OrderTracking.Gps.Dao.Domain.Applicationdef)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Applicationdef;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("ApplicationdefdescriptionText.Text","Applicationdef.Applicationdefdescription");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindApplicationdefDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindApplicationdefDropdownsAndDivSelectRegions();
		}


		
		
		private void BindApplicationdefDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveApplicationdef(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Applicationdef);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




