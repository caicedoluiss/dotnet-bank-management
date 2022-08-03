using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace BankManagement.Application;

public class DeleteCustomerRequestCommandHandler : IRequestHandler<DeleteCustomerRequestCommand, ExistentCustomerDTO>
{
  private readonly IUnitOfWork unitOfwork;

  private readonly ICustomerMappingProfile mappingProfile;

  public DeleteCustomerRequestCommandHandler(IUnitOfWork unitOfwork, ICustomerMappingProfile mappingProfile)
  {
    this.unitOfwork = unitOfwork;
    this.mappingProfile = mappingProfile;
  }



  public async Task<ExistentCustomerDTO> Handle(DeleteCustomerRequestCommand request, CancellationToken cancellationToken)
  {
    if (request.CustomerId < 1) throw new ArgumentException(nameof(request.CustomerId));

    var customer = await unitOfwork.CustomersRepo.Get(request.CustomerId);

    if (customer is null) throw new ArgumentException(nameof(request.CustomerId));

    ExistentCustomerDTO result = mappingProfile.Map(customer);

    unitOfwork.CustomersRepo.Delete(customer);
    _ = await unitOfwork.Complete();

    return result;
  }
}
