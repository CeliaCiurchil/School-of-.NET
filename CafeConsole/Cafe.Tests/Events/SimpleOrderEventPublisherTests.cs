using Cafe.Application;
using Cafe.Domain.Enums;
using Cafe.Domain.Events;
using Moq;
using System;
using Xunit;

namespace Cafe.Tests.Events
{
    public class SimpleOrderEventPublisherTests
    {
        [Fact]
        public void Publish_WhenCalled_NotifiesAllSubscribersOnce()
        {
            var publisher = new SimpleOrderEventPublisher();
            var mockSubscriber = new Mock<IOrderEventSubscriber>();
            publisher.Subscribe(mockSubscriber.Object);

            var evt = new OrderPlaced(
                Guid.NewGuid(),
                DateTimeOffset.Now,
                "Espresso with milk",
                3.70m,
                PricingType.Regular,
                3.70m
            );

            publisher.Publish(evt);

            mockSubscriber.Verify(s => s.On(It.Is<OrderPlaced>(o => o.Total == 3.70m)), Times.Once);
        }
    }
}
