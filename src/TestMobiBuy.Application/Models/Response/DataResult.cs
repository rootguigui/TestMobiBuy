namespace TestMobiBuy.Application.Models.Response;

public class DataResult<T>
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public T? Data { get; set; }
    public IEnumerable<string> Errors { get; set; }

    public DataResult(bool isSuccess, string message, T? data, IEnumerable<string> errors)
    {
        IsSuccess = isSuccess;
        Message = message;
        Data = data;
        Errors = errors;
    }
   
}
