namespace OrderIngest.Services.OrderTranslation;

using Microsoft.Extensions.Configuration;
using OpenAI;
using OpenAI.Responses;

public class OpenAIClientsWrapper : IOpenAIClientsWrapper
{
    private readonly OpenAIResponseClient _responseClient;

    /// <summary>
    /// Creates an instance of the <see cref="IOpenAIClientsWrapper"/>.
    /// </summary>
    /// <param name="configuration">The <see cref="IConfiguration"/> instance.</param>
    public OpenAIClientsWrapper(IConfiguration configuration)
    {
        string apiKey = configuration.GetValue<string>("OpenAIKey");
        string model = configuration.GetValue<string>("OpenAIModel");
        OpenAIClient client = new OpenAIClient(apiKey);
        _responseClient = client.GetOpenAIResponseClient(model);
    }

    /// <inheritdoc/>
    public async Task<string> CreateResponseAsync(string prompt)
    {
        OpenAIResponse response = await _responseClient.CreateResponseAsync(prompt);
        return response.GetOutputText();
    }
}