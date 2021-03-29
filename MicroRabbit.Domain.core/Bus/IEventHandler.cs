using MicroRabbit.Domain.core.Events;
using System.Threading.Tasks;

namespace MicroRabbit.Domain.core.Bus
{
    public interface IEventHandler<in TEvent> : IEventHandler where TEvent : Event
    {
        Task Handle(TEvent @event);
    }

    public interface IEventHandler
    {
    }
}