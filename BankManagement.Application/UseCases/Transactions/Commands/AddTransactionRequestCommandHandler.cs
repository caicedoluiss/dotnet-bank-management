using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BankManagement.Domain;
using MediatR;

namespace BankManagement.Application;

public class AddTransactionRequestCommandHandler : IRequestHandler<AddTransactionRequestCommand, int>
{
  private readonly IUnitOfWork unitOfwork;
  private readonly ITransactionMappingProfile mappingProfile;

  public AddTransactionRequestCommandHandler(IUnitOfWork unitOfwork, ITransactionMappingProfile mappingProfile)
  {
    this.unitOfwork = unitOfwork;
    this.mappingProfile = mappingProfile;
  }


  public async Task<int> Handle(AddTransactionRequestCommand request, CancellationToken cancellationToken)
  {
    if (request.TransactionInfo is null) throw new ArgumentException(nameof(request.TransactionInfo));

    var validator = new UpdatingTransactionDTOValidator(unitOfwork);
    var validationResult = await validator.ValidateAsync(request.TransactionInfo, cancellationToken);

    if (!validationResult.IsValid) throw new ArgumentException(string.Join(", ", validationResult.Errors.Select(x => x.PropertyName)));

    Transaction transaction = mappingProfile.Map(request.TransactionInfo);

    Transaction result = unitOfwork.TransactionsRepo.Add(transaction);
    _ = await unitOfwork.Complete();

    return result.Id;
  }
}
