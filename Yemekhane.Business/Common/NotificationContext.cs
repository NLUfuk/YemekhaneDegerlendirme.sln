namespace Yemekhane.Business.Common;

public class NotificationContext : INotificationContext
{
    private readonly List<Notification> _list = new();

    public bool HasErrors => _list.Any(n => n.Type == NotificationType.Error);
    public IReadOnlyCollection<Notification> All => _list.AsReadOnly();

    public void Add(Notification n) => _list.Add(n);
    public void Error(string code, string message, string? field = null)
      => Add(new(code, message, NotificationType.Error));
    public void Warn(string code, string message, string? field = null)
      => Add(new(code, message, NotificationType.Warning));
    public void Info(string code, string message, string? field = null)
      => Add(new(code, message, NotificationType.Info));

    public void Clear() => _list.Clear();
}
