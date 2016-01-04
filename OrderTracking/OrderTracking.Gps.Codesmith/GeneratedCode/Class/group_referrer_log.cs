
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Groupreferrerlog

	/// <summary>
	/// Groupreferrerlog object for NHibernate mapped table 'group_referrer_log'.
	/// </summary>
	public partial  class Groupreferrerlog
		{
		#region Member Variables
		
		protected int? _id;
		protected string _refurl;
		protected long? _hits;
		protected DateTime? _timestamp;
		protected DateTime? _created;
		protected Group _group = new Group();
		
		

		#endregion

		#region Constructors

		public Groupreferrerlog() { }

		public Groupreferrerlog( string refurl, long? hits, DateTime? timestamp, DateTime? created, Group group )
		{
			this._refurl = refurl;
			this._hits = hits;
			this._timestamp = timestamp;
			this._created = created;
			this._group = group;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public string Refurl
		{
			get { return _refurl; }
			set
			{
				if ( value != null && value.Length > 255)
					throw new ArgumentOutOfRangeException("Invalid value for Refurl", value, value.ToString());
				_refurl = value;
			}
		}

		public long? Hits
		{
			get { return _hits; }
			set { _hits = value; }
		}

		public DateTime? Timestamp
		{
			get { return _timestamp; }
			set { _timestamp = value; }
		}

		public DateTime? Created
		{
			get { return _created; }
			set { _created = value; }
		}

		public Group Group
		{
			get { return _group; }
			set { _group = value; }
		}


		#endregion
		
	}

	#endregion
}



