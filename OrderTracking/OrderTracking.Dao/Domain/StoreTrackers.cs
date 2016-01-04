
using System;
using System.Collections;


namespace OrderTracking.Dao.Domain
{
	#region StoreTracker

	/// <summary>
	/// StoreTracker object for NHibernate mapped table 'tbl_StoreTrackers'.
	/// </summary>
	public  class StoreTracker : Entity.Entity
		{
		#region Member Variables
		
		protected Tracker _trackerId;
		protected Store _storeId;
		
		

		#endregion

		#region Constructors

		public StoreTracker() { }

		public StoreTracker(Tracker tracker, Store store )
		{
			this._trackerId = tracker;
			this._storeId = store;
		}

		#endregion

		#region Public Properties

		public Tracker Tracker
		{
			get { return _trackerId; }
			set { _trackerId = value; }
		}

		public Store Store
		{
			get { return _storeId; }
			set { _storeId = value; }
		}


		#endregion
		
	}

	#endregion
}



