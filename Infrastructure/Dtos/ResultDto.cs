namespace Infrastructure.Dtos
{
    public class ResultDto
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }

    public class ErrorResult : ResultDto
    {
        public ErrorResult(string message)
        {
            this.IsSuccess = false;
            this.Message = message;
        }
    }
    public class SuccessResult : ResultDto
    {
        public SuccessResult(string message)
        {
            this.IsSuccess = true;
            this.Message = message;
        }
    }
}
