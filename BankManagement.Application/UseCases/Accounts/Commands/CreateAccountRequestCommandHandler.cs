using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BankManagement.Domain;
using MediatR;

namespace BankManagement.Application;

public class CreateAccountRequestCommandHandler : IRequestHandler<CreateAccountRequestCommand, int>
{
  private readonly IUnitOfWork unitOfwork;
  private readonly IAccountMappingProfile mappingProfile;

  public CreateAccountRequestCommandHandler(IUnitOfWork unitOfwork, IAccountMappingProfile mappingProfile)
  {
    this.unitOfwork = unitOfwork;
    this.mappingProfile = mappingProfile;
  }

  public async Task<int> Handle(CreateAccountRequestCommand request, CancellationToken cancellationToken)
  {
    if (request.AccountInfo is null) throw new ArgumentException(nameof(request.AccountInfo));

    var validator = new NewAccountDTOValidator(unitOfwork);
    var validationResult = await validator.ValidateAsync(request.AccountInfo, cancellationToken);

    if (!validationResult.IsValid) throw new ArgumentException(string.Join(", ", validationResult.Errors.Select(x => x.PropertyName)));

    Account account = mappingProfile.Map(request.AccountInfo);

    Account result = unitOfwork.AccountsRepo.Add(account);
    _ = await unitOfwork.Complete();

    return result.Id;
  }
}
