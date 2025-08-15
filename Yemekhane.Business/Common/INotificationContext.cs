namespace Yemekhane.Business.Common;

public interface INotificationContext
{
    bool HasErrors { get; }
    IReadOnlyCollection<Notification> All { get; }
    void Add(Notification n);
    void Error(string code, string message, string field);
    void Warn(string code, string message, string field);
    void Info(string code, string message, string field);
    void Clear();
}
