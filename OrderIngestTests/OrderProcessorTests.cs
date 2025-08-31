namespace OrderIngestTests;

using OrderIngest.Services;
using OrderIngestTests.TestMocks;
using System.Text.Json;

public class OrderProcessorTests
{
    private readonly OrderProcessor _orderProcessor;
    private readonly Mock<IOrderTranslator> _mockOrderTranslator = new();
    private readonly Mock<IAlertApiClient> _mockAlertApiClient = new();

    public OrderProcessorTests()
    {
        _orderProcessor = new OrderProcessor(
            new Mock<IOrderLogger>().Object,
            _mockOrderTranslator.Object,
            _mockAlertApiClient.Object);
    }

    [Fact]
    public async Task ProcessOrder_ReturnsTrue_WhenTranslateAndPublishSucceed()
    {
        // Arrange
        string rawOrder = TestData.SampleRawOrder1;
        string translatedOrder = TestData.SampleConvertedOrder1;
        _mockOrderTranslator
            .Setup(t => t.TranslateOrder(rawOrder))
            .ReturnsAsync(translatedOrder);
        _mockAlertApiClient
            .Setup(c => c.PublishAlertAsync<string>(translatedOrder))
            .Returns(Task.CompletedTask);

        // Act
        bool result = await _orderProcessor.ProcessOrder(rawOrder);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task ProcessOrder_ReturnsFalse_WhenTranslateFails()
    {
        // Arrange
        string rawOrder = TestData.SampleRawOrder1;
        _mockOrderTranslator
            .Setup(t => t.TranslateOrder(rawOrder))
            .ThrowsAsync(new JsonException("Translation error"));

        // Act
        bool result = await _orderProcessor.ProcessOrder(rawOrder);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task ProcessOrder_ReturnsFalse_WhenPublishFails()
    {
        // Arrange
        string rawOrder = TestData.SampleRawOrder1;
        string translatedOrder = TestData.SampleConvertedOrder1;
        _mockOrderTranslator
            .Setup(t => t.TranslateOrder(rawOrder))
            .ReturnsAsync(translatedOrder);
        _mockAlertApiClient
            .Setup(c => c.PublishAlertAsync<string>(translatedOrder))
            .ThrowsAsync(new HttpRequestException("Publish error"));

        // Act
        bool result = await _orderProcessor.ProcessOrder(rawOrder);

        // Assert
        Assert.False(result);
    }
}
