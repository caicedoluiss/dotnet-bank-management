using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BankManagement.Domain;
using MediatR;

namespace BankManagement.Application;

public class UpdateCustomerRequestCommandHandler : IRequestHandler<UpdateCustomerRequestCommand, ExistentCustomerDTO>
{
  private readonly IUnitOfWork unitOfwork;
  private readonly ICustomerMappingProfile mappingProfile;

  public UpdateCustomerRequestCommandHandler(IUnitOfWork unitOfwork, ICustomerMappingProfile mappingProfile)
  {
    this.unitOfwork = unitOfwork;
    this.mappingProfile = mappingProfile;
  }


  public async Task<ExistentCustomerDTO> Handle(UpdateCustomerRequestCommand request, CancellationToken cancellationToken)
  {
    if (request.CustomerInfo is null) throw new ArgumentException(nameof(request.CustomerInfo));

    var validator = new NewCustomerDTOValidator();
    var validationResult = await validator.ValidateAsync(request.CustomerInfo, cancellationToken);

    if (!validationResult.IsValid) throw new ArgumentException(string.Join(", ", validationResult.Errors.Select(x => x.PropertyName)));

    var customer = await unitOfwork.CustomersRepo.Get(request.CustomerId);

    if (customer is null) throw new ArgumentException(nameof(request.CustomerId));

    _ = mappingProfile.Map(request.CustomerInfo, customer);

    unitOfwork.CustomersRepo.Update(customer);
    _ = await unitOfwork.Complete();

    ExistentCustomerDTO result = mappingProfile.Map(customer);

    return result;
  }
}
