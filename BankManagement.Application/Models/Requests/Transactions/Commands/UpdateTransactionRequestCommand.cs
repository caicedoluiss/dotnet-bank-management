using MediatR;

namespace BankManagement.Application;

public class UpdateTransactionRequestCommand : IRequest<ExistentTransactionDTO>
{
  public UpdatingTransactionDTO? TransanctionInfo { get; set; }
  public bool RetrieveAccountInfo { get; set; }
  public bool RetrieveCustomerInfo { get; set; }
}
