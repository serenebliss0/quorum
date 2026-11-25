namespace Quorum.Logic.Services;

public class OperationResult
{
    public bool IsSuccess { get; }

    public string Message { get; }

    public OperationResult(bool isSuccess, string message)
    {
        IsSuccess = isSuccess;
        Message = message;
    }

    public static OperationResult Success(string message)
    {
        return new OperationResult(true, message);
    }

    public static OperationResult Failure(string message)
    {
        return new OperationResult(false, message);
    }
}