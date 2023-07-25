namespace Fischer.Core.Services.Notification;
public class ApiNotification : IApiNotification
{
    private readonly List<NotificationMessage> _notifications;

    public ApiNotification()
    {
        _notifications = new List<NotificationMessage>();
    }

    public void AddMessage(string notification) => _notifications.Add(new NotificationMessage(notification));

    public void AddNotificationMessage(NotificationMessage notification) => _notifications.Add(notification);

    public List<NotificationMessage> GetAll() => _notifications;

    public bool HasNotification() => _notifications.Any();

    public void AddRange(List<NotificationMessage> list) => _notifications.AddRange(list);
    public void AddRangeString(IEnumerable<string> erros)
        => erros.ToList().ForEach(x => AddMessage(x));
}