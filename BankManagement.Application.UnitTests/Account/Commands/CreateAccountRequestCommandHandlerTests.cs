using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BankManagement.Application.UnitTests;

public class CreateAccountRequestCommandHandlerTests
{
  private readonly CreateAccountRequestCommandHandler handler;

  private readonly CreateAccountRequestCommand request;
  private readonly NewAccountDTO newAccountDTO;

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

    request = new()
    {
      AccountInfo = newAccountDTO
    };
  }

  [Fact]
  public async void Handle_RequestNullAccountInfo_ThrowsArgumentException()
  {
    request.AccountInfo = null;
    var action = () => handler.Handle(request, default);

    await Assert.ThrowsAsync<ArgumentException>(action);
  }

  [Theory]
  [InlineData(-1)]
  [InlineData(0)]
  public async void Handle_CustomerIdLessThan1_ThrowsArgumentException(int id)
  {
    newAccountDTO.CustomerId = id;
    var action = () => handler.Handle(request, default);

    await Assert.ThrowsAsync<ArgumentException>(action);
  }

  [Fact]
  public async void Handle_CustomerIdNonExistent_ThrowsArgumentException()
  {
    newAccountDTO.CustomerId = int.MaxValue;
    var action = () => handler.Handle(request, default);

    await Assert.ThrowsAsync<ArgumentException>(action);
  }

  [Fact]
  public async void Handle_AccountNumberEmpty_ThrowsArgumentException()
  {
    newAccountDTO.Number = string.Empty;
    var action = () => handler.Handle(request, default);

    await Assert.ThrowsAsync<ArgumentException>(action);
  }

  [Theory]
  [InlineData("")]
  [InlineData("NonExistentValue")]
  [InlineData("-1")]
  public async void Handle_AccountTypeInvalid_ThrowsArgumentException(string type)
  {
    newAccountDTO.Type = type;
    var action = () => handler.Handle(request, default);

    await Assert.ThrowsAsync<ArgumentException>(action);
  }

  [Theory]
  [InlineData(null)]
  [InlineData("Savings")]
  [InlineData("Current")]
  [InlineData("0")]
  [InlineData("1")]
  public async Task Handle_AccountTypeValid_ReturnsValidId(string? type)
  {
    newAccountDTO.Type = type;

    var result = await handler.Handle(request, default);

    Assert.IsType<int>(result);
    Assert.NotEqual(result, default);
    Assert.True(result > 0);
  }
}

