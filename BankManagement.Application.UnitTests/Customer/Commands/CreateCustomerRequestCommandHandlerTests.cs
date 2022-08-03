using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BankManagement.Application.UnitTests;

public class CreateCustomerRequestCommandHandlerTests
{
  private readonly CreateCustomerRequestCommandHandler handler;

  private readonly CreateCustomerRequestCommand request;
  private readonly NewCustomerDTO newCustomerDTO;

  public CreateCustomerRequestCommandHandlerTests()
  {
    var serviceProvider = TestsConfiguration.ServiceProvider;

    handler = serviceProvider.GetRequiredService<CreateCustomerRequestCommandHandler>();
    newCustomerDTO = serviceProvider.GetRequiredService<NewCustomerDTO>();
    request = new()
    {
      CustomerInfo = newCustomerDTO
    };
  }

  [Fact]
  public async void Handle_RequestNullCustomerInfo_ThrowsArgumentException()
  {
    request.CustomerInfo = null;
    var action = () => handler.Handle(request, default);

    await Assert.ThrowsAsync<ArgumentException>(action);
  }

  [Fact]
  public async void Handle_CustomerIdNumberEmpty_ThrowsArgumentException()
  {
    newCustomerDTO.IdNumber = string.Empty;
    var action = () => handler.Handle(request, default);

    await Assert.ThrowsAsync<ArgumentException>(action);
  }

  [Fact]
  public async void Handle_InvalidEmptyCustomerName_ThrowsArgumentException()
  {
    newCustomerDTO.Name = string.Empty;
    var action = () => handler.Handle(request, default);

    await Assert.ThrowsAsync<ArgumentException>(action);
  }

  [Theory]
  [InlineData("")]
  [InlineData("NonExistentValue")]
  [InlineData("-1")]
  public async void Handle_CustomerGenderInvalid_ThrowsArgumentException(string? gender)
  {
    newCustomerDTO.Gender = gender;
    var action = () => handler.Handle(request, default);

    await Assert.ThrowsAsync<ArgumentException>(action);
  }

  [Theory]
  [InlineData(null)]
  [InlineData("None")]
  [InlineData("Female")]
  [InlineData("Male")]
  [InlineData("Other")]
  public async Task Handle_CustomerGenderValid_ReturnsValidExistentCustomerDTO(string? gender)
  {
    newCustomerDTO.Gender = gender;
    var result = await handler.Handle(request, default);

    Assert.IsType<int>(result);
    Assert.NotEqual(default, result);
    Assert.True(result > 0);
  }


  [Theory]
  [InlineData(-1)]
  [InlineData(0)]
  [InlineData(17)]
  public async void Handle_CustomerAgeLessThan18_ThrowsArgumentException(int age)
  {
    newCustomerDTO.Age = age;
    var action = () => handler.Handle(request, default);

    await Assert.ThrowsAsync<ArgumentException>(action);
  }

  [Theory]
  [InlineData(18)]
  [InlineData(50)]
  public async Task Handle_CustomerAgeGreaterEqualsThan18_ReturnsValidId(int age)
  {
    newCustomerDTO.Age = age;
    var result = await handler.Handle(request, default);

    Assert.IsType<int>(result);
    Assert.NotEqual(default, result);
    Assert.True(result > 0);
  }

  [Theory]
  [InlineData("")]
  [InlineData("qwerty")]
  public async void Handle_CustomerEmailInvalid_ThrowsArgumentException(string email)
  {
    newCustomerDTO.Email = email;
    var action = () => handler.Handle(request, default);

    await Assert.ThrowsAsync<ArgumentException>(action);
  }

  [Theory]
  [InlineData("jdoe@email.com")]
  [InlineData("jdoe2@anotheremail.com.co")]
  public async Task Handle_CustomerEmailValid_ReturnsExistentCustomerDTO(string email)
  {
    newCustomerDTO.Email = email;
    var result = await handler.Handle(request, default);

    Assert.IsType<int>(result);
    Assert.NotEqual(default, result);
    Assert.True(result > 0);
  }

  [Theory]
  [InlineData("")]
  [InlineData("65665")]
  [InlineData("+1 123456798")]
  public async void Handle_CustomerPhoneNumberInvalid_ThrowsArgumentException(string phoneNumber)
  {
    newCustomerDTO.PhoneNumber = phoneNumber;
    var action = () => handler.Handle(request, default);

    await Assert.ThrowsAsync<ArgumentException>(action);
  }

  [Theory]
  [InlineData("+57 1234567890")]
  [InlineData("+1 0987654321")]
  public async Task Handle_CustomerPhoneNumberValid_ReturnsExistentCustomerDTO(string phoneNumber)
  {
    newCustomerDTO.PhoneNumber = phoneNumber;
    var result = await handler.Handle(request, default);

    Assert.IsType<int>(result);
    Assert.NotEqual(default, result);
    Assert.True(result > 0);
  }
}
