using BankManagement.Application.Utils;
using BankManagement.Domain;
using FluentValidation;
using System.Net.Mail;

namespace BankManagement.Application;

internal class NewCustomerDTOValidator : AbstractValidator<NewCustomerDTO>
{
  public NewCustomerDTOValidator()
  {
    RuleFor(x => x.IdNumber)
      .NotNull().WithMessage("{PropertyName} required.")
      .NotEmpty().WithMessage("{PropertyName} required.");

    RuleFor(x => x.Name)
      .NotNull().WithMessage("{PropertyName} required.")
      .NotEmpty().WithMessage("{PropertyName} required.");

    RuleFor(x => x.Gender)
      .IsEnumName(typeof(PersonGender), true).WithMessage("{PropertyName} invalid");

    RuleFor(x => x.Age)
      .GreaterThanOrEqualTo(Constants.MinCustomerAge).WithMessage("{PropertName} must be greater or equals than {ComparisonValue}");

    RuleFor(x => x.Email)
      .Must(y => MailAddress.TryCreate(y, out _)).WithMessage("{PropertyName} invalid");

    RuleFor(x => x.PhoneNumber)
      .Matches(Constants.PhoneNumberRegexString).WithMessage("{PropertyName} invalid");
  }
}
