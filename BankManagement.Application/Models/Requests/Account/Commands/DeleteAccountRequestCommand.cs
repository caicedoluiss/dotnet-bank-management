using MediatR;

namespace BankManagement.Application;

public class DeleteAccountRequestCommand : IRequest<ExistentAccountDTO>
{
  public int AccountId { get; set; }
  public bool RetrieveCustomerInfo { get; set; }
}
