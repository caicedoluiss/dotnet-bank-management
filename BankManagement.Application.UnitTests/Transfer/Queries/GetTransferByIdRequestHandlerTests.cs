using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BankManagement.Application.UnitTests;

public class GetTransferByIdRequestHandlerTests
{
  private readonly GetTransferByIdRequestHandler handler;
  private readonly GetTransferByIdRequest request;

  public GetTransferByIdRequestHandlerTests()
  {
    var serviceProvider = TestsConfiguration.ServiceProvider;

    handler = serviceProvider.GetRequiredService<GetTransferByIdRequestHandler>();

    request = new()
    {
      TransferId = 1,
      RetrieveAccountInfo = false,
      RetrieveCustomerInfo = false,
      RetrieveDestinationAccountInfo = false,
      RetrieveDestinationAccountCustomerInfo = false
    };
  }


  [Theory]
  [InlineData(-1)]
  [InlineData(0)]
  public async void Handle_RequestTransferIdLessThan1_ThrowsArgumentExcetion(int id)
  {
    request.TransferId = id;
    var action = () => handler.Handle(request, default);

    await Assert.ThrowsAsync<ArgumentException>(action);
  }

  [Fact]
  public async Task Handle_RequestTransferIdNonExistent_ReturnsNull()
  {
    request.TransferId = int.MaxValue;
    var result = await handler.Handle(request, default);

    Assert.Null(result);
  }

  [Fact]
  public async Task Handle_RequestTransferIdExistent_ReturnsExistentTransferDTO()
  {
    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentTransferDTO>(result);
    Assert.True(result!.Id > 0);
    Assert.Equal(request.TransferId, result.Id);
    Assert.Null(result.Account);
    Assert.Null(result.DestinationAccountInfo);
    Assert.True(result.AccountId > 0);
    Assert.True(result.DestinationAccountId > 0);
  }

  [Fact]
  public async Task Handle_RequestGetAccountInfoTrue_ReturnsExistentTransferDTOWithAccountInfo()
  {
    request.RetrieveAccountInfo = true;
    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentTransferDTO>(result);
    Assert.Equal(request.TransferId, result!.Id);
    Assert.NotNull(result.Account);
    Assert.IsType<ExistentAccountDTO>(result.Account);
  }

  [Fact]
  public async Task Handle_RequestGetDestinationAccountInfoTrue_ReturnsExistentTransferDTOWithDestinationAccountInfo()
  {
    request.RetrieveDestinationAccountInfo = true;
    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentTransferDTO>(result);
    Assert.Equal(request.TransferId, result!.Id);
    Assert.NotNull(result.DestinationAccountInfo);
    Assert.IsType<ExistentAccountDTO>(result.DestinationAccountInfo);
  }

  [Fact]
  public async Task Handle_RequestGetAccountInfoFalse_ReturnsExistentTransferDTOWithNoAccountInfo()
  {
    request.RetrieveAccountInfo = false;
    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentTransferDTO>(result);
    Assert.Equal(request.TransferId, result!.Id);
    Assert.Null(result.Account);
  }

  [Fact]
  public async Task Handle_RequestGetDestinationAccountInfoFalse_ReturnsExistentTransferDTOWithNoDestinationAccountInfo()
  {
    request.RetrieveDestinationAccountInfo = false;
    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentTransferDTO>(result);
    Assert.Equal(request.TransferId, result!.Id);
    Assert.Null(result.DestinationAccountInfo);
  }

  [Fact]
  public async Task Handle_RequestGetCustomerInfoTrue_ReturnsExistentTransferDTOWithAccountAndCustomerInfo()
  {
    request.RetrieveCustomerInfo = true;
    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentTransferDTO>(result);
    Assert.Equal(request.TransferId, result!.Id);
    Assert.NotNull(result.Account);
    Assert.IsType<ExistentAccountDTO>(result.Account);
    Assert.NotNull(result.Account?.Customer);
    Assert.IsType<ExistentCustomerDTO>(result.Account?.Customer);
  }

  [Fact]
  public async Task Handle_RequestGetDestinationAccountCustomerInfoTrue_ReturnsExistentTransferDTOWithDestinationAccountAndItsCustomerInfo()
  {
    request.RetrieveDestinationAccountCustomerInfo = true;
    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentTransferDTO>(result);
    Assert.Equal(request.TransferId, result!.Id);
    Assert.NotNull(result.DestinationAccountInfo);
    Assert.IsType<ExistentAccountDTO>(result.DestinationAccountInfo);
    Assert.NotNull(result.DestinationAccountInfo?.Customer);
    Assert.IsType<ExistentCustomerDTO>(result.DestinationAccountInfo?.Customer);
  }

  [Fact]
  public async Task Handle_RequestGetCustomerInfoFalseGetAccountInfoTrue_ReturnsExistentTransferDTOWithAccountInfoAndNoItsCustomerInfo()
  {
    request.RetrieveAccountInfo = true;
    request.RetrieveCustomerInfo = false;
    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentTransferDTO>(result);
    Assert.Equal(request.TransferId, result!.Id);
    Assert.NotNull(result.Account);
    Assert.IsType<ExistentAccountDTO>(result.Account);
    Assert.Null(result.Account!.Customer);
  }

  [Fact]
  public async Task Handle_RequestGetDestinationCustomerInfoFalseGetAccountInfoTrue_ReturnsExistentTransferDTOWithDestinationAccountAndNoItsCustomerInfo()
  {
    request.RetrieveDestinationAccountInfo = true;
    request.RetrieveDestinationAccountCustomerInfo = false;
    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentTransferDTO>(result);
    Assert.Equal(request.TransferId, result!.Id);
    Assert.NotNull(result.DestinationAccountInfo);
    Assert.IsType<ExistentAccountDTO>(result.DestinationAccountInfo);
    Assert.Null(result.DestinationAccountInfo!.Customer);
  }
}
