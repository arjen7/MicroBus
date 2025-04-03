using MicroBus.Abstractions;
using MicroBus.Extensions;
using MicroBus.Security;
using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace MicroBus.Configuration
{
    public class MicroBusBuilder(IServiceCollection services) : IMicroBusBuilder
    {
        private readonly IServiceCollection _services = services;
        private readonly Dictionary<string, string> _serviceMap = [];

        private string? _internalToken;
        private bool _enableLogging;

        public IServiceCollection Services => _services;

        public IMicroBusBuilder WithService(string serviceName, string url)
        {
            _serviceMap[serviceName] = url;
            return this;
        }

        public IMicroBusBuilder UseInternalToken(string token)
        {
            _internalToken = token;
            _services.AddSingleton<IInternalTokenProvider>(new InternalTokenProvider(_internalToken));
            _services.AddSingleton(new InternalAuthInterceptor(_internalToken));
            return this;
        }

        public IMicroBusBuilder UseLogging()
        {
            _enableLogging = true;
            _services.AddSingleton<LoggingInterceptor>();
            return this;
        }

        internal void Register()
        {
            _services.AddSingleton<IServiceRegistry>(new ServiceRegistry(_serviceMap));

            _services.AddGrpc(o =>
            {
                o.Interceptors.Add<InternalAuthInterceptor>();

                if (_enableLogging)
                    o.Interceptors.Add<LoggingInterceptor>();
            });
        }


    }
}