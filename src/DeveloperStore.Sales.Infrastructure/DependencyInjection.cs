using DeveloperStore.Sales.Application.Common;
using DeveloperStore.Sales.Domain.Events;
using DeveloperStore.Sales.Domain.Repositories;
using DeveloperStore.Sales.Infrastructure.EventLog.Handlers;
using DeveloperStore.Sales.Infrastructure.EventLog.Repositories;
using DeveloperStore.Sales.Infrastructure.Persistence;
using DeveloperStore.Sales.Infrastructure.Persistence.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace DeveloperStore.Sales.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // PostgreSQL / EF Core
        services.AddDbContext<SalesDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<ISaleRepository, SaleRepository>();

        // MongoDB
        services.AddSingleton<IMongoClient>(_ =>
            new MongoClient(configuration["MongoDb:ConnectionString"]));

        services.AddSingleton<IMongoDatabase>(sp =>
            sp.GetRequiredService<IMongoClient>()
              .GetDatabase(configuration["MongoDb:DatabaseName"]));

        services.AddScoped<IMongoEventLogRepository, MongoEventLogRepository>();

        // MongoDB domain event handlers (run alongside Application ILogger handlers)
        services.AddTransient<INotificationHandler<DomainEventNotification<SaleCreatedEvent>>,
            DomainEventMongoHandler<SaleCreatedEvent>>();
        services.AddTransient<INotificationHandler<DomainEventNotification<SaleModifiedEvent>>,
            DomainEventMongoHandler<SaleModifiedEvent>>();
        services.AddTransient<INotificationHandler<DomainEventNotification<SaleCancelledEvent>>,
            DomainEventMongoHandler<SaleCancelledEvent>>();
        services.AddTransient<INotificationHandler<DomainEventNotification<ItemCancelledEvent>>,
            DomainEventMongoHandler<ItemCancelledEvent>>();

        return services;
    }
}
