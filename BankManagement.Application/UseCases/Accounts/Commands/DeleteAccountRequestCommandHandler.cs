using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace BankManagement.Application;

public class DeleteAccountRequestCommandHandler : IRequestHandler<DeleteAccountRequestCommand, ExistentAccountDTO>
{
  private readonly IUnitOfWork unitOfwork;

  private readonly IAccountMappingProfile mappingProfile;

  public DeleteAccountRequestCommandHandler(IUnitOfWork unitOfwork, IAccountMappingProfile mappingProfile)
  {
    this.unitOfwork = unitOfwork;
    this.mappingProfile = mappingProfile;
  }

  public async Task<ExistentAccountDTO> Handle(DeleteAccountRequestCommand request, CancellationToken cancellationToken)
  {
    if (request.AccountId < 1) throw new ArgumentException(nameof(request.AccountId));

    var account = await unitOfwork.AccountsRepo.Get(request.AccountId, request.RetrieveCustomerInfo);

    if (account is null) throw new ArgumentException(nameof(request.AccountId));

    ExistentAccountDTO result = mappingProfile.Map(account);

    unitOfwork.AccountsRepo.Delete(account);
    _ = await unitOfwork.Complete();

    return result;
  }
}
