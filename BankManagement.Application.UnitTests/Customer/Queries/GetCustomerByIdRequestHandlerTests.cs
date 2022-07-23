using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BankManagement.Application.UnitTests;

public class GetCustomerByIdRequestHandlerTests
{
  private readonly GetCustomerByIdRequestHandler handler;

  public GetCustomerByIdRequestHandlerTests()
  {
    var serviceProvider = TestsConfiguration.ServiceProvider;

    handler = serviceProvider.GetRequiredService<GetCustomerByIdRequestHandler>();
  }


  [Theory]
  [InlineData(-2)]
  [InlineData(-1)]
  [InlineData(0)]
  public void Handle_IdLessThan1_ThrowsArgumentExcetion(int customerId)
  {
    var request = new GetCustomerByIdRequest
    {
      CustomerId = customerId
    };

    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Fact]
  public async Task Handle_NonExistentId_ReturnsNull()
  {
    var request = new GetCustomerByIdRequest
    {
      CustomerId = int.MaxValue
    };

    var result = await handler.Handle(request, default);

    Assert.Null(result);
  }

  [Fact]
  public async Task Handle_ExistentId_ReturnsExistentCustomerDTO()
  {
    var request = new GetCustomerByIdRequest
    {
      CustomerId = 1
    };

    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentCustomerDTO>(result);
    Assert.Equal(request.CustomerId, result.Id);
  }

}
