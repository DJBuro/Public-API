
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Adminright

	/// <summary>
	/// Adminright form for NHibernate mapped table 'admin_rights'.
	/// </summary>
	public partial class AdminrightForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Adminright _Adminright;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Adminright Adminright
		{
			set { _Adminright = value; }
			get { return _Adminright; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Adminright = new OrderTracking.Gps.Dao.Domain.Adminright ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Adminright = (OrderTracking.Gps.Dao.Domain.Adminright)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Adminright;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("BotypeText.Text","Adminright.Botype");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindAdminrightDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindAdminrightDropdownsAndDivSelectRegions();
		}


		
		
		private void BindAdminrightDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveAdminright(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Adminright);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




