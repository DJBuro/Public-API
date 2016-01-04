
using System;
using Cacd.Web.Common.Core;


namespace Loyalty.Data
{
	#region Status

	/// <summary>
	/// Status form for NHibernate mapped table 'tbl_Status'.
	/// </summary>
	public partial class StatusForm : BaseUserControl
		{
		#region Fields
		
		private Loyalty.Dao.Domain.Status _Status;

			
		#endregion
		
		#region Properties

		public Loyalty.Dao.Domain.Status Status
		{
			set { _Status = value; }
			get { return _Status; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Status = new Loyalty.Dao.Domain.Status ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Status = (Loyalty.Dao.Domain.Status)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Status;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("NameText.Text","Status.Name");
				BindingManager.AddBinding("DescriptionText.Text","Status.Description");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindStatusDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindStatusDropdownsAndDivSelectRegions();
		}


		
		
		private void BindStatusDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveStatus(object sender, EventArgs e)
			{
				if (Validate()){
					LoyaltyDAO.Save(Status);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




