using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BankManagement.Application.UnitTests;

public class UpdateAccountRequestCommandHandlerTests
{
  private readonly UpdateAccountRequestCommandHandler handler;

  private int accountId;
  private UpdateAccountRequestCommand request;
  private NewAccountDTO newAccountDTO;

  public UpdateAccountRequestCommandHandlerTests()
  {
    var serviceProvider = TestsConfiguration.ServiceProvider;
    handler = serviceProvider.GetRequiredService<UpdateAccountRequestCommandHandler>();

    accountId = 1;

    newAccountDTO = new()
    {
      Number = "123456",
      Balance = 1550.67M,
      CustomerId = 1,
      Type = "Savings",
      State = true,
    };

    request = new UpdateAccountRequestCommand
    {
      AccountId = accountId,
      AccountInfo = newAccountDTO,
      RetrieveCustomerInfo = false
    };
  }

  [Fact]
  public void Handle_InvalidNullAccountInfo_ThrowsArgumentException()
  {
    request.AccountInfo = null;

    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Fact]
  public void Handle_InvalidEmptyAccountNumber_ThrowsArgumentException()
  {
    newAccountDTO.Number = string.Empty;

    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Theory]
  [InlineData(-1)]
  [InlineData(0)]
  public void Handle_InvalidCustomerIdLessThan1_ThrowsArgumentException(int customerId)
  {
    newAccountDTO.CustomerId = customerId;

    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Fact]
  public async Task Handle_InvalidCustomerIdNonExistentId_ReturnsNull()
  {
    request.AccountId = int.MaxValue;

    var result = await handler.Handle(request, default);

    Assert.Null(result);
  }

  [Theory]
  [InlineData("")]
  [InlineData("NonExistentValue")]
  [InlineData("-1")]
  public void Handle_InvalidAccountType_ThrowsArgumentException(string type)
  {
    newAccountDTO.Type = type;

    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Theory]
  [InlineData(null)]
  [InlineData("Savings")]
  [InlineData("Current")]
  [InlineData("0")]
  [InlineData("1")]
  public async Task Handle_ValidAccountType_ReturnsValidId(string type)
  {
    newAccountDTO.Type = type;

    var result = await handler.Handle(request, default);

    Assert.IsType<ExistentAccountDTO>(result);
    Assert.NotNull(result);
    Assert.True(result.Id > 0);
  }

  [Fact]
  public async Task Handle_ExistentIdGetCustomerInfoTrue_ReturnsExistentAccountDTOWithCustomerInfo()
  {
    request.RetrieveCustomerInfo = true;

    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentAccountDTO>(result);
    Assert.Equal(request.AccountId, result.Id);
    Assert.NotNull(result.Customer);
  }

  [Fact]
  public async Task Handle_ExistentIdGetCustomerInfoFalse_ReturnsExistentAccountDTOWithCustomerInfo()
  {
    request.RetrieveCustomerInfo = false;

    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentAccountDTO>(result);
    Assert.Equal(request.AccountId, result.Id);
    Assert.Null(result.Customer);
  }
}
