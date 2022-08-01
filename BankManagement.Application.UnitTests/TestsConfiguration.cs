using System;
using Microsoft.Extensions.DependencyInjection;

namespace BankManagement.Application.UnitTests;

public static class TestsConfiguration
{
  public static IServiceProvider ServiceProvider { get; }

  static TestsConfiguration()
  {
    ServiceProvider = ConfigureServices().BuildServiceProvider();
  }


  private static IServiceCollection ConfigureServices()
  {
    var serviceCollection = new ServiceCollection();

    serviceCollection.AddScoped<GetCustomerByIdRequestHandler>();

    return serviceCollection;
  }
}
