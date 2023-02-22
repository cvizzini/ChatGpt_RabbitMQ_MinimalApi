# ChatGpt RabbitMQ Minimal API 

> A .NET 7 Minimal API that implements basic ChatGpt Integration with Rabbit MQ and Mediator

## Installation

1. **Install Rabbit MQ**

2. Create and OpenAI account and create an api key

3. add secret ``` dotnet user-secrets set "ChatGPT:ServiceApiKey" "sk-your-key-here" ```

4. Configure the appsettings according your Rabbit MQ setup

5. Set the MinimalApi project as startup


## Features 

1. Chat GPT implementation

2. Rabbit MQ with Async Consumers and direct exchange

3. Rabbit MQ queues automattically started and listening for events at run time

4. Basic Mediatr implementation using [Mediator](https://github.com/martinothamar/Mediator)

5. .NET 7 Minimal API

6. .NET 7 Rate Limiter

7. Swagger with Swagger UI

8. Serilog logging to Console