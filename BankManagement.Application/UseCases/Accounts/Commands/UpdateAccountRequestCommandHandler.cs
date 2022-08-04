using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace BankManagement.Application;

public class UpdateAccountRequestCommandHandler : IRequestHandler<UpdateAccountRequestCommand, ExistentAccountDTO>
{
  private readonly IUnitOfWork unitOfwork;
  private readonly IAccountMappingProfile mappingProfile;

  public UpdateAccountRequestCommandHandler(IUnitOfWork unitOfwork, IAccountMappingProfile mappingProfile)
  {
    this.unitOfwork = unitOfwork;
    this.mappingProfile = mappingProfile;
  }

  public async Task<ExistentAccountDTO> Handle(UpdateAccountRequestCommand request, CancellationToken cancellationToken)
  {
    if (request.AccountInfo is null) throw new ArgumentException(nameof(request.AccountInfo));

    var validator = new NewAccountDTOValidator(unitOfwork);
    var validationResult = await validator.ValidateAsync(request.AccountInfo, cancellationToken);

    if (!validationResult.IsValid) throw new ArgumentException(string.Join(", ", validationResult.Errors.Select(x => x.PropertyName)));

    var account = await unitOfwork.AccountsRepo.Get(request.AccountId, request.RetrieveCustomerInfo);

    if (account is null) throw new ArgumentException(nameof(request.AccountId));

    _ = mappingProfile.Map(request.AccountInfo, account);

    unitOfwork.AccountsRepo.Update(account);
    _ = await unitOfwork.Complete();

    ExistentAccountDTO result = mappingProfile.Map(account);

    return result;
  }
}
