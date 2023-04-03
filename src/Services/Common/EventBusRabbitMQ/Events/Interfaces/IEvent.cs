using System;


namespace EventBusRabbitMQ.Events.Interfaces
{
    public abstract class IEvent
    {
        public Guid RequestId { get; private init; }
        public DateTime  CreateDate { get; private set; }

        public IEvent()
        {
            RequestId = Guid.NewGuid();
            CreateDate = DateTime.UtcNow;

        }
    }
}
