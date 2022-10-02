using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace BankManagement.Application;

public class UpdateTransferRequestCommandHandler : IRequestHandler<UpdateTransferRequestCommand, ExistentTransferDTO>
{
  private readonly IUnitOfWork unitOfwork;
  private readonly ITransferMappingProfile mappingProfile;

  public UpdateTransferRequestCommandHandler(IUnitOfWork unitOfwork, ITransferMappingProfile mappingProfile)
  {
    this.unitOfwork = unitOfwork;
    this.mappingProfile = mappingProfile;
  }

  public async Task<ExistentTransferDTO> Handle(UpdateTransferRequestCommand request, CancellationToken cancellationToken)
  {
    var transfer = await unitOfwork.TransfersRepo.Get(request.TransferId, request.RetrieveAccountInfo, request.RetrieveCustomerInfo
                                                , request.RetrieveDestinationAccountInfo, request.RetrieveDestinationAccountCustomerInfo);

    if (transfer is null) throw new ArgumentException(nameof(request.TransferId));
    if (request.TransferInfo is null) throw new ArgumentException(nameof(request.TransferInfo));

    var validator = new UpdatingTransferDTOValidator(unitOfwork);
    var validationResult = await validator.ValidateAsync(request.TransferInfo, cancellationToken);

    if (!validationResult.IsValid) throw new ArgumentException(string.Join(", ", validationResult.Errors.Select(x => x.PropertyName)));

    _ = mappingProfile.Map(request.TransferInfo, transfer);

    unitOfwork.TransfersRepo.Update(transfer);
    _ = await unitOfwork.Complete();

    ExistentTransferDTO result = mappingProfile.Map(transfer);

    return result;
  }
}
