using FluentValidation;

namespace BankManagement.Application;

public class UpdatingTransferDTOValidator : AbstractValidator<UpdatingTransferDTO>
{
  public UpdatingTransferDTOValidator(IUnitOfWork unitOfWork)
  {
    Include(new UpdatingTransactionDTOValidator(unitOfWork));

    RuleFor(x => x.Value)
      .GreaterThan(0)
      .WithMessage("{PropertyName} must be greater than {ComparisonValue}");

    RuleFor(x => x.DestinationAccountId)
      .NotEqual((x) => x.AccountId)
      .WithMessage("Destination account can not be the same");

    RuleFor(x => x.DestinationAccountId)
      .MustAsync((id, _) => unitOfWork.AccountsRepo.Exist(id))
      .WithMessage("Destination account must exist");

  }
}
