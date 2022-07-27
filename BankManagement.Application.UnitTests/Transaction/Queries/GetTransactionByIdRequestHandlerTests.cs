using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BankManagement.Application.UnitTests;

public class GetTransactionByIdRequestHandlerTests
{
  private readonly GetTransactionByIdRequestHandler handler;
  private GetTransactionByIdRequest request;

  public GetTransactionByIdRequestHandlerTests()
  {
    var serviceProvider = TestsConfiguration.ServiceProvider;

    handler = serviceProvider.GetRequiredService<GetTransactionByIdRequestHandler>();

    request = new()
    {
      TransactionId = 1,
      RetrieveAccountInfo = false,
      RetrieveCustomerInfo = false
    };
  }


  [Theory]
  [InlineData(-2)]
  [InlineData(-1)]
  [InlineData(0)]
  public void Handle_IdLessThan1_ThrowsArgumentExcetion(int transactionId)
  {
    request.TransactionId = transactionId;

    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Fact]
  public async Task Handle_NonExistentId_ReturnsNull()
  {
    request.TransactionId = int.MaxValue;

    var result = await handler.Handle(request, default);

    Assert.Null(result);
  }

  [Fact]
  public async Task Handle_ExistentId_ReturnsExistentTransactionDTO()
  {
    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentTransactionDTO>(result);
    Assert.Equal(request.TransactionId, result.Id);
  }

  [Fact]
  public async Task Handle_ExistentIdGetAccountInfoTrue_ReturnsExistentTransactionDTOWithAccountInfo()
  {
    request.RetrieveAccountInfo = true;

    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentTransactionDTO>(result);
    Assert.Equal(request.TransactionId, result.Id);
    Assert.NotNull(result.Account);
  }

  [Fact]
  public async Task Handle_ExistentIdGetAccountInfoFalse_ReturnsExistentTransactionDTOWithNoAccountInfo()
  {
    request.RetrieveAccountInfo = false;

    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentTransactionDTO>(result);
    Assert.Equal(request.TransactionId, result.Id);
    Assert.Null(result.Account);
  }

  [Fact]
  public async Task Handle_ExistentIdGetCustomerTrue_ReturnsExistentTransactionDTOWithAccountAndCustomerInfo()
  {
    request.RetrieveCustomerInfo = true;

    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentTransactionDTO>(result);
    Assert.Equal(request.TransactionId, result.Id);
    Assert.NotNull(result.Account);
    Assert.NotNull(result.Account?.Customer);
  }

  [Fact]
  public async Task Handle_ExistentIdGetCustomerInfoFalseGetAccountInfoTrue_ReturnsExistentTransactionDTOWithNoCustomerInfo()
  {
    request.RetrieveCustomerInfo = false;

    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentTransactionDTO>(result);
    Assert.Equal(request.TransactionId, result.Id);
    Assert.NotNull(result.Account);
    Assert.Null(result.Account!.Customer);
  }
}
