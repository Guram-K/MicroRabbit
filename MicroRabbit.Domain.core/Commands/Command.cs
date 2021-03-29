using System;

namespace MicroRabbit.Domain.core
{
    public abstract class Command : Message
    {
        public DateTime TimeStamp { get; protected set; }
        public Command()
        {
            TimeStamp = DateTime.Now;
        }
    }
}