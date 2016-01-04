
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Taguser

	/// <summary>
	/// Taguser object for NHibernate mapped table 'tag_users'.
	/// </summary>
	public partial  class Taguser
		{
		#region Member Variables
		
		protected DateTime? _servertimestamp;
		protected int? _servertimestampms;
		protected Tag _tag = new Tag();
		protected User _user = new User();
		
		

		#endregion

		#region Constructors

		public Taguser() { }

		public Taguser( DateTime? servertimestamp, int? servertimestampms, Tag tag, User user )
		{
			this._servertimestamp = servertimestamp;
			this._servertimestampms = servertimestampms;
			this._tag = tag;
			this._user = user;
		}

		#endregion

		#region Public Properties


		public DateTime? Servertimestamp
		{
			get { return _servertimestamp; }
			set { _servertimestamp = value; }
		}

		public int? Servertimestampms
		{
			get { return _servertimestampms; }
			set { _servertimestampms = value; }
		}

		public Tag Tag
		{
			get { return _tag; }
			set { _tag = value; }
		}

		public User User
		{
			get { return _user; }
			set { _user = value; }
		}


		#endregion
		
	}

	#endregion
}



