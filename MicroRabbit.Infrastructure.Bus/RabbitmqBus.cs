using MediatR;
using MicroRabbit.Domain.core;
using MicroRabbit.Domain.core.Bus;
using MicroRabbit.Domain.core.Events;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroRabbit.Infrastructure.Bus
{
    public sealed class RabbitmqBus : IEventBus
    {
        private readonly IMediator _mediator;
        private readonly Dictionary<string, List<Type>> _handlers;
        private readonly List<Type> _eventTypes;

        public int x;
        public RabbitmqBus(IMediator mediator)
        {
            _eventTypes = new List<Type>();
            _handlers = new Dictionary<string, List<Type>>();
            _mediator = mediator;
        }

        public Task SendCommand<T>(T command) where T : Command
        {
            //x = 1;
            //Publish(new TransferCreatedEvent(request.From, request.To, request.Amount));

            return _mediator.Send(command); 
        }

        public void Publish<T>(T @event) where T : Event
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var eventName = @event.GetType().Name;

                channel.QueueDeclare(eventName, false, false, false, null);

                var message = JsonConvert.SerializeObject(@event);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish("", eventName, null, body);
            }
        }

        public void Subscribe<T, TH>()
            where T : Event
            where TH : IEventHandler<T>
        {
            var eventName = typeof(T).Name;
            var handlerType = typeof(TH);

            if (!_eventTypes.Contains(typeof(T)))
                _eventTypes.Add(typeof(T));

            if (!_handlers.ContainsKey(eventName))
                _handlers.Add(eventName, new List<Type>());

            if (_handlers[eventName].Any(s => s.GetType() == handlerType))
                throw new ArgumentException($"Handler Type {handlerType.Name} already is registered for '{eventName}'", nameof(handlerType));

            _handlers[eventName].Add(handlerType);

            StartBasicConsume<T>();
        }

        private void StartBasicConsume<T>() where T : Event
        {
            var factory = new ConnectionFactory() 
            { 
                HostName = "localhost", 
                DispatchConsumersAsync = true
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            var eventName = typeof(T).Name;

            channel.QueueDeclare(eventName, false, false, false, null);

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.Received += Consumer_Received;

            channel.BasicConsume(eventName, true, consumer);
        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var eventName = e.RoutingKey;
            var message = Encoding.UTF8.GetString(e.Body.Span);

            try
            {
                await ProccessEvent(eventName, message).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                
            }
        }
            
        private async Task ProccessEvent(string eventName, string message)
        {
            if (_handlers.ContainsKey(eventName))
            {
                var subscriptions = _handlers[eventName];

                foreach (var subscription in subscriptions)
                {
                    var handler = Activator.CreateInstance(subscription);
                    if (handler == null) continue;
                    var eventType = _eventTypes.SingleOrDefault(t => t.Name == eventName);
                    var @event = JsonConvert.DeserializeObject(message, eventType);
                    var concreteType = typeof(IEventHandler<>).MakeGenericType(eventType);
                    await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { @event });
                }
            }
                
        }
    }
}
