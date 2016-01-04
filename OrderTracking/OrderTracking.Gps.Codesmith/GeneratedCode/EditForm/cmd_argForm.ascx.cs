
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Cmdarg

	/// <summary>
	/// Cmdarg form for NHibernate mapped table 'cmd_arg'.
	/// </summary>
	public partial class CmdargForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Cmdarg _Cmdarg;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Cmdarg Cmdarg
		{
			set { _Cmdarg = value; }
			get { return _Cmdarg; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Cmdarg = new OrderTracking.Gps.Dao.Domain.Cmdarg ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Cmdarg = (OrderTracking.Gps.Dao.Domain.Cmdarg)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Cmdarg;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("SentenceText.Text","Cmdarg.Sentence");
				BindingManager.AddBinding("SentenceindexText.Text","Cmdarg.Sentenceindex");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindCmdargDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindCmdargDropdownsAndDivSelectRegions();
		}


		
		
		private void BindCmdargDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveCmdarg(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Cmdarg);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




