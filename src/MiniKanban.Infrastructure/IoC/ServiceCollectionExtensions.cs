using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MiniKanban.Domain.Interfaces;
using MiniKanban.Infrastructure.Data.Context;

namespace MiniKanban.Infrastructure.IoC;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services, 
        string connectionString)
    {
        services.AddDbContext<MiniKanbanDbContext>(options =>
            options.UseNpgsql(connectionString));

        var entryAssembly = Assembly.GetEntryAssembly();
        if (entryAssembly != null)
        {
            var referencedAssemblies = entryAssembly.GetReferencedAssemblies();
            foreach (var assemblyName in referencedAssemblies)
            {
                if (assemblyName.Name != null && assemblyName.Name.StartsWith("MiniKanban"))
                {
                    try
                    {
                        Assembly.Load(assemblyName);
                    }
                    catch
                    {
                    }
                }
            }
        }

        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => a.FullName != null && a.FullName.StartsWith("MiniKanban"));

        foreach (var assembly in assemblies)
        {
            var types = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract);

            foreach (var type in types)
            {
                var isScoped = typeof(ScopedInjection).IsAssignableFrom(type);
                var isSingleton = typeof(SingletonInjection).IsAssignableFrom(type);
                var isTransient = typeof(TransientInjection).IsAssignableFrom(type);

                if (isScoped || isSingleton || isTransient)
                {
                    var cleanTypeName = type.Name;
                    if (type.IsGenericType)
                    {
                        var backtickIndex = type.Name.IndexOf('`');
                        if (backtickIndex > 0)
                        {
                            cleanTypeName = type.Name.Substring(0, backtickIndex);
                        }
                    }
                    var genericInterfaceName = "I" + cleanTypeName;

                    var matchingInterface = type.GetInterfaces()
                        .FirstOrDefault(i => {
                            var name = i.Name;
                            if (i.IsGenericType)
                            {
                                var idx = i.Name.IndexOf('`');
                                if (idx > 0)
                                {
                                    name = i.Name.Substring(0, idx);
                                }
                            }
                            return name == genericInterfaceName;
                        });

                    if (matchingInterface != null)
                    {
                        if (type.IsGenericType)
                        {
                            var openType = type.GetGenericTypeDefinition();
                            var openInterface = matchingInterface.GetGenericTypeDefinition();

                            if (isScoped)
                            {
                                services.AddScoped(openInterface, openType);
                            }
                            else if (isSingleton)
                            {
                                services.AddSingleton(openInterface, openType);
                            }
                            else if (isTransient)
                            {
                                services.AddTransient(openInterface, openType);
                            }
                        }
                        else
                        {
                            if (isScoped)
                            {
                                services.AddScoped(matchingInterface, type);
                            }
                            else if (isSingleton)
                            {
                                services.AddSingleton(matchingInterface, type);
                            }
                            else if (isTransient)
                            {
                                services.AddTransient(matchingInterface, type);
                            }
                        }
                    }
                }
            }
        }

        return services;
    }
}
