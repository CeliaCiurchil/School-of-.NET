namespace Cafe.Domain.Events;

public interface IOrderEventPublisher
{
    public void Publish(OrderPlaced orderPlaced);

    public void Subscribe(IOrderEventSubscriber subscriber);

    public void Unsubscribe(IOrderEventSubscriber subscriber);
}
