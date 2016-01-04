
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Gatemessagerecord

	/// <summary>
	/// Gatemessagerecord form for NHibernate mapped table 'gate_message_record'.
	/// </summary>
	public partial class GatemessagerecordForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Gatemessagerecord _Gatemessagerecord;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Gatemessagerecord Gatemessagerecord
		{
			set { _Gatemessagerecord = value; }
			get { return _Gatemessagerecord; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Gatemessagerecord = new OrderTracking.Gps.Dao.Domain.Gatemessagerecord ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Gatemessagerecord = (OrderTracking.Gps.Dao.Domain.Gatemessagerecord)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Gatemessagerecord;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("DataboolText.Text","Gatemessagerecord.Databool");
				BindingManager.AddBinding("DataintText.Text","Gatemessagerecord.Dataint");
				BindingManager.AddBinding("DatadoubleText.Text","Gatemessagerecord.Datadouble");
				BindingManager.AddBinding("DatatextText.Text","Gatemessagerecord.Datatext");
				BindingManager.AddBinding("DatalongtextText.Text","Gatemessagerecord.Datalongtext");
				BindingManager.AddBinding("DatadatetimeText.Text","Gatemessagerecord.Datadatetime");
				BindingManager.AddBinding("SavechangesonlyText.Text","Gatemessagerecord.Savechangesonly");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindGatemessagerecordDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindGatemessagerecordDropdownsAndDivSelectRegions();
		}


		
		
		private void BindGatemessagerecordDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveGatemessagerecord(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Gatemessagerecord);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




