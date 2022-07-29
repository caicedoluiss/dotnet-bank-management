using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BankManagement.Application.UnitTests;

public class GetAccountByIdRequestHandlerTests
{
  private readonly GetAccountByIdRequestHandler handler;

  private readonly GetAccountByIdRequest request;

  public GetAccountByIdRequestHandlerTests()
  {
    var serviceProvider = TestsConfiguration.ServiceProvider;

    handler = serviceProvider.GetRequiredService<GetAccountByIdRequestHandler>();

    request = new()
    {
      AccountId = 1,
      RetreiveCustomerInfo = false
    };
  }


  [Theory]
  [InlineData(-1)]
  [InlineData(0)]
  public void Handle_RequestAccountIdLessThan1_ThrowsArgumentExcetion(int id)
  {
    request.AccountId = id;
    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Fact]
  public async Task Handle_RequestAccountIdNonExistent_ReturnsNull()
  {
    request.AccountId = int.MaxValue;
    var result = await handler.Handle(request, default);

    Assert.Null(result);
  }

  [Fact]
  public async Task Handle_RequestAccountIdExistent_ReturnsExistentAccountDTO()
  {
    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentAccountDTO>(result);
    Assert.Equal(request.AccountId, result.Id);
    Assert.Null(result.Customer);
    Assert.True(result.CustomerId > 0);
  }

  [Fact]
  public async Task Handle_RequestGetCustomerInfoTrue_ReturnsExistentAccountDTOWithCustomerInfo()
  {
    request.RetreiveCustomerInfo = true;
    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentAccountDTO>(result);
    Assert.Equal(request.AccountId, result.Id);
    Assert.NotNull(result.Customer);
    Assert.IsType<ExistentCustomerDTO>(result.Customer);
  }

  [Fact]
  public async Task Handle_RequestGetCustomerInfoFalse_ReturnsExistentAccountDTOWithNoCustomerInfo()
  {
    request.RetreiveCustomerInfo = false;
    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentAccountDTO>(result);
    Assert.Equal(request.AccountId, result.Id);
    Assert.Null(result.Customer);
  }
}
