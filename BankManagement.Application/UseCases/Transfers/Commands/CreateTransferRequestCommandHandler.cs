using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace BankManagement.Application;

public class CreateTransferRequestCommandHandler : IRequestHandler<CreateTransferRequestCommand, int>
{
  public Task<int> Handle(CreateTransferRequestCommand request, CancellationToken cancellationToken)
  {
    throw new System.NotImplementedException();
  }
}
