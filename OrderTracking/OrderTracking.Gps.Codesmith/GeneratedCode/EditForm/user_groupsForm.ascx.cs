
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Usergroup

	/// <summary>
	/// Usergroup form for NHibernate mapped table 'user_groups'.
	/// </summary>
	public partial class UsergroupForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Usergroup _Usergroup;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Usergroup Usergroup
		{
			set { _Usergroup = value; }
			get { return _Usergroup; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Usergroup = new OrderTracking.Gps.Dao.Domain.Usergroup ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Usergroup = (OrderTracking.Gps.Dao.Domain.Usergroup)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Usergroup;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("GrouprightidText.Text","Usergroup.Grouprightid");
				BindingManager.AddBinding("AdminrightidText.Text","Usergroup.Adminrightid");
				BindingManager.AddBinding("EnablepublictracksText.Text","Usergroup.Enablepublictracks");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindUsergroupDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindUsergroupDropdownsAndDivSelectRegions();
		}


		
		
		private void BindUsergroupDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveUsergroup(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Usergroup);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




