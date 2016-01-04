
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Scriptpage

	/// <summary>
	/// Scriptpage object for NHibernate mapped table 'script_page'.
	/// </summary>
	public partial  class Scriptpage
		{
		#region Member Variables
		
		protected int? _id;
		protected string _applicationbotype;
		protected string _pagename;
		protected DateTime? _created;
		protected Scriptplugin _scriptplugin = new Scriptplugin();
		
		

		#endregion

		#region Constructors

		public Scriptpage() { }

		public Scriptpage( string applicationbotype, string pagename, DateTime? created, Scriptplugin scriptplugin )
		{
			this._applicationbotype = applicationbotype;
			this._pagename = pagename;
			this._created = created;
			this._scriptplugin = scriptplugin;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public string Applicationbotype
		{
			get { return _applicationbotype; }
			set
			{
				if ( value != null && value.Length > 128)
					throw new ArgumentOutOfRangeException("Invalid value for Applicationbotype", value, value.ToString());
				_applicationbotype = value;
			}
		}

		public string Pagename
		{
			get { return _pagename; }
			set
			{
				if ( value != null && value.Length > 255)
					throw new ArgumentOutOfRangeException("Invalid value for Pagename", value, value.ToString());
				_pagename = value;
			}
		}

		public DateTime? Created
		{
			get { return _created; }
			set { _created = value; }
		}

		public Scriptplugin Scriptplugin
		{
			get { return _scriptplugin; }
			set { _scriptplugin = value; }
		}


		#endregion
		
	}

	#endregion
}



