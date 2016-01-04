
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Userrole

	/// <summary>
	/// Userrole form for NHibernate mapped table 'user_roles'.
	/// </summary>
	public partial class UserroleForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Userrole _Userrole;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Userrole Userrole
		{
			set { _Userrole = value; }
			get { return _Userrole; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Userrole = new OrderTracking.Gps.Dao.Domain.Userrole ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Userrole = (OrderTracking.Gps.Dao.Domain.Userrole)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Userrole;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindUserroleDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindUserroleDropdownsAndDivSelectRegions();
		}


		
		
		private void BindUserroleDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveUserrole(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Userrole);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




