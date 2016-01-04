
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Data
{
	#region CommandType

	/// <summary>
	/// CommandType form for NHibernate mapped table 'tbl_CommandType'.
	/// </summary>
	public partial class CommandTypeForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Dao.Domain.CommandType _CommandType;

			
		#endregion
		
		#region Properties

		public OrderTracking.Dao.Domain.CommandType CommandType
		{
			set { _CommandType = value; }
			get { return _CommandType; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_CommandType = new OrderTracking.Dao.Domain.CommandType ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _CommandType = (OrderTracking.Dao.Domain.CommandType)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _CommandType;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("NameText.Text","CommandType.Name");
				BindingManager.AddBinding("DescriptionText.Text","CommandType.Description");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindCommandTypeDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindCommandTypeDropdownsAndDivSelectRegions();
		}


		
		
		private void BindCommandTypeDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveCommandType(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingDAO.Save(CommandType);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




