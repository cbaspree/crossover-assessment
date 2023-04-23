using System.Net;

public class Error
{
    private HttpStatusCode _statusCode;
    private string _reasonPhrase;

    public HttpStatusCode StatusCode { get => _statusCode; }
    public string ReasonPhrase { get => _reasonPhrase; }

    public Error(HttpStatusCode statusCode, string reasonPhrase)
    {
        _statusCode = statusCode;
        _reasonPhrase = reasonPhrase;
    }
}