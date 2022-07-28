using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BankManagement.Application.UnitTests;

public class GetTransferByIdRequestHandlerTests
{
  private readonly GetTransferByIdRequestHandler handler;
  private GetTransferByIdRequest request;

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
  public void Handle_IdLessThan1_ThrowsArgumentExcetion(int transferId)
  {
    request.TransferId = transferId;

    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Fact]
  public async Task Handle_NonExistentId_ReturnsNull()
  {
    request.TransferId = int.MaxValue;

    var result = await handler.Handle(request, default);

    Assert.Null(result);
  }

  [Fact]
  public async Task Handle_ExistentId_ReturnsExistentTransferDTO()
  {
    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentTransferDTO>(result);
    Assert.True(result.Id > 0);
    Assert.Equal(request.TransferId, result.Id);
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
