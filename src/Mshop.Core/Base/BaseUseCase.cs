using FluentValidation.Results;
using MShop.Core.Exception;
using MShop.Core.Message;


namespace MShop.Core.Base
{
    public abstract class BaseUseCase: Notifications
    {
        protected readonly INotification Notifications;

        protected BaseUseCase(INotification notification)
        {
            Notifications = notification;
        }

        protected void Notify(string menssagem)
        {
            Notifications.AddNotifications(menssagem);
        }

        protected bool Validate(ValidationResult result)
        {
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    Notifications.AddNotifications(error.ErrorMessage);
                }
                return false;
            }
            
            return true;

        }

        protected void NotifyException(string menssagem)
        {
            Notifications.AddNotifications(menssagem);
            throw new ApplicationValidationException("Error");
        }

        protected void NotifyExceptionIfNull(object? @object,  string menssagem)
        {
            if (@object == null)
            {
                Notifications.AddNotifications(menssagem);
                throw new ApplicationValidationException("Error");
            }
                
        }

        protected bool NotifyErrorIfNull(object? @object, string menssagem)
        {
            if (@object == null)
            {
                Notifications.AddNotifications(menssagem);
                return true;
            }
            return false;
        }


    }
}
