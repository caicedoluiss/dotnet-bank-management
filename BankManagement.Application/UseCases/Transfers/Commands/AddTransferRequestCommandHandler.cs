using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace BankManagement.Application;

public class AddTransferRequestCommandHandler : IRequestHandler<AddTransferRequestCommand, int>
{
  public Task<int> Handle(AddTransferRequestCommand request, CancellationToken cancellationToken)
  {
    throw new System.NotImplementedException();
  }
}
