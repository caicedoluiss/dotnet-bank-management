using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace BankManagement.Application;

public class GetAccountByIdRequestHandler : IRequestHandler<GetAccountByIdRequest, ExistentAccountDTO?>
{
  private readonly IUnitOfWork unitOfWork;
  private readonly IAccountMappingProfile mappingProfile;

  public GetAccountByIdRequestHandler(IUnitOfWork unitOfWork, IAccountMappingProfile mappingProfile)
  {
    this.unitOfWork = unitOfWork;
    this.mappingProfile = mappingProfile;
  }


  public async Task<ExistentAccountDTO?> Handle(GetAccountByIdRequest request, CancellationToken cancellationToken)
  {
    if (request.AccountId < 1) throw new ArgumentException(nameof(request.AccountId));

    ExistentAccountDTO? result = null;

    var account = await unitOfWork.AccountsRepo.Get(request.AccountId, request.RetreiveCustomerInfo);

    if (account is not null)
      result = mappingProfile.Map(account);

    return result;
  }
}
