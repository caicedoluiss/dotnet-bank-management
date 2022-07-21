using MediatR;

namespace BankManagement.Application;

public class DeleteTransferRequestCommand : IRequest<ExistentTransferDTO>
{
  public int TransferId { get; set; }
  public bool RetrieveAccountInfo { get; set; }
  public bool RetrieveCustomerInfo { get; set; }
  public bool RetrieveDestinationAccountInfo { get; set; }
  public bool RetrieveDestinationAccountCustomerInfo { get; set; }
}
