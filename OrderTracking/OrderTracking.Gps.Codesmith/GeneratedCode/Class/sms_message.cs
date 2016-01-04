
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Smsmessage

	/// <summary>
	/// Smsmessage object for NHibernate mapped table 'sms_message'.
	/// </summary>
	public partial  class Smsmessage
		{
		#region Member Variables
		
		protected long? _id;
		protected Providermessage _providermessage = new Providermessage();
		
		

		#endregion

		#region Constructors

		public Smsmessage() { }

		public Smsmessage( Providermessage providermessage )
		{
			this._providermessage = providermessage;
		}

		#endregion

		#region Public Properties

		public long? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public Providermessage Providermessage
		{
			get { return _providermessage; }
			set { _providermessage = value; }
		}


		#endregion
		
	}

	#endregion
}



