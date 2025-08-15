namespace Yemekhane.Business.Common;

public class Result<T>
{
    public T? Data { get; }
    public bool Success { get; }
    public IReadOnlyCollection<Notification> Notifications { get; }

    private Result(T? data, bool success, IReadOnlyCollection<Notification> notes)
        => (Data, Success, Notifications) = (data, success, notes);

    public static Result<T> Ok(T data, IReadOnlyCollection<Notification> notes) => new(data, true, notes);
    public static Result<T> Fail(IReadOnlyCollection<Notification> notes) => new(default, false, notes);
}
