
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Role

	/// <summary>
	/// Role form for NHibernate mapped table 'roles'.
	/// </summary>
	public partial class RoleForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Role _Role;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Role Role
		{
			set { _Role = value; }
			get { return _Role; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Role = new OrderTracking.Gps.Dao.Domain.Role ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Role = (OrderTracking.Gps.Dao.Domain.Role)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Role;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("RolenameText.Text","Role.Rolename");
				BindingManager.AddBinding("RoledescriptionText.Text","Role.Roledescription");
				BindingManager.AddBinding("BotypeText.Text","Role.Botype");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindRoleDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindRoleDropdownsAndDivSelectRegions();
		}


		
		
		private void BindRoleDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveRole(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Role);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




