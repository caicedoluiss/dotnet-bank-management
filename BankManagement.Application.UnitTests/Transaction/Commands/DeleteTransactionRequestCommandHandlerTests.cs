using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BankManagement.Application.UnitTests;

public class DeleteTransactionRequestCommandHandlerTests
{
  private readonly DeleteTransactionRequestCommandHandler handler;

  private DeleteTransactionRequestCommand request;

  public DeleteTransactionRequestCommandHandlerTests()
  {
    var serviceProvider = TestsConfiguration.ServiceProvider;

    handler = serviceProvider.GetRequiredService<DeleteTransactionRequestCommandHandler>();

    request = new()
    {
      TransactionId = 1,
      RetrieveCustomerInfo = false,
      RetrieveAccountInfo = false
    };
  }

  [Theory]
  [InlineData(-2)]
  [InlineData(-1)]
  [InlineData(0)]
  public void Handle_IdLessThan1_ThrowsArgumentExcetion(int accountId)
  {
    request.TransactionId = accountId;

    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Fact]
  public void Handle_NonExistentId_ThrowsArgumentExcetion()
  {
    request.TransactionId = int.MaxValue;

    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
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
    request.RetrieveAccountInfo = true;
    request.RetrieveCustomerInfo = false;

    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentTransactionDTO>(result);
    Assert.Equal(request.TransactionId, result.Id);
    Assert.NotNull(result.Account);
    Assert.Null(result.Account!.Customer);
  }
}
