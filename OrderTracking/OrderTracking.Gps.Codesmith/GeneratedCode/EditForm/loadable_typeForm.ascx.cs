
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Loadabletype

	/// <summary>
	/// Loadabletype form for NHibernate mapped table 'loadable_type'.
	/// </summary>
	public partial class LoadabletypeForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Loadabletype _Loadabletype;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Loadabletype Loadabletype
		{
			set { _Loadabletype = value; }
			get { return _Loadabletype; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Loadabletype = new OrderTracking.Gps.Dao.Domain.Loadabletype ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Loadabletype = (OrderTracking.Gps.Dao.Domain.Loadabletype)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Loadabletype;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("AssemblynameText.Text","Loadabletype.Assemblyname");
				BindingManager.AddBinding("TypenameText.Text","Loadabletype.Typename");
				BindingManager.AddBinding("TypedescriptionText.Text","Loadabletype.Typedescription");
				BindingManager.AddBinding("BasetypenameText.Text","Loadabletype.Basetypename");
				BindingManager.AddBinding("BasetypedescriptionText.Text","Loadabletype.Basetypedescription");
				BindingManager.AddBinding("DeletedText.Text","Loadabletype.Deleted");
				BindingManager.AddBinding("CreatedText.Text","Loadabletype.Created");
				BindingManager.AddBinding("VersionText.Text","Loadabletype.Version");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindLoadabletypeDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindLoadabletypeDropdownsAndDivSelectRegions();
		}


		
		
		private void BindLoadabletypeDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveLoadabletype(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Loadabletype);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




