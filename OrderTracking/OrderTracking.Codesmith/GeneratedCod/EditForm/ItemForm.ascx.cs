
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Data
{
	#region Item

	/// <summary>
	/// Item form for NHibernate mapped table 'tbl_Item'.
	/// </summary>
	public partial class ItemForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Dao.Domain.Item _Item;

			
		#endregion
		
		#region Properties

		public OrderTracking.Dao.Domain.Item Item
		{
			set { _Item = value; }
			get { return _Item; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Item = new OrderTracking.Dao.Domain.Item ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Item = (OrderTracking.Dao.Domain.Item)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Item;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("QuantityText.Text","Item.Quantity");
				BindingManager.AddBinding("NameText.Text","Item.Name");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindItemDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindItemDropdownsAndDivSelectRegions();
		}


		
		
		private void BindItemDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveItem(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingDAO.Save(Item);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




