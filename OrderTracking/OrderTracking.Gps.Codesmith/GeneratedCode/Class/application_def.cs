
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Applicationdef

	/// <summary>
	/// Applicationdef object for NHibernate mapped table 'application_def'.
	/// </summary>
	public partial  class Applicationdef
		{
		#region Member Variables
		
		protected string _id;
		protected string _applicationdefdescription;
		protected Loadabletype _loadabletype = new Loadabletype();
		
		
		protected Applicationdefreportviewer _applicationdefreportviewer;
		protected Applicationdefgateeventchannel _applicationdefgateeventchannel;

		#endregion

		#region Constructors

		public Applicationdef() { }

		public Applicationdef( string applicationdefdescription, Loadabletype loadabletype )
		{
			this._applicationdefdescription = applicationdefdescription;
			this._loadabletype = loadabletype;
		}

		#endregion

		#region Public Properties

		public string Id
		{
			get {return _id;}
			set
			{
				if ( value != null && value.Length > 128)
					throw new ArgumentOutOfRangeException("Invalid value for Id", value, value.ToString());
				_id = value;
			}
		}

		public string Applicationdefdescription
		{
			get { return _applicationdefdescription; }
			set
			{
				if ( value != null && value.Length > 255)
					throw new ArgumentOutOfRangeException("Invalid value for Applicationdefdescription", value, value.ToString());
				_applicationdefdescription = value;
			}
		}

		public Loadabletype Loadabletype
		{
			get { return _loadabletype; }
			set { _loadabletype = value; }
		}

		public Applicationdefreportviewer Applicationdefreportviewer
		{
			get { return _applicationdefreportviewer; }
			set { _applicationdefreportviewer = value; }
		}

		public Applicationdefgateeventchannel Applicationdefgateeventchannel
		{
			get { return _applicationdefgateeventchannel; }
			set { _applicationdefgateeventchannel = value; }
		}


		#endregion
		
	}

	#endregion
}



