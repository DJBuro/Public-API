
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Unit

	/// <summary>
	/// Unit form for NHibernate mapped table 'unit'.
	/// </summary>
	public partial class UnitForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Unit _Unit;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Unit Unit
		{
			set { _Unit = value; }
			get { return _Unit; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Unit = new OrderTracking.Gps.Dao.Domain.Unit ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Unit = (OrderTracking.Gps.Dao.Domain.Unit)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Unit;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("SymbolText.Text","Unit.Symbol");
				BindingManager.AddBinding("MeasureText.Text","Unit.Measure");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindUnitDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindUnitDropdownsAndDivSelectRegions();
		}


		
		
		private void BindUnitDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveUnit(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Unit);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




