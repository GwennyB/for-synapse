namespace OrderIngest.Services.OrderTranslation;

using OpenAI.Responses;

/// <summary>
/// Contract for a thin wrapper to allow consumers of the OpenAI clients to be testable.
/// </summary>
public interface IOpenAIClientsWrapper
{
    /// <summary>
    /// Gets the OpenAI Response client.
    /// </summary>
    OpenAIResponseClient ResponseClient { get; }
}
