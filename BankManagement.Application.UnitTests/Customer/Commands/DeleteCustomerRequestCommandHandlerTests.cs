using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BankManagement.Application.UnitTests;

public class DeleteCustomerRequestCommandHandlerTests
{
  private readonly DeleteCustomerRequestCommandHandler handler;

  private readonly DeleteCustomerRequestCommand request;

  public DeleteCustomerRequestCommandHandlerTests()
  {
    var serviceProvider = TestsConfiguration.ServiceProvider;

    handler = serviceProvider.GetRequiredService<DeleteCustomerRequestCommandHandler>();

    request = new()
    {
      CustomerId = 1
    };
  }

  [Theory]
  [InlineData(-1)]
  [InlineData(0)]
  public void Handle_RequestCustomerIdLessThan1_ThrowsArgumentExcetion(int id)
  {
    request.CustomerId = id;
    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Fact]
  public void Handle_RequestCustomerIdNonExistent_ThrowsArgumentExcetion()
  {
    request.CustomerId = int.MaxValue;
    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Fact]
  public async Task Handle_RequestCustomerIdExistent_ReturnsExistentCustomerDTO()
  {
    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentCustomerDTO>(result);
    Assert.Equal(request.CustomerId, result.Id);
    Assert.True(result.Id > 0);
  }
}
