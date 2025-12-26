namespace Amazon.SharedKernel.API;

public class BadRequestModel
{
    public string Message { get; }
    public bool ShouldUserSessionTerminated { get; }

    public BadRequestModel(string message, bool shouldUserSessionTerminated = false)
    {
        Message = message;
        ShouldUserSessionTerminated = shouldUserSessionTerminated;
    }
}
