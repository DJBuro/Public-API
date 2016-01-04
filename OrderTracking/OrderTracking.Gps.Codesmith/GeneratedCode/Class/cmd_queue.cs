
using System;
using System.Collections;


namespace OrderTracking.Gps.Dao.Domain
{
	#region Cmdqueue

	/// <summary>
	/// Cmdqueue object for NHibernate mapped table 'cmd_queue'.
	/// </summary>
	public partial  class Cmdqueue
		{
		#region Member Variables
		
		protected int? _id;
		protected string _botype;
		protected Device _device = new Device();
		
		
		protected IList _queueidcmdqueueitems;

		#endregion

		#region Constructors

		public Cmdqueue() { }

		public Cmdqueue( string botype, Device device )
		{
			this._botype = botype;
			this._device = device;
		}

		#endregion

		#region Public Properties

		public int? Id
		{
			get {return _id;}
			set {_id = value;}
		}

		public string Botype
		{
			get { return _botype; }
			set
			{
				if ( value != null && value.Length > 128)
					throw new ArgumentOutOfRangeException("Invalid value for Botype", value, value.ToString());
				_botype = value;
			}
		}

		public Device Device
		{
			get { return _device; }
			set { _device = value; }
		}

		public IList queue_idcmd_queue_items
		{
			get
			{
				if (_queueidcmdqueueitems==null)
				{
					_queueidcmdqueueitems = new ArrayList();
				}
				return _queueidcmdqueueitems;
			}
			set { _queueidcmdqueueitems = value; }
		}


		#endregion
		
	}

	#endregion
}



