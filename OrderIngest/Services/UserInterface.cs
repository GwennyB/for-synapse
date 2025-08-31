namespace OrderIngest.Services;

/// <summary>
/// Provides a terminal interface for feeding orders for processing.
/// This is a temporary interface for PoC purposes only. The service behind this UI can be used with a prod-suitable interface (eg, REST API, events stream, etc).
/// </summary>
public class UserInterface
{
    private readonly IOrderProcessor _orderProcessor;

    public UserInterface(IOrderProcessor orderProcessor)
    {
        _orderProcessor = orderProcessor;
    }

    /// <summary>
    /// Launches the interface and orchestrates user selections.
    /// </summary>
    public void Launch()
    {
        bool quit = false;
        while (!quit)
        {
            Console.WriteLine("Select an option (or press 'q' to quit):");
            Console.WriteLine(" 1 - Enter order here.");
            Console.WriteLine(" 2 - Fetch order from file.\r");
            ConsoleKeyInfo selected = Console.ReadKey();
            Console.WriteLine("\r");
            
            switch (selected.Key)
            {
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    HandleManualOrderEntry().GetAwaiter().GetResult();
                    break;
                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    HandleFetchOrderFromFile().GetAwaiter().GetResult();
                    break;
                case ConsoleKey.Q:
                    quit = true;
                    break;
                default:
                    Console.WriteLine($"Invalid selection ({selected.Key} - please try again.\r\r");
                    break;
            }
        }

        Console.WriteLine("Exiting... Bye!");
    }

    /// <summary>
    /// Accepts user-entered manual orders.
    /// </summary>
    private async Task HandleManualOrderEntry()
    {
        Console.WriteLine("\rPlease enter the equipment order:\r");
        string rawOrder = Console.ReadLine();
        if (rawOrder is null)
        {
            Console.WriteLine("Invalid order. Returning to main menu.\r\r");
            return;
        }

        await HandleOrderProcessing(rawOrder);
    }

    /// <summary>
    /// Accepts user-specified paths for file-based orders.
    /// </summary>
    private async Task HandleFetchOrderFromFile()
    {
        Console.WriteLine("\rPlease enter the complete file path:");
        string path = Console.ReadLine();
        if (path is null || !File.Exists(path))
        {
            Console.Write("Invalid path. Returning to main menu.\r\r");
            return;
        }

        string rawOrder = File.ReadAllText(path);

        await HandleOrderProcessing(rawOrder);
    }

    /// <summary>
    /// Handles the common order processing logic.
    /// </summary>
    /// <param name="rawOrder">The raw order to process.</param>
    /// <returns>A completed <see cref="Tasnk"/>.</returns>
    private async Task HandleOrderProcessing(string rawOrder)
    {
        bool success = await _orderProcessor.ProcessOrder(rawOrder);
        Console.WriteLine(success ? "Order processed successfully.\r\r" : "Order processing failed.\r\r");
    }
}
