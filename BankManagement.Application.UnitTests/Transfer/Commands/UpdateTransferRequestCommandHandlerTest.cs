using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BankManagement.Application.UnitTests;

public class UpdateTransferRequestCommandHandlerTest
{
  private readonly UpdateTransferRequestCommandHandler handler;

  private UpdateTransferRequestCommand request;
  private UpdatingTransferDTO transactionDTO;

  public UpdateTransferRequestCommandHandlerTest()
  {
    var serviceProvider = TestsConfiguration.ServiceProvider;
    handler = serviceProvider.GetRequiredService<UpdateTransferRequestCommandHandler>();

    transactionDTO = new()
    {
      AccountId = 1,
      Date = DateTime.UtcNow.ToLongDateString(),
      Value = -50.76M,
      Balance = 1550.67M,
      DestinationAccountId = 2
    };

    request = new()
    {
      TransferId = 1,
      TransanctionInfo = transactionDTO,
      RetrieveAccountInfo = false,
      RetrieveCustomerInfo = false,
      RetrieveDestinationAccountInfo = false,
      RetrieveDestinationAccountCustomerInfo = false
    };
  }

  [Fact]
  public void Handle_InvalidNullTransferInfo_ThrowsArgumentException()
  {
    request.TransanctionInfo = null;
    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Theory]
  [InlineData(-1)]
  [InlineData(0)]
  public void Handle_InvalidRequestTransferIdLessThan1_ThrowsArgumentException(int transactionId)
  {
    request.TransferId = transactionId;
    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Fact]
  public async Task Handle_InvalidRequestTransferIdNonExistentId_ReturnsNull()
  {
    request.TransferId = int.MaxValue;
    var result = await handler.Handle(request, default);

    Assert.Null(result);
  }

  [Fact]
  public async Task Handle_InvalidTransferIdExistentId_ReturnsExstentTransferDTO()
  {
    request.TransferId = 1;
    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentTransferDTO>(result);
    Assert.True(result.Id > 0);
  }

  [Theory]
  [InlineData(-1)]
  [InlineData(0)]
  public void Handle_InvalidTransferAccountIdLessThan1_ThrowsArgumentException(int customerId)
  {
    transactionDTO.AccountId = customerId;
    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Fact]
  public void Handle_InvalidTransferAccountIdNonExistentId_ThrowsArgumentException()
  {
    transactionDTO.AccountId = int.MaxValue;
    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Theory]
  [InlineData(null)]
  [InlineData("")]
  [InlineData("fsdf42")]
  public void Handle_InvalidDate_ThrowsArgumentException(string? date)
  {
    transactionDTO.Date = date;
    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  //TODO: Setup AddTransfer valid date test case with formatted date.
  [Theory]
  [InlineData(null)]
  public async Task Handle_ValidDate_ReturnsExistentTransferDTO(string? date)
  {
    transactionDTO.Date = date;
    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentTransferDTO>(result);
    Assert.True(result.Id > 0);
  }

  [Theory]
  [InlineData(-1000)]
  [InlineData(0)]
  [InlineData(1000)]
  [InlineData(55.09)]
  [InlineData(-895.14)]
  public async Task Handle_ValidValue_ReturnsExistentTransferDTO(decimal value)
  {
    transactionDTO.Value = value;
    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentTransferDTO>(result);
    Assert.True(result.Id > 0);
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

