using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BankManagement.Domain;
using MediatR;

namespace BankManagement.Application;

public class AddTransferRequestCommandHandler : IRequestHandler<AddTransferRequestCommand, int>
{
  private readonly IUnitOfWork unitOfwork;
  private readonly ITransferMappingProfile mappingProfile;

  public AddTransferRequestCommandHandler(IUnitOfWork unitOfwork, ITransferMappingProfile mappingProfile)
  {
    this.unitOfwork = unitOfwork;
    this.mappingProfile = mappingProfile;
  }

  public async Task<int> Handle(AddTransferRequestCommand request, CancellationToken cancellationToken)
  {
    if (request.TrasnferInfo is null) throw new ArgumentException(nameof(request.TrasnferInfo));

    var validator = new UpdatingTransferDTOValidator(unitOfwork);
    var validationResult = await validator.ValidateAsync(request.TrasnferInfo, cancellationToken);

    if (!validationResult.IsValid) throw new ArgumentException(string.Join(", ", validationResult.Errors.Select(x => x.PropertyName)));

    Transfer transfer = mappingProfile.Map(request.TrasnferInfo);

    Transfer result = unitOfwork.TransfersRepo.Add(transfer);
    _ = await unitOfwork.Complete();

    return result.Id;
  }
}
