using BankManagement.Application;
using BankManagement.Infrastructure.Persistency;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using BankManagement.Domain;

namespace BankManagement.Infrastructure;

public static class ConfigureInfrastructureServices
{
  /// <summary>
  /// Setups Services with Sql InMemory Db
  /// </summary>
  /// <param name="services"></param>
  /// <returns></returns>
  public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
  {
    services.AddDbContext<BankManagementDbContext>(options => options.UseInMemoryDatabase(databaseName: "default"));

    services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    services.AddScoped<ICustomersRepo, CustomersRepo>();
    services.AddScoped<IAccountsRepo, AccountsRepo>();
    services.AddScoped<ITransactionsRepo, TransactionsRepo>();
    services.AddScoped<ITransfersRepo, TransfersRepo>();

    services.AddScoped<IUnitOfWork, UnitOfWork>();

    return services;
  }

  /// <summary>
  /// Set up services with Sql Server db
  /// </summary>
  /// <param name="services"></param>
  /// <param name="configuration"></param>
  /// <returns></returns>
  public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string connectionString)
  {
    services.AddDbContext<BankManagementDbContext>(options => options.UseSqlServer(connectionString,
                                                      b => b.MigrationsAssembly(typeof(ConfigureInfrastructureServices).Assembly.GetName().Name)));

    services.AddScoped<ICustomersRepo, CustomersRepo>();
    services.AddScoped<IAccountsRepo, AccountsRepo>();
    services.AddScoped<ITransactionsRepo, TransactionsRepo>();
    services.AddScoped<ITransfersRepo, TransfersRepo>();
    services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

    services.AddScoped<IUnitOfWork, UnitOfWork>();

    return services;
  }
}
