using System;
using System.Collections.Generic;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace WcfRestService
{
    public class CORSEnablingBehavior : BehaviorExtensionElement, IEndpointBehavior
    {
        public void AddBindingParameters(
          ServiceEndpoint endpoint,
          BindingParameterCollection bindingParameters)
        { }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime) { }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(
              new CORSHeaderInjectingMessageInspector()
            );
        }

        public void Validate(ServiceEndpoint endpoint) { }

        public override Type BehaviorType { get { return typeof(CORSEnablingBehavior); } }

        protected override object CreateBehavior() { return new CORSEnablingBehavior(); }

        private class CORSHeaderInjectingMessageInspector : IDispatchMessageInspector
        {
            public object AfterReceiveRequest(
              ref Message request,
              IClientChannel channel,
              InstanceContext instanceContext)
            {
                var httpRequest = (HttpRequestMessageProperty)request.Properties[HttpRequestMessageProperty.Name];
                return httpRequest.Method.Equals("OPTIONS", StringComparison.InvariantCulture);
            }

            private static IDictionary<string, string> _headersToInject = new Dictionary<string, string>
      {
        { "Access-Control-Allow-Origin", "*" },
        { "Access-Control-Request-Method", "POST,GET,PUT,DELETE,OPTIONS" },
        { "Access-Control-Allow-Methods", "POST,GET,PUT,DELETE,OPTIONS"  },
        { "Access-Control-Allow-Headers", "X-Requested-With,Content-Type" }
      };

            public void BeforeSendReply(ref Message reply, object correlationState)
            {
                if ((bool)correlationState)
                {
                    var httpResponse = (HttpResponseMessageProperty)reply.Properties[HttpResponseMessageProperty.Name];
                    httpResponse.SuppressEntityBody = true;
                    httpResponse.StatusCode = HttpStatusCode.OK;
                }

                var httpHeader = reply.Properties["httpResponse"] as HttpResponseMessageProperty;
                foreach (var item in _headersToInject)
                {
                    httpHeader.Headers.Add(item.Key, item.Value);
                }
            }
        }
    }
}