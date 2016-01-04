
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Temporarycredential

	/// <summary>
	/// Temporarycredential form for NHibernate mapped table 'temporary_credential'.
	/// </summary>
	public partial class TemporarycredentialForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Temporarycredential _Temporarycredential;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Temporarycredential Temporarycredential
		{
			set { _Temporarycredential = value; }
			get { return _Temporarycredential; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Temporarycredential = new OrderTracking.Gps.Dao.Domain.Temporarycredential ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Temporarycredential = (OrderTracking.Gps.Dao.Domain.Temporarycredential)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Temporarycredential;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("ExpireText.Text","Temporarycredential.Expire");
				BindingManager.AddBinding("BotypeText.Text","Temporarycredential.Botype");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindTemporarycredentialDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindTemporarycredentialDropdownsAndDivSelectRegions();
		}


		
		
		private void BindTemporarycredentialDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveTemporarycredential(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Temporarycredential);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




