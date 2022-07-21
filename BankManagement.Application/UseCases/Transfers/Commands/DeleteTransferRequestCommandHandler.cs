using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace BankManagement.Application;

public class DeleteTransferRequestCommandHandler : IRequestHandler<DeleteTransferRequestCommand, ExistentTransferDTO>
{
  public Task<ExistentTransferDTO> Handle(DeleteTransferRequestCommand request, CancellationToken cancellationToken)
  {
    throw new System.NotImplementedException();
  }
}
