using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace BankManagement.Application;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerRequestCommand, int>
{
  public Task<int> Handle(CreateCustomerRequestCommand request, CancellationToken cancellationToken)
  {
    throw new System.NotImplementedException();
  }
}
