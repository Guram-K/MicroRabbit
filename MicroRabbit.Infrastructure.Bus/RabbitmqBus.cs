using MediatR;
using MicroRabbit.Domain.core;
using MicroRabbit.Domain.core.Bus;
using MicroRabbit.Domain.core.Events;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroRabbit.Infrastructure.Bus
{
    public sealed class RabbitmqBus : IEventBus
    {
        private readonly IMediator _mediator;
        private readonly Dictionary<string, List<Type>> _handlers;
        private readonly List<Type> _eventTypes;

        public RabbitmqBus(IMediator mediator, List<Type> eventTypes)
        {
            _eventTypes = new List<Type>();
            _handlers = new Dictionary<string, List<Type>>();
        }

        public Task SendCommand<T>(T command) where T : Command
        {
            return _mediator.Send(command);
        }

        public void Publish<T>(T @event) where T : Event
        {
            throw new NotImplementedException();
        }

        public void Subscribe<T, TH>()
            where T : Event
            where TH : IEventHandler<T>
        {
            throw new NotImplementedException();
        }
    }
}
