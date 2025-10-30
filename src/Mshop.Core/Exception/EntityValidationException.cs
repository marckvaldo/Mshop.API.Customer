using MShop.Core.Message;

namespace MShop.Core.Exception
{
    public class EntityValidationException : ApplicationException
    {
        private readonly INotification _notification;
        public EntityValidationException(
            string message
            ) : base(message)
        {
        }
    }


}