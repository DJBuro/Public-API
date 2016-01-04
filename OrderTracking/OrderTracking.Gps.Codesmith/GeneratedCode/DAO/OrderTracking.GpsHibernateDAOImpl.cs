
using System;
using System.Collections;
using System.Collections.Generic;
using Cacd.Core.Utils;
using Cacd.Domain.Directories;
using NHibernate;
using NHibernate.Expression;

namespace Cacd.DAO.NHibernate.OrderTracking.Gps
{

		#region AdminrightDAO

		/// <summary>
		/// Adminright object for NHibernate mapped table 'admin_rights'.
		/// </summary>
		public  class AdminrightDAO : GenericNHibernateDAO<Adminright,int?>
			{
			public AdminrightDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region AppsettingDAO

		/// <summary>
		/// Appsetting object for NHibernate mapped table 'app_settings'.
		/// </summary>
		public  class AppsettingDAO : GenericNHibernateDAO<Appsetting,int?>
			{
			public AppsettingDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region ApptemplateDAO

		/// <summary>
		/// Apptemplate object for NHibernate mapped table 'app_templates'.
		/// </summary>
		public  class ApptemplateDAO : GenericNHibernateDAO<Apptemplate,int?>
			{
			public ApptemplateDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region ApplicationdefDAO

		/// <summary>
		/// Applicationdef object for NHibernate mapped table 'application_def'.
		/// </summary>
		public  class ApplicationdefDAO : GenericNHibernateDAO<Applicationdef,string>
			{
			public ApplicationdefDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region ApplicationdefgateeventchannelDAO

		/// <summary>
		/// Applicationdefgateeventchannel object for NHibernate mapped table 'application_def_gate_event_channels'.
		/// </summary>
		public  class ApplicationdefgateeventchannelDAO : GenericNHibernateDAO<Applicationdefgateeventchannel,int?>
			{
			public ApplicationdefgateeventchannelDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region ApplicationdefreportviewerDAO

		/// <summary>
		/// Applicationdefreportviewer object for NHibernate mapped table 'application_def_report_viewer'.
		/// </summary>
		public  class ApplicationdefreportviewerDAO : GenericNHibernateDAO<Applicationdefreportviewer,int?>
			{
			public ApplicationdefreportviewerDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region ApplicationruleDAO

		/// <summary>
		/// Applicationrule object for NHibernate mapped table 'application_rule'.
		/// </summary>
		public  class ApplicationruleDAO : GenericNHibernateDAO<Applicationrule,int?>
			{
			public ApplicationruleDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region ApplicationDAO

		/// <summary>
		/// Application object for NHibernate mapped table 'applications'.
		/// </summary>
		public  class ApplicationDAO : GenericNHibernateDAO<Application,int?>
			{
			public ApplicationDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region AttributeDAO

		/// <summary>
		/// Attribute object for NHibernate mapped table 'attribute'.
		/// </summary>
		public  class AttributeDAO : GenericNHibernateDAO<Attribute,int?>
			{
			public AttributeDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region ChannelnotifierDAO

		/// <summary>
		/// Channelnotifier object for NHibernate mapped table 'channel_notifier'.
		/// </summary>
		public  class ChannelnotifierDAO : GenericNHibernateDAO<Channelnotifier,int?>
			{
			public ChannelnotifierDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region CmdargDAO

		/// <summary>
		/// Cmdarg object for NHibernate mapped table 'cmd_arg'.
		/// </summary>
		public  class CmdargDAO : GenericNHibernateDAO<Cmdarg,long?>
			{
			public CmdargDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region CmdqueueDAO

		/// <summary>
		/// Cmdqueue object for NHibernate mapped table 'cmd_queue'.
		/// </summary>
		public  class CmdqueueDAO : GenericNHibernateDAO<Cmdqueue,int?>
			{
			public CmdqueueDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region CmdqueueitemDAO

		/// <summary>
		/// Cmdqueueitem object for NHibernate mapped table 'cmd_queue_item'.
		/// </summary>
		public  class CmdqueueitemDAO : GenericNHibernateDAO<Cmdqueueitem,long?>
			{
			public CmdqueueitemDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region CountryDAO

		/// <summary>
		/// Country object for NHibernate mapped table 'country'.
		/// </summary>
		public  class CountryDAO : GenericNHibernateDAO<Country,string>
			{
			public CountryDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region DatumDAO

		/// <summary>
		/// Datum object for NHibernate mapped table 'datum'.
		/// </summary>
		public  class DatumDAO : GenericNHibernateDAO<Datum,int?>
			{
			public DatumDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region DeviceDAO

		/// <summary>
		/// Device object for NHibernate mapped table 'device'.
		/// </summary>
		public  class DeviceDAO : GenericNHibernateDAO<Device,int?>
			{
			public DeviceDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region DevicedefDAO

		/// <summary>
		/// Devicedef object for NHibernate mapped table 'device_def'.
		/// </summary>
		public  class DevicedefDAO : GenericNHibernateDAO<Devicedef,int?>
			{
			public DevicedefDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region DevicedeffieldDAO

		/// <summary>
		/// Devicedeffield object for NHibernate mapped table 'device_def_field'.
		/// </summary>
		public  class DevicedeffieldDAO : GenericNHibernateDAO<Devicedeffield,int?>
			{
			public DevicedeffieldDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region DevicedefgatecommandDAO

		/// <summary>
		/// Devicedefgatecommand object for NHibernate mapped table 'device_def_gate_command'.
		/// </summary>
		public  class DevicedefgatecommandDAO : GenericNHibernateDAO<Devicedefgatecommand,int?>
			{
			public DevicedefgatecommandDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region DevicedefmsgfielddictionaryDAO

		/// <summary>
		/// Devicedefmsgfielddictionary object for NHibernate mapped table 'device_def_msg_field_dictionary'.
		/// </summary>
		public  class DevicedefmsgfielddictionaryDAO : GenericNHibernateDAO<Devicedefmsgfielddictionary,int?>
			{
			public DevicedefmsgfielddictionaryDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region DevicemsgfielddictionaryDAO

		/// <summary>
		/// Devicemsgfielddictionary object for NHibernate mapped table 'device_msg_field_dictionary'.
		/// </summary>
		public  class DevicemsgfielddictionaryDAO : GenericNHibernateDAO<Devicemsgfielddictionary,int?>
			{
			public DevicemsgfielddictionaryDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region EmailnotifierDAO

		/// <summary>
		/// Emailnotifier object for NHibernate mapped table 'email_notifier'.
		/// </summary>
		public  class EmailnotifierDAO : GenericNHibernateDAO<Emailnotifier,int?>
			{
			public EmailnotifierDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region EvaluatorconditionDAO

		/// <summary>
		/// Evaluatorcondition object for NHibernate mapped table 'evaluator_condition'.
		/// </summary>
		public  class EvaluatorconditionDAO : GenericNHibernateDAO<Evaluatorcondition,int?>
			{
			public EvaluatorconditionDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region EvaluatorconditiondayofweekperiodDAO

		/// <summary>
		/// Evaluatorconditiondayofweekperiod object for NHibernate mapped table 'evaluator_condition_dayofweek_period'.
		/// </summary>
		public  class EvaluatorconditiondayofweekperiodDAO : GenericNHibernateDAO<Evaluatorconditiondayofweekperiod,int?>
			{
			public EvaluatorconditiondayofweekperiodDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region EvaluatorconditioneventdurationDAO

		/// <summary>
		/// Evaluatorconditioneventduration object for NHibernate mapped table 'evaluator_condition_event_duration'.
		/// </summary>
		public  class EvaluatorconditioneventdurationDAO : GenericNHibernateDAO<Evaluatorconditioneventduration,int?>
			{
			public EvaluatorconditioneventdurationDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region FatpointDAO

		/// <summary>
		/// Fatpoint object for NHibernate mapped table 'fat_point'.
		/// </summary>
		public  class FatpointDAO : GenericNHibernateDAO<Fatpoint,long?>
			{
			public FatpointDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region GatecommandDAO

		/// <summary>
		/// Gatecommand object for NHibernate mapped table 'gate_command'.
		/// </summary>
		public  class GatecommandDAO : GenericNHibernateDAO<Gatecommand,int?>
			{
			public GatecommandDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region GateeventDAO

		/// <summary>
		/// Gateevent object for NHibernate mapped table 'gate_event'.
		/// </summary>
		public  class GateeventDAO : GenericNHibernateDAO<Gateevent,long?>
			{
			public GateeventDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region GateeventargumentDAO

		/// <summary>
		/// Gateeventargument object for NHibernate mapped table 'gate_event_argument'.
		/// </summary>
		public  class GateeventargumentDAO : GenericNHibernateDAO<Gateeventargument,int?>
			{
			public GateeventargumentDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region GateeventargumentgenericDAO

		/// <summary>
		/// Gateeventargumentgeneric object for NHibernate mapped table 'gate_event_argument_generic'.
		/// </summary>
		public  class GateeventargumentgenericDAO : GenericNHibernateDAO<Gateeventargumentgeneric,int?>
			{
			public GateeventargumentgenericDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region GateeventchannelDAO

		/// <summary>
		/// Gateeventchannel object for NHibernate mapped table 'gate_event_channel'.
		/// </summary>
		public  class GateeventchannelDAO : GenericNHibernateDAO<Gateeventchannel,int?>
			{
			public GateeventchannelDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region GateeventchanneldictionaryDAO

		/// <summary>
		/// Gateeventchanneldictionary object for NHibernate mapped table 'gate_event_channel_dictionary'.
		/// </summary>
		public  class GateeventchanneldictionaryDAO : GenericNHibernateDAO<Gateeventchanneldictionary,int?>
			{
			public GateeventchanneldictionaryDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region GateeventchanneldictionaryentryDAO

		/// <summary>
		/// Gateeventchanneldictionaryentry object for NHibernate mapped table 'gate_event_channel_dictionary_entry'.
		/// </summary>
		public  class GateeventchanneldictionaryentryDAO : GenericNHibernateDAO<Gateeventchanneldictionaryentry,int?>
			{
			public GateeventchanneldictionaryentryDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region GateeventexpressionDAO

		/// <summary>
		/// Gateeventexpression object for NHibernate mapped table 'gate_event_expression'.
		/// </summary>
		public  class GateeventexpressionDAO : GenericNHibernateDAO<Gateeventexpression,int?>
			{
			public GateeventexpressionDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region GateeventexpressionevaluatorDAO

		/// <summary>
		/// Gateeventexpressionevaluator object for NHibernate mapped table 'gate_event_expression_evaluator'.
		/// </summary>
		public  class GateeventexpressionevaluatorDAO : GenericNHibernateDAO<Gateeventexpressionevaluator,int?>
			{
			public GateeventexpressionevaluatorDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region GateeventexpressionevaluatorchannelDAO

		/// <summary>
		/// Gateeventexpressionevaluatorchannel object for NHibernate mapped table 'gate_event_expression_evaluator_channels'.
		/// </summary>
		public  class GateeventexpressionevaluatorchannelDAO : GenericNHibernateDAO<Gateeventexpressionevaluatorchannel,int?>
			{
			public GateeventexpressionevaluatorchannelDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region GateeventexpressionevaluatorevaluatorconditionDAO

		/// <summary>
		/// Gateeventexpressionevaluatorevaluatorcondition object for NHibernate mapped table 'gate_event_expression_evaluator_evaluator_condition'.
		/// </summary>
		public  class GateeventexpressionevaluatorevaluatorconditionDAO : GenericNHibernateDAO<Gateeventexpressionevaluatorevaluatorcondition,int?>
			{
			public GateeventexpressionevaluatorevaluatorconditionDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region GateeventexpressionevaluatornotifierDAO

		/// <summary>
		/// Gateeventexpressionevaluatornotifier object for NHibernate mapped table 'gate_event_expression_evaluator_notifier'.
		/// </summary>
		public  class GateeventexpressionevaluatornotifierDAO : GenericNHibernateDAO<Gateeventexpressionevaluatornotifier,int?>
			{
			public GateeventexpressionevaluatornotifierDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region GateeventexpressionevaluatortagDAO

		/// <summary>
		/// Gateeventexpressionevaluatortag object for NHibernate mapped table 'gate_event_expression_evaluator_tags'.
		/// </summary>
		public  class GateeventexpressionevaluatortagDAO : GenericNHibernateDAO<Gateeventexpressionevaluatortag,int?>
			{
			public GateeventexpressionevaluatortagDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region GateeventexpressionevaluatoruserDAO

		/// <summary>
		/// Gateeventexpressionevaluatoruser object for NHibernate mapped table 'gate_event_expression_evaluator_users'.
		/// </summary>
		public  class GateeventexpressionevaluatoruserDAO : GenericNHibernateDAO<Gateeventexpressionevaluatoruser,int?>
			{
			public GateeventexpressionevaluatoruserDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region GateeventexpressionstateDAO

		/// <summary>
		/// Gateeventexpressionstate object for NHibernate mapped table 'gate_event_expression_state'.
		/// </summary>
		public  class GateeventexpressionstateDAO : GenericNHibernateDAO<Gateeventexpressionstate,int?>
			{
			public GateeventexpressionstateDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region GateeventlogDAO

		/// <summary>
		/// Gateeventlog object for NHibernate mapped table 'gate_event_log'.
		/// </summary>
		public  class GateeventlogDAO : GenericNHibernateDAO<Gateeventlog,long?>
			{
			public GateeventlogDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region GateeventmessagespanDAO

		/// <summary>
		/// Gateeventmessagespan object for NHibernate mapped table 'gate_event_message_span'.
		/// </summary>
		public  class GateeventmessagespanDAO : GenericNHibernateDAO<Gateeventmessagespan,long?>
			{
			public GateeventmessagespanDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region GateeventstateDAO

		/// <summary>
		/// Gateeventstate object for NHibernate mapped table 'gate_event_state'.
		/// </summary>
		public  class GateeventstateDAO : GenericNHibernateDAO<Gateeventstate,int?>
			{
			public GateeventstateDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region GatemessageDAO

		/// <summary>
		/// Gatemessage object for NHibernate mapped table 'gate_message'.
		/// </summary>
		public  class GatemessageDAO : GenericNHibernateDAO<Gatemessage,long?>
			{
			public GatemessageDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region GatemessagerecordDAO

		/// <summary>
		/// Gatemessagerecord object for NHibernate mapped table 'gate_message_record'.
		/// </summary>
		public  class GatemessagerecordDAO : GenericNHibernateDAO<Gatemessagerecord,long?>
			{
			public GatemessagerecordDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region GaterecordlatestDAO

		/// <summary>
		/// Gaterecordlatest object for NHibernate mapped table 'gate_record_latest'.
		/// </summary>
		public  class GaterecordlatestDAO : GenericNHibernateDAO<Gaterecordlatest,long?>
			{
			public GaterecordlatestDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region GateuserDAO

		/// <summary>
		/// Gateuser object for NHibernate mapped table 'gate_user'.
		/// </summary>
		public  class GateuserDAO : GenericNHibernateDAO<Gateuser,int?>
			{
			public GateuserDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region GateviewDAO

		/// <summary>
		/// Gateview object for NHibernate mapped table 'gate_view'.
		/// </summary>
		public  class GateviewDAO : GenericNHibernateDAO<Gateview,int?>
			{
			public GateviewDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region GateviewtagDAO

		/// <summary>
		/// Gateviewtag object for NHibernate mapped table 'gate_view_tags'.
		/// </summary>
		public  class GateviewtagDAO : GenericNHibernateDAO<Gateviewtag,int?>
			{
			public GateviewtagDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region GeocoderDAO

		/// <summary>
		/// Geocoder object for NHibernate mapped table 'geocoder'.
		/// </summary>
		public  class GeocoderDAO : GenericNHibernateDAO<Geocoder,int?>
			{
			public GeocoderDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region GeocoderapplicationDAO

		/// <summary>
		/// Geocoderapplication object for NHibernate mapped table 'geocoder_application'.
		/// </summary>
		public  class GeocoderapplicationDAO : GenericNHibernateDAO<Geocoderapplication,int?>
			{
			public GeocoderapplicationDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region GeocoderproviderDAO

		/// <summary>
		/// Geocoderprovider object for NHibernate mapped table 'geocoder_provider'.
		/// </summary>
		public  class GeocoderproviderDAO : GenericNHibernateDAO<Geocoderprovider,int?>
			{
			public GeocoderproviderDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region GeofenceDAO

		/// <summary>
		/// Geofence object for NHibernate mapped table 'geofence'.
		/// </summary>
		public  class GeofenceDAO : GenericNHibernateDAO<Geofence,int?>
			{
			public GeofenceDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region GeofencecircularDAO

		/// <summary>
		/// Geofencecircular object for NHibernate mapped table 'geofence_circular'.
		/// </summary>
		public  class GeofencecircularDAO : GenericNHibernateDAO<Geofencecircular,int?>
			{
			public GeofencecircularDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region GeofenceeventargumentDAO

		/// <summary>
		/// Geofenceeventargument object for NHibernate mapped table 'geofence_event_argument'.
		/// </summary>
		public  class GeofenceeventargumentDAO : GenericNHibernateDAO<Geofenceeventargument,int?>
			{
			public GeofenceeventargumentDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region GeofenceeventexpressionDAO

		/// <summary>
		/// Geofenceeventexpression object for NHibernate mapped table 'geofence_event_expression'.
		/// </summary>
		public  class GeofenceeventexpressionDAO : GenericNHibernateDAO<Geofenceeventexpression,int?>
			{
			public GeofenceeventexpressionDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region GeofencepolygonDAO

		/// <summary>
		/// Geofencepolygon object for NHibernate mapped table 'geofence_polygon'.
		/// </summary>
		public  class GeofencepolygonDAO : GenericNHibernateDAO<Geofencepolygon,int?>
			{
			public GeofencepolygonDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region GridDAO

		/// <summary>
		/// Grid object for NHibernate mapped table 'grid'.
		/// </summary>
		public  class GridDAO : GenericNHibernateDAO<Grid,int?>
			{
			public GridDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region GroupreferrerlogDAO

		/// <summary>
		/// Groupreferrerlog object for NHibernate mapped table 'group_referrer_log'.
		/// </summary>
		public  class GroupreferrerlogDAO : GenericNHibernateDAO<Groupreferrerlog,int?>
			{
			public GroupreferrerlogDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region GrouprightDAO

		/// <summary>
		/// Groupright object for NHibernate mapped table 'group_rights'.
		/// </summary>
		public  class GrouprightDAO : GenericNHibernateDAO<Groupright,int?>
			{
			public GrouprightDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region GrouprouteruleDAO

		/// <summary>
		/// Grouprouterule object for NHibernate mapped table 'group_route_rule'.
		/// </summary>
		public  class GrouprouteruleDAO : GenericNHibernateDAO<Grouprouterule,int?>
			{
			public GrouprouteruleDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region GroupsettingDAO

		/// <summary>
		/// Groupsetting object for NHibernate mapped table 'group_settings'.
		/// </summary>
		public  class GroupsettingDAO : GenericNHibernateDAO<Groupsetting,int?>
			{
			public GroupsettingDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region GroupDAO

		/// <summary>
		/// Group object for NHibernate mapped table 'groups'.
		/// </summary>
		public  class GroupDAO : GenericNHibernateDAO<Group,int?>
			{
			public GroupDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region LicenseDAO

		/// <summary>
		/// License object for NHibernate mapped table 'license'.
		/// </summary>
		public  class LicenseDAO : GenericNHibernateDAO<License,int?>
			{
			public LicenseDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region ListenerDAO

		/// <summary>
		/// Listener object for NHibernate mapped table 'listener'.
		/// </summary>
		public  class ListenerDAO : GenericNHibernateDAO<Listener,int?>
			{
			public ListenerDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region LoadabletypeDAO

		/// <summary>
		/// Loadabletype object for NHibernate mapped table 'loadable_type'.
		/// </summary>
		public  class LoadabletypeDAO : GenericNHibernateDAO<Loadabletype,int?>
			{
			public LoadabletypeDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region MapdataDAO

		/// <summary>
		/// Mapdata object for NHibernate mapped table 'map_data'.
		/// </summary>
		public  class MapdataDAO : GenericNHibernateDAO<Mapdata,int?>
			{
			public MapdataDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region MaplibDAO

		/// <summary>
		/// Maplib object for NHibernate mapped table 'map_lib'.
		/// </summary>
		public  class MaplibDAO : GenericNHibernateDAO<Maplib,int?>
			{
			public MaplibDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region MapmetadataDAO

		/// <summary>
		/// Mapmetadata object for NHibernate mapped table 'map_meta_data'.
		/// </summary>
		public  class MapmetadataDAO : GenericNHibernateDAO<Mapmetadata,int?>
			{
			public MapmetadataDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region MapsgroupDAO

		/// <summary>
		/// Mapsgroup object for NHibernate mapped table 'maps_groups'.
		/// </summary>
		public  class MapsgroupDAO : GenericNHibernateDAO<Mapsgroup,int?>
			{
			public MapsgroupDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region MessageproviderDAO

		/// <summary>
		/// Messageprovider object for NHibernate mapped table 'message_provider'.
		/// </summary>
		public  class MessageproviderDAO : GenericNHibernateDAO<Messageprovider,int?>
			{
			public MessageproviderDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region MessagetemplateDAO

		/// <summary>
		/// Messagetemplate object for NHibernate mapped table 'message_template'.
		/// </summary>
		public  class MessagetemplateDAO : GenericNHibernateDAO<Messagetemplate,int?>
			{
			public MessagetemplateDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region MobilenetworkDAO

		/// <summary>
		/// Mobilenetwork object for NHibernate mapped table 'mobile_network'.
		/// </summary>
		public  class MobilenetworkDAO : GenericNHibernateDAO<Mobilenetwork,int?>
			{
			public MobilenetworkDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region MsgfieldDAO

		/// <summary>
		/// Msgfield object for NHibernate mapped table 'msg_field'.
		/// </summary>
		public  class MsgfieldDAO : GenericNHibernateDAO<Msgfield,int?>
			{
			public MsgfieldDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region MsgfielddictionaryDAO

		/// <summary>
		/// Msgfielddictionary object for NHibernate mapped table 'msg_field_dictionary'.
		/// </summary>
		public  class MsgfielddictionaryDAO : GenericNHibernateDAO<Msgfielddictionary,int?>
			{
			public MsgfielddictionaryDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region MsgfielddictionaryentryDAO

		/// <summary>
		/// Msgfielddictionaryentry object for NHibernate mapped table 'msg_field_dictionary_entry'.
		/// </summary>
		public  class MsgfielddictionaryentryDAO : GenericNHibernateDAO<Msgfielddictionaryentry,int?>
			{
			public MsgfielddictionaryentryDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region MsgnamespaceDAO

		/// <summary>
		/// Msgnamespace object for NHibernate mapped table 'msg_namespace'.
		/// </summary>
		public  class MsgnamespaceDAO : GenericNHibernateDAO<Msgnamespace,int?>
			{
			public MsgnamespaceDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region MsgprovsettingDAO

		/// <summary>
		/// Msgprovsetting object for NHibernate mapped table 'msg_prov_settings'.
		/// </summary>
		public  class MsgprovsettingDAO : GenericNHibernateDAO<Msgprovsetting,int?>
			{
			public MsgprovsettingDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region NotifierDAO

		/// <summary>
		/// Notifier object for NHibernate mapped table 'notifier'.
		/// </summary>
		public  class NotifierDAO : GenericNHibernateDAO<Notifier,int?>
			{
			public NotifierDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region NotifiertagDAO

		/// <summary>
		/// Notifiertag object for NHibernate mapped table 'notifier_tags'.
		/// </summary>
		public  class NotifiertagDAO : GenericNHibernateDAO<Notifiertag,int?>
			{
			public NotifiertagDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region OutgoingwebserviceDAO

		/// <summary>
		/// Outgoingwebservice object for NHibernate mapped table 'outgoing_web_service'.
		/// </summary>
		public  class OutgoingwebserviceDAO : GenericNHibernateDAO<Outgoingwebservice,int?>
			{
			public OutgoingwebserviceDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region PostprocessorDAO

		/// <summary>
		/// Postprocessor object for NHibernate mapped table 'post_processor'.
		/// </summary>
		public  class PostprocessorDAO : GenericNHibernateDAO<Postprocessor,int?>
			{
			public PostprocessorDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region ProtocolDAO

		/// <summary>
		/// Protocol object for NHibernate mapped table 'protocol'.
		/// </summary>
		public  class ProtocolDAO : GenericNHibernateDAO<Protocol,string>
			{
			public ProtocolDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region ProtocolversionDAO

		/// <summary>
		/// Protocolversion object for NHibernate mapped table 'protocol_version'.
		/// </summary>
		public  class ProtocolversionDAO : GenericNHibernateDAO<Protocolversion,int?>
			{
			public ProtocolversionDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region ProvidermessageDAO

		/// <summary>
		/// Providermessage object for NHibernate mapped table 'provider_message'.
		/// </summary>
		public  class ProvidermessageDAO : GenericNHibernateDAO<Providermessage,long?>
			{
			public ProvidermessageDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region ProvidermessagequeueDAO

		/// <summary>
		/// Providermessagequeue object for NHibernate mapped table 'provider_message_queue'.
		/// </summary>
		public  class ProvidermessagequeueDAO : GenericNHibernateDAO<Providermessagequeue,int?>
			{
			public ProvidermessagequeueDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region RecorderruleDAO

		/// <summary>
		/// Recorderrule object for NHibernate mapped table 'recorder_rule'.
		/// </summary>
		public  class RecorderruleDAO : GenericNHibernateDAO<Recorderrule,int?>
			{
			public RecorderruleDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region ReferrerDAO

		/// <summary>
		/// Referrer object for NHibernate mapped table 'referrer'.
		/// </summary>
		public  class ReferrerDAO : GenericNHibernateDAO<Referrer,int?>
			{
			public ReferrerDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region ReportDAO

		/// <summary>
		/// Report object for NHibernate mapped table 'report'.
		/// </summary>
		public  class ReportDAO : GenericNHibernateDAO<Report,int?>
			{
			public ReportDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region ReportparameterDAO

		/// <summary>
		/// Reportparameter object for NHibernate mapped table 'report_parameter'.
		/// </summary>
		public  class ReportparameterDAO : GenericNHibernateDAO<Reportparameter,int?>
			{
			public ReportparameterDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region ReportviewerDAO

		/// <summary>
		/// Reportviewer object for NHibernate mapped table 'report_viewer'.
		/// </summary>
		public  class ReportviewerDAO : GenericNHibernateDAO<Reportviewer,int?>
			{
			public ReportviewerDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region RoleDAO

		/// <summary>
		/// Role object for NHibernate mapped table 'roles'.
		/// </summary>
		public  class RoleDAO : GenericNHibernateDAO<Role,int?>
			{
			public RoleDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region RulechainDAO

		/// <summary>
		/// Rulechain object for NHibernate mapped table 'rule_chain'.
		/// </summary>
		public  class RulechainDAO : GenericNHibernateDAO<Rulechain,int?>
			{
			public RulechainDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region SchedulerDAO

		/// <summary>
		/// Scheduler object for NHibernate mapped table 'scheduler'.
		/// </summary>
		public  class SchedulerDAO : GenericNHibernateDAO<Scheduler,int?>
			{
			public SchedulerDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region SchedulertaskDAO

		/// <summary>
		/// Schedulertask object for NHibernate mapped table 'scheduler_task'.
		/// </summary>
		public  class SchedulertaskDAO : GenericNHibernateDAO<Schedulertask,int?>
			{
			public SchedulertaskDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region SchedulertaskparameterDAO

		/// <summary>
		/// Schedulertaskparameter object for NHibernate mapped table 'scheduler_task_parameter'.
		/// </summary>
		public  class SchedulertaskparameterDAO : GenericNHibernateDAO<Schedulertaskparameter,int?>
			{
			public SchedulertaskparameterDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region SchedulertasktriggerDAO

		/// <summary>
		/// Schedulertasktrigger object for NHibernate mapped table 'scheduler_task_trigger'.
		/// </summary>
		public  class SchedulertasktriggerDAO : GenericNHibernateDAO<Schedulertasktrigger,int?>
			{
			public SchedulertasktriggerDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region ScriptfileDAO

		/// <summary>
		/// Scriptfile object for NHibernate mapped table 'script_file'.
		/// </summary>
		public  class ScriptfileDAO : GenericNHibernateDAO<Scriptfile,int?>
			{
			public ScriptfileDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region ScriptpageDAO

		/// <summary>
		/// Scriptpage object for NHibernate mapped table 'script_page'.
		/// </summary>
		public  class ScriptpageDAO : GenericNHibernateDAO<Scriptpage,int?>
			{
			public ScriptpageDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region ScriptpluginDAO

		/// <summary>
		/// Scriptplugin object for NHibernate mapped table 'script_plugin'.
		/// </summary>
		public  class ScriptpluginDAO : GenericNHibernateDAO<Scriptplugin,int?>
			{
			public ScriptpluginDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region ScriptpluginapplicationDAO

		/// <summary>
		/// Scriptpluginapplication object for NHibernate mapped table 'script_plugin_application'.
		/// </summary>
		public  class ScriptpluginapplicationDAO : GenericNHibernateDAO<Scriptpluginapplication,int?>
			{
			public ScriptpluginapplicationDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region ScriptserviceDAO

		/// <summary>
		/// Scriptservice object for NHibernate mapped table 'script_service'.
		/// </summary>
		public  class ScriptserviceDAO : GenericNHibernateDAO<Scriptservice,int?>
			{
			public ScriptserviceDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region ServerversionDAO

		/// <summary>
		/// Serverversion object for NHibernate mapped table 'server_version'.
		/// </summary>
		public  class ServerversionDAO : GenericNHibernateDAO<Serverversion,string>
			{
			public ServerversionDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region ServicepluginDAO

		/// <summary>
		/// Serviceplugin object for NHibernate mapped table 'service_plugin'.
		/// </summary>
		public  class ServicepluginDAO : GenericNHibernateDAO<Serviceplugin,int?>
			{
			public ServicepluginDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region SessionDAO

		/// <summary>
		/// Session object for NHibernate mapped table 'sessions'.
		/// </summary>
		public  class SessionDAO : GenericNHibernateDAO<Session,int?>
			{
			public SessionDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region SettingDAO

		/// <summary>
		/// Setting object for NHibernate mapped table 'settings'.
		/// </summary>
		public  class SettingDAO : GenericNHibernateDAO<Setting,int?>
			{
			public SettingDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region SitesettingDAO

		/// <summary>
		/// Sitesetting object for NHibernate mapped table 'site_settings'.
		/// </summary>
		public  class SitesettingDAO : GenericNHibernateDAO<Sitesetting,int?>
			{
			public SitesettingDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region SmsmessageDAO

		/// <summary>
		/// Smsmessage object for NHibernate mapped table 'sms_message'.
		/// </summary>
		public  class SmsmessageDAO : GenericNHibernateDAO<Smsmessage,long?>
			{
			public SmsmessageDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region SmsproxyDAO

		/// <summary>
		/// Smsproxy object for NHibernate mapped table 'sms_proxy'.
		/// </summary>
		public  class SmsproxyDAO : GenericNHibernateDAO<Smsproxy,int?>
			{
			public SmsproxyDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region SmsproxyqueueDAO

		/// <summary>
		/// Smsproxyqueue object for NHibernate mapped table 'sms_proxy_queue'.
		/// </summary>
		public  class SmsproxyqueueDAO : GenericNHibernateDAO<Smsproxyqueue,int?>
			{
			public SmsproxyqueueDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region StatisticdataDAO

		/// <summary>
		/// Statisticdata object for NHibernate mapped table 'statistic_data'.
		/// </summary>
		public  class StatisticdataDAO : GenericNHibernateDAO<Statisticdata,int?>
			{
			public StatisticdataDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region StatistickeyDAO

		/// <summary>
		/// Statistickey object for NHibernate mapped table 'statistic_key'.
		/// </summary>
		public  class StatistickeyDAO : GenericNHibernateDAO<Statistickey,int?>
			{
			public StatistickeyDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region TagDAO

		/// <summary>
		/// Tag object for NHibernate mapped table 'tag'.
		/// </summary>
		public  class TagDAO : GenericNHibernateDAO<Tag,int?>
			{
			public TagDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region TaggeofenceDAO

		/// <summary>
		/// Taggeofence object for NHibernate mapped table 'tag_geofences'.
		/// </summary>
		public  class TaggeofenceDAO : GenericNHibernateDAO<Taggeofence,int?>
			{
			public TaggeofenceDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region TaguserDAO

		/// <summary>
		/// Taguser object for NHibernate mapped table 'tag_users'.
		/// </summary>
		public  class TaguserDAO : GenericNHibernateDAO<Taguser,int?>
			{
			public TaguserDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region TemplatecmdstepDAO

		/// <summary>
		/// Templatecmdstep object for NHibernate mapped table 'template_cmd_step'.
		/// </summary>
		public  class TemplatecmdstepDAO : GenericNHibernateDAO<Templatecmdstep,int?>
			{
			public TemplatecmdstepDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region TemporarycredentialDAO

		/// <summary>
		/// Temporarycredential object for NHibernate mapped table 'temporary_credential'.
		/// </summary>
		public  class TemporarycredentialDAO : GenericNHibernateDAO<Temporarycredential,int?>
			{
			public TemporarycredentialDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region TrackcategoryDAO

		/// <summary>
		/// Trackcategory object for NHibernate mapped table 'track_category'.
		/// </summary>
		public  class TrackcategoryDAO : GenericNHibernateDAO<Trackcategory,string>
			{
			public TrackcategoryDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region TrackdataDAO

		/// <summary>
		/// Trackdata object for NHibernate mapped table 'track_data'.
		/// </summary>
		public  class TrackdataDAO : GenericNHibernateDAO<Trackdata,long?>
			{
			public TrackdataDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region TrackdatamodDAO

		/// <summary>
		/// Trackdatamod object for NHibernate mapped table 'track_data_mod'.
		/// </summary>
		public  class TrackdatamodDAO : GenericNHibernateDAO<Trackdatamod,long?>
			{
			public TrackdatamodDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region TrackinfoDAO

		/// <summary>
		/// Trackinfo object for NHibernate mapped table 'track_info'.
		/// </summary>
		public  class TrackinfoDAO : GenericNHibernateDAO<Trackinfo,int?>
			{
			public TrackinfoDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region TrackinfomodDAO

		/// <summary>
		/// Trackinfomod object for NHibernate mapped table 'track_info_mod'.
		/// </summary>
		public  class TrackinfomodDAO : GenericNHibernateDAO<Trackinfomod,int?>
			{
			public TrackinfomodDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region TrackpostprocessorlogDAO

		/// <summary>
		/// Trackpostprocessorlog object for NHibernate mapped table 'track_post_processor_log'.
		/// </summary>
		public  class TrackpostprocessorlogDAO : GenericNHibernateDAO<Trackpostprocessorlog,long?>
			{
			public TrackpostprocessorlogDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region TrackrecorderDAO

		/// <summary>
		/// Trackrecorder object for NHibernate mapped table 'track_recorder'.
		/// </summary>
		public  class TrackrecorderDAO : GenericNHibernateDAO<Trackrecorder,int?>
			{
			public TrackrecorderDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region UicontrolstateDAO

		/// <summary>
		/// Uicontrolstate object for NHibernate mapped table 'ui_control_state'.
		/// </summary>
		public  class UicontrolstateDAO : GenericNHibernateDAO<Uicontrolstate,string>
			{
			public UicontrolstateDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region UnitDAO

		/// <summary>
		/// Unit object for NHibernate mapped table 'unit'.
		/// </summary>
		public  class UnitDAO : GenericNHibernateDAO<Unit,string>
			{
			public UnitDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region UserattributeDAO

		/// <summary>
		/// Userattribute object for NHibernate mapped table 'user_attribute'.
		/// </summary>
		public  class UserattributeDAO : GenericNHibernateDAO<Userattribute,int?>
			{
			public UserattributeDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region UserattributenotifierDAO

		/// <summary>
		/// Userattributenotifier object for NHibernate mapped table 'user_attribute_notifier'.
		/// </summary>
		public  class UserattributenotifierDAO : GenericNHibernateDAO<Userattributenotifier,int?>
			{
			public UserattributenotifierDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region UsergroupDAO

		/// <summary>
		/// Usergroup object for NHibernate mapped table 'user_groups'.
		/// </summary>
		public  class UsergroupDAO : GenericNHibernateDAO<Usergroup,int?>
			{
			public UsergroupDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region UserroleDAO

		/// <summary>
		/// Userrole object for NHibernate mapped table 'user_roles'.
		/// </summary>
		public  class UserroleDAO : GenericNHibernateDAO<Userrole,int?>
			{
			public UserroleDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region UsersettingDAO

		/// <summary>
		/// Usersetting object for NHibernate mapped table 'user_settings'.
		/// </summary>
		public  class UsersettingDAO : GenericNHibernateDAO<Usersetting,int?>
			{
			public UsersettingDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region UserDAO

		/// <summary>
		/// User object for NHibernate mapped table 'users'.
		/// </summary>
		public  class UserDAO : GenericNHibernateDAO<User,int?>
			{
			public UserDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region VersionDAO

		/// <summary>
		/// Version object for NHibernate mapped table 'version'.
		/// </summary>
		public  class VersionDAO : GenericNHibernateDAO<Version,string>
			{
			public VersionDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion
		#region WorkspaceDAO

		/// <summary>
		/// Workspace object for NHibernate mapped table 'workspace'.
		/// </summary>
		public  class WorkspaceDAO : GenericNHibernateDAO<Workspace,int?>
			{
			public WorkspaceDAO(String sessionFactoryConfigPath,INHibernateSessionManager _manager) : base(sessionFactoryConfigPath,_manager)
			{
			}
		}
	
		#endregion

}





		

