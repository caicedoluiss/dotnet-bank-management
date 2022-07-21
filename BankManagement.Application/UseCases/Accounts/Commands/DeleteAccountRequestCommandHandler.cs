using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace BankManagement.Application;

public class DeleteAccountRequestCommandHandler : IRequestHandler<DeleteAccountRequestCommand, ExistentAccountDTO>
{
  public Task<ExistentAccountDTO> Handle(DeleteAccountRequestCommand request, CancellationToken cancellationToken)
  {
    throw new System.NotImplementedException();
  }
}
