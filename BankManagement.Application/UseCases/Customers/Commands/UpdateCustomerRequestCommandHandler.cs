using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace BankManagement.Application;

public class UpdateCustomerRequestCommandHandler : IRequestHandler<UpdateCustomerRequestCommand, ExistentCustomerDTO>
{
  public Task<ExistentCustomerDTO> Handle(UpdateCustomerRequestCommand request, CancellationToken cancellationToken)
  {
    throw new System.NotImplementedException();
  }
}
