# Minimal API  Playground

> A .NET 7 Minimal API Playground that integrations with various technologies. 

## Installation

1. **Install Rabbit MQ**

2. Create and OpenAI account and create an api key

3. add secret ``` dotnet user-secrets set "ChatGPT:ServiceApiKey" "sk-your-key-here" ```

4. Configure the appsettings according your Rabbit MQ setup

5. Set the MinimalApi project as startup

6. (optional) Run the ClientApp/index.html to test DALL-E


## Features 

1. Chat GPT and DALL-E integration

2. Rabbit MQ with Async Consumers, direct and fanout exchanges

3. Rabbit MQ queues automatically started and listening for messages at run time

4. Basic Mediatr implementation using [Mediator](https://github.com/martinothamar/Mediator)

5. .NET 7 Minimal API

6. .NET 7 Rate Limiter

7. Swagger with Swagger UI

8. Serilog logging to Console

9. Cap InMemory Queue[Cap](https://cap.dotnetcore.xyz)