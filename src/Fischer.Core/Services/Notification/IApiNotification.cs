namespace Fischer.Core.Services.Notification;
public interface IApiNotification
{
    void AddNotificationMessage(NotificationMessage notificacao);
    void AddMessage(string notificacao);
    List<NotificationMessage> GetAll();
    bool HasNotification();
    void AddRange(List<NotificationMessage> erros);
    void AddRangeString(IEnumerable<string> erros);
}
