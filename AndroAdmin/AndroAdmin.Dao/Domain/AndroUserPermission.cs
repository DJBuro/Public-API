
using System;
using System.Collections;


namespace AndroAdmin.Dao.Domain
{
	#region AndroUserPermission

	/// <summary>
	/// AndroUserPermission object for NHibernate mapped table 'tbl_AndroUserPermission'.
	/// </summary>
	public class AndroUserPermission : Entity.Entity
		{
		#region Member Variables
		
		protected AndroUser _androUser;
		protected Project _project;

		#endregion

		#region Constructors

		public AndroUserPermission() { }

		public AndroUserPermission( AndroUser androUser, Project project )
		{
			this._androUser = androUser;
			this._project = project;
		}

		#endregion

		#region Public Properties

		public AndroUser AndroUser
		{
			get { return _androUser; }
			set { _androUser = value; }
		}

		public Project Project
		{
			get { return _project; }
			set { _project = value; }
		}


		#endregion
		
	}

	#endregion
}



