using Microsoft.Extensions.DependencyInjection;

namespace BankManagement.Application;

public static class ConfigureApplicationServices
{
  public static IServiceCollection AddApplicationServices(this IServiceCollection services)
  {
    services.AddSingleton<ICustomerMappingProfile, CustomerMappingProfile>();
    services.AddSingleton<IAccountMappingProfile, AccountMappingProfile>();
    services.AddSingleton<ITransactionMappingProfile, TransactionMappingProfile>();
    services.AddSingleton<ITransferMappingProfile, TransferMappingProfile>();

    return services;
  }
}
