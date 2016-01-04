
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Trackcategory

	/// <summary>
	/// Trackcategory form for NHibernate mapped table 'track_category'.
	/// </summary>
	public partial class TrackcategoryForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Trackcategory _Trackcategory;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Trackcategory Trackcategory
		{
			set { _Trackcategory = value; }
			get { return _Trackcategory; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Trackcategory = new OrderTracking.Gps.Dao.Domain.Trackcategory ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Trackcategory = (OrderTracking.Gps.Dao.Domain.Trackcategory)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Trackcategory;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("BotypeText.Text","Trackcategory.Botype");
				BindingManager.AddBinding("NameText.Text","Trackcategory.Name");
				BindingManager.AddBinding("DescriptionText.Text","Trackcategory.Description");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindTrackcategoryDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindTrackcategoryDropdownsAndDivSelectRegions();
		}


		
		
		private void BindTrackcategoryDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveTrackcategory(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Trackcategory);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




