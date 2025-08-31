namespace OrderIngest.Models;

using System.Text.Json.Serialization;

/// <summary>
/// Base contract for all equipment orders.
/// </summary>
public record Order
{
    /// <summary>
    /// Common input for unknown properties.
    /// </summary>
    private const string Unknown = "Unknown";

    /// <summary>
    /// Gets or sets the device type.
    /// </summary>
    public string Device { get; set; } = Unknown;

    /// <summary>
    /// Gets or sets the patient's name.
    /// </summary>
    public string PatientName { get; set; } = Unknown;

    /// <summary>
    /// Gets or sets the patient's date of birth.
    /// </summary>
    [JsonPropertyName("dob")]
    public string DateOfBirth { get; set; } = Unknown;

    /// <summary>
    /// Gets or sets the ordering provider's name.
    /// </summary>

    public string OrderingProvider { get; set; } = Unknown;

    /// <summary>
    /// Gets or sets the mask type (when used).
    /// </summary>
    public string? MaskType { get; set; }

    /// <summary>
    /// Gets or sets the capacity of an oxygen tank (when used).
    /// </summary>
    public string? Liters { get; set; }

    /// <summary>
    /// Gets or sets the add-ons for the device (when used).
    /// </summary>
    public List<string>? AddOns { get; set; }

    /// <summary>
    /// Gets or sets the qualifier for device usage.
    /// </summary>
    public string? Qualifier { get; set; }

    /// <summary>
    /// Gets or sets the usage conditions (optional).
    /// </summary>
    public string? Usage { get; set; }

    /// <summary>
    /// Gets or sets the patient's diagnosis (optional).
    /// </summary>
    public string? Diagnosis { get; set; }
}
