using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace BankManagement.Application;

public class GetTransactionByIdRequestHandler : IRequestHandler<GetTransactionByIdRequest, ExistentTransactionDTO>
{
  public Task<ExistentTransactionDTO> Handle(GetTransactionByIdRequest request, CancellationToken cancellationToken)
  {
    throw new System.NotImplementedException();
  }
}
