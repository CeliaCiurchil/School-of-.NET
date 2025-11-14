using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Domain.Events
{
    public interface IOrderEventPublisher
    {
        public void Publish(OrderPlaced orderPlaced);

        public void Subscribe(IOrderEventSubscriber subscriber);

        public void Unsubscribe(IOrderEventSubscriber subscriber);
    }
}
