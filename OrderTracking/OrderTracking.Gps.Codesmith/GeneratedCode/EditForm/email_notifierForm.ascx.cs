
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Emailnotifier

	/// <summary>
	/// Emailnotifier form for NHibernate mapped table 'email_notifier'.
	/// </summary>
	public partial class EmailnotifierForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Emailnotifier _Emailnotifier;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Emailnotifier Emailnotifier
		{
			set { _Emailnotifier = value; }
			get { return _Emailnotifier; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Emailnotifier = new OrderTracking.Gps.Dao.Domain.Emailnotifier ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Emailnotifier = (OrderTracking.Gps.Dao.Domain.Emailnotifier)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Emailnotifier;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("SubjectText.Text","Emailnotifier.Subject");
				BindingManager.AddBinding("IshtmlText.Text","Emailnotifier.Ishtml");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindEmailnotifierDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindEmailnotifierDropdownsAndDivSelectRegions();
		}


		
		
		private void BindEmailnotifierDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveEmailnotifier(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Emailnotifier);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




