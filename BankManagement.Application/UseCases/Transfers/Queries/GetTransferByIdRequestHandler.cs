using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace BankManagement.Application;

public class GetTransferByIdRequestHandler : IRequestHandler<GetTransferByIdRequest, ExistentTransferDTO?>
{
  private readonly IUnitOfWork unitOfWork;
  private readonly ITransferMappingProfile mappingProfile;

  public GetTransferByIdRequestHandler(IUnitOfWork unitOfWork, ITransferMappingProfile mappingProfile)
  {
    this.unitOfWork = unitOfWork;
    this.mappingProfile = mappingProfile;
  }

  public async Task<ExistentTransferDTO?> Handle(GetTransferByIdRequest request, CancellationToken cancellationToken)
  {
    if (request.TransferId < 1) throw new ArgumentException(nameof(request.TransferId));

    ExistentTransferDTO? result = null;

    var transfer = await unitOfWork.TransfersRepo.Get(request.TransferId, request.RetrieveAccountInfo, request.RetrieveCustomerInfo
                                                , request.RetrieveDestinationAccountInfo, request.RetrieveDestinationAccountCustomerInfo);

    if (transfer is not null)
      result = mappingProfile.Map(transfer);

    return result;
  }
}
