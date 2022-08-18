using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BankManagement.Application.UnitTests;

public class DeleteTransactionRequestCommandHandlerTests
{
  private readonly DeleteTransactionRequestCommandHandler handler;

  private readonly DeleteTransactionRequestCommand request;

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
  [InlineData(-1)]
  [InlineData(0)]
  public async void Handle_RequestTransactionIdLessThan1_ThrowsArgumentExcetion(int id)
  {
    request.TransactionId = id;
    var action = () => handler.Handle(request, default);

    await Assert.ThrowsAsync<ArgumentException>(action);
  }

  [Fact]
  public async void Handle_RequestTransactionIdNonExistent_ThrowsArgumentExcetion()
  {
    request.TransactionId = int.MaxValue;
    var action = () => handler.Handle(request, default);

    await Assert.ThrowsAsync<ArgumentException>(action);
  }

  [Fact]
  public async Task Handle_RequestTransactionIdExistent_ReturnsExistentTransferDTO()
  {
    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentTransactionDTO>(result);
    Assert.True(result.Id > 0);
    Assert.Equal(request.TransactionId, result.Id);
    Assert.Null(result.Account);
    Assert.True(result.AccountId > 0);
  }

  [Fact]
  public async Task Handle_RequestGetAccountInfoTrue_ReturnsExistentTransactionDTOWithAccountInfo()
  {
    request.RetrieveAccountInfo = true;
    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentTransactionDTO>(result);
    Assert.Equal(request.TransactionId, result.Id);
    Assert.NotNull(result.Account);
    Assert.IsType<ExistentAccountDTO>(result.Account);
  }

  [Fact]
  public async Task Handle_RequestGetAccountInfoFalse_ReturnsExistentTransactionDTOWithNoAccountInfo()
  {
    request.RetrieveAccountInfo = false;
    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentTransactionDTO>(result);
    Assert.Equal(request.TransactionId, result.Id);
    Assert.Null(result.Account);
  }

  [Fact]
  public async Task Handle_RequestGetCustomerInfoTrue_ReturnsExistentTransactionDTOWithAccountAndCustomerInfo()
  {
    request.RetrieveCustomerInfo = true;
    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentTransactionDTO>(result);
    Assert.Equal(request.TransactionId, result.Id);
    Assert.NotNull(result.Account);
    Assert.IsType<ExistentAccountDTO>(result.Account);
    Assert.NotNull(result.Account?.Customer);
    Assert.IsType<ExistentCustomerDTO>(result.Account?.Customer);
  }

  [Fact]
  public async Task Handle_RequestGetCustomerInfoFalseGetAccountInfoTrue_ReturnsExistentTransactionDTOWithAccountInfoAndNoCustomerInfo()
  {
    request.RetrieveAccountInfo = true;
    request.RetrieveCustomerInfo = false;
    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentTransactionDTO>(result);
    Assert.Equal(request.TransactionId, result.Id);
    Assert.NotNull(result.Account);
    Assert.IsType<ExistentAccountDTO>(result.Account);
    Assert.Null(result.Account!.Customer);
  }
}
