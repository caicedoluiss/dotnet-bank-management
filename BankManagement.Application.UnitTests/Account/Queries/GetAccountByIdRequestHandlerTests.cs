using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BankManagement.Application.UnitTests;

public class GetAccountByIdRequestHandlerTests
{
  private readonly GetAccountByIdRequestHandler handler;

  public GetAccountByIdRequestHandlerTests()
  {
    var serviceProvider = TestsConfiguration.ServiceProvider;

    handler = serviceProvider.GetRequiredService<GetAccountByIdRequestHandler>();
  }


  [Theory]
  [InlineData(-2)]
  [InlineData(-1)]
  [InlineData(0)]
  public void Handle_IdLessThan1_ThrowsArgumentExcetion(int accountId)
  {
    var request = new GetAccountByIdRequest
    {
      AccountId = accountId
    };

    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Fact]
  public async Task Handle_NonExistentId_ReturnsNull()
  {
    var request = new GetAccountByIdRequest
    {
      AccountId = int.MaxValue
    };

    var result = await handler.Handle(request, default);

    Assert.Null(result);
  }

  [Fact]
  public async Task Handle_ExistentId_ReturnsExistentAccountDTO()
  {
    var request = new GetAccountByIdRequest
    {
      AccountId = 1
    };

    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentAccountDTO>(result);
    Assert.Equal(request.AccountId, result.Id);
  }

  [Fact]
  public async Task Handle_ExistentIdGetCustomerInfoTrue_ReturnsExistentAccountDTOWithCustomerInfo()
  {
    var request = new GetAccountByIdRequest
    {
      AccountId = 1,
      RetreiveCustomerInfo = true
    };

    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentAccountDTO>(result);
    Assert.Equal(request.AccountId, result.Id);
    Assert.NotNull(result.Customer);
  }

  [Fact]
  public async Task Handle_ExistentIdGetCustomerInfoFalse_ReturnsExistentAccountDTOWithNoCustomerInfo()
  {
    var request = new GetAccountByIdRequest
    {
      AccountId = 1,
      RetreiveCustomerInfo = false
    };

    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentAccountDTO>(result);
    Assert.Equal(request.AccountId, result.Id);
    Assert.Null(result.Customer);
  }
}
