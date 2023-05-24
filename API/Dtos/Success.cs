namespace API.Dtos
{
    public class Success : BaseReponse
    {
        public Success(int statusCode, string message) : base(true, message, statusCode){ }
    }
}
