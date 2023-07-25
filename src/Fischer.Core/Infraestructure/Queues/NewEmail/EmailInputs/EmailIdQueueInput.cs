namespace Fischer.Core.Infraestructure.Queues.NewEmail.EmailInputs;
public sealed class EmailIdQueueInput : QueueInput
{
    public EmailIdQueueInput(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; }
}