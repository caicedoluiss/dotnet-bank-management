using Microsoft.Extensions.DependencyInjection;

namespace BankManagement.Application;

public static class ConfigureApplicationServices
{
  public static IServiceCollection AddApplicationServices(this IServiceCollection services)
  {
    services.AddSingleton<ICustomerMappingProfile, CustomerMappingProfile>();

    return services;
  }
}
