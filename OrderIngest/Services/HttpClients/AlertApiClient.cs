namespace OrderIngest.Services.HttpClients;
using System.Threading.Tasks;

public class AlertApiClient : ApiClientBase, IAlertApiClient
{
    /// <summary>
    /// The relative path for publishing alerts.
    /// </summary>
    private const string PublishAlertsPath = "DrExtract";

    public AlertApiClient(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

    /// <inheritdoc/>
    protected override string BaseAddress => "https://alert-api.com/";

    /// <inheritdoc/>
    public async Task PublishAlertAsync<T>(T message)
    {
        StringContent content = PackageRequestContent(message);
        Uri uri = CreateUrl(PublishAlertsPath);

        // TODO: Enable when the Alert API is available. Stubbed for now.
        // await _client.PostAsync(uri, content);

        string stubFilePath = Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\Outputs", "last_alert.json");
        File.WriteAllText(stubFilePath, message?.ToString());
        await Task.CompletedTask;
    }
}
