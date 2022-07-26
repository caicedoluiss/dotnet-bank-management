using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BankManagement.Application.UnitTests;

public class DeleteAccountRequestCommandHandlerTests
{
  private readonly DeleteAccountRequestCommandHandler handler;

  private DeleteAccountRequestCommand request;

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
  [InlineData(-2)]
  [InlineData(-1)]
  [InlineData(0)]
  public void Handle_IdLessThan1_ThrowsArgumentExcetion(int accountId)
  {
    request.AccountId = accountId;

    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Fact]
  public void Handle_NonExistentId_ThrowsArgumentExcetion()
  {
    request.AccountId = int.MaxValue;

    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Fact]
  public async Task Handle_ExistentIdGetCustomerInfoFalse_ReturnsExistentCustomerDTO()
  {
    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentAccountDTO>(result);
    Assert.Equal(request.AccountId, result.Id);
    Assert.Null(result.Customer);
  }

  [Fact]
  public async Task Handle_ExistentIdGetCustomerInfoTrue_ReturnsExistentCustomerDTO()
  {
    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentAccountDTO>(result);
    Assert.Equal(request.AccountId, result.Id);
    Assert.NotNull(result.Customer);
  }
}
