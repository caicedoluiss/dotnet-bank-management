using MediatR;

namespace BankManagement.Application;

public class GetAccountByIdRequest : IRequest<ExistentAccountDTO?>
{
  public int AccountId { get; set; }
  public bool RetreiveCustomerInfo { get; set; }
}
