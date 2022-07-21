using MediatR;

namespace BankManagement.Application;

public class UpdateAccountRequestCommand : IRequest<ExistentAccountDTO>
{
  public NewAccountDTO? AccountInfo { get; set; }
  public bool RetrieveCustomerInfo { get; set; }
}
