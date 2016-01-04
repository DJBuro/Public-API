
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Scriptpage

	/// <summary>
	/// Scriptpage form for NHibernate mapped table 'script_page'.
	/// </summary>
	public partial class ScriptpageForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Scriptpage _Scriptpage;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Scriptpage Scriptpage
		{
			set { _Scriptpage = value; }
			get { return _Scriptpage; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Scriptpage = new OrderTracking.Gps.Dao.Domain.Scriptpage ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Scriptpage = (OrderTracking.Gps.Dao.Domain.Scriptpage)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Scriptpage;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("ApplicationbotypeText.Text","Scriptpage.Applicationbotype");
				BindingManager.AddBinding("PagenameText.Text","Scriptpage.Pagename");
				BindingManager.AddBinding("CreatedText.Text","Scriptpage.Created");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindScriptpageDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindScriptpageDropdownsAndDivSelectRegions();
		}


		
		
		private void BindScriptpageDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveScriptpage(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Scriptpage);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




