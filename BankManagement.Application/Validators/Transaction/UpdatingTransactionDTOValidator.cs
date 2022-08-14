using System;
using BankManagement.Application.Utils;
using FluentValidation;

namespace BankManagement.Application;

internal class UpdatingTransactionDTOValidator : AbstractValidator<UpdatingTransactionDTO>
{
  public UpdatingTransactionDTOValidator(IUnitOfWork unitOfWork)
  {
    RuleFor(x => x.AccountId)
      .MustAsync((id, _) => unitOfWork.AccountsRepo.Exist(id))
      .WithMessage("Account must exist");

    RuleFor(x => x.Date)
      .NotNull().WithMessage("{PropertyName} required.")
      .NotEmpty().WithMessage("{PropertyName} required.")
      .Must(y => DateTime.TryParseExact(y, Constants.DateFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out _));
  }
}
