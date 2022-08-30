using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace BankManagement.Application;

public class DeleteTransferRequestCommandHandler : IRequestHandler<DeleteTransferRequestCommand, ExistentTransferDTO>
{
  private readonly IUnitOfWork unitOfwork;
  private readonly ITransferMappingProfile mappingProfile;

  public DeleteTransferRequestCommandHandler(IUnitOfWork unitOfwork, ITransferMappingProfile mappingProfile)
  {
    this.unitOfwork = unitOfwork;
    this.mappingProfile = mappingProfile;
  }

  public async Task<ExistentTransferDTO> Handle(DeleteTransferRequestCommand request, CancellationToken cancellationToken)
  {
    if (request.TransferId < 1) throw new ArgumentException(nameof(request.TransferId));

    var transfer = await unitOfwork.TransfersRepo.Get(request.TransferId, request.RetrieveAccountInfo, request.RetrieveCustomerInfo
                                                , request.RetrieveDestinationAccountInfo, request.RetrieveDestinationAccountCustomerInfo);

    if (transfer is null) throw new ArgumentException(nameof(request.TransferId));

    ExistentTransferDTO result = mappingProfile.Map(transfer);

    unitOfwork.TransfersRepo.Delete(transfer);
    _ = await unitOfwork.Complete();

    return result;
  }
}
