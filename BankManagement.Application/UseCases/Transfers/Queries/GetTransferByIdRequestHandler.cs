using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace BankManagement.Application;

public class GetTransferByIdRequestHandler : IRequestHandler<GetTransferByIdRequest, ExistentTransferDTO>
{
  public Task<ExistentTransferDTO> Handle(GetTransferByIdRequest request, CancellationToken cancellationToken)
  {
    throw new System.NotImplementedException();
  }
}
