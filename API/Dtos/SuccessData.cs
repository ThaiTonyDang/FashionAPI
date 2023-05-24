namespace API.Dtos
{
    public class SuccessData<T> : BaseReponse
    {
        public T Data { get; private set; }
        public SuccessData(int statusCode, string message, T data) : base(true, message, statusCode)
        {
            this.Data = data;
        }
    }
}
