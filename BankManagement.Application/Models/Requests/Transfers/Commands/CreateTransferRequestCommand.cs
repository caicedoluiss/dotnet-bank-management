using MediatR;

namespace BankManagement.Application;

public class CreateTransferRequestCommand : IRequest<int>
{
  public NewTransferDTO? TrasnferInfo { get; set; }
}
