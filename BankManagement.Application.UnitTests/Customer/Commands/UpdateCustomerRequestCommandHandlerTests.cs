using System;
using System.Threading.Tasks;
using BankManagement.Domain;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BankManagement.Application.UnitTests;

public class UpdateCustomerRequestCommandHandlerTests
{
  private readonly UpdateCustomerRequestCommandHandler handler;

  private int customerId;
  private NewCustomerDTO newCustomerDTO;

  public UpdateCustomerRequestCommandHandlerTests()
  {
    var serviceProvider = TestsConfiguration.ServiceProvider;
    handler = serviceProvider.GetRequiredService<UpdateCustomerRequestCommandHandler>();

    customerId = 1;
    newCustomerDTO = new()
    {
      IdNumber = "123456",
      Name = "John Doe",
      Gender = "Male",
      Age = 20,
      Email = "jdoe@email.com",
      PhoneNumber = "+57 1234567890",
      State = true
    };
  }

  [Fact]
  public void Handle_InvalidNullCustomerInfo_ThrowsArgumentException()
  {
    var request = new UpdateCustomerRequestCommand
    {
      CustomerId = customerId,
      CustomerInfo = null
    };

    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Fact]
  public void Handle_InvalidEmptyCustomerIdNumber_ThrowsArgumentException()
  {
    newCustomerDTO.IdNumber = string.Empty;
    var request = new UpdateCustomerRequestCommand
    {
      CustomerId = customerId,
      CustomerInfo = newCustomerDTO
    };

    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Fact]
  public void Handle_InvalidEmptyCustomerName_ThrowsArgumentException()
  {
    newCustomerDTO.Name = string.Empty;
    var request = new UpdateCustomerRequestCommand
    {
      CustomerId = customerId,
      CustomerInfo = newCustomerDTO
    };

    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Theory]
  [InlineData("")]
  [InlineData("NonExistentValue")]
  [InlineData("-1")]
  public void Handle_InvalidCustomerGender_ThrowsArgumentException(string? gender)
  {
    newCustomerDTO.Gender = gender;
    var request = new UpdateCustomerRequestCommand
    {
      CustomerId = customerId,
      CustomerInfo = newCustomerDTO
    };

    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Theory]
  [InlineData(null)]
  [InlineData("None")]
  [InlineData("Female")]
  [InlineData("Male")]
  [InlineData("Other")]
  [InlineData("0")]
  [InlineData("1")]
  [InlineData("2")]
  [InlineData("3")]
  public async Task Handle_ValidCustomerGender_ReturnsValidExistentCustomerDTO(string? gender)
  {
    newCustomerDTO.Gender = gender;
    var request = new UpdateCustomerRequestCommand
    {
      CustomerId = customerId,
      CustomerInfo = newCustomerDTO
    };

    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentCustomerDTO>(result);
    Assert.True(result.Id > 0);
  }


  [Theory]
  [InlineData(-1)]
  [InlineData(0)]
  [InlineData(17)]
  public void Handle_InvalidCustomerAgeLessThan18_ThrowsArgumentException(int age)
  {
    newCustomerDTO.Age = age;
    var request = new UpdateCustomerRequestCommand
    {
      CustomerId = customerId,
      CustomerInfo = newCustomerDTO
    };

    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Theory]
  [InlineData(18)]
  [InlineData(50)]
  public async Task Handle_ValidCustomerAgeGreaterEqualsThan18_ReturnsValidId(int age)
  {
    newCustomerDTO.Age = age;
    var request = new UpdateCustomerRequestCommand
    {
      CustomerId = customerId,
      CustomerInfo = newCustomerDTO
    };

    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentCustomerDTO>(result);
    Assert.True(result.Id > 0);
  }

  [Theory]
  [InlineData("")]
  [InlineData("asdass")]
  [InlineData("jdoe@em")]
  public void Handle_InvalidCustomerEmail_ThrowsArgumentException(string email)
  {
    newCustomerDTO.Email = email;
    var request = new UpdateCustomerRequestCommand
    {
      CustomerId = customerId,
      CustomerInfo = newCustomerDTO
    };

    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Theory]
  [InlineData("")]
  [InlineData("65665")]
  [InlineData("+1 123456798")]
  public void Handle_InvalidCustomerPhoneNumber_ThrowsArgumentException(string phoneNumber)
  {
    newCustomerDTO.PhoneNumber = phoneNumber;
    var request = new UpdateCustomerRequestCommand
    {
      CustomerId = customerId,
      CustomerInfo = newCustomerDTO
    };

    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Fact]
  public async Task Handle_ValidRequestExistentCustomerId_ReturnsValidId()
  {
    var request = new UpdateCustomerRequestCommand
    {
      CustomerId = customerId,
      CustomerInfo = newCustomerDTO
    };

    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentCustomerDTO>(result);
    Assert.True(result.Id > 0);
  }

  [Theory]
  [InlineData(-1)]
  [InlineData(0)]
  public void Handle_InvalidCustomerId_ThrowsArgumentException(int customerId)
  {
    var request = new UpdateCustomerRequestCommand
    {
      CustomerId = customerId,
      CustomerInfo = newCustomerDTO
    };

    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Fact]
  public void Handle_InvalidCustomerIdNonExistent_ThrowsArgumentException()
  {
    var request = new UpdateCustomerRequestCommand
    {
      CustomerId = int.MaxValue,
      CustomerInfo = newCustomerDTO
    };

    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }
}
