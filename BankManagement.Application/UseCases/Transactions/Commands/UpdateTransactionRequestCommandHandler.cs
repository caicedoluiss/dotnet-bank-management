using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace BankManagement.Application;

public class UpdateTransactionRequestCommandHandler : IRequestHandler<UpdateTransactionRequestCommand, ExistentTransactionDTO>
{
  public Task<ExistentTransactionDTO> Handle(UpdateTransactionRequestCommand request, CancellationToken cancellationToken)
  {
    throw new System.NotImplementedException();
  }
}
