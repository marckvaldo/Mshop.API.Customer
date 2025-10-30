using MShop.Core.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Core.Base
{
    public abstract class BaseQuery
    {
        protected readonly INotification Notifications;

        protected BaseQuery(INotification notification)
        {
            Notifications = notification;
        }

        protected void Notificar(string menssagem)
        {
            Notifications.AddNotifications(menssagem);
        }

        protected bool Validate(Notification validation)
        {
            var result = validation.Validate();
            if (result.HasErrors()) return true;

            return false;
        }

        protected bool TheareErrors()
        {
            if (Notifications.HasErrors()) return true;
            return false;
        }
    }
}
