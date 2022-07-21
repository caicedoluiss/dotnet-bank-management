using MediatR;

namespace BankManagement.Application;

public class CreateAccountRequestCommand : IRequest<int>
{
  public NewAccountDTO? AccountInfo { get; set; }
}
