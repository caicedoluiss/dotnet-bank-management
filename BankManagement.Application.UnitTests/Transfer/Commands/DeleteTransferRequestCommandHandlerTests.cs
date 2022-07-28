using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BankManagement.Application.UnitTests;

public class DeleteTransferRequestCommandHandlerTests
{
  private readonly DeleteTransferRequestCommandHandler handler;

  private DeleteTransferRequestCommand request;

  public DeleteTransferRequestCommandHandlerTests()
  {
    var serviceProvider = TestsConfiguration.ServiceProvider;

    handler = serviceProvider.GetRequiredService<DeleteTransferRequestCommandHandler>();

    request = new()
    {
      TransferId = 1,
      RetrieveCustomerInfo = false,
      RetrieveAccountInfo = false,
      RetrieveDestinationAccountInfo = false,
      RetrieveDestinationAccountCustomerInfo = false
    };
  }

  [Theory]
  [InlineData(-1)]
  [InlineData(0)]
  public void Handle_IdLessThan1_ThrowsArgumentExcetion(int accountId)
  {
    request.TransferId = accountId;

    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Fact]
  public void Handle_NonExistentId_ThrowsArgumentExcetion()
  {
    request.TransferId = int.MaxValue;

    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Fact]
  public async Task Handle_ExistentIdGetAccountInfoTrue_ReturnsExistentTransferDTOWithAccountInfo()
  {
    request.RetrieveAccountInfo = true;

    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentTransferDTO>(result);
    Assert.Equal(request.TransferId, result.Id);
    Assert.NotNull(result.Account);
    Assert.IsType<ExistentAccountDTO>(result.Account);
  }

  [Fact]
  public async Task Handle_ExistentIdGetDestinationAccountInfoTrue_ReturnsExistentTransferDTOWithDestinationAccountInfo()
  {
    request.RetrieveDestinationAccountInfo = true;

    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentTransferDTO>(result);
    Assert.Equal(request.TransferId, result.Id);
    Assert.NotNull(result.DestinationAccountInfo);
    Assert.IsType<ExistentAccountDTO>(result.DestinationAccountInfo);
    Assert.True(result.DestinationAccountId > 0);
  }

  [Fact]
  public async Task Handle_ExistentIdGetAccountInfoFalse_ReturnsExistentTransferDTOWithNoAccountInfo()
  {
    request.RetrieveAccountInfo = false;

    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentTransferDTO>(result);
    Assert.Equal(request.TransferId, result.Id);
    Assert.Null(result.Account);
  }

  [Fact]
  public async Task Handle_ExistentIdGetDestinationAccountInfoFalse_ReturnsExistentTransferDTOWithNoDestinationAccountInfo()
  {
    request.RetrieveDestinationAccountInfo = false;

    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentTransferDTO>(result);
    Assert.Equal(request.TransferId, result.Id);
    Assert.Null(result.DestinationAccountInfo);
  }

  [Fact]
  public async Task Handle_ExistentIdGetCustomerTrue_ReturnsExistentTransferDTOWithAccountAndCustomerInfo()
  {
    request.RetrieveCustomerInfo = true;

    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentTransferDTO>(result);
    Assert.Equal(request.TransferId, result.Id);
    Assert.NotNull(result.Account);
    Assert.IsType<ExistentAccountDTO>(result.Account);
    Assert.NotNull(result.Account?.Customer);
    Assert.IsType<ExistentCustomerDTO>(result.Account?.Customer);
  }

  [Fact]
  public async Task Handle_ExistentIdGetDestinationAccountCustomerTrue_ReturnsExistentTransferDTOWithDestinationAccountAndItsCustomerInfo()
  {
    request.RetrieveDestinationAccountCustomerInfo = true;

    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentTransferDTO>(result);
    Assert.Equal(request.TransferId, result.Id);
    Assert.NotNull(result.DestinationAccountInfo);
    Assert.IsType<ExistentAccountDTO>(result.DestinationAccountInfo);
    Assert.NotNull(result.DestinationAccountInfo?.Customer);
    Assert.IsType<ExistentCustomerDTO>(result.DestinationAccountInfo?.Customer);
  }

  [Fact]
  public async Task Handle_ExistentIdGetCustomerInfoFalseGetAccountInfoTrue_ReturnsExistentTransferDTOWithNoCustomerInfo()
  {
    request.RetrieveAccountInfo = true;
    request.RetrieveCustomerInfo = false;

    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentTransferDTO>(result);
    Assert.Equal(request.TransferId, result.Id);
    Assert.NotNull(result.Account);
    Assert.IsType<ExistentAccountDTO>(result.Account);
    Assert.Null(result.Account!.Customer);
  }

  [Fact]
  public async Task Handle_ExistentIdGetDestinationCustomerInfoFalseGetAccountInfoTrue_ReturnsExistentTransferDTOWithNoDestinationCustomerInfo()
  {
    request.RetrieveDestinationAccountInfo = true;
    request.RetrieveDestinationAccountCustomerInfo = false;

    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentTransferDTO>(result);
    Assert.Equal(request.TransferId, result.Id);
    Assert.NotNull(result.DestinationAccountInfo);
    Assert.IsType<ExistentAccountDTO>(result.DestinationAccountInfo);
    Assert.NotNull(result.Account!.Customer);
    Assert.IsType<ExistentCustomerDTO>(result.DestinationAccountInfo!.Customer);
  }
}
