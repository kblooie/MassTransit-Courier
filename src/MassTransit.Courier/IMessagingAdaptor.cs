namespace MassTransit.Courier
{
    using System;
    using Contracts;

    public interface IMessagingAdaptor
    {
        Uri GetCurrentHostAddress();

        void Forward(RoutingSlip routingSlip, Uri address);

        void Forward(RoutingSlip routingSlip, Uri address, Uri sourceAddress);

        void Publish<T>(T message) where T : class;
    }
}