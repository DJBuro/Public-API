
using System;
using System.Collections;
using System.Xml.Serialization;


namespace OrderTracking.Dao.Domain
{
	#region ClientStore

	/// <summary>
    /// ClientStore object for NHibernate mapped table 'ClientStore'.
	/// </summary>
    public class ClientStore : Entity.Entity
		{
		#region Member Variables
		
		protected long _clientId;
        protected long _storeId;

		#endregion

		#region Constructors

		public ClientStore() 
		{ 
		}

        public ClientStore(
            long clientId,
            long storeId)
		{
            this._clientId = clientId;
            this._storeId = storeId;
		}

		#endregion

		#region Public Properties

        public long ClientId
		{
            get { return _clientId; }
			set
			{
                _clientId = value;
			}
		}

        public long StoreId
		{
            get { return _storeId; }
			set
			{
                _storeId = value;
			}
		}

		#endregion
		
	}

	#endregion
}



