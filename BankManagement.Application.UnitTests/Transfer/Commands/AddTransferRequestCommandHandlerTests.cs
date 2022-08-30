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
      Value = 50.76M,
      Balance = 1550.67M,
      DestinationAccountId = 2
    };

    request = new()
    {
      TransferInfo = transferDTO
    };
  }


  [Fact]
  public async void Handle_RequestNullTransferInfo_ThrowsArgumentException()
  {
    request.TransferInfo = null;
    var action = () => handler.Handle(request, default);

    await Assert.ThrowsAsync<ArgumentException>(action);
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
  public async void Handle_TransferAccountIdNonExistentId_ThrowsArgumentException()
  {
    transferDTO.AccountId = int.MaxValue;
    var action = () => handler.Handle(request, default);

    await Assert.ThrowsAsync<ArgumentException>(action);
  }

  [Theory]
  [InlineData(-1)]
  [InlineData(0)]
  public async void Handle_TransferDestinationAccountIdLessThan1_ThrowsArgumentException(int id)
  {
    transferDTO.DestinationAccountId = id;
    var action = () => handler.Handle(request, default);

    await Assert.ThrowsAsync<ArgumentException>(action);
  }

  [Fact]
  public async void Handle_TransferDestinationAccountIdNonExistentId_ThrowsArgumentException()
  {
    transferDTO.DestinationAccountId = int.MaxValue;
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

    Assert.IsType<int>(result);
    Assert.NotEqual(default, result);
    Assert.True(result > 0);
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
  public async Task Handle_TransferValueValueValid_ReturnsExistentTransferDTO(decimal value)
  {
    transferDTO.Value = value;
    int result = await handler.Handle(request, default);

    Assert.IsType<int>(result);
    Assert.NotEqual(default, result);
    Assert.True(result > 0);
  }
}

