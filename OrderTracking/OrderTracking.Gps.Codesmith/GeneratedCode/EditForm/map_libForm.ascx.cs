
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Maplib

	/// <summary>
	/// Maplib form for NHibernate mapped table 'map_lib'.
	/// </summary>
	public partial class MaplibForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Maplib _Maplib;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Maplib Maplib
		{
			set { _Maplib = value; }
			get { return _Maplib; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Maplib = new OrderTracking.Gps.Dao.Domain.Maplib ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Maplib = (OrderTracking.Gps.Dao.Domain.Maplib)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Maplib;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("NameText.Text","Maplib.Name");
				BindingManager.AddBinding("BotypeText.Text","Maplib.Botype");
				BindingManager.AddBinding("CreatedText.Text","Maplib.Created");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindMaplibDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindMaplibDropdownsAndDivSelectRegions();
		}


		
		
		private void BindMaplibDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveMaplib(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Maplib);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




