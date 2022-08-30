using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BankManagement.Application.UnitTests;

public class UpdateTransferRequestCommandHandlerTest
{
  private readonly UpdateTransferRequestCommandHandler handler;

  private readonly UpdateTransferRequestCommand request;
  private readonly UpdatingTransferDTO transferDTO;

  public UpdateTransferRequestCommandHandlerTest()
  {
    var serviceProvider = TestsConfiguration.ServiceProvider;
    handler = serviceProvider.GetRequiredService<UpdateTransferRequestCommandHandler>();

    transferDTO = new()
    {
      AccountId = 1,
      Date = "2022-07-29 00:00:00.000-5:00",
      Value = 50.76M,
      Balance = 1550.67M,
      DestinationAccountId = 2
    };

    request = new()
    {
      TransferId = 1,
      TransanctionInfo = transferDTO,
      RetrieveAccountInfo = false,
      RetrieveCustomerInfo = false,
      RetrieveDestinationAccountInfo = false,
      RetrieveDestinationAccountCustomerInfo = false
    };
  }


  [Fact]
  public async void Handle_RequestNullTransferInfo_ThrowsArgumentException()
  {
    request.TransanctionInfo = null;
    var action = () => handler.Handle(request, default);

    await Assert.ThrowsAsync<ArgumentException>(action);
  }

  [Theory]
  [InlineData(-1)]
  [InlineData(0)]
  public async void Handle_RequestTransferIdLessThan1_ThrowsArgumentException(int id)
  {
    request.TransferId = id;
    var action = () => handler.Handle(request, default);

    await Assert.ThrowsAsync<ArgumentException>(action);
  }

  [Fact]
  public async void Handle_RequestTransferIdNonExistent_ThrowsArgumentException()
  {
    request.TransferId = int.MaxValue;
    var action = () => handler.Handle(request, default);

    await Assert.ThrowsAsync<ArgumentException>(action);
  }

  [Fact]
  public async Task Handle_RequestTransferIdExistent_ReturnsExistentTransferDTO()
  {
    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentTransferDTO>(result);
    Assert.True(result.Id > 0);
    Assert.Equal(request.TransferId, result.Id);
    Assert.Null(result.Account);
    Assert.Null(result.DestinationAccountInfo);
    Assert.True(result.AccountId > 0);
    Assert.True(result.DestinationAccountId > 0);
  }

  [Theory]
  [InlineData(-1)]
  [InlineData(0)]
  public async void Handle_TransferAccountIdLessThan1_ThrowsArgumentException(int id)
  {
    transferDTO.AccountId = id;
    var action = () => handler.Handle(request, default);

    await Assert.ThrowsAsync<ArgumentException>(action);
  }

  [Fact]
  public async void Handle_TransferAccountIdNonExistent_ThrowsArgumentException()
  {
    transferDTO.AccountId = int.MaxValue;
    var action = () => handler.Handle(request, default);

    await Assert.ThrowsAsync<ArgumentException>(action);
  }

  [Fact]
  public async void Handle_TransferDestinationAccountIdSameAsAccountId_ThrowsArgumentException()
  {
    transferDTO.DestinationAccountId = transferDTO.AccountId;
    var action = () => handler.Handle(request, default);

    await Assert.ThrowsAsync<ArgumentException>(action);
  }

  [Theory]
  [InlineData(null)]
  [InlineData("")]
  [InlineData("qwerty")]
  [InlineData("07-28-2022")]
  public async void Handle_TransferDateInvalid_ThrowsArgumentException(string? date)
  {
    transferDTO.Date = date;
    var action = () => handler.Handle(request, default);

    await Assert.ThrowsAsync<ArgumentException>(action);
  }

  [Theory]
  [InlineData("2021-05-15 00:00:00.000Z")]
  [InlineData("2000-11-01 10:00:00.000+2:00")]
  [InlineData("1998-08-25 18:00:00.000-5:00")]
  public async Task Handle_TransferDateValid_ReturnsExistentTransferDTO(string? date)
  {
    transferDTO.Date = date;
    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentTransferDTO>(result);
    Assert.True(result.Id > 0);
  }

  [Theory]
  [InlineData(0)]
  [InlineData(-1)]
  public async void Handle_TransferValueLessOrEqualsThan0_ThrowsArgumentException(decimal value)
  {
    transferDTO.Value = value;
    var action = () => handler.Handle(request, default);

    await Assert.ThrowsAsync<ArgumentException>(action);
  }

  [Theory]
  [InlineData(0.1)]
  [InlineData(1000)]
  [InlineData(55.09)]
  [InlineData(895.14)]
  public async Task Handle_TransferValueValid_ReturnsExistentTransferDTO(decimal value)
  {
    transferDTO.Value = value;
    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentTransferDTO>(result);
    Assert.True(result.Id > 0);
  }

  [Fact]
  public async Task Handle_RequestGetAccountInfoTrue_ReturnsExistentTransferDTOWithAccountInfo()
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
  public async Task Handle_RequestGetDestinationAccountInfoTrue_ReturnsExistentTransferDTOWithDestinationAccountInfo()
  {
    request.RetrieveDestinationAccountInfo = true;
    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentTransferDTO>(result);
    Assert.Equal(request.TransferId, result.Id);
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
    Assert.Equal(request.TransferId, result.Id);
    Assert.Null(result.Account);
  }

  [Fact]
  public async Task Handle_RequestGetDestinationAccountInfoFalse_ReturnsExistentTransferDTOWithNoDestinationAccountInfo()
  {
    request.RetrieveDestinationAccountInfo = false;
    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentTransferDTO>(result);
    Assert.Equal(request.TransferId, result.Id);
    Assert.Null(result.DestinationAccountInfo);
  }

  [Fact]
  public async Task Handle_RequestGetCustomerInfoTrue_ReturnsExistentTransferDTOWithAccountAndCustomerInfo()
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
  public async Task Handle_RequestGetDestinationAccountCustomerInfoTrue_ReturnsExistentTransferDTOWithDestinationAccountAndItsCustomerInfo()
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
  public async Task Handle_RequestGetCustomerInfoFalseGetAccountInfoTrue_ReturnsExistentTransferDTOWithAccountInfoAndNoItsCustomerInfo()
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
  public async Task Handle_RequestGetDestinationCustomerInfoFalseGetAccountInfoTrue_ReturnsExistentTransferDTOWithDestinationAccountAndNoItsCustomerInfo()
  {
    request.RetrieveDestinationAccountInfo = true;
    request.RetrieveDestinationAccountCustomerInfo = false;
    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentTransferDTO>(result);
    Assert.Equal(request.TransferId, result.Id);
    Assert.NotNull(result.DestinationAccountInfo);
    Assert.IsType<ExistentAccountDTO>(result.DestinationAccountInfo);
    Assert.Null(result.DestinationAccountInfo!.Customer);
  }
}

