using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BankManagement.Domain;
using MediatR;

namespace BankManagement.Application;

public class CreateTransactionRequestCommandHandler : IRequestHandler<CreateTransactionRequestCommand, int>
{
  private readonly IUnitOfWork unitOfwork;
  private readonly ITransactionMappingProfile mappingProfile;

  public CreateTransactionRequestCommandHandler(IUnitOfWork unitOfwork, ITransactionMappingProfile mappingProfile)
  {
    this.unitOfwork = unitOfwork;
    this.mappingProfile = mappingProfile;
  }

  public async Task<int> Handle(CreateTransactionRequestCommand request, CancellationToken cancellationToken)
  {
    if (request.TransactionInfo is null) throw new ArgumentException(nameof(request.TransactionInfo));

    var validator = new NewTransactionDTOValidator(unitOfwork);
    var validationResult = await validator.ValidateAsync(request.TransactionInfo, cancellationToken);

    if (!validationResult.IsValid) throw new ArgumentException(string.Join(", ", validationResult.Errors.Select(x => x.PropertyName)));

    //get account tracking it for updating its balance
    var account = await unitOfwork.AccountsRepo.Get(request.TransactionInfo.AccountId, true);

    if (account is null) throw new ArgumentException(nameof(request.TransactionInfo.AccountId));

    Transaction transaction = mappingProfile.Map(request.TransactionInfo);
    //Update account balance
    account.Balance += transaction.Value;
    transaction.Balance = account.Balance;
    transaction.Date = DateTime.UtcNow;

    Transaction result = unitOfwork.TransactionsRepo.Add(transaction);
    _ = await unitOfwork.Complete();

    return result.Id;
  }
}
