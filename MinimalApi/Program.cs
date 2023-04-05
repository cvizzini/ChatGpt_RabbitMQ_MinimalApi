// See https://aka.ms/new-console-template for more information

using MinimalApi.APIs;
using MinimalApi.Core.CapQueue;
using MinimalApi.Core.ChatGpt;
using MinimalApi.Core.Mediator;
using MinimalApi.Core.RabbitMQ;
using MinimalApi.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDefaultRateLimiter();
builder.Services.SetUpRabbitMq(builder.Configuration);
builder.Services.SetupChatGpt(builder.Configuration);
builder.Services.SetUpMediator(builder.Configuration);
builder.Services.SetUpCapQueue(builder.Configuration);

// add policy
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin() // or .WithOrigins("http://localhost:4200") 
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin();
    });
});



builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);

});
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.DisplayRequestDuration();
    options.EnableTryItOutByDefault();
});

app.UseSerilogRequestLogging();
app.WithPromptApi();

app.UseCors(); // enable policy

app.StartRabbitMqConsumers();
await app.RunAsync();