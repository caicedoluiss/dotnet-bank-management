using MediatR;

namespace BankManagement.Application;

public class GetTransactionByIdRequest : IRequest<ExistentTransactionDTO>
{
  public int TransactionId { get; set; }
  public bool RetrieveAccountInfo { get; set; }
  public bool RetrieveCustomerInfo { get; set; }
}
