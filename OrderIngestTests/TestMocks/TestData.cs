namespace OrderIngestTests.TestMocks;

/// <summary>
/// Reusable test data for unit tests.
/// </summary>
public class TestData
{
    /// <summary>
    /// Sample raw order in natural language and its expected converted JSON format.
    /// </summary>
    public static string SampleRawOrder1 => "Patient needs a CPAP with full face mask and humidifier. AHI > 20. Ordered by Dr. Cameron.";
    public static string SampleConvertedOrder1 => """
        {
            "device": "CPAP",
            "patient_name": "Unknown",
            "dob": "Unknown",
            "ordering_provider": "Dr. Cameron",
            "mask_type": "full face mask",
            "qualifier": "AHI > 20",
            "add_ons": ["humidifier"]
        }
        """;

    /// <summary>
    /// Sample raw order in key-value pair format and its expected converted JSON format.
    /// </summary>
    public static string SampleRawOrder2 => """
        Patient Name: Harold Finch
        DOB: 04/12/1952
        Diagnosis: COPD
        Prescription: Requires a portable oxygen tank delivering 2 L per minute.
        Usage: During sleep and exertion.
        Ordering Physician: Dr. Cuddy
        """;
    public static string SampleConvertedOrder2 => """
        {
            "device": "Oxygen Tank",
            "patient_name": "Harold Finch",
            "dob": "04/12/1952",
            "ordering_provider": "Dr. Cuddy",
            "liters": "2 L",
            "usage": "During sleep and exertion.",
            "diagnosis": "COPD"
        }
        """;

    /// <summary>
    /// Sample raw order in JSON-wrapped string format and its expected converted JSON format.
    /// </summary>
    public static string SampleRawOrder3 => """
        {
            "data": "Patient Name: Lisa Turner\nDOB: 09/23/1984\nDiagnosis: Severe sleep apnea\nRecommendation: CPAP therapy with full face mask and heated humidifier.\nAHI: 28\nOrdering Physician: Dr. Foreman\n"
        }
        """;
    public static string SampleConvertedOrder3 => """
        {
            "device": "CPAP",
            "patient_name": "Lisa Turner",
            "dob": "09/23/1984",
            "ordering_provider": "Dr. Foreman",
            "mask_type": "full face mask",
            "qualifier": "AHI: 28",
            "add_ons": ["heated humidifier"],
            "diagnosis": "Severe sleep apnea"
        }
        """;    

    public static string InvalidConvertedOrder => """
        {
            "unknown_property": "",
        }
        """;
}
