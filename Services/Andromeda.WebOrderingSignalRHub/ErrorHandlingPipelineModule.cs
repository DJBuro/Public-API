using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR.Hubs;

namespace Andromeda.WebOrderingSignalRHub
{
    public class ErrorHandlingPipelineModule : HubPipelineModule
    {
        protected override void OnIncomingError(ExceptionContext exceptionContext, IHubIncomingInvokerContext context)
        {
            Global.Log.Error("Hub: Unhandled exception", exceptionContext.Error);

            base.OnIncomingError(exceptionContext, context);
        }
    }
}