
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Notifier

	/// <summary>
	/// Notifier form for NHibernate mapped table 'notifier'.
	/// </summary>
	public partial class NotifierForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Notifier _Notifier;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Notifier Notifier
		{
			set { _Notifier = value; }
			get { return _Notifier; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Notifier = new OrderTracking.Gps.Dao.Domain.Notifier ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Notifier = (OrderTracking.Gps.Dao.Domain.Notifier)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Notifier;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("NameText.Text","Notifier.Name");
				BindingManager.AddBinding("BotypeText.Text","Notifier.Botype");
				BindingManager.AddBinding("CreatedText.Text","Notifier.Created");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindNotifierDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindNotifierDropdownsAndDivSelectRegions();
		}


		
		
		private void BindNotifierDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveNotifier(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Notifier);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




