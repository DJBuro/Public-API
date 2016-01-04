
using System;
using System.Collections;
using System.Collections.Generic;
using Cacd.Core.Utils;
using Cacd.Domain.Directories;
using NHibernate;
using NHibernate.Expression;

namespace Cacd.DAO.NHibernate.OrderTracking
{

		#region AccountDAO

		/// <summary>
		/// Account object for NHibernate mapped table 'tbl_Account'.
		/// </summary>
		public  class AccountDAO : GenericNHibernateDAO<Account,long?>
			{
			public AccountDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region ApnDAO

		/// <summary>
		/// Apn object for NHibernate mapped table 'tbl_Apn'.
		/// </summary>
		public  class ApnDAO : GenericNHibernateDAO<Apn,long?>
			{
			public ApnDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region CommandTypeDAO

		/// <summary>
		/// CommandType object for NHibernate mapped table 'tbl_CommandType'.
		/// </summary>
		public  class CommandTypeDAO : GenericNHibernateDAO<CommandType,long?>
			{
			public CommandTypeDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region CoordinateDAO

		/// <summary>
		/// Coordinate object for NHibernate mapped table 'tbl_Coordinates'.
		/// </summary>
		public  class CoordinateDAO : GenericNHibernateDAO<Coordinate,long?>
			{
			public CoordinateDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region CustomerDAO

		/// <summary>
		/// Customer object for NHibernate mapped table 'tbl_Customer'.
		/// </summary>
		public  class CustomerDAO : GenericNHibernateDAO<Customer,long?>
			{
			public CustomerDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region CustomerOrderDAO

		/// <summary>
		/// CustomerOrder object for NHibernate mapped table 'tbl_CustomerOrders'.
		/// </summary>
		public  class CustomerOrderDAO : GenericNHibernateDAO<CustomerOrder,long?>
			{
			public CustomerOrderDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region DriverDAO

		/// <summary>
		/// Driver object for NHibernate mapped table 'tbl_Driver'.
		/// </summary>
		public  class DriverDAO : GenericNHibernateDAO<Driver,long?>
			{
			public DriverDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region DriverOrderDAO

		/// <summary>
		/// DriverOrder object for NHibernate mapped table 'tbl_DriverOrders'.
		/// </summary>
		public  class DriverOrderDAO : GenericNHibernateDAO<DriverOrder,long?>
			{
			public DriverOrderDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region ItemDAO

		/// <summary>
		/// Item object for NHibernate mapped table 'tbl_Item'.
		/// </summary>
		public  class ItemDAO : GenericNHibernateDAO<Item,long?>
			{
			public ItemDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region LogDAO

		/// <summary>
		/// Log object for NHibernate mapped table 'tbl_Log'.
		/// </summary>
		public  class LogDAO : GenericNHibernateDAO<Log,long?>
			{
			public LogDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region OrderDAO

		/// <summary>
		/// Order object for NHibernate mapped table 'tbl_Order'.
		/// </summary>
		public  class OrderDAO : GenericNHibernateDAO<Order,long?>
			{
			public OrderDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region OrderStatusDAO

		/// <summary>
		/// OrderStatus object for NHibernate mapped table 'tbl_OrderStatus'.
		/// </summary>
		public  class OrderStatusDAO : GenericNHibernateDAO<OrderStatus,long?>
			{
			public OrderStatusDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region PostCodeDAO

		/// <summary>
		/// PostCode object for NHibernate mapped table 'tbl_PostCode'.
		/// </summary>
		public  class PostCodeDAO : GenericNHibernateDAO<PostCode,long?>
			{
			public PostCodeDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region SmsCredentialDAO

		/// <summary>
		/// SmsCredential object for NHibernate mapped table 'tbl_SmsCredentials'.
		/// </summary>
		public  class SmsCredentialDAO : GenericNHibernateDAO<SmsCredential,long?>
			{
			public SmsCredentialDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region StatusDAO

		/// <summary>
		/// Status object for NHibernate mapped table 'tbl_Status'.
		/// </summary>
		public  class StatusDAO : GenericNHibernateDAO<Status,long?>
			{
			public StatusDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region StoreDAO

		/// <summary>
		/// Store object for NHibernate mapped table 'tbl_Store'.
		/// </summary>
		public  class StoreDAO : GenericNHibernateDAO<Store,long?>
			{
			public StoreDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region TrackerDAO

		/// <summary>
		/// Tracker object for NHibernate mapped table 'tbl_Tracker'.
		/// </summary>
		public  class TrackerDAO : GenericNHibernateDAO<Tracker,long?>
			{
			public TrackerDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region TrackerCommandDAO

		/// <summary>
		/// TrackerCommand object for NHibernate mapped table 'tbl_TrackerCommands'.
		/// </summary>
		public  class TrackerCommandDAO : GenericNHibernateDAO<TrackerCommand,long?>
			{
			public TrackerCommandDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region TrackerStatusDAO

		/// <summary>
		/// TrackerStatus object for NHibernate mapped table 'tbl_TrackerStatus'.
		/// </summary>
		public  class TrackerStatusDAO : GenericNHibernateDAO<TrackerStatus,long?>
			{
			public TrackerStatusDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region TrackerTypeDAO

		/// <summary>
		/// TrackerType object for NHibernate mapped table 'tbl_TrackerType'.
		/// </summary>
		public  class TrackerTypeDAO : GenericNHibernateDAO<TrackerType,long?>
			{
			public TrackerTypeDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion

}





		

