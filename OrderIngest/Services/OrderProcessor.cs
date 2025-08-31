namespace OrderIngest.Services;

using OrderIngest.Services.Common;
using OrderIngest.Services.HttpClients;
using OrderIngest.Services.OrderTranslation;
using System.Threading.Tasks;

/// <summary>
/// An implementation of <see cref="IOrderProcessor"/> that processes orders.
/// </summary>
public class OrderProcessor : IOrderProcessor
{
    private IOrderLogger _logger;
    private IOrderTranslator _orderTranslator;
    private IAlertApiClient _alertApiClient;

    /// <summary>
    /// Creates a new <see cref="OrderProcessor"/> instance.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="orderTranslator"></param>
    /// <param name="alertApiClient"></param>
    public OrderProcessor(IOrderLogger logger, IOrderTranslator orderTranslator, IAlertApiClient alertApiClient)
    {
        _logger = logger;
        _orderTranslator = orderTranslator;
        _alertApiClient = alertApiClient;
    }

    /// <inheritdoc/>
    public async Task<bool> ProcessOrder(string rawOrder)
    {
        string order = string.Empty;
        try
        {
            // Translate the order:
            order = await _orderTranslator.TranslateOrder(rawOrder);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(LogCategory.Ingest, $"Order translation failed: {ex.Message}");
            // Order string contains sensitive data. Do not log.
            return false;
        }

        try
        {
            // Publish the order to the Alerts API:
            await _alertApiClient.PublishAlertAsync<string>(order);
            return true;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogWarning(LogCategory.Publish, $"Failed to publish the order to the Alerts API: {ex.Message}");
            return false;
        }
    }
}
