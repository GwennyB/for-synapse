namespace OrderIngest.Services.HttpClients;

using OrderIngest.Utils;
using System.Net.Mime;
using System.Text;

/// <summary>
/// A common base for purpose-specific HTTP clients.
/// </summary>
public abstract class ApiClientBase
{
    /// <summary>
    /// The API's base address.
    /// </summary>
    protected abstract string BaseAddress { get; }
    protected readonly HttpClient _client;

    protected ApiClientBase(IHttpClientFactory httpFactory)
    {
        _client = httpFactory.CreateClient(BaseAddress);
    }

    /// <summary>
    /// Converts the specified content to <see cref="StringContent"/> for HTTP requests.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="content"></param>
    /// <returns></returns>
    protected virtual StringContent PackageRequestContent<T>(T content)
    {
        string json = content.ToJson();
        return new StringContent(
            json,
            Encoding.UTF8,
            MediaTypeNames.Application.Json);
    }

    /// <summary>
    /// Creates a full URL by combining the base address with the specified relative path.
    /// </summary>
    /// <param name="relativePath"></param>
    /// <returns></returns>
    protected Uri CreateUrl(string relativePath) => new Uri($"{BaseAddress.TrimEnd('/')}/{relativePath.TrimStart('/')}");
}