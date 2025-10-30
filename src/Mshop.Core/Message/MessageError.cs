namespace MShop.Core.Message
{
    public class MessageError
    {
        public string Message { get; }
        public MessageError(string message)
        {
            Message = message;
        }
    }
}
