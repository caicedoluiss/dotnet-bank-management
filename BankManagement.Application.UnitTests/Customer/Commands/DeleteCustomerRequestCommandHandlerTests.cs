using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BankManagement.Application.UnitTests;

public class DeleteCustomerRequestCommandHandlerTests
{
  private readonly DeleteCustomerRequestCommandHandler handler;

  public DeleteCustomerRequestCommandHandlerTests()
  {
    var serviceProvider = TestsConfiguration.ServiceProvider;

    handler = serviceProvider.GetRequiredService<DeleteCustomerRequestCommandHandler>();
  }

  [Theory]
  [InlineData(-2)]
  [InlineData(-1)]
  [InlineData(0)]
  public void Handle_IdLessThan1_ThrowsArgumentExcetion(int customerId)
  {
    var request = new DeleteCustomerRequestCommand
    {
      CustomerId = customerId
    };

    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Fact]
  public void Handle_NonExistentId_ThrowsArgumentExcetion()
  {
    var request = new DeleteCustomerRequestCommand
    {
      CustomerId = int.MaxValue
    };

    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Fact]
  public async Task Handle_ExistentId_ReturnsExistentCustomerDTO()
  {
    var request = new DeleteCustomerRequestCommand
    {
      CustomerId = 1
    };

    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentCustomerDTO>(result);
    Assert.Equal(request.CustomerId, result.Id);
  }
}
