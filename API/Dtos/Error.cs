using System.Text.Json;

namespace API.Dtos
{
    public class Error<T> : BaseReponse
    {
        public T Errors { get; set; }
        public Error(int statusCode, string message, T errors) : base(false, message, statusCode)
        {
            Errors = errors;
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
