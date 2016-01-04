
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Data
{
	#region Status

	/// <summary>
	/// Status form for NHibernate mapped table 'tbl_Status'.
	/// </summary>
	public partial class StatusForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Dao.Domain.Status _Status;

			
		#endregion
		
		#region Properties

		public OrderTracking.Dao.Domain.Status Status
		{
			set { _Status = value; }
			get { return _Status; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Status = new OrderTracking.Dao.Domain.Status ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Status = (OrderTracking.Dao.Domain.Status)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Status;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("NameText.Text","Status.Name");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindStatusDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindStatusDropdownsAndDivSelectRegions();
		}


		
		
		private void BindStatusDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveStatus(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingDAO.Save(Status);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




