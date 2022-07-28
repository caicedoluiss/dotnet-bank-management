using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BankManagement.Application.UnitTests;

public class CreateTransferRequestCommandHandlerTests
{
  private readonly CreateTransferRequestCommandHandler handler;

  private NewTransferDTO newTransferDTO;
  private CreateTransferRequestCommand request;

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
  public void Handle_InvalidNullTransferInfo_ThrowsArgumentException()
  {
    request.TrasnferInfo = null;
    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Theory]
  [InlineData(-1)]
  [InlineData(0)]
  public void Handle_InvalidTransferAccountIdLessThan1_ThrowsArgumentException(int id)
  {
    newTransferDTO.AccountId = id;
    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Fact]
  public void Handle_InvalidTransferAccountIdNonExistent_ThrowsArgumentException()
  {
    newTransferDTO.AccountId = int.MaxValue;
    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Theory]
  [InlineData(-1)]
  [InlineData(0)]
  public void Handle_InvalidDestinationAccountIdLessThan1_ThrowsArgumentException(int id)
  {
    newTransferDTO.DestinationAccountId = id;
    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Fact]
  public void Handle_InvalidTransferDestinationAccountIdNonExistent_ThrowsArgumentException()
  {
    newTransferDTO.DestinationAccountId = int.MaxValue;
    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Theory]
  [InlineData(-1000)]
  [InlineData(0)]
  [InlineData(1000)]
  [InlineData(55.09)]
  [InlineData(-895.14)]
  public async Task Handle_ValidValue_ReturnsValidId(decimal value)
  {
    newTransferDTO.Value = value;
    int result = await handler.Handle(request, default);

    Assert.IsType<int>(result);
    Assert.NotEqual(default, result);
    Assert.True(result > 0);
  }
}


