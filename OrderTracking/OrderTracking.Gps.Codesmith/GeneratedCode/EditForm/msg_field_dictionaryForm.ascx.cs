
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Msgfielddictionary

	/// <summary>
	/// Msgfielddictionary form for NHibernate mapped table 'msg_field_dictionary'.
	/// </summary>
	public partial class MsgfielddictionaryForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Msgfielddictionary _Msgfielddictionary;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Msgfielddictionary Msgfielddictionary
		{
			set { _Msgfielddictionary = value; }
			get { return _Msgfielddictionary; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Msgfielddictionary = new OrderTracking.Gps.Dao.Domain.Msgfielddictionary ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Msgfielddictionary = (OrderTracking.Gps.Dao.Domain.Msgfielddictionary)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Msgfielddictionary;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("NameText.Text","Msgfielddictionary.Name");
				BindingManager.AddBinding("DescriptionText.Text","Msgfielddictionary.Description");
				BindingManager.AddBinding("BotypeText.Text","Msgfielddictionary.Botype");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindMsgfielddictionaryDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindMsgfielddictionaryDropdownsAndDivSelectRegions();
		}


		
		
		private void BindMsgfielddictionaryDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveMsgfielddictionary(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Msgfielddictionary);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




