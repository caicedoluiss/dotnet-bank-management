using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BankManagement.Application.UnitTests;

public class AddTransactionRequestCommandHandlerTests
{
  private readonly AddTransactionRequestCommandHandler handler;

  private AddTransactionRequestCommand request;
  private UpdatingTransactionDTO transactionDTO;

  public AddTransactionRequestCommandHandlerTests()
  {
    var serviceProvider = TestsConfiguration.ServiceProvider;
    handler = serviceProvider.GetRequiredService<AddTransactionRequestCommandHandler>();

    transactionDTO = new()
    {
      AccountId = 1,
      Date = DateTime.UtcNow.ToLongDateString(),
      Value = -50.76M,
      Balance = 1550.67M,
    };

    request = new()
    {
      TransctionInfo = transactionDTO
    };
  }

  [Fact]
  public void Handle_InvalidNullTransactionInfo_ThrowsArgumentException()
  {
    request.TransctionInfo = null;
    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Fact]
  public void Handle_InvalidEmptyDate_ThrowsArgumentException()
  {
    transactionDTO.Date = string.Empty;
    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Theory]
  [InlineData(-1)]
  [InlineData(0)]
  public void Handle_InvalidTransactionAccountIdLessThan1_ThrowsArgumentException(int customerId)
  {
    transactionDTO.AccountId = customerId;
    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Fact]
  public async Task Handle_InvalidTransactionAccountIdNonExistentId_ReturnsNull()
  {
    transactionDTO.AccountId = int.MaxValue;
    var result = await handler.Handle(request, default);

    Assert.Null(result);
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

  //TODO: Setup AddTransaction valid date test case with formatted date.
  [Theory]
  [InlineData(null)]
  public async Task Handle_ValidDate_ReturnsValidId(string? date)
  {
    transactionDTO.Date = date;
    var result = await handler.Handle(request, default);

    Assert.IsType<int>(result);
    Assert.NotEqual(default, result);
    Assert.True(result > 0);
  }

  [Theory]
  [InlineData(-1000)]
  [InlineData(0)]
  [InlineData(1000)]
  [InlineData(55.09)]
  [InlineData(-895.14)]
  public async Task Handle_ValidValue_ReturnsValidId(decimal value)
  {
    transactionDTO.Value = value;
    int result = await handler.Handle(request, default);

    Assert.IsType<int>(result);
    Assert.NotEqual(default, result);
    Assert.True(result > 0);
  }
}

