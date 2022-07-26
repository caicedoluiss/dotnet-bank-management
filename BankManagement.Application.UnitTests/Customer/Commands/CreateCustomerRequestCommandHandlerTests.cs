using System;
using System.Threading.Tasks;
using BankManagement.Domain;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BankManagement.Application.UnitTests;

public class CreateCustomerRequestCommandHandlerTests
{
  private readonly CreateCustomerRequestCommandHandler handler;

  private NewCustomerDTO newCustomerDTO;

  public CreateCustomerRequestCommandHandlerTests()
  {
    var serviceProvider = TestsConfiguration.ServiceProvider;
    handler = serviceProvider.GetRequiredService<CreateCustomerRequestCommandHandler>();

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
    var request = new CreateCustomerRequestCommand
    {
      CustomerInfo = null
    };

    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Fact]
  public void Handle_InvalidEmptyCustomerIdNumber_ThrowsArgumentException()
  {
    newCustomerDTO.IdNumber = string.Empty;
    var request = new CreateCustomerRequestCommand
    {
      CustomerInfo = newCustomerDTO
    };

    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Fact]
  public void Handle_InvalidEmptyCustomerName_ThrowsArgumentException()
  {
    newCustomerDTO.Name = string.Empty;
    var request = new CreateCustomerRequestCommand
    {
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
    var request = new CreateCustomerRequestCommand
    {
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
  public async Task Handle_ValidCustomerGender_ReturnsValidId(string? gender)
  {
    newCustomerDTO.Gender = gender;
    var request = new CreateCustomerRequestCommand
    {
      CustomerInfo = newCustomerDTO
    };

    int result = await handler.Handle(request, default);

    Assert.IsType<int>(result);
    Assert.NotEqual(default, result);
    Assert.True(result > 0);
  }


  [Theory]
  [InlineData(-1)]
  [InlineData(0)]
  [InlineData(17)]
  public void Handle_InvalidCustomerAgeLessThan18_ThrowsArgumentException(int age)
  {
    newCustomerDTO.Age = age;
    var request = new CreateCustomerRequestCommand
    {
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
    var request = new CreateCustomerRequestCommand
    {
      CustomerInfo = newCustomerDTO
    };

    int result = await handler.Handle(request, default);

    Assert.IsType<int>(result);
    Assert.NotEqual(default, result);
    Assert.True(result > 0);
  }

  [Theory]
  [InlineData("")]
  [InlineData("asdass")]
  [InlineData("jdoe@em")]
  public void Handle_InvalidCustomerEmail_ThrowsArgumentException(string email)
  {
    newCustomerDTO.Email = email;
    var request = new CreateCustomerRequestCommand
    {
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
    var request = new CreateCustomerRequestCommand
    {
      CustomerInfo = newCustomerDTO
    };

    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Fact]
  public async Task Handle_ValidRequest_ReturnsValidId()
  {
    var request = new CreateCustomerRequestCommand
    {
      CustomerInfo = newCustomerDTO
    };

    int result = await handler.Handle(request, default);

    Assert.IsType<int>(result);
    Assert.NotEqual(default, result);
    Assert.True(result > 0);
  }
}
