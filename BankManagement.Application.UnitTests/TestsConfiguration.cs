using System;
using BankManagement.Domain;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace BankManagement.Application.UnitTests;

public static class TestsConfiguration
{
  internal static Customer[] Customers = new Customer[]
  {
    new Customer
    {
      Id = 1,
      IdNumber = "1",
      Age = 20,
      Email = "john.doe@email.com",
      Gender = PersonGender.Male,
      Name = "John Doe",
      PhoneNumber = "+1 1234567890",
      State = true,
      CreatedAt = DateTime.UtcNow,
      UpdatedAt = DateTime.UtcNow
    },
    new Customer
    {
      Id = 2,
      IdNumber = "2",
      Age = 36,
      Email = "jane.doe@email.com",
      Gender = PersonGender.Female,
      Name = "Jane Doe",
      PhoneNumber = "+2 1023456789",
      State = true
    },
  };
  internal static Account SampleAccount = new()
  {
    Id = 1,
    Number = "001",
    Type = AccountType.Savings,
    Balance = 500,
    CustomerId = 1,
    State = true,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    Customer = null
  };
  internal static Account SampleAccountWithCustomer = new()
  {
    Id = 1,
    Number = "001",
    Type = AccountType.Savings,
    Balance = 500,
    CustomerId = 1,
    State = true,
    CreatedAt = DateTime.UtcNow,
    UpdatedAt = DateTime.UtcNow,
    Customer = Customers[0]
  };

  internal static IServiceProvider ServiceProvider { get; }

  static TestsConfiguration()
  {
    ServiceProvider = ConfigureServices().BuildServiceProvider();
  }


  private static IServiceCollection ConfigureServices()
  {
    var serviceCollection = new ServiceCollection();

    serviceCollection.AddApplicationServices();

    serviceCollection.AddScoped<IUnitOfWork>(o => GetMockUnitOfWork());

    serviceCollection.AddScoped<GetCustomerByIdRequestHandler>();
    serviceCollection.AddScoped<CreateCustomerRequestCommandHandler>();
    serviceCollection.AddScoped<UpdateCustomerRequestCommandHandler>();
    serviceCollection.AddScoped<DeleteCustomerRequestCommandHandler>();

    serviceCollection.AddTransient<NewCustomerDTO>(o => new()
    {
      IdNumber = "1",
      Age = 20,
      Email = "john.doe@email.com",
      Gender = "Male",
      Name = "John Doe",
      PhoneNumber = "+1 1234567890",
      State = true,
    });

    serviceCollection.AddScoped<GetAccountByIdRequestHandler>();


    return serviceCollection;
  }

  private static ICustomersRepo GetMockCustomersRepo()
  {
    var mockCustomersRepo = new Mock<ICustomersRepo>();
    mockCustomersRepo.Setup(x => x.Get(1, It.IsAny<bool>())).ReturnsAsync(Customers[0]);
    mockCustomersRepo.Setup(x => x.Get(2, It.IsAny<bool>())).ReturnsAsync(Customers[1]);

    mockCustomersRepo.Setup(x => x.Add(It.IsAny<Customer>())).Returns(Customers[0]);

    return mockCustomersRepo.Object;
  }

  private static IAccountsRepo GetMockAccountsRepo()
  {
    var mockAccountsRepo = new Mock<IAccountsRepo>();
    mockAccountsRepo.Setup(x => x.Get(1, false, It.IsAny<bool>())).ReturnsAsync(SampleAccount);
    mockAccountsRepo.Setup(x => x.Get(1, true, It.IsAny<bool>())).ReturnsAsync(SampleAccountWithCustomer);

    return mockAccountsRepo.Object;
  }

  private static IUnitOfWork GetMockUnitOfWork()
  {
    var mockUnitOfWork = new Mock<IUnitOfWork>();
    mockUnitOfWork.Setup(x => x.CustomersRepo).Returns(GetMockCustomersRepo());
    mockUnitOfWork.Setup(x => x.AccountsRepo).Returns(GetMockAccountsRepo());

    return mockUnitOfWork.Object;
  }
}
