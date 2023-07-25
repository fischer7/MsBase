namespace Fischer.Core.Services.Notification;
public class NotificationMessage
{
    public NotificationMessage(string message)
    {
        Message = message;
    }

    public string Message { get; }
}
