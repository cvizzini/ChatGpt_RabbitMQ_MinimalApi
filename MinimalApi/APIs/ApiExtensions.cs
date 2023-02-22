using Mediator;
using MinimalApi.Core.Mediator.Handler;
using MinimalApi.Core.Model;
using MinimalApi.Core.RabbitMQ;
using MinimalApi.Core.RabbitMQ.Queues;
using MinimalApi.Core.Services;

namespace MinimalApi.APIs;
public static class ApiExtensions
{
    public static WebApplication WithPromptApi(this WebApplication app)
    {
        app.MapPost(
                "/chatgpt/{prompt}",
                async ([FromRoute, Required] string prompt, [FromServices] ChatGptService chatGptService) =>
                {
                    return await chatGptService.ExecuteCommand(prompt);

                })
            .WithSummary("Prompt chat gpt for a response")
            .WithDescription("Prompt chat gpt for a response")
            .WithDisplayName("Prompt ChatGPT")
            .WithName("Prompt ChatGPT")
            .WithOpenApi();

        app.MapPost("/queue/product",
                ([FromBody, Required] ProductDto product, [FromServices] RabbitSender rabbitSender) =>
                {
                    rabbitSender.PublishMessage<ProductDto>(product, "storeproduct", RabbitMqQueues.ProductQueue);
                }).WithSummary("Create a product on queue")
            .WithDescription("Product on RabbitMq")
            .WithDisplayName("Product Queue")
            .WithName("Product Queue")
            .WithOpenApi();

        app.MapPost("/queue/order",
                 ([FromBody, Required] OrderDto order, [FromServices] RabbitSender rabbitSender) =>
                {

                    rabbitSender.PublishMessage<OrderDto>(order, "storeorder", RabbitMqQueues.OrderQueue);
                }).WithSummary("Create a order on queue")
            .WithDescription("Order on RabbitMq")
            .WithDisplayName("Order Queue")
            .WithName("Order Queue")
            .WithOpenApi();

        app.MapPost("/mediator/order",
                async ([FromBody, Required] OrderDto order, [FromServices] IMediator mediator) =>
                {
                    return await mediator.Send(order);
                }).WithSummary("Create a order with mediator")
            .WithDescription("Order on Mediator")
            .WithDisplayName("Order Mediator")
            .WithName("Order Mediator")
            .WithOpenApi();

        app.MapGet("/mediator/ping/{id}",
                async ([FromServices] IMediator mediator, [FromRoute, Required] string id) =>
                {
                    var ping = new Ping(id);
                    return await mediator.Send(ping);
                }).WithSummary("Ping pong test with mediator")
            .WithDescription("Ping Pong on Mediator")
            .WithDisplayName("Ping Pong Mediator")
            .WithName("Ping Pong Mediator")
            .WithOpenApi();
        return app;
    }


    public static WebApplication StartRabbitMqConsumers(this WebApplication app)
    {
        var queueHandlers = app.Services.GetService<IEnumerable<IQueueHandler>>();
        Task.WaitAll(queueHandlers.Select(x => x.Consume()).ToArray());
        return app;
    }
}