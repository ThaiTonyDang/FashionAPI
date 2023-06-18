namespace Infrastructure.Dtos
{
    public class Result
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }

    public class ErrorResult : Result
    {
        public ErrorResult(string message)
        {
            this.IsSuccess = false;
            this.Message = message;
        }
    }
    public class SuccessResult : Result
    {
        public SuccessResult(string message)
        {
            this.IsSuccess = true;
            this.Message = message;
        }
    }
}
