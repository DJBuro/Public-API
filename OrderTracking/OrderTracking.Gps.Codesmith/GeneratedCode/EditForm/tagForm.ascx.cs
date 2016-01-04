
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Tag

	/// <summary>
	/// Tag form for NHibernate mapped table 'tag'.
	/// </summary>
	public partial class TagForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Tag _Tag;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Tag Tag
		{
			set { _Tag = value; }
			get { return _Tag; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Tag = new OrderTracking.Gps.Dao.Domain.Tag ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Tag = (OrderTracking.Gps.Dao.Domain.Tag)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Tag;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("TagnameText.Text","Tag.Tagname");
				BindingManager.AddBinding("TagdescriptionText.Text","Tag.Tagdescription");
				BindingManager.AddBinding("BotypeText.Text","Tag.Botype");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindTagDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindTagDropdownsAndDivSelectRegions();
		}


		
		
		private void BindTagDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveTag(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Tag);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




