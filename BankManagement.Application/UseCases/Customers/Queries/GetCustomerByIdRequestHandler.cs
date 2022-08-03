using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace BankManagement.Application;

public class GetCustomerByIdRequestHandler : IRequestHandler<GetCustomerByIdRequest, ExistentCustomerDTO?>
{
  private readonly IUnitOfWork unitOfWork;
  private readonly ICustomerMappingProfile mappingProfile;

  public GetCustomerByIdRequestHandler(IUnitOfWork unitOfWork, ICustomerMappingProfile mappingProfile)
  {
    this.unitOfWork = unitOfWork;
    this.mappingProfile = mappingProfile;
  }

  public async Task<ExistentCustomerDTO?> Handle(GetCustomerByIdRequest request, CancellationToken cancellationToken)
  {
    if (request.CustomerId < 1) throw new ArgumentException(nameof(request.CustomerId));

    ExistentCustomerDTO? existentCustomer = null;

    var result = await unitOfWork.CustomersRepo.Get(request.CustomerId);

    if (result is not null)
      existentCustomer = mappingProfile.Map(result);

    return existentCustomer;
  }
}
