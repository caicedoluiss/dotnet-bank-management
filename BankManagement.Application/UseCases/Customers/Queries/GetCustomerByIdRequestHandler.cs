using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace BankManagement.Application;

public class GetCustomerByIdRequestHandler : IRequestHandler<GetCustomerByIdRequest, ExistentCustomerDTO>
{
  public Task<ExistentCustomerDTO> Handle(GetCustomerByIdRequest request, CancellationToken cancellationToken)
  {
    throw new System.NotImplementedException();
  }
}
