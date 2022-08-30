using MediatR;

namespace BankManagement.Application;

public class CreateTransferRequestCommand : IRequest<int>
{
  public NewTransferDTO? TransferInfo { get; set; }
}
