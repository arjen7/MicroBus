# MicroBus
# MicroBus Inter-Service Communication

This project implements a custom MicroBus architecture for inter-service communication, inspired by MediatR. It enables secure, scalable, and flexible messaging between services using gRPC with MessagePack serialization.

## Features

- **MediatR-Like Pattern:** Decoupled messaging that allows you to send requests and receive responses.
- **Mandatory Internal Token:** Every micro request requires an internal token for authentication.
- **Route Attributes:** Each micro request must include a route attribute to correctly direct it to the appropriate service.
- **Shared Request/Response Contracts:** Request and response types are shared between services to ensure consistency, while handlers can be implemented independently in each service.
- **High Performance:** Utilizes gRPC for fast communication and MessagePack for efficient binary serialization.

## Configuration

### Service Registration

Add MicroBus to your dependency injection container. The configuration is similar to MediatR and must include an internal token, logging, and service endpoint registration.

```csharp
builder.Services.AddMicroBus()
    .UseInternalToken("secret") // **Mandatory:** The internal token ("secret") is required for security.
    .UseLogging()               // Enables logging for all micro requests.
    .WithService("UserService", "https://localhost:7217"); // Specifies the target service endpoint.
