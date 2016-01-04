
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Grouprouterule

	/// <summary>
	/// Grouprouterule form for NHibernate mapped table 'group_route_rule'.
	/// </summary>
	public partial class GrouprouteruleForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Grouprouterule _Grouprouterule;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Grouprouterule Grouprouterule
		{
			set { _Grouprouterule = value; }
			get { return _Grouprouterule; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Grouprouterule = new OrderTracking.Gps.Dao.Domain.Grouprouterule ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Grouprouterule = (OrderTracking.Gps.Dao.Domain.Grouprouterule)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Grouprouterule;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("ServerroutelabelText.Text","Grouprouterule.Serverroutelabel");
				BindingManager.AddBinding("ProviderroutelabelText.Text","Grouprouterule.Providerroutelabel");
				BindingManager.AddBinding("TransportText.Text","Grouprouterule.Transport");
				BindingManager.AddBinding("AutoaddText.Text","Grouprouterule.Autoadd");
				BindingManager.AddBinding("BotypeText.Text","Grouprouterule.Botype");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindGrouprouteruleDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindGrouprouteruleDropdownsAndDivSelectRegions();
		}


		
		
		private void BindGrouprouteruleDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveGrouprouterule(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Grouprouterule);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




