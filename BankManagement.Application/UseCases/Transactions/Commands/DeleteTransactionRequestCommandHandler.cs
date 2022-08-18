using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace BankManagement.Application;

public class DeleteTransactionRequestCommandHandler : IRequestHandler<DeleteTransactionRequestCommand, ExistentTransactionDTO>
{
  private readonly IUnitOfWork unitOfwork;
  private readonly ITransactionMappingProfile mappingProfile;

  public DeleteTransactionRequestCommandHandler(IUnitOfWork unitOfwork, ITransactionMappingProfile mappingProfile)
  {
    this.unitOfwork = unitOfwork;
    this.mappingProfile = mappingProfile;
  }

  public async Task<ExistentTransactionDTO> Handle(DeleteTransactionRequestCommand request, CancellationToken cancellationToken)
  {
    if (request.TransactionId < 1) throw new ArgumentException(nameof(request.TransactionId));

    var transaction = await unitOfwork.TransactionsRepo.Get(request.TransactionId, request.RetrieveAccountInfo, request.RetrieveCustomerInfo);

    if (transaction is null) throw new ArgumentException(nameof(request.TransactionId));

    ExistentTransactionDTO result = mappingProfile.Map(transaction);

    unitOfwork.TransactionsRepo.Delete(transaction);
    _ = await unitOfwork.Complete();

    return result;
  }
}
