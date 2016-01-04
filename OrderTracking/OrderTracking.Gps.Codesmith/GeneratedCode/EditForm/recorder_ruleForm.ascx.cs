
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Recorderrule

	/// <summary>
	/// Recorderrule form for NHibernate mapped table 'recorder_rule'.
	/// </summary>
	public partial class RecorderruleForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Recorderrule _Recorderrule;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Recorderrule Recorderrule
		{
			set { _Recorderrule = value; }
			get { return _Recorderrule; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Recorderrule = new OrderTracking.Gps.Dao.Domain.Recorderrule ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Recorderrule = (OrderTracking.Gps.Dao.Domain.Recorderrule)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Recorderrule;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("NameText.Text","Recorderrule.Name");
				BindingManager.AddBinding("DescriptionText.Text","Recorderrule.Description");
				BindingManager.AddBinding("EnabledText.Text","Recorderrule.Enabled");
				BindingManager.AddBinding("ExeorderText.Text","Recorderrule.Exeorder");
				BindingManager.AddBinding("BotypeText.Text","Recorderrule.Botype");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindRecorderruleDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindRecorderruleDropdownsAndDivSelectRegions();
		}


		
		
		private void BindRecorderruleDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveRecorderrule(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Recorderrule);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




