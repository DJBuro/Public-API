
using System;
using System.Collections;


namespace Dashboard.Dao.Domain
{
	#region SiteMessage

	/// <summary>
	/// SiteMessage object for NHibernate mapped table 'SiteMessages'.
	/// </summary>
	public class SiteMessage : Entity
		{
		#region Member Variables
		
		protected string _message;
		protected DateTime? _createdDate;
		protected string _createdBy;
		protected HeadOffice _headOffice;
		
		

		#endregion

		#region Constructors

		public SiteMessage() { }

		public SiteMessage( string message, DateTime? createdDate, string createdBy, HeadOffice headOffice )
		{
			this._message = message;
			this._createdDate = createdDate;
			this._createdBy = createdBy;
			this._headOffice = headOffice;
		}

		#endregion

		#region Public Properties

		public string Message
		{
			get { return _message; }
			set
			{
				if ( value != null && value.Length > 1000)
					throw new ArgumentOutOfRangeException("Invalid value for Message", value, value.ToString());
				_message = value;
			}
		}

		public DateTime? CreatedDate
		{
			get { return _createdDate; }
			set { _createdDate = value; }
		}

		public string CreatedBy
		{
			get { return _createdBy; }
			set
			{
				if ( value != null && value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for CreatedBy", value, value.ToString());
				_createdBy = value;
			}
		}

		public HeadOffice HeadOffice
		{
			get { return _headOffice; }
			set { _headOffice = value; }
		}


		#endregion
		
	}

	#endregion
}



