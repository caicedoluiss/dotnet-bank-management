using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace BankManagement.Application;

public class DeleteCustomerRequestCommandHandler : IRequestHandler<DeleteCustomerRequestCommand, ExistentCustomerDTO>
{
  public Task<ExistentCustomerDTO> Handle(DeleteCustomerRequestCommand request, CancellationToken cancellationToken)
  {
    throw new System.NotImplementedException();
  }
}
