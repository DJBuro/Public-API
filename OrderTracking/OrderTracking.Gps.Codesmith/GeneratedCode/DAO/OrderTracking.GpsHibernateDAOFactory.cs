
using Cacd.DAO.NHibernate.OrderTracking.Gps;

namespace Cacd.DAO.Factory
{
	public class OrderTracking.GpsHibernateDAOFactory : AbstractHibernateDAOFactory{ 

			private AdminrightDAO _AdminrightDAO;
			private AppsettingDAO _AppsettingDAO;
			private ApptemplateDAO _ApptemplateDAO;
			private ApplicationdefDAO _ApplicationdefDAO;
			private ApplicationdefgateeventchannelDAO _ApplicationdefgateeventchannelDAO;
			private ApplicationdefreportviewerDAO _ApplicationdefreportviewerDAO;
			private ApplicationruleDAO _ApplicationruleDAO;
			private ApplicationDAO _ApplicationDAO;
			private AttributeDAO _AttributeDAO;
			private ChannelnotifierDAO _ChannelnotifierDAO;
			private CmdargDAO _CmdargDAO;
			private CmdqueueDAO _CmdqueueDAO;
			private CmdqueueitemDAO _CmdqueueitemDAO;
			private CountryDAO _CountryDAO;
			private DatumDAO _DatumDAO;
			private DeviceDAO _DeviceDAO;
			private DevicedefDAO _DevicedefDAO;
			private DevicedeffieldDAO _DevicedeffieldDAO;
			private DevicedefgatecommandDAO _DevicedefgatecommandDAO;
			private DevicedefmsgfielddictionaryDAO _DevicedefmsgfielddictionaryDAO;
			private DevicemsgfielddictionaryDAO _DevicemsgfielddictionaryDAO;
			private EmailnotifierDAO _EmailnotifierDAO;
			private EvaluatorconditionDAO _EvaluatorconditionDAO;
			private EvaluatorconditiondayofweekperiodDAO _EvaluatorconditiondayofweekperiodDAO;
			private EvaluatorconditioneventdurationDAO _EvaluatorconditioneventdurationDAO;
			private FatpointDAO _FatpointDAO;
			private GatecommandDAO _GatecommandDAO;
			private GateeventDAO _GateeventDAO;
			private GateeventargumentDAO _GateeventargumentDAO;
			private GateeventargumentgenericDAO _GateeventargumentgenericDAO;
			private GateeventchannelDAO _GateeventchannelDAO;
			private GateeventchanneldictionaryDAO _GateeventchanneldictionaryDAO;
			private GateeventchanneldictionaryentryDAO _GateeventchanneldictionaryentryDAO;
			private GateeventexpressionDAO _GateeventexpressionDAO;
			private GateeventexpressionevaluatorDAO _GateeventexpressionevaluatorDAO;
			private GateeventexpressionevaluatorchannelDAO _GateeventexpressionevaluatorchannelDAO;
			private GateeventexpressionevaluatorevaluatorconditionDAO _GateeventexpressionevaluatorevaluatorconditionDAO;
			private GateeventexpressionevaluatornotifierDAO _GateeventexpressionevaluatornotifierDAO;
			private GateeventexpressionevaluatortagDAO _GateeventexpressionevaluatortagDAO;
			private GateeventexpressionevaluatoruserDAO _GateeventexpressionevaluatoruserDAO;
			private GateeventexpressionstateDAO _GateeventexpressionstateDAO;
			private GateeventlogDAO _GateeventlogDAO;
			private GateeventmessagespanDAO _GateeventmessagespanDAO;
			private GateeventstateDAO _GateeventstateDAO;
			private GatemessageDAO _GatemessageDAO;
			private GatemessagerecordDAO _GatemessagerecordDAO;
			private GaterecordlatestDAO _GaterecordlatestDAO;
			private GateuserDAO _GateuserDAO;
			private GateviewDAO _GateviewDAO;
			private GateviewtagDAO _GateviewtagDAO;
			private GeocoderDAO _GeocoderDAO;
			private GeocoderapplicationDAO _GeocoderapplicationDAO;
			private GeocoderproviderDAO _GeocoderproviderDAO;
			private GeofenceDAO _GeofenceDAO;
			private GeofencecircularDAO _GeofencecircularDAO;
			private GeofenceeventargumentDAO _GeofenceeventargumentDAO;
			private GeofenceeventexpressionDAO _GeofenceeventexpressionDAO;
			private GeofencepolygonDAO _GeofencepolygonDAO;
			private GridDAO _GridDAO;
			private GroupreferrerlogDAO _GroupreferrerlogDAO;
			private GrouprightDAO _GrouprightDAO;
			private GrouprouteruleDAO _GrouprouteruleDAO;
			private GroupsettingDAO _GroupsettingDAO;
			private GroupDAO _GroupDAO;
			private LicenseDAO _LicenseDAO;
			private ListenerDAO _ListenerDAO;
			private LoadabletypeDAO _LoadabletypeDAO;
			private MapdataDAO _MapdataDAO;
			private MaplibDAO _MaplibDAO;
			private MapmetadataDAO _MapmetadataDAO;
			private MapsgroupDAO _MapsgroupDAO;
			private MessageproviderDAO _MessageproviderDAO;
			private MessagetemplateDAO _MessagetemplateDAO;
			private MobilenetworkDAO _MobilenetworkDAO;
			private MsgfieldDAO _MsgfieldDAO;
			private MsgfielddictionaryDAO _MsgfielddictionaryDAO;
			private MsgfielddictionaryentryDAO _MsgfielddictionaryentryDAO;
			private MsgnamespaceDAO _MsgnamespaceDAO;
			private MsgprovsettingDAO _MsgprovsettingDAO;
			private NotifierDAO _NotifierDAO;
			private NotifiertagDAO _NotifiertagDAO;
			private OutgoingwebserviceDAO _OutgoingwebserviceDAO;
			private PostprocessorDAO _PostprocessorDAO;
			private ProtocolDAO _ProtocolDAO;
			private ProtocolversionDAO _ProtocolversionDAO;
			private ProvidermessageDAO _ProvidermessageDAO;
			private ProvidermessagequeueDAO _ProvidermessagequeueDAO;
			private RecorderruleDAO _RecorderruleDAO;
			private ReferrerDAO _ReferrerDAO;
			private ReportDAO _ReportDAO;
			private ReportparameterDAO _ReportparameterDAO;
			private ReportviewerDAO _ReportviewerDAO;
			private RoleDAO _RoleDAO;
			private RulechainDAO _RulechainDAO;
			private SchedulerDAO _SchedulerDAO;
			private SchedulertaskDAO _SchedulertaskDAO;
			private SchedulertaskparameterDAO _SchedulertaskparameterDAO;
			private SchedulertasktriggerDAO _SchedulertasktriggerDAO;
			private ScriptfileDAO _ScriptfileDAO;
			private ScriptpageDAO _ScriptpageDAO;
			private ScriptpluginDAO _ScriptpluginDAO;
			private ScriptpluginapplicationDAO _ScriptpluginapplicationDAO;
			private ScriptserviceDAO _ScriptserviceDAO;
			private ServerversionDAO _ServerversionDAO;
			private ServicepluginDAO _ServicepluginDAO;
			private SessionDAO _SessionDAO;
			private SettingDAO _SettingDAO;
			private SitesettingDAO _SitesettingDAO;
			private SmsmessageDAO _SmsmessageDAO;
			private SmsproxyDAO _SmsproxyDAO;
			private SmsproxyqueueDAO _SmsproxyqueueDAO;
			private StatisticdataDAO _StatisticdataDAO;
			private StatistickeyDAO _StatistickeyDAO;
			private TagDAO _TagDAO;
			private TaggeofenceDAO _TaggeofenceDAO;
			private TaguserDAO _TaguserDAO;
			private TemplatecmdstepDAO _TemplatecmdstepDAO;
			private TemporarycredentialDAO _TemporarycredentialDAO;
			private TrackcategoryDAO _TrackcategoryDAO;
			private TrackdataDAO _TrackdataDAO;
			private TrackdatamodDAO _TrackdatamodDAO;
			private TrackinfoDAO _TrackinfoDAO;
			private TrackinfomodDAO _TrackinfomodDAO;
			private TrackpostprocessorlogDAO _TrackpostprocessorlogDAO;
			private TrackrecorderDAO _TrackrecorderDAO;
			private UicontrolstateDAO _UicontrolstateDAO;
			private UnitDAO _UnitDAO;
			private UserattributeDAO _UserattributeDAO;
			private UserattributenotifierDAO _UserattributenotifierDAO;
			private UsergroupDAO _UsergroupDAO;
			private UserroleDAO _UserroleDAO;
			private UsersettingDAO _UsersettingDAO;
			private UserDAO _UserDAO;
			private VersionDAO _VersionDAO;
			private WorkspaceDAO _WorkspaceDAO;
		

			public AdminrightDAO AdminrightDAO
			{
				get
				{
					if (_AdminrightDAO == null) _AdminrightDAO = new AdminrightDAO(this.ConnectionDetails,this.SessionManager);
					return _AdminrightDAO;
				}
			}
			public AppsettingDAO AppsettingDAO
			{
				get
				{
					if (_AppsettingDAO == null) _AppsettingDAO = new AppsettingDAO(this.ConnectionDetails,this.SessionManager);
					return _AppsettingDAO;
				}
			}
			public ApptemplateDAO ApptemplateDAO
			{
				get
				{
					if (_ApptemplateDAO == null) _ApptemplateDAO = new ApptemplateDAO(this.ConnectionDetails,this.SessionManager);
					return _ApptemplateDAO;
				}
			}
			public ApplicationdefDAO ApplicationdefDAO
			{
				get
				{
					if (_ApplicationdefDAO == null) _ApplicationdefDAO = new ApplicationdefDAO(this.ConnectionDetails,this.SessionManager);
					return _ApplicationdefDAO;
				}
			}
			public ApplicationdefgateeventchannelDAO ApplicationdefgateeventchannelDAO
			{
				get
				{
					if (_ApplicationdefgateeventchannelDAO == null) _ApplicationdefgateeventchannelDAO = new ApplicationdefgateeventchannelDAO(this.ConnectionDetails,this.SessionManager);
					return _ApplicationdefgateeventchannelDAO;
				}
			}
			public ApplicationdefreportviewerDAO ApplicationdefreportviewerDAO
			{
				get
				{
					if (_ApplicationdefreportviewerDAO == null) _ApplicationdefreportviewerDAO = new ApplicationdefreportviewerDAO(this.ConnectionDetails,this.SessionManager);
					return _ApplicationdefreportviewerDAO;
				}
			}
			public ApplicationruleDAO ApplicationruleDAO
			{
				get
				{
					if (_ApplicationruleDAO == null) _ApplicationruleDAO = new ApplicationruleDAO(this.ConnectionDetails,this.SessionManager);
					return _ApplicationruleDAO;
				}
			}
			public ApplicationDAO ApplicationDAO
			{
				get
				{
					if (_ApplicationDAO == null) _ApplicationDAO = new ApplicationDAO(this.ConnectionDetails,this.SessionManager);
					return _ApplicationDAO;
				}
			}
			public AttributeDAO AttributeDAO
			{
				get
				{
					if (_AttributeDAO == null) _AttributeDAO = new AttributeDAO(this.ConnectionDetails,this.SessionManager);
					return _AttributeDAO;
				}
			}
			public ChannelnotifierDAO ChannelnotifierDAO
			{
				get
				{
					if (_ChannelnotifierDAO == null) _ChannelnotifierDAO = new ChannelnotifierDAO(this.ConnectionDetails,this.SessionManager);
					return _ChannelnotifierDAO;
				}
			}
			public CmdargDAO CmdargDAO
			{
				get
				{
					if (_CmdargDAO == null) _CmdargDAO = new CmdargDAO(this.ConnectionDetails,this.SessionManager);
					return _CmdargDAO;
				}
			}
			public CmdqueueDAO CmdqueueDAO
			{
				get
				{
					if (_CmdqueueDAO == null) _CmdqueueDAO = new CmdqueueDAO(this.ConnectionDetails,this.SessionManager);
					return _CmdqueueDAO;
				}
			}
			public CmdqueueitemDAO CmdqueueitemDAO
			{
				get
				{
					if (_CmdqueueitemDAO == null) _CmdqueueitemDAO = new CmdqueueitemDAO(this.ConnectionDetails,this.SessionManager);
					return _CmdqueueitemDAO;
				}
			}
			public CountryDAO CountryDAO
			{
				get
				{
					if (_CountryDAO == null) _CountryDAO = new CountryDAO(this.ConnectionDetails,this.SessionManager);
					return _CountryDAO;
				}
			}
			public DatumDAO DatumDAO
			{
				get
				{
					if (_DatumDAO == null) _DatumDAO = new DatumDAO(this.ConnectionDetails,this.SessionManager);
					return _DatumDAO;
				}
			}
			public DeviceDAO DeviceDAO
			{
				get
				{
					if (_DeviceDAO == null) _DeviceDAO = new DeviceDAO(this.ConnectionDetails,this.SessionManager);
					return _DeviceDAO;
				}
			}
			public DevicedefDAO DevicedefDAO
			{
				get
				{
					if (_DevicedefDAO == null) _DevicedefDAO = new DevicedefDAO(this.ConnectionDetails,this.SessionManager);
					return _DevicedefDAO;
				}
			}
			public DevicedeffieldDAO DevicedeffieldDAO
			{
				get
				{
					if (_DevicedeffieldDAO == null) _DevicedeffieldDAO = new DevicedeffieldDAO(this.ConnectionDetails,this.SessionManager);
					return _DevicedeffieldDAO;
				}
			}
			public DevicedefgatecommandDAO DevicedefgatecommandDAO
			{
				get
				{
					if (_DevicedefgatecommandDAO == null) _DevicedefgatecommandDAO = new DevicedefgatecommandDAO(this.ConnectionDetails,this.SessionManager);
					return _DevicedefgatecommandDAO;
				}
			}
			public DevicedefmsgfielddictionaryDAO DevicedefmsgfielddictionaryDAO
			{
				get
				{
					if (_DevicedefmsgfielddictionaryDAO == null) _DevicedefmsgfielddictionaryDAO = new DevicedefmsgfielddictionaryDAO(this.ConnectionDetails,this.SessionManager);
					return _DevicedefmsgfielddictionaryDAO;
				}
			}
			public DevicemsgfielddictionaryDAO DevicemsgfielddictionaryDAO
			{
				get
				{
					if (_DevicemsgfielddictionaryDAO == null) _DevicemsgfielddictionaryDAO = new DevicemsgfielddictionaryDAO(this.ConnectionDetails,this.SessionManager);
					return _DevicemsgfielddictionaryDAO;
				}
			}
			public EmailnotifierDAO EmailnotifierDAO
			{
				get
				{
					if (_EmailnotifierDAO == null) _EmailnotifierDAO = new EmailnotifierDAO(this.ConnectionDetails,this.SessionManager);
					return _EmailnotifierDAO;
				}
			}
			public EvaluatorconditionDAO EvaluatorconditionDAO
			{
				get
				{
					if (_EvaluatorconditionDAO == null) _EvaluatorconditionDAO = new EvaluatorconditionDAO(this.ConnectionDetails,this.SessionManager);
					return _EvaluatorconditionDAO;
				}
			}
			public EvaluatorconditiondayofweekperiodDAO EvaluatorconditiondayofweekperiodDAO
			{
				get
				{
					if (_EvaluatorconditiondayofweekperiodDAO == null) _EvaluatorconditiondayofweekperiodDAO = new EvaluatorconditiondayofweekperiodDAO(this.ConnectionDetails,this.SessionManager);
					return _EvaluatorconditiondayofweekperiodDAO;
				}
			}
			public EvaluatorconditioneventdurationDAO EvaluatorconditioneventdurationDAO
			{
				get
				{
					if (_EvaluatorconditioneventdurationDAO == null) _EvaluatorconditioneventdurationDAO = new EvaluatorconditioneventdurationDAO(this.ConnectionDetails,this.SessionManager);
					return _EvaluatorconditioneventdurationDAO;
				}
			}
			public FatpointDAO FatpointDAO
			{
				get
				{
					if (_FatpointDAO == null) _FatpointDAO = new FatpointDAO(this.ConnectionDetails,this.SessionManager);
					return _FatpointDAO;
				}
			}
			public GatecommandDAO GatecommandDAO
			{
				get
				{
					if (_GatecommandDAO == null) _GatecommandDAO = new GatecommandDAO(this.ConnectionDetails,this.SessionManager);
					return _GatecommandDAO;
				}
			}
			public GateeventDAO GateeventDAO
			{
				get
				{
					if (_GateeventDAO == null) _GateeventDAO = new GateeventDAO(this.ConnectionDetails,this.SessionManager);
					return _GateeventDAO;
				}
			}
			public GateeventargumentDAO GateeventargumentDAO
			{
				get
				{
					if (_GateeventargumentDAO == null) _GateeventargumentDAO = new GateeventargumentDAO(this.ConnectionDetails,this.SessionManager);
					return _GateeventargumentDAO;
				}
			}
			public GateeventargumentgenericDAO GateeventargumentgenericDAO
			{
				get
				{
					if (_GateeventargumentgenericDAO == null) _GateeventargumentgenericDAO = new GateeventargumentgenericDAO(this.ConnectionDetails,this.SessionManager);
					return _GateeventargumentgenericDAO;
				}
			}
			public GateeventchannelDAO GateeventchannelDAO
			{
				get
				{
					if (_GateeventchannelDAO == null) _GateeventchannelDAO = new GateeventchannelDAO(this.ConnectionDetails,this.SessionManager);
					return _GateeventchannelDAO;
				}
			}
			public GateeventchanneldictionaryDAO GateeventchanneldictionaryDAO
			{
				get
				{
					if (_GateeventchanneldictionaryDAO == null) _GateeventchanneldictionaryDAO = new GateeventchanneldictionaryDAO(this.ConnectionDetails,this.SessionManager);
					return _GateeventchanneldictionaryDAO;
				}
			}
			public GateeventchanneldictionaryentryDAO GateeventchanneldictionaryentryDAO
			{
				get
				{
					if (_GateeventchanneldictionaryentryDAO == null) _GateeventchanneldictionaryentryDAO = new GateeventchanneldictionaryentryDAO(this.ConnectionDetails,this.SessionManager);
					return _GateeventchanneldictionaryentryDAO;
				}
			}
			public GateeventexpressionDAO GateeventexpressionDAO
			{
				get
				{
					if (_GateeventexpressionDAO == null) _GateeventexpressionDAO = new GateeventexpressionDAO(this.ConnectionDetails,this.SessionManager);
					return _GateeventexpressionDAO;
				}
			}
			public GateeventexpressionevaluatorDAO GateeventexpressionevaluatorDAO
			{
				get
				{
					if (_GateeventexpressionevaluatorDAO == null) _GateeventexpressionevaluatorDAO = new GateeventexpressionevaluatorDAO(this.ConnectionDetails,this.SessionManager);
					return _GateeventexpressionevaluatorDAO;
				}
			}
			public GateeventexpressionevaluatorchannelDAO GateeventexpressionevaluatorchannelDAO
			{
				get
				{
					if (_GateeventexpressionevaluatorchannelDAO == null) _GateeventexpressionevaluatorchannelDAO = new GateeventexpressionevaluatorchannelDAO(this.ConnectionDetails,this.SessionManager);
					return _GateeventexpressionevaluatorchannelDAO;
				}
			}
			public GateeventexpressionevaluatorevaluatorconditionDAO GateeventexpressionevaluatorevaluatorconditionDAO
			{
				get
				{
					if (_GateeventexpressionevaluatorevaluatorconditionDAO == null) _GateeventexpressionevaluatorevaluatorconditionDAO = new GateeventexpressionevaluatorevaluatorconditionDAO(this.ConnectionDetails,this.SessionManager);
					return _GateeventexpressionevaluatorevaluatorconditionDAO;
				}
			}
			public GateeventexpressionevaluatornotifierDAO GateeventexpressionevaluatornotifierDAO
			{
				get
				{
					if (_GateeventexpressionevaluatornotifierDAO == null) _GateeventexpressionevaluatornotifierDAO = new GateeventexpressionevaluatornotifierDAO(this.ConnectionDetails,this.SessionManager);
					return _GateeventexpressionevaluatornotifierDAO;
				}
			}
			public GateeventexpressionevaluatortagDAO GateeventexpressionevaluatortagDAO
			{
				get
				{
					if (_GateeventexpressionevaluatortagDAO == null) _GateeventexpressionevaluatortagDAO = new GateeventexpressionevaluatortagDAO(this.ConnectionDetails,this.SessionManager);
					return _GateeventexpressionevaluatortagDAO;
				}
			}
			public GateeventexpressionevaluatoruserDAO GateeventexpressionevaluatoruserDAO
			{
				get
				{
					if (_GateeventexpressionevaluatoruserDAO == null) _GateeventexpressionevaluatoruserDAO = new GateeventexpressionevaluatoruserDAO(this.ConnectionDetails,this.SessionManager);
					return _GateeventexpressionevaluatoruserDAO;
				}
			}
			public GateeventexpressionstateDAO GateeventexpressionstateDAO
			{
				get
				{
					if (_GateeventexpressionstateDAO == null) _GateeventexpressionstateDAO = new GateeventexpressionstateDAO(this.ConnectionDetails,this.SessionManager);
					return _GateeventexpressionstateDAO;
				}
			}
			public GateeventlogDAO GateeventlogDAO
			{
				get
				{
					if (_GateeventlogDAO == null) _GateeventlogDAO = new GateeventlogDAO(this.ConnectionDetails,this.SessionManager);
					return _GateeventlogDAO;
				}
			}
			public GateeventmessagespanDAO GateeventmessagespanDAO
			{
				get
				{
					if (_GateeventmessagespanDAO == null) _GateeventmessagespanDAO = new GateeventmessagespanDAO(this.ConnectionDetails,this.SessionManager);
					return _GateeventmessagespanDAO;
				}
			}
			public GateeventstateDAO GateeventstateDAO
			{
				get
				{
					if (_GateeventstateDAO == null) _GateeventstateDAO = new GateeventstateDAO(this.ConnectionDetails,this.SessionManager);
					return _GateeventstateDAO;
				}
			}
			public GatemessageDAO GatemessageDAO
			{
				get
				{
					if (_GatemessageDAO == null) _GatemessageDAO = new GatemessageDAO(this.ConnectionDetails,this.SessionManager);
					return _GatemessageDAO;
				}
			}
			public GatemessagerecordDAO GatemessagerecordDAO
			{
				get
				{
					if (_GatemessagerecordDAO == null) _GatemessagerecordDAO = new GatemessagerecordDAO(this.ConnectionDetails,this.SessionManager);
					return _GatemessagerecordDAO;
				}
			}
			public GaterecordlatestDAO GaterecordlatestDAO
			{
				get
				{
					if (_GaterecordlatestDAO == null) _GaterecordlatestDAO = new GaterecordlatestDAO(this.ConnectionDetails,this.SessionManager);
					return _GaterecordlatestDAO;
				}
			}
			public GateuserDAO GateuserDAO
			{
				get
				{
					if (_GateuserDAO == null) _GateuserDAO = new GateuserDAO(this.ConnectionDetails,this.SessionManager);
					return _GateuserDAO;
				}
			}
			public GateviewDAO GateviewDAO
			{
				get
				{
					if (_GateviewDAO == null) _GateviewDAO = new GateviewDAO(this.ConnectionDetails,this.SessionManager);
					return _GateviewDAO;
				}
			}
			public GateviewtagDAO GateviewtagDAO
			{
				get
				{
					if (_GateviewtagDAO == null) _GateviewtagDAO = new GateviewtagDAO(this.ConnectionDetails,this.SessionManager);
					return _GateviewtagDAO;
				}
			}
			public GeocoderDAO GeocoderDAO
			{
				get
				{
					if (_GeocoderDAO == null) _GeocoderDAO = new GeocoderDAO(this.ConnectionDetails,this.SessionManager);
					return _GeocoderDAO;
				}
			}
			public GeocoderapplicationDAO GeocoderapplicationDAO
			{
				get
				{
					if (_GeocoderapplicationDAO == null) _GeocoderapplicationDAO = new GeocoderapplicationDAO(this.ConnectionDetails,this.SessionManager);
					return _GeocoderapplicationDAO;
				}
			}
			public GeocoderproviderDAO GeocoderproviderDAO
			{
				get
				{
					if (_GeocoderproviderDAO == null) _GeocoderproviderDAO = new GeocoderproviderDAO(this.ConnectionDetails,this.SessionManager);
					return _GeocoderproviderDAO;
				}
			}
			public GeofenceDAO GeofenceDAO
			{
				get
				{
					if (_GeofenceDAO == null) _GeofenceDAO = new GeofenceDAO(this.ConnectionDetails,this.SessionManager);
					return _GeofenceDAO;
				}
			}
			public GeofencecircularDAO GeofencecircularDAO
			{
				get
				{
					if (_GeofencecircularDAO == null) _GeofencecircularDAO = new GeofencecircularDAO(this.ConnectionDetails,this.SessionManager);
					return _GeofencecircularDAO;
				}
			}
			public GeofenceeventargumentDAO GeofenceeventargumentDAO
			{
				get
				{
					if (_GeofenceeventargumentDAO == null) _GeofenceeventargumentDAO = new GeofenceeventargumentDAO(this.ConnectionDetails,this.SessionManager);
					return _GeofenceeventargumentDAO;
				}
			}
			public GeofenceeventexpressionDAO GeofenceeventexpressionDAO
			{
				get
				{
					if (_GeofenceeventexpressionDAO == null) _GeofenceeventexpressionDAO = new GeofenceeventexpressionDAO(this.ConnectionDetails,this.SessionManager);
					return _GeofenceeventexpressionDAO;
				}
			}
			public GeofencepolygonDAO GeofencepolygonDAO
			{
				get
				{
					if (_GeofencepolygonDAO == null) _GeofencepolygonDAO = new GeofencepolygonDAO(this.ConnectionDetails,this.SessionManager);
					return _GeofencepolygonDAO;
				}
			}
			public GridDAO GridDAO
			{
				get
				{
					if (_GridDAO == null) _GridDAO = new GridDAO(this.ConnectionDetails,this.SessionManager);
					return _GridDAO;
				}
			}
			public GroupreferrerlogDAO GroupreferrerlogDAO
			{
				get
				{
					if (_GroupreferrerlogDAO == null) _GroupreferrerlogDAO = new GroupreferrerlogDAO(this.ConnectionDetails,this.SessionManager);
					return _GroupreferrerlogDAO;
				}
			}
			public GrouprightDAO GrouprightDAO
			{
				get
				{
					if (_GrouprightDAO == null) _GrouprightDAO = new GrouprightDAO(this.ConnectionDetails,this.SessionManager);
					return _GrouprightDAO;
				}
			}
			public GrouprouteruleDAO GrouprouteruleDAO
			{
				get
				{
					if (_GrouprouteruleDAO == null) _GrouprouteruleDAO = new GrouprouteruleDAO(this.ConnectionDetails,this.SessionManager);
					return _GrouprouteruleDAO;
				}
			}
			public GroupsettingDAO GroupsettingDAO
			{
				get
				{
					if (_GroupsettingDAO == null) _GroupsettingDAO = new GroupsettingDAO(this.ConnectionDetails,this.SessionManager);
					return _GroupsettingDAO;
				}
			}
			public GroupDAO GroupDAO
			{
				get
				{
					if (_GroupDAO == null) _GroupDAO = new GroupDAO(this.ConnectionDetails,this.SessionManager);
					return _GroupDAO;
				}
			}
			public LicenseDAO LicenseDAO
			{
				get
				{
					if (_LicenseDAO == null) _LicenseDAO = new LicenseDAO(this.ConnectionDetails,this.SessionManager);
					return _LicenseDAO;
				}
			}
			public ListenerDAO ListenerDAO
			{
				get
				{
					if (_ListenerDAO == null) _ListenerDAO = new ListenerDAO(this.ConnectionDetails,this.SessionManager);
					return _ListenerDAO;
				}
			}
			public LoadabletypeDAO LoadabletypeDAO
			{
				get
				{
					if (_LoadabletypeDAO == null) _LoadabletypeDAO = new LoadabletypeDAO(this.ConnectionDetails,this.SessionManager);
					return _LoadabletypeDAO;
				}
			}
			public MapdataDAO MapdataDAO
			{
				get
				{
					if (_MapdataDAO == null) _MapdataDAO = new MapdataDAO(this.ConnectionDetails,this.SessionManager);
					return _MapdataDAO;
				}
			}
			public MaplibDAO MaplibDAO
			{
				get
				{
					if (_MaplibDAO == null) _MaplibDAO = new MaplibDAO(this.ConnectionDetails,this.SessionManager);
					return _MaplibDAO;
				}
			}
			public MapmetadataDAO MapmetadataDAO
			{
				get
				{
					if (_MapmetadataDAO == null) _MapmetadataDAO = new MapmetadataDAO(this.ConnectionDetails,this.SessionManager);
					return _MapmetadataDAO;
				}
			}
			public MapsgroupDAO MapsgroupDAO
			{
				get
				{
					if (_MapsgroupDAO == null) _MapsgroupDAO = new MapsgroupDAO(this.ConnectionDetails,this.SessionManager);
					return _MapsgroupDAO;
				}
			}
			public MessageproviderDAO MessageproviderDAO
			{
				get
				{
					if (_MessageproviderDAO == null) _MessageproviderDAO = new MessageproviderDAO(this.ConnectionDetails,this.SessionManager);
					return _MessageproviderDAO;
				}
			}
			public MessagetemplateDAO MessagetemplateDAO
			{
				get
				{
					if (_MessagetemplateDAO == null) _MessagetemplateDAO = new MessagetemplateDAO(this.ConnectionDetails,this.SessionManager);
					return _MessagetemplateDAO;
				}
			}
			public MobilenetworkDAO MobilenetworkDAO
			{
				get
				{
					if (_MobilenetworkDAO == null) _MobilenetworkDAO = new MobilenetworkDAO(this.ConnectionDetails,this.SessionManager);
					return _MobilenetworkDAO;
				}
			}
			public MsgfieldDAO MsgfieldDAO
			{
				get
				{
					if (_MsgfieldDAO == null) _MsgfieldDAO = new MsgfieldDAO(this.ConnectionDetails,this.SessionManager);
					return _MsgfieldDAO;
				}
			}
			public MsgfielddictionaryDAO MsgfielddictionaryDAO
			{
				get
				{
					if (_MsgfielddictionaryDAO == null) _MsgfielddictionaryDAO = new MsgfielddictionaryDAO(this.ConnectionDetails,this.SessionManager);
					return _MsgfielddictionaryDAO;
				}
			}
			public MsgfielddictionaryentryDAO MsgfielddictionaryentryDAO
			{
				get
				{
					if (_MsgfielddictionaryentryDAO == null) _MsgfielddictionaryentryDAO = new MsgfielddictionaryentryDAO(this.ConnectionDetails,this.SessionManager);
					return _MsgfielddictionaryentryDAO;
				}
			}
			public MsgnamespaceDAO MsgnamespaceDAO
			{
				get
				{
					if (_MsgnamespaceDAO == null) _MsgnamespaceDAO = new MsgnamespaceDAO(this.ConnectionDetails,this.SessionManager);
					return _MsgnamespaceDAO;
				}
			}
			public MsgprovsettingDAO MsgprovsettingDAO
			{
				get
				{
					if (_MsgprovsettingDAO == null) _MsgprovsettingDAO = new MsgprovsettingDAO(this.ConnectionDetails,this.SessionManager);
					return _MsgprovsettingDAO;
				}
			}
			public NotifierDAO NotifierDAO
			{
				get
				{
					if (_NotifierDAO == null) _NotifierDAO = new NotifierDAO(this.ConnectionDetails,this.SessionManager);
					return _NotifierDAO;
				}
			}
			public NotifiertagDAO NotifiertagDAO
			{
				get
				{
					if (_NotifiertagDAO == null) _NotifiertagDAO = new NotifiertagDAO(this.ConnectionDetails,this.SessionManager);
					return _NotifiertagDAO;
				}
			}
			public OutgoingwebserviceDAO OutgoingwebserviceDAO
			{
				get
				{
					if (_OutgoingwebserviceDAO == null) _OutgoingwebserviceDAO = new OutgoingwebserviceDAO(this.ConnectionDetails,this.SessionManager);
					return _OutgoingwebserviceDAO;
				}
			}
			public PostprocessorDAO PostprocessorDAO
			{
				get
				{
					if (_PostprocessorDAO == null) _PostprocessorDAO = new PostprocessorDAO(this.ConnectionDetails,this.SessionManager);
					return _PostprocessorDAO;
				}
			}
			public ProtocolDAO ProtocolDAO
			{
				get
				{
					if (_ProtocolDAO == null) _ProtocolDAO = new ProtocolDAO(this.ConnectionDetails,this.SessionManager);
					return _ProtocolDAO;
				}
			}
			public ProtocolversionDAO ProtocolversionDAO
			{
				get
				{
					if (_ProtocolversionDAO == null) _ProtocolversionDAO = new ProtocolversionDAO(this.ConnectionDetails,this.SessionManager);
					return _ProtocolversionDAO;
				}
			}
			public ProvidermessageDAO ProvidermessageDAO
			{
				get
				{
					if (_ProvidermessageDAO == null) _ProvidermessageDAO = new ProvidermessageDAO(this.ConnectionDetails,this.SessionManager);
					return _ProvidermessageDAO;
				}
			}
			public ProvidermessagequeueDAO ProvidermessagequeueDAO
			{
				get
				{
					if (_ProvidermessagequeueDAO == null) _ProvidermessagequeueDAO = new ProvidermessagequeueDAO(this.ConnectionDetails,this.SessionManager);
					return _ProvidermessagequeueDAO;
				}
			}
			public RecorderruleDAO RecorderruleDAO
			{
				get
				{
					if (_RecorderruleDAO == null) _RecorderruleDAO = new RecorderruleDAO(this.ConnectionDetails,this.SessionManager);
					return _RecorderruleDAO;
				}
			}
			public ReferrerDAO ReferrerDAO
			{
				get
				{
					if (_ReferrerDAO == null) _ReferrerDAO = new ReferrerDAO(this.ConnectionDetails,this.SessionManager);
					return _ReferrerDAO;
				}
			}
			public ReportDAO ReportDAO
			{
				get
				{
					if (_ReportDAO == null) _ReportDAO = new ReportDAO(this.ConnectionDetails,this.SessionManager);
					return _ReportDAO;
				}
			}
			public ReportparameterDAO ReportparameterDAO
			{
				get
				{
					if (_ReportparameterDAO == null) _ReportparameterDAO = new ReportparameterDAO(this.ConnectionDetails,this.SessionManager);
					return _ReportparameterDAO;
				}
			}
			public ReportviewerDAO ReportviewerDAO
			{
				get
				{
					if (_ReportviewerDAO == null) _ReportviewerDAO = new ReportviewerDAO(this.ConnectionDetails,this.SessionManager);
					return _ReportviewerDAO;
				}
			}
			public RoleDAO RoleDAO
			{
				get
				{
					if (_RoleDAO == null) _RoleDAO = new RoleDAO(this.ConnectionDetails,this.SessionManager);
					return _RoleDAO;
				}
			}
			public RulechainDAO RulechainDAO
			{
				get
				{
					if (_RulechainDAO == null) _RulechainDAO = new RulechainDAO(this.ConnectionDetails,this.SessionManager);
					return _RulechainDAO;
				}
			}
			public SchedulerDAO SchedulerDAO
			{
				get
				{
					if (_SchedulerDAO == null) _SchedulerDAO = new SchedulerDAO(this.ConnectionDetails,this.SessionManager);
					return _SchedulerDAO;
				}
			}
			public SchedulertaskDAO SchedulertaskDAO
			{
				get
				{
					if (_SchedulertaskDAO == null) _SchedulertaskDAO = new SchedulertaskDAO(this.ConnectionDetails,this.SessionManager);
					return _SchedulertaskDAO;
				}
			}
			public SchedulertaskparameterDAO SchedulertaskparameterDAO
			{
				get
				{
					if (_SchedulertaskparameterDAO == null) _SchedulertaskparameterDAO = new SchedulertaskparameterDAO(this.ConnectionDetails,this.SessionManager);
					return _SchedulertaskparameterDAO;
				}
			}
			public SchedulertasktriggerDAO SchedulertasktriggerDAO
			{
				get
				{
					if (_SchedulertasktriggerDAO == null) _SchedulertasktriggerDAO = new SchedulertasktriggerDAO(this.ConnectionDetails,this.SessionManager);
					return _SchedulertasktriggerDAO;
				}
			}
			public ScriptfileDAO ScriptfileDAO
			{
				get
				{
					if (_ScriptfileDAO == null) _ScriptfileDAO = new ScriptfileDAO(this.ConnectionDetails,this.SessionManager);
					return _ScriptfileDAO;
				}
			}
			public ScriptpageDAO ScriptpageDAO
			{
				get
				{
					if (_ScriptpageDAO == null) _ScriptpageDAO = new ScriptpageDAO(this.ConnectionDetails,this.SessionManager);
					return _ScriptpageDAO;
				}
			}
			public ScriptpluginDAO ScriptpluginDAO
			{
				get
				{
					if (_ScriptpluginDAO == null) _ScriptpluginDAO = new ScriptpluginDAO(this.ConnectionDetails,this.SessionManager);
					return _ScriptpluginDAO;
				}
			}
			public ScriptpluginapplicationDAO ScriptpluginapplicationDAO
			{
				get
				{
					if (_ScriptpluginapplicationDAO == null) _ScriptpluginapplicationDAO = new ScriptpluginapplicationDAO(this.ConnectionDetails,this.SessionManager);
					return _ScriptpluginapplicationDAO;
				}
			}
			public ScriptserviceDAO ScriptserviceDAO
			{
				get
				{
					if (_ScriptserviceDAO == null) _ScriptserviceDAO = new ScriptserviceDAO(this.ConnectionDetails,this.SessionManager);
					return _ScriptserviceDAO;
				}
			}
			public ServerversionDAO ServerversionDAO
			{
				get
				{
					if (_ServerversionDAO == null) _ServerversionDAO = new ServerversionDAO(this.ConnectionDetails,this.SessionManager);
					return _ServerversionDAO;
				}
			}
			public ServicepluginDAO ServicepluginDAO
			{
				get
				{
					if (_ServicepluginDAO == null) _ServicepluginDAO = new ServicepluginDAO(this.ConnectionDetails,this.SessionManager);
					return _ServicepluginDAO;
				}
			}
			public SessionDAO SessionDAO
			{
				get
				{
					if (_SessionDAO == null) _SessionDAO = new SessionDAO(this.ConnectionDetails,this.SessionManager);
					return _SessionDAO;
				}
			}
			public SettingDAO SettingDAO
			{
				get
				{
					if (_SettingDAO == null) _SettingDAO = new SettingDAO(this.ConnectionDetails,this.SessionManager);
					return _SettingDAO;
				}
			}
			public SitesettingDAO SitesettingDAO
			{
				get
				{
					if (_SitesettingDAO == null) _SitesettingDAO = new SitesettingDAO(this.ConnectionDetails,this.SessionManager);
					return _SitesettingDAO;
				}
			}
			public SmsmessageDAO SmsmessageDAO
			{
				get
				{
					if (_SmsmessageDAO == null) _SmsmessageDAO = new SmsmessageDAO(this.ConnectionDetails,this.SessionManager);
					return _SmsmessageDAO;
				}
			}
			public SmsproxyDAO SmsproxyDAO
			{
				get
				{
					if (_SmsproxyDAO == null) _SmsproxyDAO = new SmsproxyDAO(this.ConnectionDetails,this.SessionManager);
					return _SmsproxyDAO;
				}
			}
			public SmsproxyqueueDAO SmsproxyqueueDAO
			{
				get
				{
					if (_SmsproxyqueueDAO == null) _SmsproxyqueueDAO = new SmsproxyqueueDAO(this.ConnectionDetails,this.SessionManager);
					return _SmsproxyqueueDAO;
				}
			}
			public StatisticdataDAO StatisticdataDAO
			{
				get
				{
					if (_StatisticdataDAO == null) _StatisticdataDAO = new StatisticdataDAO(this.ConnectionDetails,this.SessionManager);
					return _StatisticdataDAO;
				}
			}
			public StatistickeyDAO StatistickeyDAO
			{
				get
				{
					if (_StatistickeyDAO == null) _StatistickeyDAO = new StatistickeyDAO(this.ConnectionDetails,this.SessionManager);
					return _StatistickeyDAO;
				}
			}
			public TagDAO TagDAO
			{
				get
				{
					if (_TagDAO == null) _TagDAO = new TagDAO(this.ConnectionDetails,this.SessionManager);
					return _TagDAO;
				}
			}
			public TaggeofenceDAO TaggeofenceDAO
			{
				get
				{
					if (_TaggeofenceDAO == null) _TaggeofenceDAO = new TaggeofenceDAO(this.ConnectionDetails,this.SessionManager);
					return _TaggeofenceDAO;
				}
			}
			public TaguserDAO TaguserDAO
			{
				get
				{
					if (_TaguserDAO == null) _TaguserDAO = new TaguserDAO(this.ConnectionDetails,this.SessionManager);
					return _TaguserDAO;
				}
			}
			public TemplatecmdstepDAO TemplatecmdstepDAO
			{
				get
				{
					if (_TemplatecmdstepDAO == null) _TemplatecmdstepDAO = new TemplatecmdstepDAO(this.ConnectionDetails,this.SessionManager);
					return _TemplatecmdstepDAO;
				}
			}
			public TemporarycredentialDAO TemporarycredentialDAO
			{
				get
				{
					if (_TemporarycredentialDAO == null) _TemporarycredentialDAO = new TemporarycredentialDAO(this.ConnectionDetails,this.SessionManager);
					return _TemporarycredentialDAO;
				}
			}
			public TrackcategoryDAO TrackcategoryDAO
			{
				get
				{
					if (_TrackcategoryDAO == null) _TrackcategoryDAO = new TrackcategoryDAO(this.ConnectionDetails,this.SessionManager);
					return _TrackcategoryDAO;
				}
			}
			public TrackdataDAO TrackdataDAO
			{
				get
				{
					if (_TrackdataDAO == null) _TrackdataDAO = new TrackdataDAO(this.ConnectionDetails,this.SessionManager);
					return _TrackdataDAO;
				}
			}
			public TrackdatamodDAO TrackdatamodDAO
			{
				get
				{
					if (_TrackdatamodDAO == null) _TrackdatamodDAO = new TrackdatamodDAO(this.ConnectionDetails,this.SessionManager);
					return _TrackdatamodDAO;
				}
			}
			public TrackinfoDAO TrackinfoDAO
			{
				get
				{
					if (_TrackinfoDAO == null) _TrackinfoDAO = new TrackinfoDAO(this.ConnectionDetails,this.SessionManager);
					return _TrackinfoDAO;
				}
			}
			public TrackinfomodDAO TrackinfomodDAO
			{
				get
				{
					if (_TrackinfomodDAO == null) _TrackinfomodDAO = new TrackinfomodDAO(this.ConnectionDetails,this.SessionManager);
					return _TrackinfomodDAO;
				}
			}
			public TrackpostprocessorlogDAO TrackpostprocessorlogDAO
			{
				get
				{
					if (_TrackpostprocessorlogDAO == null) _TrackpostprocessorlogDAO = new TrackpostprocessorlogDAO(this.ConnectionDetails,this.SessionManager);
					return _TrackpostprocessorlogDAO;
				}
			}
			public TrackrecorderDAO TrackrecorderDAO
			{
				get
				{
					if (_TrackrecorderDAO == null) _TrackrecorderDAO = new TrackrecorderDAO(this.ConnectionDetails,this.SessionManager);
					return _TrackrecorderDAO;
				}
			}
			public UicontrolstateDAO UicontrolstateDAO
			{
				get
				{
					if (_UicontrolstateDAO == null) _UicontrolstateDAO = new UicontrolstateDAO(this.ConnectionDetails,this.SessionManager);
					return _UicontrolstateDAO;
				}
			}
			public UnitDAO UnitDAO
			{
				get
				{
					if (_UnitDAO == null) _UnitDAO = new UnitDAO(this.ConnectionDetails,this.SessionManager);
					return _UnitDAO;
				}
			}
			public UserattributeDAO UserattributeDAO
			{
				get
				{
					if (_UserattributeDAO == null) _UserattributeDAO = new UserattributeDAO(this.ConnectionDetails,this.SessionManager);
					return _UserattributeDAO;
				}
			}
			public UserattributenotifierDAO UserattributenotifierDAO
			{
				get
				{
					if (_UserattributenotifierDAO == null) _UserattributenotifierDAO = new UserattributenotifierDAO(this.ConnectionDetails,this.SessionManager);
					return _UserattributenotifierDAO;
				}
			}
			public UsergroupDAO UsergroupDAO
			{
				get
				{
					if (_UsergroupDAO == null) _UsergroupDAO = new UsergroupDAO(this.ConnectionDetails,this.SessionManager);
					return _UsergroupDAO;
				}
			}
			public UserroleDAO UserroleDAO
			{
				get
				{
					if (_UserroleDAO == null) _UserroleDAO = new UserroleDAO(this.ConnectionDetails,this.SessionManager);
					return _UserroleDAO;
				}
			}
			public UsersettingDAO UsersettingDAO
			{
				get
				{
					if (_UsersettingDAO == null) _UsersettingDAO = new UsersettingDAO(this.ConnectionDetails,this.SessionManager);
					return _UsersettingDAO;
				}
			}
			public UserDAO UserDAO
			{
				get
				{
					if (_UserDAO == null) _UserDAO = new UserDAO(this.ConnectionDetails,this.SessionManager);
					return _UserDAO;
				}
			}
			public VersionDAO VersionDAO
			{
				get
				{
					if (_VersionDAO == null) _VersionDAO = new VersionDAO(this.ConnectionDetails,this.SessionManager);
					return _VersionDAO;
				}
			}
			public WorkspaceDAO WorkspaceDAO
			{
				get
				{
					if (_WorkspaceDAO == null) _WorkspaceDAO = new WorkspaceDAO(this.ConnectionDetails,this.SessionManager);
					return _WorkspaceDAO;
				}
			}
	}
}
		




