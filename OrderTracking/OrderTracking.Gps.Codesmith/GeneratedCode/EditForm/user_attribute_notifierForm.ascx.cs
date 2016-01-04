
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Userattributenotifier

	/// <summary>
	/// Userattributenotifier form for NHibernate mapped table 'user_attribute_notifier'.
	/// </summary>
	public partial class UserattributenotifierForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Userattributenotifier _Userattributenotifier;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Userattributenotifier Userattributenotifier
		{
			set { _Userattributenotifier = value; }
			get { return _Userattributenotifier; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Userattributenotifier = new OrderTracking.Gps.Dao.Domain.Userattributenotifier ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Userattributenotifier = (OrderTracking.Gps.Dao.Domain.Userattributenotifier)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Userattributenotifier;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("HeaderText.Text","Userattributenotifier.Header");
				BindingManager.AddBinding("TypeText.Text","Userattributenotifier.Type");
				BindingManager.AddBinding("AttributekeyText.Text","Userattributenotifier.Attributekey");
				BindingManager.AddBinding("AttributevaluestartText.Text","Userattributenotifier.Attributevaluestart");
				BindingManager.AddBinding("AttributevalueendText.Text","Userattributenotifier.Attributevalueend");
				BindingManager.AddBinding("ApplicationidText.Text","Userattributenotifier.Applicationid");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindUserattributenotifierDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindUserattributenotifierDropdownsAndDivSelectRegions();
		}


		
		
		private void BindUserattributenotifierDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveUserattributenotifier(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Userattributenotifier);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




