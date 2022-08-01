using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BankManagement.Application.UnitTests;

public class AddTransferRequestCommandHandlerTests
{
  private readonly AddTransferRequestCommandHandler handler;

  private readonly AddTransferRequestCommand request;
  private readonly UpdatingTransferDTO transferDTO;

  public AddTransferRequestCommandHandlerTests()
  {
    var serviceProvider = TestsConfiguration.ServiceProvider;
    handler = serviceProvider.GetRequiredService<AddTransferRequestCommandHandler>();

    transferDTO = new()
    {
      AccountId = 1,
      Date = "2022-07-29 00:00:00.000-5:00",
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
  public void Handle_RequestNullTransferInfo_ThrowsArgumentException()
  {
    request.TrasnferInfo = null;
    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Theory]
  [InlineData(-1)]
  [InlineData(0)]
  public void Handle_TransferAccountIdLessThan1_ThrowsArgumentException(int id)
  {
    transferDTO.AccountId = id;
    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Fact]
  public void Handle_TransferAccountIdNonExistentId_ThrowsArgumentException()
  {
    transferDTO.AccountId = int.MaxValue;
    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Theory]
  [InlineData(-1)]
  [InlineData(0)]
  public void Handle_TransferDestinationAccountIdLessThan1_ThrowsArgumentException(int id)
  {
    transferDTO.DestinationAccountId = id;
    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Fact]
  public void Handle_TransferDestinationAccountIdNonExistentId_ThrowsArgumentException()
  {
    transferDTO.DestinationAccountId = int.MaxValue;
    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Theory]
  [InlineData(null)]
  [InlineData("")]
  [InlineData("qwerty")]
  [InlineData("07-28-2022")]
  public void Handle_TransferDateInvalid_ThrowsArgumentException(string? date)
  {
    transferDTO.Date = date;
    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Theory]
  [InlineData("2021-05-15 00:00:00.000Z")]
  [InlineData("2000-11-01 10:00:00.000+2:00")]
  [InlineData("1998-08-25 18:00:00.000-5:00")]
  public async Task Handle_TransferDateValid_ReturnsExistentTransferDTO(string? date)
  {
    transferDTO.Date = date;
    var result = await handler.Handle(request, default);

    Assert.IsType<int>(result);
    Assert.NotEqual(default, result);
    Assert.True(result > 0);
  }

  [Fact]
  public void Handle_TransferValueEquals0_ThrowsArgumentException()
  {
    transferDTO.Value = 0;
    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Theory]
  [InlineData(-1000)]
  [InlineData(1000)]
  [InlineData(55.09)]
  [InlineData(-895.14)]
  public async Task Handle_TransferValueValueValid_ReturnsExistentTransferDTO(decimal value)
  {
    transferDTO.Value = value;
    int result = await handler.Handle(request, default);

    Assert.IsType<int>(result);
    Assert.NotEqual(default, result);
    Assert.True(result > 0);
  }
}

