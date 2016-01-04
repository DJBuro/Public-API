
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Messagetemplate

	/// <summary>
	/// Messagetemplate form for NHibernate mapped table 'message_template'.
	/// </summary>
	public partial class MessagetemplateForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Messagetemplate _Messagetemplate;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Messagetemplate Messagetemplate
		{
			set { _Messagetemplate = value; }
			get { return _Messagetemplate; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Messagetemplate = new OrderTracking.Gps.Dao.Domain.Messagetemplate ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Messagetemplate = (OrderTracking.Gps.Dao.Domain.Messagetemplate)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Messagetemplate;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("TemplatetextText.Text","Messagetemplate.Templatetext");
				BindingManager.AddBinding("BotypeText.Text","Messagetemplate.Botype");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindMessagetemplateDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindMessagetemplateDropdownsAndDivSelectRegions();
		}


		
		
		private void BindMessagetemplateDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveMessagetemplate(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Messagetemplate);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




