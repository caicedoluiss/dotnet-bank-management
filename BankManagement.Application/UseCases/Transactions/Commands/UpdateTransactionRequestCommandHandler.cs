using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace BankManagement.Application;

public class UpdateTransactionRequestCommandHandler : IRequestHandler<UpdateTransactionRequestCommand, ExistentTransactionDTO>
{
  private readonly IUnitOfWork unitOfwork;
  private readonly ITransactionMappingProfile mappingProfile;

  public UpdateTransactionRequestCommandHandler(IUnitOfWork unitOfwork, ITransactionMappingProfile mappingProfile)
  {
    this.unitOfwork = unitOfwork;
    this.mappingProfile = mappingProfile;
  }

  public async Task<ExistentTransactionDTO> Handle(UpdateTransactionRequestCommand request, CancellationToken cancellationToken)
  {
    var transaction = await unitOfwork.TransactionsRepo.Get(request.TransactionId, request.RetrieveAccountInfo, request.RetrieveCustomerInfo);

    if (transaction is null) throw new ArgumentException(nameof(request.TransactionId));
    if (request.TransactionInfo is null) throw new ArgumentException(nameof(request.TransactionInfo));

    var validator = new UpdatingTransactionDTOValidator(unitOfwork);
    var validationResult = await validator.ValidateAsync(request.TransactionInfo, cancellationToken);

    if (!validationResult.IsValid) throw new ArgumentException(string.Join(", ", validationResult.Errors.Select(x => x.PropertyName)));

    _ = mappingProfile.Map(request.TransactionInfo, transaction);

    unitOfwork.TransactionsRepo.Update(transaction);
    _ = await unitOfwork.Complete();

    ExistentTransactionDTO result = mappingProfile.Map(transaction);

    return result;
  }
}
