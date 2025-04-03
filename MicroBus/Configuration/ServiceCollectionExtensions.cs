using MicroBus.Abstractions;
using MicroBus.Transport.Dispatcher;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MicroBus.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static IMicroBusBuilder AddMicroBus(this IServiceCollection services)
        {
            services.AddScoped<IMicroRequestSender, MicroRequestSender>();
            services.AddSingleton<MicroHandlerRegistry>();
            services.AddGrpc();
            var builder = new MicroBusBuilder(services);
            services.AddSingleton(builder);
            builder.Register();
            services.AddMicroBusHandlers();
            return builder;
        }
        public static void AddMicroBusHandlers(this IServiceCollection services)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                Type[] types;
                try
                {
                    types = assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    types = ex.Types.Where(t => t != null).ToArray()!;
                }

                foreach (var type in types)
                {
                    if (type.IsAbstract || type.IsInterface)
                        continue;

                    foreach (var iface in type.GetInterfaces())
                    {
                        if (iface.IsGenericType && iface.GetGenericTypeDefinition() == typeof(IMicroHandler<,>))
                        {
                            Console.WriteLine($"[MicroBus] Registering handler: {type.FullName} as {iface.FullName}");
                            services.AddScoped(iface, type);
                        }
                    }
                }
            }
        }


    }
}