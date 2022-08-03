using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BankManagement.Domain;
using MediatR;

namespace BankManagement.Application;

public class CreateCustomerRequestCommandHandler : IRequestHandler<CreateCustomerRequestCommand, int>
{
  private readonly IUnitOfWork unitOfwork;
  private readonly ICustomerMappingProfile mappingProfile;

  public CreateCustomerRequestCommandHandler(IUnitOfWork unitOfwork, ICustomerMappingProfile mappingProfile)
  {
    this.unitOfwork = unitOfwork;
    this.mappingProfile = mappingProfile;
  }

  public async Task<int> Handle(CreateCustomerRequestCommand request, CancellationToken cancellationToken)
  {
    if (request.CustomerInfo is null) throw new ArgumentException(nameof(request.CustomerInfo));

    var validator = new NewCustomerDTOValidator();
    var validationResult = await validator.ValidateAsync(request.CustomerInfo, cancellationToken);

    if (!validationResult.IsValid) throw new ArgumentException(string.Join(", ", validationResult.Errors.Select(x => x.PropertyName)));

    Customer customer = mappingProfile.Map(request.CustomerInfo);

    Customer result = unitOfwork.CustomersRepo.Add(customer);

    _ = await unitOfwork.Complete();

    return result.Id;
  }
}
