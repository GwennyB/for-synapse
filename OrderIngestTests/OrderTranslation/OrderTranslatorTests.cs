namespace OrderIngestTests.OrderTranslation;

using OrderIngestTests.TestMocks;
using System.Text.Json;

public class OrderTranslatorTests
{
    private OrderTranslator _orderTranslator;
    private Mock<IOpenAIClientsWrapper> _mockOpenAIClientsWrapper = new();

    public OrderTranslatorTests()
    {
        _orderTranslator = new OrderTranslator(new Mock<IOrderLogger>().Object, _mockOpenAIClientsWrapper.Object);
    }

    [Fact]
    public async Task TranslateOrder_ReturnsTranslatedOrder_WhenAgentResponseValid()
    {
        string rawOrder = TestData.SampleRawOrder1;
        string expectedConvertedOrder = TestData.SampleConvertedOrder1;

        _mockOpenAIClientsWrapper.Setup(m => m.CreateResponseAsync(It.IsAny<string>()))
            .ReturnsAsync(expectedConvertedOrder);

        var result = await _orderTranslator.TranslateOrder(rawOrder);

        Assert.Equal(expectedConvertedOrder, result);
    }

    [Fact]
    public async Task TranslateOrder_Throws_WhenAgentResponseInvalid()
    {
        string rawOrder = TestData.SampleRawOrder1;
        string expectedConvertedOrder = TestData.InvalidConvertedOrder;

        _mockOpenAIClientsWrapper.Setup(m => m.CreateResponseAsync(It.IsAny<string>()))
            .ReturnsAsync(expectedConvertedOrder);

        await Assert.ThrowsAsync<JsonException>(async () => await _orderTranslator.TranslateOrder(rawOrder));
    }

    [Fact]
    public async Task TranslateOrder_Throws_WhenAgentFails()
    {
        string rawOrder = TestData.SampleRawOrder1;

        _mockOpenAIClientsWrapper.Setup(m => m.CreateResponseAsync(It.IsAny<string>()))
            .ThrowsAsync(new HttpRequestException());

        await Assert.ThrowsAsync<HttpRequestException>(async () => await _orderTranslator.TranslateOrder(rawOrder));
    }
}
