
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Statisticdata

	/// <summary>
	/// Statisticdata form for NHibernate mapped table 'statistic_data'.
	/// </summary>
	public partial class StatisticdataForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Statisticdata _Statisticdata;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Statisticdata Statisticdata
		{
			set { _Statisticdata = value; }
			get { return _Statisticdata; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Statisticdata = new OrderTracking.Gps.Dao.Domain.Statisticdata ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Statisticdata = (OrderTracking.Gps.Dao.Domain.Statisticdata)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Statisticdata;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("ValueText.Text","Statisticdata.Value");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindStatisticdataDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindStatisticdataDropdownsAndDivSelectRegions();
		}


		
		
		private void BindStatisticdataDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveStatisticdata(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Statisticdata);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




