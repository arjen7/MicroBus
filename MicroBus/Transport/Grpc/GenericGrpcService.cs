using Grpc.Core;
using MessagePack;
using MicroBus.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Text.Json;

namespace MicroBus.Transport.Grpc;

public class GenericGrpcService(IServiceProvider serviceProvider) : MicroBus.MicroBusBase
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly MessagePackSerializerOptions options = MessagePackSerializerOptions.Standard
            .WithResolver(MessagePack.Resolvers.ContractlessStandardResolver.Instance);
    public override async Task<GenericResponse> Handle(GenericRequest request, ServerCallContext context)
    {
        var requestType = Type.GetType(request.RequestType)
                          ?? throw new RpcException(new Status(StatusCode.InvalidArgument, $"Unknown request type: {request.RequestType}"));

        var responseType = Type.GetType(request.ResponseType)
                           ?? throw new RpcException(new Status(StatusCode.InvalidArgument, $"Unknown response type: {request.ResponseType}"));

        var handlerType = typeof(IMicroHandler<,>).MakeGenericType(requestType, responseType);
        var handler = _serviceProvider.GetService(handlerType)
                      ?? throw new RpcException(new Status(StatusCode.NotFound, $"Handler service not registered for type {handlerType.Name}"));

        var payloadBytes = request.Payload.ToByteArray();
        var deserializedRequest = MessagePack.MessagePackSerializer.Deserialize(requestType, payloadBytes,options)
                                  ?? throw new RpcException(new Status(StatusCode.InvalidArgument, "Request deserialization failed."));

        var method = handlerType.GetMethod("HandleAsync")
                     ?? throw new RpcException(new Status(StatusCode.Internal, "Handler does not implement HandleAsync"));

        var task = (Task)method.Invoke(handler, [deserializedRequest, context.CancellationToken])!;
        await task.ConfigureAwait(false);

        dynamic result = ((dynamic)task).Result;

        byte[] responseBytes = MessagePack.MessagePackSerializer.Serialize(responseType, result,options);

        return new GenericResponse
        {
            Payload = Google.Protobuf.ByteString.CopyFrom(responseBytes)
        };
    }

}
