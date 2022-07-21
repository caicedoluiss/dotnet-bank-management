using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace BankManagement.Application;

public class CreateAccountRequestCommandHandler : IRequestHandler<CreateAccountRequestCommand, int>
{
  public Task<int> Handle(CreateAccountRequestCommand request, CancellationToken cancellationToken)
  {
    throw new System.NotImplementedException();
  }
}
