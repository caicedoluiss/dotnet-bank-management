using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace BankManagement.Application;

public class GetAccountByIdRequestHandler : IRequestHandler<GetAccountByIdRequest, ExistentAccountDTO?>
{
  public Task<ExistentAccountDTO?> Handle(GetAccountByIdRequest request, CancellationToken cancellationToken)
  {
    throw new System.NotImplementedException();
  }
}
