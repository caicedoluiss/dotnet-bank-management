using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace BankManagement.Application;

public class CreateTransactionRequestCommandHandler : IRequestHandler<CreateTransactionRequestCommand, int>
{
  public Task<int> Handle(CreateTransactionRequestCommand request, CancellationToken cancellationToken)
  {
    throw new System.NotImplementedException();
  }
}
