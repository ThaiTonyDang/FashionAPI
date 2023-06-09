namespace API.Dtos
{
    public class BaseReponse
    {
        public bool IsSuccess { get; private set; }
        public string Message { get; private set; }
        public int StatusCode { get; private set; }

        public BaseReponse(bool isSuccess)
        {
            this.IsSuccess = isSuccess;
        }

        public BaseReponse(bool isSuccess, string message, int statusCode) : this(isSuccess)
        {
            Message = message;
            StatusCode = statusCode;
        }
    }
}
