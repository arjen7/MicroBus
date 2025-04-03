using Grpc.Core;
using Grpc.Net.Client;
using MessagePack;
using MicroBus.Security;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace MicroBus.Transport.Grpc;

public class GenericGrpcClient
{
    private readonly MicroBus.MicroBusClient _client;
    private readonly string _internalToken;
    private readonly MessagePackSerializerOptions options = MessagePackSerializerOptions.Standard
            .WithResolver(MessagePack.Resolvers.ContractlessStandardResolver.Instance);
    public GenericGrpcClient(GrpcChannel channel, IServiceProvider provider)
    {
        _client = new MicroBus.MicroBusClient(channel);

        var registry = provider.GetRequiredService<IInternalTokenProvider>();
        _internalToken = registry.Token ?? throw new InvalidOperationException("MicroBus: Token not found in client.");
    }

    public async Task<TResponse> SendAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
    {
        var requestType = typeof(TRequest).AssemblyQualifiedName!;
        var responseType = typeof(TResponse).AssemblyQualifiedName!;
        
        byte[] packedRequest = MessagePackSerializer.Serialize(request, options,cancellationToken: cancellationToken);

        var grpcRequest = new GenericRequest
        {
            RequestType = requestType,
            ResponseType = responseType,
            Payload = Google.Protobuf.ByteString.CopyFrom(packedRequest)
        };

        var headers = new Metadata
        {
            { "x-internal-token", _internalToken }
        };

        var grpcResponse = await _client.HandleAsync(grpcRequest, headers, cancellationToken: cancellationToken);

        
        return MessagePackSerializer.Deserialize<TResponse>(grpcResponse.Payload.ToByteArray(),options, cancellationToken: cancellationToken);
    }
}
