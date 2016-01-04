
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Templatecmdstep

	/// <summary>
	/// Templatecmdstep form for NHibernate mapped table 'template_cmd_step'.
	/// </summary>
	public partial class TemplatecmdstepForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Templatecmdstep _Templatecmdstep;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Templatecmdstep Templatecmdstep
		{
			set { _Templatecmdstep = value; }
			get { return _Templatecmdstep; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Templatecmdstep = new OrderTracking.Gps.Dao.Domain.Templatecmdstep ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Templatecmdstep = (OrderTracking.Gps.Dao.Domain.Templatecmdstep)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Templatecmdstep;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("TemplateText.Text","Templatecmdstep.Template");
				BindingManager.AddBinding("TransportText.Text","Templatecmdstep.Transport");
				BindingManager.AddBinding("StepdescriptionText.Text","Templatecmdstep.Stepdescription");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindTemplatecmdstepDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindTemplatecmdstepDropdownsAndDivSelectRegions();
		}


		
		
		private void BindTemplatecmdstepDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveTemplatecmdstep(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Templatecmdstep);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




