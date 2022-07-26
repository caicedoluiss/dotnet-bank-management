using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BankManagement.Application.UnitTests;

public class CreateAccountRequestCommandHandlerTests
{
  private readonly CreateAccountRequestCommandHandler handler;

  private NewAccountDTO newAccountDTO;

  public CreateAccountRequestCommandHandlerTests()
  {
    var serviceProvider = TestsConfiguration.ServiceProvider;
    handler = serviceProvider.GetRequiredService<CreateAccountRequestCommandHandler>();

    newAccountDTO = new()
    {
      Number = "123456",
      Balance = 1550.67M,
      CustomerId = 1,
      Type = "Savings",
      State = true
    };
  }

  [Fact]
  public void Handle_InvalidNullAccountInfo_ThrowsArgumentException()
  {
    var request = new CreateAccountRequestCommand
    {
      AccountInfo = null
    };

    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Fact]
  public void Handle_InvalidEmptyAccountNumber_ThrowsArgumentException()
  {
    newAccountDTO.Number = string.Empty;
    var request = new CreateAccountRequestCommand
    {
      AccountInfo = newAccountDTO
    };

    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Theory]
  [InlineData(-1)]
  [InlineData(0)]
  public void Handle_InvalidCustomerIdLessThan1_ThrowsArgumentException(int customerId)
  {
    newAccountDTO.CustomerId = customerId;
    var request = new CreateAccountRequestCommand
    {
      AccountInfo = newAccountDTO
    };

    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Theory]
  [InlineData("")]
  [InlineData("NonExistentValue")]
  [InlineData("-1")]
  public void Handle_InvalidAccountType_ThrowsArgumentException(string type)
  {
    newAccountDTO.Type = type;
    var request = new CreateAccountRequestCommand
    {
      AccountInfo = newAccountDTO
    };

    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Theory]
  [InlineData(null)]
  [InlineData("Savings")]
  [InlineData("Current")]
  [InlineData("0")]
  [InlineData("1")]
  public async Task Handle_ValidAccountType_ReturnsValidId(string? type)
  {
    newAccountDTO.Type = type;
    var request = new CreateAccountRequestCommand
    {
      AccountInfo = newAccountDTO
    };

    int result = await handler.Handle(request, default);

    Assert.IsType<int>(result);
    Assert.NotEqual(default, result);
    Assert.True(result > 0);
  }
}

