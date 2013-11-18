namespace MassTransit.Courier
{
    using Contracts;

    public interface IRoutingSlipContext : RoutingSlip
    {
        T GetActivityArguments<T>();
        T GetActivityLog<T>();
    }
}