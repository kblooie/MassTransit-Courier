namespace MassTransit.Courier.Tests.Testing
{
    using System;
    using Contracts;

    public class TestMessagingAdaptor : IMessagingAdaptor
    {
        readonly IServiceBus _bus;

        public TestMessagingAdaptor(IServiceBus bus)
        {
            _bus = bus;
        }

        public Uri GetCurrentHostAddress()
        {
            return _bus.Endpoint.Address.Uri;
        }

        public void Forward(RoutingSlip routingSlip, Uri address)
        {
            _bus.GetEndpoint(address).Send(routingSlip);
        }

        public void Forward(RoutingSlip routingSlip, Uri address, Uri sourceAddress)
        {
            _bus.GetEndpoint(address).Send(routingSlip, c => c.SetSourceAddress(sourceAddress));
        }

        public void Publish<T>(T message) where T : class
        {
            _bus.Publish(message);
        }
    }
}