using BankManagement.Domain;
using FluentValidation;

namespace BankManagement.Application;

internal class NewAccountDTOValidator : AbstractValidator<NewAccountDTO>
{
  public NewAccountDTOValidator(IUnitOfWork unitOfWork)
  {
    RuleFor(x => x.Number)
      .NotNull().WithMessage("{PropertyName} required")
      .NotEmpty().WithMessage("{PropertyName} required");

    RuleFor(x => x.Type)
      .IsEnumName(typeof(AccountType), true).WithName("{PropertyName} invalid");

    RuleFor(x => x.CustomerId)
      .MustAsync((id, _) => unitOfWork.CustomersRepo.Exist(id))
      .WithMessage("Customer must exist");
  }
}
