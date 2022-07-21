using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace BankManagement.Application;

public class DeleteTransactionRequestCommandHandler : IRequestHandler<DeleteTransactionRequestCommand, ExistentTransactionDTO>
{
  public Task<ExistentTransactionDTO> Handle(DeleteTransactionRequestCommand request, CancellationToken cancellationToken)
  {
    throw new System.NotImplementedException();
  }
}
