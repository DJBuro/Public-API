
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Gaterecordlatest

	/// <summary>
	/// Gaterecordlatest form for NHibernate mapped table 'gate_record_latest'.
	/// </summary>
	public partial class GaterecordlatestForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Gaterecordlatest _Gaterecordlatest;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Gaterecordlatest Gaterecordlatest
		{
			set { _Gaterecordlatest = value; }
			get { return _Gaterecordlatest; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Gaterecordlatest = new OrderTracking.Gps.Dao.Domain.Gaterecordlatest ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Gaterecordlatest = (OrderTracking.Gps.Dao.Domain.Gaterecordlatest)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Gaterecordlatest;
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
				BindGaterecordlatestDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindGaterecordlatestDropdownsAndDivSelectRegions();
		}


		
		
		private void BindGaterecordlatestDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveGaterecordlatest(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Gaterecordlatest);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




