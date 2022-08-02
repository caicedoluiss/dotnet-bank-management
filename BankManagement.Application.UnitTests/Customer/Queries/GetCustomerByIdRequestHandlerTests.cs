using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BankManagement.Application.UnitTests;

public class GetCustomerByIdRequestHandlerTests
{
  private readonly GetCustomerByIdRequestHandler handler;
  private readonly GetCustomerByIdRequest request;

  public GetCustomerByIdRequestHandlerTests()
  {
    var serviceProvider = TestsConfiguration.ServiceProvider;

    handler = serviceProvider.GetRequiredService<GetCustomerByIdRequestHandler>();

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
    var action = () => handler.Handle(request, default);

    Assert.ThrowsAsync<ArgumentException>(action);
  }

  [Fact]
  public async Task Handle_RequestCustomerIdNonExistent_ReturnsNull()
  {
    request.CustomerId = int.MaxValue;
    var result = await handler.Handle(request, default);

    Assert.Null(result);
  }

  [Fact]
  public async Task Handle_RequestIdExistent_ReturnsExistentCustomerDTO()
  {
    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentCustomerDTO>(result);
    Assert.Equal(request.CustomerId, result!.Id);
    Assert.True(result.Id > 0);
  }

}
