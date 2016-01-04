
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Msgfield

	/// <summary>
	/// Msgfield form for NHibernate mapped table 'msg_field'.
	/// </summary>
	public partial class MsgfieldForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Msgfield _Msgfield;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Msgfield Msgfield
		{
			set { _Msgfield = value; }
			get { return _Msgfield; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Msgfield = new OrderTracking.Gps.Dao.Domain.Msgfield ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Msgfield = (OrderTracking.Gps.Dao.Domain.Msgfield)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Msgfield;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("TypeText.Text","Msgfield.Type");
				BindingManager.AddBinding("NameText.Text","Msgfield.Name");
				BindingManager.AddBinding("DescriptionText.Text","Msgfield.Description");
				BindingManager.AddBinding("LocalizationkeyText.Text","Msgfield.Localizationkey");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindMsgfieldDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindMsgfieldDropdownsAndDivSelectRegions();
		}


		
		
		private void BindMsgfieldDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveMsgfield(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Msgfield);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




