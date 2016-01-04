
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Smsmessage

	/// <summary>
	/// Smsmessage form for NHibernate mapped table 'sms_message'.
	/// </summary>
	public partial class SmsmessageForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Smsmessage _Smsmessage;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Smsmessage Smsmessage
		{
			set { _Smsmessage = value; }
			get { return _Smsmessage; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Smsmessage = new OrderTracking.Gps.Dao.Domain.Smsmessage ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Smsmessage = (OrderTracking.Gps.Dao.Domain.Smsmessage)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Smsmessage;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindSmsmessageDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindSmsmessageDropdownsAndDivSelectRegions();
		}


		
		
		private void BindSmsmessageDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveSmsmessage(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Smsmessage);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




