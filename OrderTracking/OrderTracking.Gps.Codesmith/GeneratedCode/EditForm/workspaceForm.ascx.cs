
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Workspace

	/// <summary>
	/// Workspace form for NHibernate mapped table 'workspace'.
	/// </summary>
	public partial class WorkspaceForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Workspace _Workspace;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Workspace Workspace
		{
			set { _Workspace = value; }
			get { return _Workspace; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Workspace = new OrderTracking.Gps.Dao.Domain.Workspace ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Workspace = (OrderTracking.Gps.Dao.Domain.Workspace)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Workspace;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("NameText.Text","Workspace.Name");
				BindingManager.AddBinding("StateText.Text","Workspace.State");
				BindingManager.AddBinding("SharedText.Text","Workspace.Shared");
				BindingManager.AddBinding("CreatedText.Text","Workspace.Created");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindWorkspaceDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindWorkspaceDropdownsAndDivSelectRegions();
		}


		
		
		private void BindWorkspaceDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveWorkspace(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Workspace);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




