
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Providermessagequeue

	/// <summary>
	/// Providermessagequeue form for NHibernate mapped table 'provider_message_queue'.
	/// </summary>
	public partial class ProvidermessagequeueForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Providermessagequeue _Providermessagequeue;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Providermessagequeue Providermessagequeue
		{
			set { _Providermessagequeue = value; }
			get { return _Providermessagequeue; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Providermessagequeue = new OrderTracking.Gps.Dao.Domain.Providermessagequeue ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Providermessagequeue = (OrderTracking.Gps.Dao.Domain.Providermessagequeue)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Providermessagequeue;
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
				BindProvidermessagequeueDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindProvidermessagequeueDropdownsAndDivSelectRegions();
		}


		
		
		private void BindProvidermessagequeueDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveProvidermessagequeue(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Providermessagequeue);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




