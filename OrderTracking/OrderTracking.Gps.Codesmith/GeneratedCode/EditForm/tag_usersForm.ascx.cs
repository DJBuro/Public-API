
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Taguser

	/// <summary>
	/// Taguser form for NHibernate mapped table 'tag_users'.
	/// </summary>
	public partial class TaguserForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Taguser _Taguser;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Taguser Taguser
		{
			set { _Taguser = value; }
			get { return _Taguser; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Taguser = new OrderTracking.Gps.Dao.Domain.Taguser ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Taguser = (OrderTracking.Gps.Dao.Domain.Taguser)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Taguser;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("ServertimestampText.Text","Taguser.Servertimestamp");
				BindingManager.AddBinding("ServertimestampmsText.Text","Taguser.Servertimestampms");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindTaguserDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindTaguserDropdownsAndDivSelectRegions();
		}


		
		
		private void BindTaguserDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveTaguser(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Taguser);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




