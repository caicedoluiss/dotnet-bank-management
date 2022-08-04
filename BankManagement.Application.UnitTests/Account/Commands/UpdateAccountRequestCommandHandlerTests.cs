using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BankManagement.Application.UnitTests;

public class UpdateAccountRequestCommandHandlerTests
{
  private readonly UpdateAccountRequestCommandHandler handler;

  private readonly UpdateAccountRequestCommand request;
  private readonly NewAccountDTO newAccountDTO;

  public UpdateAccountRequestCommandHandlerTests()
  {
    var serviceProvider = TestsConfiguration.ServiceProvider;
    handler = serviceProvider.GetRequiredService<UpdateAccountRequestCommandHandler>();

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
      AccountId = 1,
      AccountInfo = newAccountDTO,
      RetrieveCustomerInfo = false
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
  public async void Handle_RequestAccountIdLessThan1_ThrowsArgumentException(int id)
  {
    request.AccountId = id;
    var action = () => handler.Handle(request, default);

    await Assert.ThrowsAsync<ArgumentException>(action);
  }

  [Fact]
  public async void Handle_RequestAccountIdNonExistent_ThrowsArgumentException()
  {
    request.AccountId = int.MaxValue;
    var action = () => handler.Handle(request, default);

    await Assert.ThrowsAsync<ArgumentException>(action);
  }

  [Fact]
  public async Task Handle_RequestAccountIdExistent_ReturnsExistentAccountDTO()
  {
    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentAccountDTO>(result);
    Assert.True(result.Id > 0);
    Assert.Null(result.Customer);
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
  public async Task Handle_AccountTypeValid_ReturnsValidId(string type)
  {
    newAccountDTO.Type = type;
    var result = await handler.Handle(request, default);

    Assert.IsType<ExistentAccountDTO>(result);
    Assert.NotNull(result);
    Assert.True(result.Id > 0);
  }

  [Fact]
  public async Task Handle_RequestGetCustomerInfoTrue_ReturnsExistentAccountDTOWithCustomerInfo()
  {
    request.RetrieveCustomerInfo = true;
    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentAccountDTO>(result);
    Assert.Equal(request.AccountId, result.Id);
    Assert.NotNull(result.Customer);
    Assert.IsType<ExistentCustomerDTO>(result.Customer);
  }

  [Fact]
  public async Task Handle_RequestGetCustomerInfoFalse_ReturnsExistentAccountDTOWithNoCustomerInfo()
  {
    request.RetrieveCustomerInfo = false;
    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentAccountDTO>(result);
    Assert.Equal(request.AccountId, result.Id);
    Assert.Null(result.Customer);
  }
}
