
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Statistickey

	/// <summary>
	/// Statistickey form for NHibernate mapped table 'statistic_key'.
	/// </summary>
	public partial class StatistickeyForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Statistickey _Statistickey;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Statistickey Statistickey
		{
			set { _Statistickey = value; }
			get { return _Statistickey; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Statistickey = new OrderTracking.Gps.Dao.Domain.Statistickey ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Statistickey = (OrderTracking.Gps.Dao.Domain.Statistickey)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Statistickey;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("NameText.Text","Statistickey.Name");
				BindingManager.AddBinding("TypeText.Text","Statistickey.Type");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindStatistickeyDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindStatistickeyDropdownsAndDivSelectRegions();
		}


		
		
		private void BindStatistickeyDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveStatistickey(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Statistickey);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




