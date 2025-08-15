using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Yemekhane.Business.Common;
namespace Yemekhane.API.Filters;

public class ModelStateToNotificationFilter : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var notes = context.ModelState
                .SelectMany(kv => kv.Value!.Errors.Select(e =>
                    new Notification("MODEL_VALIDATION", $"{kv.Key}: {e.ErrorMessage}", NotificationType.Error)))
                .ToList();

            var payload = new
            {
                success = false,
                data = (object?)null,
                notifications = notes.Select(n => new {
                    type = n.Type.ToString().ToLower(),
                    code = n.Code,
                    message = n.Message
                })
            };

            context.Result = new UnprocessableEntityObjectResult(payload);
        }
    }
}
