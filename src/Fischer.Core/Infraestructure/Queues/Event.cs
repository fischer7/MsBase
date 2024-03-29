using MediatR;

namespace Fischer.Core.Infraestructure.Queues;
public class Event : Message, INotification
{
    public DateTime Timestamp { get; private set; }

    protected Event()
    {
        Timestamp = DateTime.Now;
    }
}