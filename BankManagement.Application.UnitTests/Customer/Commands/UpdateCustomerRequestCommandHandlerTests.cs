using System;
using System.Threading.Tasks;
using BankManagement.Domain;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BankManagement.Application.UnitTests;

public class UpdateCustomerRequestCommandHandlerTests
{
  private readonly UpdateCustomerRequestCommandHandler handler;

  private readonly UpdateCustomerRequestCommand request;
  private readonly NewCustomerDTO newCustomerDTO;

  public UpdateCustomerRequestCommandHandlerTests()
  {
    var serviceProvider = TestsConfiguration.ServiceProvider;

    handler = serviceProvider.GetRequiredService<UpdateCustomerRequestCommandHandler>();

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

    request = new()
    {
      CustomerId = 1,
      CustomerInfo = newCustomerDTO
    };
  }

  [Fact]
  public void Handle_RequestNullCustomerInfo_ThrowsArgumentException()
  {
    request.CustomerInfo = null;
    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Theory]
  [InlineData(-1)]
  [InlineData(0)]
  public void Handle_RequestCustomerIdLessThan1_ThrowsArgumentException(int id)
  {
    request.CustomerId = id;
    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Fact]
  public void Handle_RequestCustomerIdNonExistent_ThrowsArgumentException()
  {
    request.CustomerId = int.MaxValue;
    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Fact]
  public async Task Handle_RequestCustomerIdExistent_ReturnsValidId()
  {
    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentCustomerDTO>(result);
    Assert.True(result.Id > 0);
  }

  [Fact]
  public void Handle_CustomerIdNumberEmpty_ThrowsArgumentException()
  {
    newCustomerDTO.IdNumber = string.Empty;
    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Fact]
  public void Handle_InvalidEmptyCustomerName_ThrowsArgumentException()
  {
    newCustomerDTO.Name = string.Empty;
    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Theory]
  [InlineData("")]
  [InlineData("NonExistentValue")]
  [InlineData("-1")]
  public void Handle_CustomerGenderInvalid_ThrowsArgumentException(string? gender)
  {
    newCustomerDTO.Gender = gender;
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
  public async Task Handle_CustomerGenderValid_ReturnsValidExistentCustomerDTO(string? gender)
  {
    newCustomerDTO.Gender = gender;
    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentCustomerDTO>(result);
    Assert.True(result.Id > 0);
  }


  [Theory]
  [InlineData(-1)]
  [InlineData(0)]
  [InlineData(17)]
  public void Handle_CustomerAgeLessThan18_ThrowsArgumentException(int age)
  {
    newCustomerDTO.Age = age;
    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Theory]
  [InlineData(18)]
  [InlineData(50)]
  public async Task Handle_CustomerAgeGreaterEqualsThan18_ReturnsValidId(int age)
  {
    newCustomerDTO.Age = age;
    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentCustomerDTO>(result);
    Assert.True(result.Id > 0);
  }

  [Theory]
  [InlineData("")]
  [InlineData("qwerty")]
  [InlineData("jdoe@em")]
  public void Handle_CustomerEmailInvalid_ThrowsArgumentException(string email)
  {
    newCustomerDTO.Email = email;
    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Theory]
  [InlineData("jdoe@email.com")]
  [InlineData("jdoe2@anotheremail.com.co")]
  public async Task Handle_CustomerEmailValid_ReturnsExistentCustomerDTO(string email)
  {
    newCustomerDTO.Email = email;
    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentCustomerDTO>(result);
    Assert.True(result.Id > 0);
  }

  [Theory]
  [InlineData("")]
  [InlineData("65665")]
  [InlineData("+1 123456798")]
  public void Handle_CustomerPhoneNumberInvalid_ThrowsArgumentException(string phoneNumber)
  {
    newCustomerDTO.PhoneNumber = phoneNumber;
    Action action = () => handler.Handle(request, default);

    Assert.Throws<ArgumentException>(action);
  }

  [Theory]
  [InlineData("+57 1234567890")]
  [InlineData("+1 0987654321")]
  public async Task Handle_CustomerPhoneNumberValid_ReturnsExistentCustomerDTO(string phoneNumber)
  {
    newCustomerDTO.PhoneNumber = phoneNumber;
    var result = await handler.Handle(request, default);

    Assert.NotNull(result);
    Assert.IsType<ExistentCustomerDTO>(result);
    Assert.True(result.Id > 0);
  }
}
