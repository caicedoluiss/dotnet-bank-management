using MediatR;

namespace BankManagement.Application;

public class UpdateAccountRequestCommand : IRequest<ExistentAccountDTO>
{
  public int AccountId { get; set; }
  public NewAccountDTO? AccountInfo { get; set; }
  public bool RetrieveCustomerInfo { get; set; }
}
