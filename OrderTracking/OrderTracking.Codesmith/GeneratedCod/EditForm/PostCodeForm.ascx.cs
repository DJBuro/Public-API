
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Data
{
	#region PostCode

	/// <summary>
	/// PostCode form for NHibernate mapped table 'tbl_PostCode'.
	/// </summary>
	public partial class PostCodeForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Dao.Domain.PostCode _PostCode;

			
		#endregion
		
		#region Properties

		public OrderTracking.Dao.Domain.PostCode PostCode
		{
			set { _PostCode = value; }
			get { return _PostCode; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_PostCode = new OrderTracking.Dao.Domain.PostCode ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _PostCode = (OrderTracking.Dao.Domain.PostCode)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _PostCode;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("PostCodeText.Text","PostCode.PostCode");
				BindingManager.AddBinding("Del1Text.Text","PostCode.Del1");
				BindingManager.AddBinding("LongitudeText.Text","PostCode.Longitude");
				BindingManager.AddBinding("LatitudeText.Text","PostCode.Latitude");
				BindingManager.AddBinding("Del2Text.Text","PostCode.Del2");
				BindingManager.AddBinding("Del3Text.Text","PostCode.Del3");
				BindingManager.AddBinding("Del4Text.Text","PostCode.Del4");
				BindingManager.AddBinding("Del5Text.Text","PostCode.Del5");
				BindingManager.AddBinding("Del6Text.Text","PostCode.Del6");
				BindingManager.AddBinding("Del7Text.Text","PostCode.Del7");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindPostCodeDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindPostCodeDropdownsAndDivSelectRegions();
		}


		
		
		private void BindPostCodeDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SavePostCode(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingDAO.Save(PostCode);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




