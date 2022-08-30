using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BankManagement.Application.UnitTests;

public class CreateTransferRequestCommandHandlerTests
{
  private readonly CreateTransferRequestCommandHandler handler;

  private readonly NewTransferDTO newTransferDTO;
  private readonly CreateTransferRequestCommand request;

  public CreateTransferRequestCommandHandlerTests()
  {
    var serviceProvider = TestsConfiguration.ServiceProvider;
    handler = serviceProvider.GetRequiredService<CreateTransferRequestCommandHandler>();

    newTransferDTO = new()
    {
      AccountId = 1,
      Value = 1000M,
      DestinationAccountId = 2
    };

    request = new()
    {
      TrasnferInfo = newTransferDTO
    };
  }


  [Fact]
  public async void Handle_RequestNullTransferInfo_ThrowsArgumentException()
  {
    request.TrasnferInfo = null;
    var action = () => handler.Handle(request, default);

    await Assert.ThrowsAsync<ArgumentException>(action);
  }

  [Theory]
  [InlineData(-1)]
  [InlineData(0)]
  public async void Handle_TransferAccountIdLessThan1_ThrowsArgumentException(int id)
  {
    newTransferDTO.AccountId = id;
    var action = () => handler.Handle(request, default);

    await Assert.ThrowsAsync<ArgumentException>(action);
  }

  [Fact]
  public async void Handle_TransferAccountIdNonExistent_ThrowsArgumentException()
  {
    newTransferDTO.AccountId = int.MaxValue;
    var action = () => handler.Handle(request, default);

    await Assert.ThrowsAsync<ArgumentException>(action);
  }

  [Theory]
  [InlineData(-1)]
  [InlineData(0)]
  public async void Handle_TransferDestinationAccountIdLessThan1_ThrowsArgumentException(int id)
  {
    newTransferDTO.DestinationAccountId = id;
    var action = () => handler.Handle(request, default);

    await Assert.ThrowsAsync<ArgumentException>(action);
  }

  [Fact]
  public async void Handle_TransferDestinationAccountIdNonExistent_ThrowsArgumentException()
  {
    newTransferDTO.DestinationAccountId = int.MaxValue;
    var action = () => handler.Handle(request, default);

    await Assert.ThrowsAsync<ArgumentException>(action);
  }

  [Fact]
  public async void Handle_TransferValueEquals0_ThrowsArgumentException()
  {
    newTransferDTO.Value = 0;
    var action = () => handler.Handle(request, default);

    await Assert.ThrowsAsync<ArgumentException>(action);
  }

  [Theory]
  [InlineData(0)]
  [InlineData(-1)]
  public async void Handle_TransferValueLessOrEqualsThan0_ThrowsArgumentException(decimal value)
  {
    newTransferDTO.Value = value;
    var action = () => handler.Handle(request, default);

    await Assert.ThrowsAsync<ArgumentException>(action);
  }

  [Theory]
  [InlineData(0.1)]
  [InlineData(1000)]
  [InlineData(55.09)]
  [InlineData(895.14)]
  public async Task Handle_TransferValueValid_ReturnsValidId(decimal value)
  {
    newTransferDTO.Value = value;
    int result = await handler.Handle(request, default);

    Assert.IsType<int>(result);
    Assert.NotEqual(default, result);
    Assert.True(result > 0);
  }
}


