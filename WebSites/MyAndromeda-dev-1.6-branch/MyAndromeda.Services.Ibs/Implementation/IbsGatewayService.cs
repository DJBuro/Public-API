using System.ServiceModel.Channels;
using System.Web.Services.Protocols;
using MyAndromeda.Core;
using MyAndromeda.Services.Ibs.IbsWebOrderApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel.Dispatcher;
using MyAndromeda.Logging;
using System.Reactive.Subjects;
using System.ServiceModel.Description;

namespace MyAndromeda.Services.Ibs.Implementation
{
    public class IbsGatewayService : ITransientDependency
    {
        private readonly IIbsStoreDevice ibsStoreDevice;
        private readonly IInspectorBehavior inspectBehavior;
        private readonly IMyAndromedaLogger logger; 

        public IbsGatewayService(IIbsStoreDevice ibsStoreDevice, IInspectorBehavior inspectBehavior, IMyAndromedaLogger logger)
        {
            this.logger = logger;
            this.inspectBehavior = inspectBehavior;
            this.ibsStoreDevice = ibsStoreDevice;

            this.inspectBehavior.RequestMessage.Subscribe((e) => {
                this.logger.Info("IBS Call:");
                this.logger.Info(e);
            });
            this.inspectBehavior.RequestResponse.Subscribe((e) => {
                this.logger.Info("IBS Response:");
                this.logger.Info(e);
            });
        }

        internal async Task<WebOrdersAPISoapClient> CreateClient(int andromedaSiteId)
        {
            WebOrdersAPISoapClient client; 

            //<endpoint address="https://server1.stocklinkonline.com/stocknetapi/WebOrdersAPI.asmx" binding="basicHttpBinding" bindingConfiguration="WebOrdersAPISoap" contract="IbsWebOrderApi.WebOrdersAPISoap" name="WebOrdersAPISoap" />
            
            var settings = await this.ibsStoreDevice.GetIbsStoreDeviceAsync(andromedaSiteId);

            BasicHttpBinding binding = new BasicHttpBinding();
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.MaxBufferSize = int.MaxValue;

            if (settings.Url.Contains("https")) 
            {
                BasicHttpSecurityMode securitymode = BasicHttpSecurityMode.Transport;
                binding.Security.Mode = securitymode;    
            }
            
            Uri uri = new Uri(settings.Url); 

            var endpoint = new EndpointAddress(uri);

            client = new IbsWebOrderApi.WebOrdersAPISoapClient(binding, endpoint);
            client.Endpoint.EndpointBehaviors.Add(inspectBehavior);

            return client;
        }
    }

    public interface IInspectorBehavior : IEndpointBehavior, ITransientDependency 
    {
        ISubject<string> RequestMessage { get; }
        ISubject<string> RequestResponse { get; }
    }
    
    public class InspectorBehavior : IInspectorBehavior
    {
        private readonly IMyMessageInspector inspector;

        public ISubject<string> RequestMessage { get; private set; }
        public ISubject<string> RequestResponse { get; private set; }

        public InspectorBehavior(IMyMessageInspector inspector)
        {
            this.inspector = inspector;
            this.RequestMessage = inspector.RequestMessage;
            this.RequestResponse = inspector.RequestResponse;
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.ClientMessageInspectors.Add(inspector);
        }
    }

    public interface IMyMessageInspector : IClientMessageInspector, ITransientDependency
    {
        ISubject<string> RequestMessage { get; }
        ISubject<string> RequestResponse { get; }
    }
    public class MyMessageInspector : IMyMessageInspector
    {
        public MyMessageInspector() 
        {
            this.RequestResponse = new Subject<string>();
            this.RequestMessage = new Subject<string>();
        }

        public ISubject<string> RequestMessage { get; private set; }
        public ISubject<string> RequestResponse { get; private set; }

        public void AfterReceiveReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
            var response = reply.ToString();

            this.RequestResponse.OnNext(response);
        }

        public object BeforeSendRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel)
        {
            var lastRequestXml = request.ToString();

            this.RequestMessage.OnNext(lastRequestXml);

            return request;
        }
    }

}
