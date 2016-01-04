
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Scriptpluginapplication

	/// <summary>
	/// Scriptpluginapplication object for NHibernate mapped table 'script_plugin_application'.
	/// </summary>
	public partial  class Scriptpluginapplication
		{
		#region Member Variables
		
		protected int? _id;
		protected Application _application = new Application();
		protected Scriptplugin _scriptplugin = new Scriptplugin();
		
		

		#endregion

		#region Constructors

		public Scriptpluginapplication() { }

		public Scriptpluginapplication( Application application, Scriptplugin scriptplugin )
		{
			this._application = application;
			this._scriptplugin = scriptplugin;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public Application Application
		{
			get { return _application; }
			set { _application = value; }
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



