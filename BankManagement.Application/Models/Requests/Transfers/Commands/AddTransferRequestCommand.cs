using MediatR;

namespace BankManagement.Application;

public class AddTransferRequestCommand : IRequest<int>
{
  public UpdatingTransferDTO? TransferInfo { get; set; }
}
