
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Gateviewtag

	/// <summary>
	/// Gateviewtag form for NHibernate mapped table 'gate_view_tags'.
	/// </summary>
	public partial class GateviewtagForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Gateviewtag _Gateviewtag;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Gateviewtag Gateviewtag
		{
			set { _Gateviewtag = value; }
			get { return _Gateviewtag; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Gateviewtag = new OrderTracking.Gps.Dao.Domain.Gateviewtag ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Gateviewtag = (OrderTracking.Gps.Dao.Domain.Gateviewtag)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Gateviewtag;
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
				BindGateviewtagDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindGateviewtagDropdownsAndDivSelectRegions();
		}


		
		
		private void BindGateviewtagDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveGateviewtag(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Gateviewtag);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




