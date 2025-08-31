namespace OrderIngest.Services.OrderTranslation;

using OpenAI.Responses;
using OrderIngest.Models;
using OrderIngest.Services.Common;
using OrderIngest.Utils;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

/// <summary>
/// A service that translates orders from original source (natural language, structured, or other) to defined <see cref="Order"/> format.
/// NOTE: This can be replaced by a rules-based conversion tool (no AI use) or a different AI agent as needed by swapping the implementation in the DI container.
/// Consider replacing this assistant with an Azure AI Agent which can benefit from access to the most current conversion rules documents.
/// </summary>
public class OrderTranslator : IOrderTranslator
{
    /// <summary>
    /// The base prompt for order conversion. This prompt determines the quality, precision, and accuracy of order conversions by the OpenAI assistant.
    /// </summary>
    public const string ConvertOrderPrompt = """
        You are a helpful assistant who converts medical equipment orders from their original format to a common structured format.
        The original orders might be:
            - natural language (ex: 'Patient needs a CPAP with full face mask and humidifier. AHI > 20. Ordered by Dr. Cameron.')
            - a list of key-value pairs
                ex:
                    Patient Name: Harold Finch
                    DOB: 04/12/1952
                    Diagnosis: COPD
                    Prescription: Requires a portable oxygen tank delivering 2 L per minute.
                    Usage: During sleep and exertion.
                    Ordering Physician: Dr. Cuddy
            - a JSON-wrapped string
                ex:
                    {
                        "data": "Patient Name: Lisa Turner\nDOB: 09/23/1984\nDiagnosis: Severe sleep apnea\nRecommendation: CPAP therapy with full face mask and heated humidifier.\nAHI: 28\nOrdering Physician: Dr. Foreman\n"
                    }
        Regardless of the original form, please extract the following required pieces of data from the original order and put them into this JSON format:
            {
                "device": "This is the device type. It should be 'CPAP', 'Oxygen Tank', or 'Wheelchair'.",
                "patient_name": "This is the patient's name.",
                "dob": "This is the patient's date of birth.",
                "ordering_provider": "This is the name of the doctor or provider who wrote the order.",
                "mask_type": "This is the type of mask to be used with the CPAP machine. If device type is not CPAP, exclude this property.",
                "liters": "This is the capacity of the oxygen tank. Include units (ex: 1 liter should be shown as '1 L'). If device type is not Oxygen Tank, exclude this property.",
                "qualifier": "This is a qualifying condition for the order. ex: 'AHI > 10' is a qualifier for CPAP use meaning 'Apnea-Hypnea Index greater than 10'. If not specified, exclude this property.",
                "add_ons": "This is an array of additional accessories or add-ons for the equipment. ex: A CPAP machine might require a humidifier. If empty, exclude this property.",
                "usage": "This specifies the conditions under which the patient should use the equipment. ex: A doctor may specify that an oxygen tank is to be used for strenuous activity. If empty, exclude this property.",
                "diagnosis": "The diagnosis that warrants use of the specified equipment. If empty, exclude this property.",
            }
        Your response should contain only the converted JSON object - please do not add a greeting, presentation text, content type designators, or other content. Delete any properties that contain empty sets, strings, or nulls.
        The source order is 
        """;

    private readonly IOrderLogger _logger;
    private readonly IOpenAIClientsWrapper _openAIClientsWrapper;

    /// <summary>
    /// Creates a new instance of the <see cref="IOrderTranslator"/>.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="openAIClientWrapper"></param>
    public OrderTranslator(IOrderLogger logger, IOpenAIClientsWrapper openAIClientWrapper)
    {
        _logger = logger;
        _openAIClientsWrapper = openAIClientWrapper;
    }

    /// <inheritdoc/>
    public async Task<string> TranslateOrder(string rawOrder)
    {
        try
        {
            _logger.LogTrace(LogCategory.Ingest, "Starting translation...");
            string prompt = ConvertOrderPrompt + rawOrder;
            string result = await _openAIClientsWrapper.CreateResponseAsync(prompt);
            _logger.LogTrace(LogCategory.Ingest, "Translation complete.");

            // Validate the result by attempting to convert it to an Order object.
            _ = result.ConvertJsonToOrder();

            return result;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogWarning(LogCategory.Ingest, $"OpenAI order translation failed: {ex.Message}");
            // TODO: create custom exception types for clearer handling.
            throw;
        }
        catch (JsonException ex)
        {
            _logger.LogWarning(LogCategory.Ingest, $"JSON conversion of translated failed: {ex.Message}");
            // Order string contains sensitive data. Do not log.
            // Orders that failed conversion should be dead-lettered for manual handling.
            // Re-throwing until dead-lettering is available.
            throw;
        }
    }
}
