using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BankManagement.Application.UnitTests;

public class DeleteAccountRequestCommandHandlerTests
{
  private readonly DeleteAccountRequestCommandHandler handler;

  private readonly DeleteAccountRequestCommand request;

  public DeleteAccountRequestCommandHandlerTests()
  {
    var serviceProvider = TestsConfiguration.ServiceProvider;

    handler = serviceProvider.GetRequiredService<DeleteAccountRequestCommandHandler>();

    request = new()
    {
      AccountId = 1,
      RetrieveCustomerInfo = false
    };
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
  public async Task Handle_RequestGetCustomerInfoFalse_ReturnsExistentAccountDTOWithCustomerInfo()
  {
    request.RetrieveCustomerInfo = false;

    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentAccountDTO>(result);
    Assert.Equal(request.AccountId, result.Id);
    Assert.Null(result.Customer);
  }
}
