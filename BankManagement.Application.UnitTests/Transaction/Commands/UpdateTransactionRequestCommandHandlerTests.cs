using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BankManagement.Application.UnitTests;

public class UpdateTransactionRequestCommandHandlerTests
{
  private readonly UpdateTransactionRequestCommandHandler handler;

  private readonly UpdateTransactionRequestCommand request;
  private readonly UpdatingTransactionDTO transactionDTO;

  public UpdateTransactionRequestCommandHandlerTests()
  {
    var serviceProvider = TestsConfiguration.ServiceProvider;
    handler = serviceProvider.GetRequiredService<UpdateTransactionRequestCommandHandler>();

    transactionDTO = new()
    {
      AccountId = 1,
      Date = DateTime.UtcNow.ToLongDateString(),
      Value = -50.76M,
      Balance = 1550.67M,
    };

    request = new()
    {
      TransactionId = 1,
      TransanctionInfo = transactionDTO,
      RetrieveAccountInfo = false,
      RetrieveCustomerInfo = false
    };
  }


  [Fact]
  public void Handle_RequestNullTransactionInfo_ThrowsArgumentException()
  {
    request.TransanctionInfo = null;
    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Theory]
  [InlineData(-1)]
  [InlineData(0)]
  public void Handle_RequestTransactionIdLessThan1_ThrowsArgumentException(int id)
  {
    request.TransactionId = id;
    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Fact]
  public void Handle_RequestTransactionIdNonExistent_ThrowsArgumentException()
  {
    request.TransactionId = int.MaxValue;
    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Fact]
  public async Task Handle_RequestTransactionIdExistent_ReturnsExistentTransactionDTO()
  {
    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentTransactionDTO>(result);
    Assert.True(result.Id > 0);
    Assert.Equal(request.TransactionId, result.Id);
    Assert.Null(result.Account);
    Assert.True(result.AccountId > 0);
  }

  [Theory]
  [InlineData(-1)]
  [InlineData(0)]
  public void Handle_TransactionAccountIdLessThan1_ThrowsArgumentException(int id)
  {
    transactionDTO.AccountId = id;
    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Fact]
  public void Handle_TransactionAccountIdNonExistent_ThrowsArgumentException()
  {
    transactionDTO.AccountId = int.MaxValue;
    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Theory]
  [InlineData(null)]
  [InlineData("")]
  [InlineData("qwerty")]
  [InlineData("07-28-2022")]
  public void Handle_TransactionDateInvalid_ThrowsArgumentException(string? date)
  {
    transactionDTO.Date = date;
    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  //TODO: Setup AddTransaction valid date test case with formatted date.
  [Theory]
  [InlineData(null)]
  public async Task Handle_TransactionDateValid_ReturnsExistentTransactionDTO(string? date)
  {
    transactionDTO.Date = date;
    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentTransactionDTO>(result);
    Assert.True(result.Id > 0);
  }

  [Theory]
  [InlineData(-1000)]
  [InlineData(0)]
  [InlineData(1000)]
  [InlineData(55.09)]
  [InlineData(-895.14)]
  public async Task Handle_TransactionValueValid_ReturnsExistentTransactionDTO(decimal value)
  {
    transactionDTO.Value = value;
    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentTransactionDTO>(result);
    Assert.True(result.Id > 0);
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

