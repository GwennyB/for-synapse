namespace OrderIngest.Services.OrderTranslation;

using OrderIngest.Models;

/// <summary>
/// Contract for a service that ingests orders from various sources.
/// </summary>
public interface IOrderTranslator
{
    /// <summary>
    /// Ingests orders and converts them to match the <see cref="Order"/> format.
    /// </summary>
    /// <param name="rawOrder">The raw content of the original order.</param>
    /// <returns>The translated order string, matching <see cref="Order"/> format.</returns>
    Task<string> TranslateOrder(string rawOrder);
}
