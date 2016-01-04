
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Data
{
	#region Log

	/// <summary>
	/// Log form for NHibernate mapped table 'tbl_Log'.
	/// </summary>
	public partial class LogForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Dao.Domain.Log _Log;

			
		#endregion
		
		#region Properties

		public OrderTracking.Dao.Domain.Log Log
		{
			set { _Log = value; }
			get { return _Log; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Log = new OrderTracking.Dao.Domain.Log ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Log = (OrderTracking.Dao.Domain.Log)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Log;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("StoreIdText.Text","Log.StoreId");
				BindingManager.AddBinding("SeverityText.Text","Log.Severity");
				BindingManager.AddBinding("MessageText.Text","Log.Message");
				BindingManager.AddBinding("MethodText.Text","Log.Method");
				BindingManager.AddBinding("SourceText.Text","Log.Source");
				BindingManager.AddBinding("CreatedText.Text","Log.Created");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindLogDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindLogDropdownsAndDivSelectRegions();
		}


		
		
		private void BindLogDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveLog(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingDAO.Save(Log);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




