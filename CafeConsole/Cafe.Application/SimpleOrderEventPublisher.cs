using Cafe.Domain.Events;

namespace Cafe.Application;

public class SimpleOrderEventPublisher : IOrderEventPublisher
{
    private List<IOrderEventSubscriber> subscribers = new();

    public void Publish(OrderPlaced orderPlaced)
    {
        foreach (var subscriber in subscribers)
        {
            subscriber.On(orderPlaced);
        }
    }

    public void Subscribe(IOrderEventSubscriber subscriber)
    {
        subscribers.Add(subscriber);
    }

    public void Unsubscribe(IOrderEventSubscriber subscriber)
    {
        subscribers.Remove(subscriber);
    }
}
