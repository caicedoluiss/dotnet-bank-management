using MediatR;

namespace BankManagement.Application;

public class UpdateTransferRequestCommand : IRequest<ExistentTransferDTO>
{
  public UpdatingTransferDTO? TransanctionInfo { get; set; }
  public bool RetrieveAccountInfo { get; set; }
  public bool RetrieveCustomerInfo { get; set; }
  public bool RetrieveDestinationAccountInfo { get; set; }
  public bool RetrieveDestinationAccountCustomerInfo { get; set; }
}
