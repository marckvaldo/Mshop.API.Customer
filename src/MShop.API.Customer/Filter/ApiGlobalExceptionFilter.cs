using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using MShop.Core.Message;
using MShop.API.Customer.Extension;

namespace MShop.API.Customer.Filter
{
    public class ApiGlobalExceptionFilter : IExceptionFilter
    {
        private readonly IHostEnvironment _hostEnvironment;
        private readonly INotification _notification;

        public ApiGlobalExceptionFilter(INotification notification, IHostEnvironment hostEnvironment)
        {
            _notification = notification;
            _hostEnvironment = hostEnvironment;
        }

        public void OnException(ExceptionContext context)
        {
            var details = new ProblemDetails();
            var exception = context.Exception;

            Notify(exception.Message);

            details.Title = "An error occurred while processing your request.";
            details.Status = StatusCodes.Status500InternalServerError;
            details.Detail = _hostEnvironment.IsDevelopment() ? exception!.Message : "An unexpected error occurred.";
            context.HttpContext.Response.StatusCode = (int)details.Status;

            context.Result = new ObjectResult(ExtensionResponse.Error(_notification.Errors().Select(x => x.Message).ToList()));
            context.ExceptionHandled = true;

        }

        public void Notify(string message)
        {
            _notification.AddNotifications(message);
        }
    }
}
