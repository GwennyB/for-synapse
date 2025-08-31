namespace OrderIngest.Services.OrderTranslation;

using OpenAI.Responses;

/// <summary>
/// Contract for a thin wrapper to allow consumers of the OpenAI clients to be testable.
/// </summary>
public interface IOpenAIClientsWrapper
{
    /// <summary>
    /// Testable pass-through for the response from the OpenAI Responses API client.
    /// </summary>
    Task<string> CreateResponseAsync(string prompt);
}
