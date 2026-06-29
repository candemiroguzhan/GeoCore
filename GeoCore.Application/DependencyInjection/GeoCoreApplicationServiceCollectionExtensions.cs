using FluentValidation;
using GeoCore.Application.Mapping;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;

namespace GeoCore.Application.DependencyInjection;

public static class GeoCoreApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddGeoCoreApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(GeoCoreApplicationServiceCollectionExtensions).Assembly);

        var config = TypeAdapterConfig.GlobalSettings.Clone();
        config.Scan(typeof(GeoCoreMappingRegister).Assembly);

        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();

        return services;
    }
}
