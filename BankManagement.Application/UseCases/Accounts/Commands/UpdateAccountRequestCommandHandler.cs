using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace BankManagement.Application;

public class UpdateAccountRequestCommandHandler : IRequestHandler<UpdateAccountRequestCommand, ExistentAccountDTO>
{
  public Task<ExistentAccountDTO> Handle(UpdateAccountRequestCommand request, CancellationToken cancellationToken)
  {
    throw new System.NotImplementedException();
  }
}
