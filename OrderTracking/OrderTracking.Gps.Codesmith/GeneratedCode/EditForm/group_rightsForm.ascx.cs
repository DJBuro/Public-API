
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Groupright

	/// <summary>
	/// Groupright form for NHibernate mapped table 'group_rights'.
	/// </summary>
	public partial class GrouprightForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Groupright _Groupright;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Groupright Groupright
		{
			set { _Groupright = value; }
			get { return _Groupright; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Groupright = new OrderTracking.Gps.Dao.Domain.Groupright ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Groupright = (OrderTracking.Gps.Dao.Domain.Groupright)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Groupright;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("BotypeText.Text","Groupright.Botype");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindGrouprightDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindGrouprightDropdownsAndDivSelectRegions();
		}


		
		
		private void BindGrouprightDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveGroupright(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Groupright);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




