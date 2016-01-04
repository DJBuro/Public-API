
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Group

	/// <summary>
	/// Group form for NHibernate mapped table 'groups'.
	/// </summary>
	public partial class GroupForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Group _Group;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Group Group
		{
			set { _Group = value; }
			get { return _Group; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Group = new OrderTracking.Gps.Dao.Domain.Group ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Group = (OrderTracking.Gps.Dao.Domain.Group)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Group;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("GroupnameText.Text","Group.Groupname");
				BindingManager.AddBinding("GroupdescriptionText.Text","Group.Groupdescription");
				BindingManager.AddBinding("BotypeText.Text","Group.Botype");
				BindingManager.AddBinding("CreatedText.Text","Group.Created");
				BindingManager.AddBinding("PublicflagText.Text","Group.Publicflag");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindGroupDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindGroupDropdownsAndDivSelectRegions();
		}


		
		
		private void BindGroupDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveGroup(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Group);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




