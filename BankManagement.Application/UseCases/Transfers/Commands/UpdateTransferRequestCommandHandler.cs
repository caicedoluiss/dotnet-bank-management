using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace BankManagement.Application;

public class UpdateTransferRequestCommandHandler : IRequestHandler<UpdateTransferRequestCommand, ExistentTransferDTO>
{
  public Task<ExistentTransferDTO> Handle(UpdateTransferRequestCommand request, CancellationToken cancellationToken)
  {
    throw new System.NotImplementedException();
  }
}
