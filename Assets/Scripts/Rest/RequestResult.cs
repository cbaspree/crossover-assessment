public class RequestResult
{
    private string _content;
    private Error _error;

    public string Content { get => _content; }
    public Error Error { get => _error; }

    public RequestResult(string content, Error error)
    {
        _content = content;
        _error = error;
    }
}
