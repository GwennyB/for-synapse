namespace OrderIngest.Services.Common;

using System;

public interface IOrderLogger
{
    /// <summary>
    /// Logs a trace-level message.
    /// </summary>
    /// <param name="category">The category indicates what the application was doing when the log fired.</param>
    /// <param name="message">The message to log.</param>
    void LogTrace(LogCategory category, string message);

    /// <summary>
    /// Logs a debug-level message.
    /// </summary>
    /// <param name="category">The category indicates what the application was doing when the log fired.</param>
    /// <param name="message">The message to log.</param>
    void LogDebug(LogCategory category, string message);

    /// <summary>
    /// Logs an information-level message.
    /// </summary>
    /// <param name="category">The category indicates what the application was doing when the log fired.</param>
    /// <param name="message">The message to log.</param>
    void LogInformation(LogCategory category, string message);

    /// <summary>
    /// Logs a warning-level message.
    /// </summary>
    /// <param name="category">The category indicates what the application was doing when the log fired.</param>
    /// <param name="message">The message to log.</param>
    void LogWarning(LogCategory category, string message);

    /// <summary>
    /// Logs an error-level message.
    /// </summary>
    /// <param name="category">The category indicates what the application was doing when the log fired.</param>
    /// <param name="message">The message to log.</param>
    /// <param name="ex">the exception to log (if any)</param>
    void LogError(LogCategory category, string message, Exception? ex);
}
