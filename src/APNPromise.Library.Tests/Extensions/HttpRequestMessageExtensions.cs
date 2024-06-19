namespace APNPromise.Library.Tests.Extensions;

public static class HttpRequestMessageExtensions
{
    public static void AssertAuthorizationHeader(this HttpRequestMessage request, string expectedToken)
    {
        if (!request.Headers.Contains("Authorization"))
        {
            throw new Exception("Authorization header is missing");
        }

        var authHeader = request.Headers.GetValues("Authorization").FirstOrDefault();
        if (authHeader == null || !authHeader.StartsWith("Bearer ") || authHeader != $"Bearer {expectedToken}")
        {
            throw new Exception("Authorization header is invalid");
        }
    }
}