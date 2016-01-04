
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Postprocessor

	/// <summary>
	/// Postprocessor form for NHibernate mapped table 'post_processor'.
	/// </summary>
	public partial class PostprocessorForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Postprocessor _Postprocessor;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Postprocessor Postprocessor
		{
			set { _Postprocessor = value; }
			get { return _Postprocessor; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Postprocessor = new OrderTracking.Gps.Dao.Domain.Postprocessor ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Postprocessor = (OrderTracking.Gps.Dao.Domain.Postprocessor)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Postprocessor;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("NameText.Text","Postprocessor.Name");
				BindingManager.AddBinding("DescriptionText.Text","Postprocessor.Description");
				BindingManager.AddBinding("BotypeText.Text","Postprocessor.Botype");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindPostprocessorDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindPostprocessorDropdownsAndDivSelectRegions();
		}


		
		
		private void BindPostprocessorDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SavePostprocessor(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Postprocessor);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




