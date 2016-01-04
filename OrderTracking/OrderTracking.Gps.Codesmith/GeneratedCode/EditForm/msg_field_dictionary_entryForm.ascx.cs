
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Msgfielddictionaryentry

	/// <summary>
	/// Msgfielddictionaryentry form for NHibernate mapped table 'msg_field_dictionary_entry'.
	/// </summary>
	public partial class MsgfielddictionaryentryForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Msgfielddictionaryentry _Msgfielddictionaryentry;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Msgfielddictionaryentry Msgfielddictionaryentry
		{
			set { _Msgfielddictionaryentry = value; }
			get { return _Msgfielddictionaryentry; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Msgfielddictionaryentry = new OrderTracking.Gps.Dao.Domain.Msgfielddictionaryentry ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Msgfielddictionaryentry = (OrderTracking.Gps.Dao.Domain.Msgfielddictionaryentry)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Msgfielddictionaryentry;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("MultiplicatorText.Text","Msgfielddictionaryentry.Multiplicator");
				BindingManager.AddBinding("ConstantText.Text","Msgfielddictionaryentry.Constant");
				BindingManager.AddBinding("EnabledText.Text","Msgfielddictionaryentry.Enabled");
				BindingManager.AddBinding("SavewithposText.Text","Msgfielddictionaryentry.Savewithpos");
				BindingManager.AddBinding("SavechangesonlyText.Text","Msgfielddictionaryentry.Savechangesonly");
				BindingManager.AddBinding("MultiplicatorformulaText.Text","Msgfielddictionaryentry.Multiplicatorformula");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindMsgfielddictionaryentryDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindMsgfielddictionaryentryDropdownsAndDivSelectRegions();
		}


		
		
		private void BindMsgfielddictionaryentryDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveMsgfielddictionaryentry(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Msgfielddictionaryentry);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




