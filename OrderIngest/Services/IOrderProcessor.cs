namespace OrderIngest.Services;

/// <summary>
/// Contract for a service that processes equipment orders.
/// </summary>
public interface IOrderProcessor
{
    /// <summary>
    /// Processes an order.
    /// </summary>
    /// <param name="rawOrder"></param>
    /// <returns>Whether the order was processed successfully.</returns>
    Task<bool> ProcessOrder(string rawOrder);
}
