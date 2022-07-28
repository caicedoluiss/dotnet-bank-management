using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BankManagement.Application.UnitTests;

public class AddTransferRequestCommandHandlerTests
{
  private readonly AddTransferRequestCommandHandler handler;

  private AddTransferRequestCommand request;
  private UpdatingTransferDTO transferDTO;

  public AddTransferRequestCommandHandlerTests()
  {
    var serviceProvider = TestsConfiguration.ServiceProvider;
    handler = serviceProvider.GetRequiredService<AddTransferRequestCommandHandler>();

    transferDTO = new()
    {
      AccountId = 1,
      Date = DateTime.UtcNow.ToLongDateString(),
      Value = -50.76M,
      Balance = 1550.67M,
      DestinationAccountId = 2
    };

    request = new()
    {
      TrasnferInfo = transferDTO
    };
  }

  [Fact]
  public void Handle_InvalidNullTransferInfo_ThrowsArgumentException()
  {
    request.TrasnferInfo = null;
    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Theory]
  [InlineData(-1)]
  [InlineData(0)]
  public void Handle_InvalidTransferAccountIdLessThan1_ThrowsArgumentException(int accountId)
  {
    transferDTO.AccountId = accountId;
    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Fact]
  public void Handle_InvalidTransferAccountIdNonExistentId_ThrowsArgumentException()
  {
    transferDTO.AccountId = int.MaxValue;
    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Theory]
  [InlineData(-1)]
  [InlineData(0)]
  public void Handle_InvalidTransferDestinationAccountIdLessThan1_ThrowsArgumentException(int destinationAccountId)
  {
    transferDTO.DestinationAccountId = destinationAccountId;
    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Fact]
  public void Handle_InvalidTransferDestinationAccountIdNonExistentId_ThrowsArgumentException()
  {
    transferDTO.DestinationAccountId = int.MaxValue;
    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Theory]
  [InlineData(null)]
  [InlineData("")]
  [InlineData("fsdf42")]
  public void Handle_InvalidDate_ThrowsArgumentException(string? date)
  {
    transferDTO.Date = date;
    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  //TODO: Setup AddTransaction valid date test case with formatted date.
  [Theory]
  [InlineData(null)]
  public async Task Handle_ValidDate_ReturnsValidId(string? date)
  {
    transferDTO.Date = date;
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
    transferDTO.Value = value;
    int result = await handler.Handle(request, default);

    Assert.IsType<int>(result);
    Assert.NotEqual(default, result);
    Assert.True(result > 0);
  }
}

