namespace Yemekhane.Business.Common;

public record Notification(
    string Code,
    string Message,
    NotificationType Type = NotificationType.Error

);
