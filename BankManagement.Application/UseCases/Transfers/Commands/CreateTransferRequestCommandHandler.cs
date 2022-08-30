using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BankManagement.Domain;
using MediatR;

namespace BankManagement.Application;

public class CreateTransferRequestCommandHandler : IRequestHandler<CreateTransferRequestCommand, int>
{
  private readonly IUnitOfWork unitOfwork;
  private readonly ITransferMappingProfile mappingProfile;

  public CreateTransferRequestCommandHandler(IUnitOfWork unitOfwork, ITransferMappingProfile mappingProfile)
  {
    this.unitOfwork = unitOfwork;
    this.mappingProfile = mappingProfile;
  }

  public async Task<int> Handle(CreateTransferRequestCommand request, CancellationToken cancellationToken)
  {
    if (request.TransferInfo is null) throw new ArgumentException(nameof(request.TransferInfo));

    var validator = new NewTransferDTOValidator(unitOfwork);
    var validationResult = await validator.ValidateAsync(request.TransferInfo, cancellationToken);

    if (!validationResult.IsValid) throw new ArgumentException(string.Join(", ", validationResult.Errors.Select(x => x.PropertyName)));

    //get account tracking it for updating its balance
    var account = await unitOfwork.AccountsRepo.Get(request.TransferInfo.AccountId, true);
    //get destination account tracking it for updating its balance
    var destAccount = await unitOfwork.AccountsRepo.Get(request.TransferInfo.DestinationAccountId, true);

    if (account is null) throw new ArgumentException(nameof(request.TransferInfo.AccountId));
    if (destAccount is null) throw new ArgumentException(nameof(request.TransferInfo.DestinationAccountId));

    Transfer transfer = mappingProfile.Map(request.TransferInfo);
    //Update account balance
    account.Balance -= transfer.Value;
    //Update destination account balance
    destAccount.Balance += transfer.Value;

    transfer.Balance = account.Balance;
    transfer.Date = DateTime.UtcNow;

    Transfer result = unitOfwork.TransfersRepo.Add(transfer);
    _ = await unitOfwork.Complete();

    return result.Id;
  }
}
