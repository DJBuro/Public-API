
using System;
using Cacd.Web.Common.Core;


namespace Loyalty.Data
{
	#region TblStatus

	/// <summary>
	/// TblStatus form for NHibernate mapped table 'tbl_Status'.
	/// </summary>
	public partial class TblStatusForm : BaseUserControl
		{
		#region Fields
		
		private Loyalty.Dao.Domain.TblStatus _TblStatus;

			
		#endregion
		
		#region Properties

		public Loyalty.Dao.Domain.TblStatus TblStatus
		{
			set { _TblStatus = value; }
			get { return _TblStatus; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_TblStatus = new Loyalty.Dao.Domain.TblStatus ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _TblStatus = (Loyalty.Dao.Domain.TblStatus)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _TblStatus;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("NameText.Text","TblStatus.Name");
				BindingManager.AddBinding("DescriptionText.Text","TblStatus.Description");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindTblStatusDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindTblStatusDropdownsAndDivSelectRegions();
		}


		
		
		private void BindTblStatusDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveTblStatus(object sender, EventArgs e)
			{
				if (Validate()){
					LoyaltyDAO.Save(TblStatus);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




