
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Gateeventchanneldictionaryentry

	/// <summary>
	/// Gateeventchanneldictionaryentry form for NHibernate mapped table 'gate_event_channel_dictionary_entry'.
	/// </summary>
	public partial class GateeventchanneldictionaryentryForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Gateeventchanneldictionaryentry _Gateeventchanneldictionaryentry;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Gateeventchanneldictionaryentry Gateeventchanneldictionaryentry
		{
			set { _Gateeventchanneldictionaryentry = value; }
			get { return _Gateeventchanneldictionaryentry; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Gateeventchanneldictionaryentry = new OrderTracking.Gps.Dao.Domain.Gateeventchanneldictionaryentry ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Gateeventchanneldictionaryentry = (OrderTracking.Gps.Dao.Domain.Gateeventchanneldictionaryentry)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Gateeventchanneldictionaryentry;
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
				BindGateeventchanneldictionaryentryDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindGateeventchanneldictionaryentryDropdownsAndDivSelectRegions();
		}


		
		
		private void BindGateeventchanneldictionaryentryDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveGateeventchanneldictionaryentry(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Gateeventchanneldictionaryentry);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




