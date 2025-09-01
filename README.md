# About the project

This project is responsive to the [Technical Assessment prompt](https://dev.azure.com/SynapsePDI/_git/Technical%20Assessment) requesting refactor of overly 'minimalist' service implementation. The requirements are as follows:
1. Refactor for readability, testability, and relevance.
2. Add logging and basic error handling.
3. Add unit testing.
4. Retain functionality.

This refactor addresses the above while also touching stretch goals:
1. Replace rules-based (manual) extraction with AI solution.
   This solution uses the OpenAI Responses API for translation of orders into structured format.
2. Accept multiple input formats.
   Using OpenAI allowed for offloading of this effort onto the AI model, a task for which they are particularly well equipped. With additional prompt tuning, this is theoretically the least restrictive solution for accommodating input formats.
3. Support more DME device types or qualifiers.
   Also thanks to the OpenAI Responses API, this is also theoretically unlimited, although the prompt will need to afford more info on how to know when an input is missing required info based on the device type.

## Tools used

- Visual Studio 2022
- [OpenAI Responses API](https://platform.openai.com/docs/api-reference/introduction)
- [Assignment prompt](https://dev.azure.com/SynapsePDI/_git/Technical%20Assessment)

## Running the solution

Add your OpenAI API key and model selection in user secrets:
```
dotnet user-secrets set OpenAIKey <your-api-key-here>
dotnet user-secets set OpenAIModel <your-model-choice-here>
```
_NOTE: The `gpt-4o-mini` model was used for development, but feel free to try and compare others._

Run using `dotnet run` or Visual Studio's runer. A temporary terminal-based UI is provided for development purposes, and it allows the user to enter either the raw order text or a file path from which to fetch the raw order text. For each submission, it outputs the result (that would be published to the Alerts API) into the `<root>/OrderIngest/Outputs/` directory.

## Discussion

### The AI assistant

The OpenAI solution was chosen over rules-based extraction because the rules for extraction are not clearly defined by the prompt. LLMs are particularly excellent at converting unstructured data to structured, and I made an assumption that device types, their properties, and their orders will change over time, which makes rules-based extraction a very brittle alternative.
The AI assistant is a bit limited in that all context-specific rules must be delivered in the prompt, which makes it a little difficult to maintain. It's expected that an actual production service would likely run in a cloud compute environment, in which case a better tuned purpose-specific AI agent (ref: [Azure AI Foundry](https://azure.microsoft.com/en-us/products/ai-foundry)) could be developed to replace it. Agents can be enhanced by adding source documents to their knowledge pool, which is a reasonable mechanism to address growth and changes in the supported devices list.

### The temporary UI

The terminal-based UI is not a reasonable solution for a production service. This solution is built such that other, more appropriate, interfaces can be exposed on it by simply adding them to the app container. All live services are exposed on the DI container and can be easily injected for use elsewhere.
The prompt affords no information to hazard a guess at required scale, so the most suitable interface was not obvious. Given that info, I'd choose from these 3 options, which also drive toward specific cloud compute options:
- light, sporadic traffic, perhaps high seasonality (periods with no traffic)
  For this I'd likely consider a serverless function solution as the pricing is based on quantity of requests. This becomes less attractive once request volume - even if inconsistent - exceeds the threshold at which an always-on service would cost the same or less.
  ref for introducing the Azure Function Apps interface: https://learn.microsoft.com/en-us/azure/azure-functions/functions-develop-vs?pivots=isolated
- consistent traffic, but not terribly heavy
  For this I'd probably look at a REST API in a Docker container on a simple compute solution (like Azure App Service). The pricing model is primarily per-hour, there are many available compute SKUs to choose from, and it's fast and easy to get up and running. The per-hour cost is higher than other (more complicated) compute solutions, but that's a tradeoff between overhead maintenance and simplicity.
  Introducing a REST API on the existing service requires little more than implementing controllers and adding support to the DI container (ref: https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-9.0&tabs=visual-studio)
- heavy traffic, consistent or inconsistent
  For high-scale needs I'd look at introducing an events stream (ex: Azure Event Hubs or AWS Kinesis) rather than a REST API (both can be accommodated simultaneously if needed), and house the service in a cluster (Kubernetes, Service Fabric, or other large compute solution). This allows for scalability, and the service is stateless and well situation to be distributed.
  Introducing an events stream requires implementing a [stream consumer](https://learn.microsoft.com/en-us/azure/event-hubs/event-hubs-dotnet-standard-getstarted-send?tabs=passwordless%2Croles-azure-portal#receive-events-from-the-event-hub) to interact with the stream and a [background worker](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-9.0&tabs=visual-studio) to keep the job running continously.

Finally, I'll highlight that this service has no protections at all. To make this production-ready, one needs to add an authentication/authorization layer and logic to scrutinize order inputs for threats. 




