namespace OrderIngest.Services.Common;

using Microsoft.Extensions.Logging;
using System;

public class OrderLogger : IOrderLogger
{
    /// <summary>
    /// The base logger service instance.
    /// </summary>
    private readonly ILogger<OrderLogger> _logger;

    /// <summary>
    /// Creates a new <see cref="IOrderLogger"/> instance.
    /// </summary>
    /// <param name="logger"></param>
    public OrderLogger(ILogger<OrderLogger> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc/>
    public void LogTrace(LogCategory category, string message)
    {
        _logger.LogTrace("[{Category}] {Message}", category, message);
    }

    /// <inheritdoc/>
    public void LogDebug(LogCategory category, string message)
    {
        _logger.LogDebug("[{Category}] {Message}", category, message);
    }

    /// <inheritdoc/>
    public void LogInformation(LogCategory category, string message)
    {
        _logger.LogInformation("[{Category}] {Message}", category, message);
    }

    /// <inheritdoc/>
    public void LogWarning(LogCategory category, string message)
    {
        _logger.LogWarning("[{Category}] {Message}", category, message);
    }

    /// <inheritdoc/>
    public void LogError(LogCategory category, string message, Exception? ex)
    {
        _logger.LogError(ex, "[{Category}] {Message}", category, message);
    }
}

/// <summary>
/// The category of the log message, associated with the various application features.
/// </summary>
public enum LogCategory
{
    /// <summary>
    /// Log messages related to application startup.
    /// </summary>
    Startup,

    /// <summary>
    /// Log messages related to ingesting the raw order message.
    /// </summary>
    Ingest,

    /// <summary>
    /// Log messages related to publishing the structured order message.
    /// </summary>
    Publish,
}
