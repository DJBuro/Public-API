
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Applicationrule

	/// <summary>
	/// Applicationrule form for NHibernate mapped table 'application_rule'.
	/// </summary>
	public partial class ApplicationruleForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Applicationrule _Applicationrule;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Applicationrule Applicationrule
		{
			set { _Applicationrule = value; }
			get { return _Applicationrule; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Applicationrule = new OrderTracking.Gps.Dao.Domain.Applicationrule ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Applicationrule = (OrderTracking.Gps.Dao.Domain.Applicationrule)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Applicationrule;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("NameText.Text","Applicationrule.Name");
				BindingManager.AddBinding("DescriptionText.Text","Applicationrule.Description");
				BindingManager.AddBinding("EnabledText.Text","Applicationrule.Enabled");
				BindingManager.AddBinding("ExeorderText.Text","Applicationrule.Exeorder");
				BindingManager.AddBinding("BotypeText.Text","Applicationrule.Botype");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindApplicationruleDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindApplicationruleDropdownsAndDivSelectRegions();
		}


		
		
		private void BindApplicationruleDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveApplicationrule(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Applicationrule);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




