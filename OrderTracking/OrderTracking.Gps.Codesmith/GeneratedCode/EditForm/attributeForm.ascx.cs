
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Attribute

	/// <summary>
	/// Attribute form for NHibernate mapped table 'attribute'.
	/// </summary>
	public partial class AttributeForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Attribute _Attribute;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Attribute Attribute
		{
			set { _Attribute = value; }
			get { return _Attribute; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Attribute = new OrderTracking.Gps.Dao.Domain.Attribute ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Attribute = (OrderTracking.Gps.Dao.Domain.Attribute)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Attribute;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("AttributenameText.Text","Attribute.Attributename");
				BindingManager.AddBinding("AttributetypeText.Text","Attribute.Attributetype");
				BindingManager.AddBinding("IntvalueText.Text","Attribute.Intvalue");
				BindingManager.AddBinding("DoublevalueText.Text","Attribute.Doublevalue");
				BindingManager.AddBinding("StringvalueText.Text","Attribute.Stringvalue");
				BindingManager.AddBinding("BoolvalueText.Text","Attribute.Boolvalue");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindAttributeDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindAttributeDropdownsAndDivSelectRegions();
		}


		
		
		private void BindAttributeDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveAttribute(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Attribute);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




