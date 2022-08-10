using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace BankManagement.Application;

public class GetTransactionByIdRequestHandler : IRequestHandler<GetTransactionByIdRequest, ExistentTransactionDTO?>
{
  private readonly IUnitOfWork unitOfWork;
  private readonly ITransactionMappingProfile mappingProfile;

  public GetTransactionByIdRequestHandler(IUnitOfWork unitOfWork, ITransactionMappingProfile mappingProfile)
  {
    this.unitOfWork = unitOfWork;
    this.mappingProfile = mappingProfile;
  }

  public async Task<ExistentTransactionDTO?> Handle(GetTransactionByIdRequest request, CancellationToken cancellationToken)
  {
    if (request.TransactionId < 1) throw new ArgumentException(nameof(request.TransactionId));

    ExistentTransactionDTO? result = null;

    var transaction = await unitOfWork.TransactionsRepo.Get(request.TransactionId, request.RetrieveAccountInfo, request.RetrieveCustomerInfo);

    if (transaction is not null)
      result = mappingProfile.Map(transaction);

    return result;
  }
}
