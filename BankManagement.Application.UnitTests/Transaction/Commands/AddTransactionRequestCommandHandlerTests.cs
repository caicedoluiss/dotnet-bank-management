using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BankManagement.Application.UnitTests;

public class AddTransactionRequestCommandHandlerTests
{
  private readonly AddTransactionRequestCommandHandler handler;

  private readonly AddTransactionRequestCommand request;
  private readonly UpdatingTransactionDTO transactionDTO;

  public AddTransactionRequestCommandHandlerTests()
  {
    var serviceProvider = TestsConfiguration.ServiceProvider;
    handler = serviceProvider.GetRequiredService<AddTransactionRequestCommandHandler>();

    transactionDTO = new()
    {
      AccountId = 1,
      Date = "2022-07-29 00:00:00.000-5:00",
      Value = -50.76M,
      Balance = 1550.67M,
    };

    request = new()
    {
      TransctionInfo = transactionDTO
    };
  }


  [Fact]
  public void Handle_RequestNullTransactionInfo_ThrowsArgumentException()
  {
    request.TransctionInfo = null;
    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
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
  public async Task Handle_TransactionAccountIdNonExistent_ThrowsArgumentException()
  {
    transactionDTO.AccountId = int.MaxValue;
    var result = await handler.Handle(request, default);

    Assert.Null(result);
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

  [Theory]
  [InlineData("2021-05-15 00:00:00.000Z")]
  [InlineData("2000-11-01 10:00:00.000+2:00")]
  [InlineData("1998-08-25 18:00:00.000-5:00")]
  public async Task Handle_TransactionDateValid_ReturnsValidId(string? date)
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
  public async Task Handle_TransactionValueValid_ReturnsValidId(decimal value)
  {
    transactionDTO.Value = value;
    int result = await handler.Handle(request, default);

    Assert.IsType<int>(result);
    Assert.NotEqual(default, result);
    Assert.True(result > 0);
  }
}

