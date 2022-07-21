using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace BankManagement.Application;

public class AddTransactionRequestCommandHandler : IRequestHandler<AddTransactionRequestCommand, int>
{
  public Task<int> Handle(AddTransactionRequestCommand request, CancellationToken cancellationToken)
  {
    throw new System.NotImplementedException();
  }
}
