namespace MassTransit.Courier.MassTransitIntegration
{
    using System;
    using Contracts;

    public class MassTransitMessagingAdaptor : IMessagingAdaptor
    {
        readonly IConsumeContext _context;

        public MassTransitMessagingAdaptor(IConsumeContext context)
        {
            _context = context;
        }

        public Uri GetCurrentHostAddress()
        {
            return _context.Endpoint.Address.Uri;
        }

        public void Forward(RoutingSlip routingSlip, Uri address)
        {
            var endpoint = _context.Bus.GetEndpoint(address);
            endpoint.Forward(_context, routingSlip);
        }

        public void Forward(RoutingSlip routingSlip, Uri address, Uri sourceAddress)
        {
            var endpoint = _context.Bus.GetEndpoint(address);
            endpoint.Send(routingSlip, x => x.SetSourceAddress(sourceAddress));
        }
        
        public void Publish<T>(T message) where T : class
        {
            _context.Bus.Publish<T>(message);
        }
    }
}