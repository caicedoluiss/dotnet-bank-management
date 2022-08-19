using FluentValidation;

namespace BankManagement.Application;

public class NewTransactionDTOValidator : AbstractValidator<NewTransactionDTO>
{
  public NewTransactionDTOValidator(IUnitOfWork unitOfWork)
  {
    RuleFor(x => x.AccountId)
      .MustAsync((id, _) => unitOfWork.AccountsRepo.Exist(id))
      .WithMessage("Account must exist");
  }
}
