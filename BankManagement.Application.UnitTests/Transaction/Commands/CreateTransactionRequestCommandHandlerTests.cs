using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BankManagement.Application.UnitTests;

public class CreateTransactionRequestCommandHandlerTests
{
  private readonly CreateTransactionRequestCommandHandler handler;

  private readonly NewTransactionDTO newTransactionDTO;
  private readonly CreateTransactionRequestCommand request;

  public CreateTransactionRequestCommandHandlerTests()
  {
    var serviceProvider = TestsConfiguration.ServiceProvider;
    handler = serviceProvider.GetRequiredService<CreateTransactionRequestCommandHandler>();

    newTransactionDTO = new()
    {
      AccountId = 1,
      Value = 1000M
    };

    request = new()
    {
      TransactionInfo = newTransactionDTO
    };
  }


  [Fact]
  public async void Handle_RequestNullTransactionInfo_ThrowsArgumentException()
  {
    request.TransactionInfo = null;
    var action = () => handler.Handle(request, default);

    await Assert.ThrowsAsync<ArgumentException>(action);
  }

  [Theory]
  [InlineData(-1)]
  [InlineData(0)]
  public async void Handle_TransactionAccountIdLessThan1_ThrowsArgumentException(int id)
  {
    newTransactionDTO.AccountId = id;
    var action = () => handler.Handle(request, default);

    await Assert.ThrowsAsync<ArgumentException>(action);
  }

  [Fact]
  public async void Handle_TransactionAccountIdNonExistent_ThrowsArgumentException()
  {
    newTransactionDTO.AccountId = int.MaxValue;
    var action = () => handler.Handle(request, default);

    await Assert.ThrowsAsync<ArgumentException>(action);
  }

  [Theory]
  [InlineData(-1000)]
  [InlineData(0)]
  [InlineData(1000)]
  [InlineData(55.09)]
  [InlineData(-895.14)]
  public async Task Handle_TransactionValueValid_ReturnsValidId(decimal value)
  {
    newTransactionDTO.Value = value;
    int result = await handler.Handle(request, default);

    Assert.IsType<int>(result);
    Assert.NotEqual(default, result);
    Assert.True(result > 0);
  }
}


